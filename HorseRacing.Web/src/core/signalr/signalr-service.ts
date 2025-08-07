import * as signalR from '@microsoft/signalr'
import { useAuthStore } from '~/stores/auth-store'
import { SignalRHubName, SignalRJoinToGame, SignalROnAvailableSuitsUpdated, SignalROnGameListUpdated, SignalROnLobbyPlayerListUpdated, SignalRSubscribeGameListUpdate } from './constants'

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

    /** Gracefully stop & clean up */
    async disconnect(): Promise<void> {
        if (this.connection) {
            await this.connection.stop()
            this.connection = null
            console.info(`SignalR connection stopped`)
        }
    }

    getConnectionState(): signalR.HubConnectionState | 'Disconnected' {
        if (!this.connection) {
            return 'Disconnected'
        }
        return this.connection.state
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

    // Event handling
    onEvent(eventName: string, callback: (...args: unknown[]) => void): void {
        this.ensureConnected(SignalRHubName)
            .then(() => this.connection!.on(eventName, callback))
            .catch(console.error)
        console.log(`Subscribed to event ${eventName}`)
    }

    offEvent(eventName: string): void {
        this.ensureConnected(SignalRHubName)
            .then(() => this.connection!.off(eventName))
            .catch(console.error)
        console.log(`Unsubscribed from event ${eventName}`)
    }

    onGameListUpdated(callback: (updatedList: unknown) => void): void {
        this.onEvent(SignalROnGameListUpdated, callback)
    }

    offGameListUpdated(): void {
        this.offEvent(SignalROnGameListUpdated)
    }

    onLobbyPlayerListUpdated(callback: (updatedList: unknown) => void): void {
        this.onEvent(SignalROnLobbyPlayerListUpdated, callback)
    }

    offLobbyPlayerListUpdated(): void {
        this.offEvent(SignalROnLobbyPlayerListUpdated)
    }

    onAvailableSuitsUpdated(callback: (updatedList: unknown) => void): void {
        this.onEvent(SignalROnAvailableSuitsUpdated, callback)
    }

    offAvailableSuitsUpdated(): void {
        this.offEvent(SignalROnAvailableSuitsUpdated)
    }

}

export const signalRService = new SignalRService()
