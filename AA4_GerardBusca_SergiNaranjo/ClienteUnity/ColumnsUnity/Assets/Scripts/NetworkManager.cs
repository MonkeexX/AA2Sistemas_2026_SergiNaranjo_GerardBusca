using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

[System.Serializable]
public class ServerMessage
{
    public string type;
    public string data;
}

[System.Serializable]
public class GameInfo
{
    public int id;
    public int players;
}

[System.Serializable]
public class GamesList
{
    public GameInfo[] games;
}

public class NetworkManager : MonoBehaviour
{
    public string serverIP = "127.0.0.1";
    public int serverPort = 3000;

    TcpClient client;
    StreamReader reader;
    StreamWriter writer;
    Thread listenThread;

    ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

    public static NetworkManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Connect();
    }

    public void Connect()
    {
        try
        {
            client = new TcpClient(serverIP, serverPort);

            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            listenThread = new Thread(ListenServer);
            listenThread.IsBackground = true;
            listenThread.Start();

            Debug.Log("Conectado al servidor");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error de conexión: " + e.Message);
        }
    }

    void ListenServer()
    {
        while (client != null && client.Connected)
        {
            try
            {
                string message = reader.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    messageQueue.Enqueue(message);
                }
            }
            catch
            {
                break;
            }
        }
    }

    void Update()
    {
        while (messageQueue.TryDequeue(out string msg))
        {
            HandleMessage(msg);
        }
    }

    public void Send(string msg)
    {
        if (writer != null)
        {
            writer.WriteLine(msg);
        }
    }

    void HandleMessage(string msg)
    {
        Debug.Log("Servidor: " + msg);

        ServerMessage baseMsg = JsonUtility.FromJson<ServerMessage>(msg);

        switch (baseMsg.type)
        {
            case "welcome":
                Debug.Log("Mensaje bienvenida: " + baseMsg.data);
                break;

            case "gamesList":
                HandleGamesList(baseMsg.data);
                break;

            case "gridUpdate":
                HandleGridUpdate(baseMsg.data);
                break;

            case "replayList":
                HandleReplayList(baseMsg.data);
                break;

            default:
                Debug.LogWarning("Tipo de mensaje desconocido: " + baseMsg.type);
                break;
        }
    }

    void HandleGamesList(string json)
    {
        GamesList list = JsonUtility.FromJson<GamesList>(json);

        Debug.Log("Salas disponibles:");
        foreach (GameInfo game in list.games)
        {
            Debug.Log("Sala {game.id} | Jugadores: {game.players}");
        }
    }

    void HandleGridUpdate(string json)
    {
        Debug.Log("Grid recibida");

        // Aquí:
        // 1. Parsear grid
        // 2. Actualizar NodeGrid[,]
        // 3. Llamar a GridRenderer
    }

    void HandleReplayList(string json)
    {
        Debug.Log("Lista de replays recibida");
    }

    void OnApplicationQuit()
    {
        listenThread?.Abort();
        reader?.Close();
        writer?.Close();
        client?.Close();
    }
}
