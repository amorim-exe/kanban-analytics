import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

export function useSignalR() {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    const conn = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/hubs/revo')
      .withAutomaticReconnect()
      .build();

    setConnection(conn);

    conn.start()
      .then(() => {
        console.log('SignalR Connected');
        setIsConnected(true);
      })
      .catch((err) => console.error('SignalR Connection Error:', err));

    return () => {
      if (conn) {
        conn.stop();
      }
    };
  }, []);

  return { connection, isConnected };
}
