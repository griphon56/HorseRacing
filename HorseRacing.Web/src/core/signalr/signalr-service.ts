import * as signalR from '@microsoft/signalr'
import { useAuthStore } from '~/stores/auth-store'
import { SignalRHubName, SignalRJoinToGame, SignalROnAvailableSuitsUpdated, SignalROnGameListUpdated, SignalROnGameSimulationResult, SignalROnLobbyPlayerListUpdated, SignalRRegisterReadyToStart, SignalRSubscribeGameListUpdate } from './constants'
import type { PlayGameResponse } from '~/interfaces/api/contracts/model/game/responses/play-game/play-game-response'

class SignalRService {
    private connection: signalR.HubConnection | null = null
    private starting: Promise<void> | null = null

    // Base methods
    async connectToHub(hubName: string): Promise<void> {
        if (this.connection?.state === signalR.HubConnectionState.Connected) {
            console.debug(`Already connected to hub: ${hubName}`)
            return
        }

        if (this.starting) {
            return this.starting
        }

        this.starting = (async () => {
            const authStore = useAuthStore()
            const token = authStore.tokens?.AccessToken
            if (!token) {
                throw new Error('Missing SignalR access token')
            }

            const hubUrl = `/hubs/${hubName}`
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl(hubUrl, { accessTokenFactory: () => token })
                .withAutomaticReconnect()
                .build()

            // Reset on close
            this.connection.onclose(() => {
                console.info(`SignalR connection to ${hubName} closed`)
                this.connection = null
            })

            await this.connection.start()
            console.info(`SignalR connected to ${hubName}`)
            this.starting = null
        })()

        return this.starting
    }

    getConnectionState(): signalR.HubConnectionState | 'Disconnected' {
        if (!this.connection) {
            return 'Disconnected'
        }
        return this.connection.state
    }

    async disconnect(): Promise<void> {
        if (this.connection) {
            await this.connection.stop()
            this.connection = null
            console.info(`SignalR connection stopped`)
        }
    }

    private async ensureConnected(hubName: string) {
        if (!this.connection || this.connection.state !== signalR.HubConnectionState.Connected) {
            await this.connectToHub(hubName)
        }
    }

    // Invoke methods
    async invokeMethod<T>(methodName: string, ...args: unknown[]): Promise<T> {
        await this.ensureConnected(SignalRHubName)

        try {
            const result = await this.connection!.invoke<T>(methodName, ...args)
            console.log(`SignalR method ${methodName} invoked successfully`)
            return result
        } catch (err) {
            console.error(`Error invoking SignalR method ${methodName}:`, err)
            throw err
        }
    }

    async joinToGame(gameId: string): Promise<void> {
        await this.invokeMethod<void>(SignalRJoinToGame, gameId)
        console.log(`Joined game group ${gameId}`)
    }

    async subscribeGameListUpdates(): Promise<void> {
        await this.invokeMethod<void>(SignalRSubscribeGameListUpdate)
    }

    async registerReadyToStart(gameId: string): Promise<void> {
        await this.invokeMethod<void>(SignalRRegisterReadyToStart, gameId)
        console.log(`Registered ready to start for game ${gameId}`)
    }

    // Event handling
    async onEvent(eventName: string, callback: (...args: unknown[]) => void): Promise<void> {
        this.ensureConnected(SignalRHubName)
            .then(() => this.connection!.on(eventName, callback))
            .catch(console.error)
        console.log(`Subscribed to event ${eventName}`)
    }

     async offEvent(eventName: string): Promise<void> {
        if (!this.connection) {
            console.debug(`No connection - nothing to unsubscribe for ${eventName}`);
            return;
        }

        try {
            this.connection.off(eventName);
            console.log(`Unsubscribed from event ${eventName}`);
        } catch (err) {
            console.error(`Error unsubscribing from ${eventName}:`, err);
        }
    }

    /** Подписка на одно событие с ожиданием результата и таймаутом */
    async once<T = unknown>(eventName: string, timeoutMs = 30000): Promise<T> {
        await this.ensureConnected(SignalRHubName)

        if (!this.connection) throw new Error('SignalR connection not available')

        return new Promise<T>((resolve, reject) => {
        let timer: ReturnType<typeof setTimeout> | null = null

        const handler = (...args: unknown[]) => {
            try { this.connection?.off(eventName, handler) } catch {}
            if (timer) { clearTimeout(timer); timer = null }
            if (args.length === 0) resolve(undefined as unknown as T)
            else if (args.length === 1) resolve(args[0] as T)
            else resolve(args as unknown as T)
        }

        // подписываемся синхронно — connection.on возвращает сразу
        this.connection!.on(eventName, handler)

        console.log(`Subscribed once to ${eventName}`)

        timer = setTimeout(() => {
            try { this.connection?.off(eventName, handler) } catch {}
            reject(new Error(`Timeout (${timeoutMs}ms) waiting for SignalR event '${eventName}'`))
        }, timeoutMs)
        })
    }

    async onGameListUpdated(callback: (updatedList: unknown) => void): Promise<void> {
        this.onEvent(SignalROnGameListUpdated, callback)
    }

    async offGameListUpdated(): Promise<void> {
        this.offEvent(SignalROnGameListUpdated)
    }

    async onLobbyPlayerListUpdated(callback: (updatedList: unknown) => void): Promise<void> {
        this.onEvent(SignalROnLobbyPlayerListUpdated, callback)
    }

    async offLobbyPlayerListUpdated(): Promise<void> {
        this.offEvent(SignalROnLobbyPlayerListUpdated)
    }

    async onAvailableSuitsUpdated(callback: (updatedList: unknown) => void): Promise<void> {
        this.onEvent(SignalROnAvailableSuitsUpdated, callback)
    }

    async offAvailableSuitsUpdated(): Promise<void> {
        this.offEvent(SignalROnAvailableSuitsUpdated)
    }

    async onGameSimulationResult(): Promise<PlayGameResponse> {
         const result = await this.once<PlayGameResponse>(SignalROnGameSimulationResult, 60000)
         return result as PlayGameResponse;
    }

    async offGameSimulationResult(): Promise<void> {
        this.offEvent(SignalROnGameSimulationResult)
    }

}

export const signalRService = new SignalRService()
