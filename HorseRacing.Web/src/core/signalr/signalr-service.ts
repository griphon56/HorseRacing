import * as signalR from '@microsoft/signalr';
import { useAuthStore } from '~/stores/auth-store';

class SignalRService {
  private connection: signalR.HubConnection | null = null;

  async connectToHub(gameId: string): Promise<void> {
    const authStore = useAuthStore();
    const token = authStore.tokens?.AccessToken;
    const userId = authStore.user?.Id;

    if (!token || !userId || !gameId) {
      throw new Error('Missing required parameters for SignalR connection');
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/commonHub', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.connection.onclose(() => {
      console.log('SignalR connection closed');
    });

    try {
      await this.connection.start();
      console.log('SignalR connection established');
    } catch (err) {
      console.error('Error starting SignalR connection:', err);
      throw err;
    }
  }

  async invokeMethod(methodName: string, ...args: any[]): Promise<void> {
    if (!this.connection) {
      throw new Error('SignalR connection is not established');
    }

    try {
      await this.connection.invoke(methodName, ...args);
      console.log(`SignalR method ${methodName} invoked successfully`);
    } catch (err) {
      console.error(`Error invoking SignalR method ${methodName}:`, err);
      throw err;
    }
  }

  disconnect(): void {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
      console.log('SignalR connection stopped');
    }
  }
}

export const signalRService = new SignalRService();
