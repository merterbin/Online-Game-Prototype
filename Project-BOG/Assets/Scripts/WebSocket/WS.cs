using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System;

public class WS : MonoBehaviour
{
    WebSocket websocket;
    public Transform transform;
    public GameObject player;
    public GameObject clone;

    Rigidbody2D rb;

    string queuedMessage;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080/ws");
        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
            Vector2 position = player.transform.position;
            string data = JsonUtility.ToJson(position);
            Debug.Log(position);
            websocket.SendText(data);
        };


        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Vector2 clonePosition = JsonUtility.FromJson<Vector2>(message);
            clonePosition = new Vector2(clonePosition.x + 15f, clonePosition.y);

            PlayerMovement cloneMove = clone.GetComponent<PlayerMovement>();
            cloneMove.Move();

            queuedMessage = message;
        };

        await websocket.Connect();
    }

    async void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif

        if (!string.IsNullOrEmpty(queuedMessage))
        {
            websocket.SendText(queuedMessage);
            queuedMessage = null;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Vector2 position = player.transform.position;
            string data = JsonUtility.ToJson(position);
            websocket.SendText(data);
        }
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.Send(new byte[] { 10, 20, 30 });
            await websocket.SendText("plain text message");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
