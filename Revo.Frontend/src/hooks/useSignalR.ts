
import { useEffect, useState } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from "@microsoft/signalr";

let sharedConnection: HubConnection | null = null;
let startPromise: Promise<void> | null = null;
let cleanupTimer: ReturnType<typeof setTimeout> | null = null;
let subscribers = 0;

function getConnection(): HubConnection {
  if (!sharedConnection) {
    sharedConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5149/hubs/revo")
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();
  }

  return sharedConnection;
}

async function startConnection(connection: HubConnection) {
  if (connection.state === HubConnectionState.Connected) {
    return;
  }

  if (startPromise) {
    return startPromise;
  }

  if (connection.state !== HubConnectionState.Disconnected) {
    return;
  }

  startPromise = connection.start();

  try {
    await startPromise;
    console.log("SignalR Connected");
  } finally {
    startPromise = null;
  }
}

export function useSignalR() {
  const [connection, setConnection] =
    useState<HubConnection | null>(null);

  useEffect(() => {
    let cancelled = false;
    const hubConnection = getConnection();

    subscribers++;

    if (cleanupTimer) {
      clearTimeout(cleanupTimer);
      cleanupTimer = null;
    }

    void startConnection(hubConnection)
      .then(() => {
        if (!cancelled) {
          setConnection(hubConnection);
        }
      })
      .catch((error) => {
        if (!cancelled) {
          console.error("SignalR Connection Error:", error);
        }
      });

    return () => {
      cancelled = true;
      subscribers--;

      cleanupTimer = setTimeout(() => {
        cleanupTimer = null;

        if (subscribers === 0 && sharedConnection) {
          const connectionToStop = sharedConnection;

          sharedConnection = null;
          startPromise = null;

          void connectionToStop.stop().catch((error) => {
            console.error("SignalR Stop Error:", error);
          });
        }
      }, 0);
    };
  }, []);

  return { connection };
}
