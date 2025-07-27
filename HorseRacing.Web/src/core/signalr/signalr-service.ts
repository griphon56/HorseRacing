import * as signalR from '@microsoft/signalr';
import { useAuthStore } from '~/stores/auth-store';

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private starting: Promise<void> | null = null

  // Base methods
  async connectToHub(hubName: string): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      console.debug(`Already connected to hub: ${hubName}`);
      return;
    }

    if (this.starting) {
      return this.starting;
    }

    this.starting = (async () => {
      const authStore = useAuthStore();
      const token = authStore.tokens?.AccessToken;
      if (!token) {
        throw new Error('Missing SignalR access token');
      }

      const hubUrl = `/hubs/${hubName}`;
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl, { accessTokenFactory: () => token })
        .withAutomaticReconnect()
        .build();

      // Reset on close
      this.connection.onclose(() => {
        console.info(`SignalR connection to ${hubName} closed`);
        this.connection = null;
      });

      await this.connection.start();
      console.info(`SignalR connected to ${hubName}`);
      this.starting = null;
    })();

    return this.starting;
  }

  /** Gracefully stop & clean up */
  async disconnect(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
      console.info(`SignalR connection stopped`);
    }
  }

  getConnectionState(): signalR.HubConnectionState | 'Disconnected' {
    if (!this.connection) {
      return 'Disconnected';
    }
    return this.connection.state;
  }

    private async ensureConnected(hubName: string) {
        if (!this.connection || this.connection.state !== signalR.HubConnectionState.Connected) {
        await this.connectToHub(hubName)
        }
    }

  // Invoke methods
  async invokeMethod<T>(methodName: string, ...args: unknown[]): Promise<T> {
    await this.ensureConnected('commonHub')

    try {
      const result = await this.connection!.invoke<T>(methodName, ...args);
      console.log(`SignalR method ${methodName} invoked successfully`);
      return result;
    } catch (err) {
      console.error(`Error invoking SignalR method ${methodName}:`, err);
      throw err;
    }
  }

  async joinToGame(gameId: string): Promise<void> {
    await this.invokeMethod<void>('JoinToGame', gameId);
    console.log(`Joined game group ${gameId}`);
  }
    async subscribeToUpdateListLobby(): Promise<void> {
        await this.invokeMethod<void>('SubscribeToUpdateListLobby');
    }

  // Event handling
  onEvent(eventName: string, callback: (...args: unknown[]) => void): void {
    this.ensureConnected('commonHub')
      .then(() => this.connection!.on(eventName, callback))
      .catch(console.error)
    console.log(`Subscribed to event ${eventName}`);
  }

  offEvent(eventName: string): void {
    this.ensureConnected('commonHub')
      .then(() => this.connection!.off(eventName))
      .catch(console.error)
      console.log(`Unsubscribed from event ${eventName}`);
  }

  onUpdateListLobby(callback: (updatedList: unknown) => void): void {
    this.onEvent('UpdateListLobby', callback);
  }

  offUpdateListLobby(): void {
    this.offEvent('UpdateListLobby');
  }

  onStartGame(callback: () => void): void {
    this.onEvent('StartGame', callback);
  }
}

export const signalRService = new SignalRService();
