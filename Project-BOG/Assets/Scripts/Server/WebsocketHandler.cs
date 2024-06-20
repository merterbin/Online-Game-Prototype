using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;

public class WebsocketHandler : MonoBehaviour
{
    static WebSocket websocket;

    private RoomHandler rh;
    private GameManager gameManager;

    [SerializeField] public string wsURL = "ws://localhost:8080/ws";
    [SerializeField] public bool isConnection = false;

    // Start is called before the first frame update
    async void Start()
    {
        //Game
        rh = GetComponent<RoomHandler>();
        gameManager = GetComponent<GameManager>();

        // Websocket
        websocket = new WebSocket(wsURL);

        websocket.OnOpen += () =>
        {
            isConnection = true;
            Debug.Log("Connection open!");
            rh.FindRoom();

        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            isConnection = false;
            rh.ExitRoom();
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            //Debug.Log("OnMessage! " + jsonString);
            
            WsResponse res = new WsResponse();
            JsonUtility.FromJsonOverwrite(jsonString, res);
            gameManager.GameHandler(res);
        };

        await websocket.Connect();
    }

    async void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif

    }

    public async void loginRoom(string value)
    {
        WsData wsData = new WsData();
        wsData.Type = "login";
        wsData.Data = value;
        await websocket.SendText(JsonUtility.ToJson(wsData));
    }

    public async void exitRoom(string value)
    {
        WsData wsData = new WsData();
        wsData.Type = "exit";
        wsData.Data = value;
        await websocket.SendText(JsonUtility.ToJson(wsData));
    }

    public async void playerMovement(string type, string direction)
    {
        WsMovement move = new WsMovement();
        move.RoomID = rh.RoomID;
        move.Player = gameManager.youAre;
        move.Type = type;
        move.Direction = direction;

        WsData wsData = new WsData();
        wsData.Type = "move";
        wsData.Data = JsonUtility.ToJson(move).ToString();

        await websocket.SendText(JsonUtility.ToJson(wsData));
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
