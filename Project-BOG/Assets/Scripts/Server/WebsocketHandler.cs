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
            Debug.Log("OnMessage! " + jsonString);
            
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

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("left");
            await websocket.SendText("left");
        }

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

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
