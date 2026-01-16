using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Linq;
using UnityEngine;
using static NodeGrid;

public class SocketIOController : MonoBehaviour
{
    public static SocketIOController Instance { get; private set; }

    [Header("Config")]
    [SerializeField] private string serverURL = "http://127.0.0.1:3000";

    [Header("References")]
    [SerializeField] private NodeGrid grid;

    private SocketIOUnity socket;

    #region ===== UNITY LIFECYCLE =====

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeSocket();
        RegisterSocketEvents();
        socket.Connect();
    }

    #endregion

    #region ===== SOCKET SETUP =====

    private void InitializeSocket()
    {
        socket = new SocketIOUnity(new Uri(serverURL));
    }

    private void RegisterSocketEvents()
    {
        socket.OnConnected += OnSocketConnected;

        socket.On("ReceiveRecording", OnReceiveRecording);
        socket.On("ReceiveRecordings", OnReceiveRecordings);
       
        socket.OnConnected += (sender, e) => Debug.Log("✅ Socket conectado");
        socket.OnDisconnected += (sender, e) => Debug.Log("❌ Socket desconectado");
        
        socket.On("unityMove", OnUnityMove);
    }


    public SocketIOUnity GetSocket() => socket;

    #endregion

    #region ===== SOCKET EVENTS =====

    private void OnSocketConnected(object sender, EventArgs e)
    {
        Debug.Log("Socket connected");
        socket.Emit("UnityTest", "GetRecordings");
    }


    private void OnReceiveRecording(SocketIOResponse response)
    {
        Debug.Log("Recording received");

        try
        {
            var updatesArray = ExtractInnerArray(response.ToString());
            grid.ClearRecording();

            foreach (var update in ParseGridUpdates(updatesArray))
            {
                grid.AddUpdate(update);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error processing recording: {ex.Message}");
        }
    }

    private void OnReceiveRecordings(SocketIOResponse response)
    {
        Debug.Log("Recordings list received");

        try
        {
            var recordsArray = ExtractInnerArray(response.ToString());

            foreach (var token in recordsArray)
            {
                if (token["id"] != null)
                {
                    grid.recordingIDs.Add(token["id"].Value<int>());
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error processing recordings list: {ex.Message}");
        }
    }


    private void OnUnityMove(SocketIOResponse response)
    {
        string direction = response.GetValue<string>();
        Debug.Log("Unity recibió: " + direction);
        switch (direction.ToLower())
        {
            case "arriba":
                Debug.Log("Up");
                break;
            case "abajo":
                Debug.Log("Down");
                break;
            case "izquierda":
                Debug.Log("Left");
                break;
            case "derecha":
                Debug.Log("Right");
                break;
            default:
                Debug.Log("Movimiento desconocido: " + direction);
                break;
        }
    }


    #endregion

    #region ===== JSON HELPERS =====

    private static JToken ExtractInnerArray(string rawMessage)
    {
        var outerArray = JArray.Parse(rawMessage);
        return outerArray[0][0];
    }

    private static GridUpdate[] ParseGridUpdates(JToken updatesArray)
    {
        var updates = new GridUpdate[updatesArray.Count()];

        for (int i = 0; i < updatesArray.Count(); i++)
        {
            var jsonData = updatesArray[i]["json_data"];
            if (jsonData == null)
                continue;

            updates[i] = JsonUtility.FromJson<GridUpdate>(jsonData.ToString());
        }

        return updates;
    }

    #endregion
}
