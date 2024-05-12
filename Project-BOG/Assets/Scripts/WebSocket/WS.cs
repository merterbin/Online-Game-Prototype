using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using System;

public class WS : MonoBehaviour
{
    WebSocket websocket;
    public Transform transform;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080/ws");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };


        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };
            
        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
        };

        
        await websocket.Connect();
    }

    async void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif

        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            Debug.Log("SEND");
            await websocket.SendText("SES deneme 1 2 deneme 1 2");
        }
        if (Input.GetKeyDown(KeyCode.B) == true)
        {
            Debug.Log("SEND");
            await websocket.Send(new byte[] { Convert.ToByte(transform.position.x), Convert.ToByte(transform.position.y)});
        }
        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            Debug.Log("SEND");
            await websocket.SendText("Ping");
        }
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText("plain text message");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}

