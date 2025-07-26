import * as signalR from '@microsoft/signalr';
import { useAuthStore } from '~/stores/auth-store';

class SignalRService {
  private connections: Record<string, signalR.HubConnection> = {};

  async connectToHub(hubName: string): Promise<void> {
    const authStore = useAuthStore();
    const token = authStore.tokens?.AccessToken;
    const userId = authStore.user?.Id;

    if (!token || !userId) {
      throw new Error('Missing required parameters for SignalR connection');
    }

    const hubUrl = `/hubs/${hubName}`;
    if (this.connections[hubName]) {
      console.log(`Already connected to hub: ${hubName}`);
      return;
    }

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    connection.onclose(() => {
      console.log(`SignalR connection to ${hubName} closed`);
      delete this.connections[hubName];
    });

    try {
      await connection.start();
      console.log(`SignalR connection to ${hubName} established`);
      this.connections[hubName] = connection;
    } catch (err) {
      console.error(`Error starting SignalR connection to ${hubName}:`, err);
      throw err;
    }
  }

  async invokeMethod<T>(hubName: string, methodName: string, ...args: unknown[]): Promise<T> {
    const connection = this.connections[hubName];
    if (!connection) {
      throw new Error(`SignalR connection to hub ${hubName} is not established`);
    }

    try {
      const result = await connection.invoke<T>(methodName, ...args);
      console.log(`SignalR method ${methodName} invoked successfully on hub ${hubName}`);
      return result;
    } catch (err) {
      console.error(`Error invoking SignalR method ${methodName} on hub ${hubName}:`, err);
      throw err;
    }
  }

  onEvent(hubName: string, eventName: string, callback: (...args: unknown[]) => void): void {
    const connection = this.connections[hubName];
    if (!connection) {
      throw new Error(`SignalR connection to hub ${hubName} is not established`);
    }

    connection.on(eventName, callback);
    console.log(`Subscribed to event ${eventName} on hub ${hubName}`);
  }
  offEvent(hubName: string, eventName: string): void {
    const connection = this.connections[hubName];
    if (connection) {
        connection.off(eventName);
        console.log(`Unsubscribed from event ${eventName} on hub ${hubName}`);
    }
  }
  disconnect(hubName: string): void {
    const connection = this.connections[hubName];
    if (connection) {
      connection.stop();
      delete this.connections[hubName];
      console.log(`SignalR connection to ${hubName} stopped`);
    }
  }

  async subscribeToLobby(hubName: string): Promise<void> {
    await this.invokeMethod<void>(hubName, 'SubscribeToLobby');
    console.log(`Subscribed to LobbyViewersGroup on hub ${hubName}`);
  }

  async unsubscribeFromLobby(hubName: string): Promise<void> {
    await this.invokeMethod<void>(hubName, 'UnsubscribeFromLobby');
    console.log(`Unsubscribed from LobbyViewersGroup on hub ${hubName}`);
  }

  onUpdateListLobby(hubName: string, callback: (updatedList: unknown) => void): void {
    this.onEvent(hubName, 'UpdateListLobby', callback);
    console.log(`Subscribed to UpdateListLobby event on hub ${hubName}`);
  }

  async joinToGame(hubName: string, gameId: string): Promise<void> {
    await this.invokeMethod<void>(hubName, 'JoinToGame', gameId);
    console.log(`Joined game group ${gameId} on hub ${hubName}`);
  }

  onStartGame(hubName: string, callback: () => void): void {
    this.onEvent(hubName, 'StartGame', callback);
    console.log(`Subscribed to StartGame event on hub ${hubName}`);
  }

  getConnectionState(hubName: string): signalR.HubConnectionState | 'Disconnected' {
    const connection = this.connections[hubName];
    if (!connection) {
      return 'Disconnected';
    }
    return connection.state;
  }
}

export const signalRService = new SignalRService();
