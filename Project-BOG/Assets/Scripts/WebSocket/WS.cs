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

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080/ws");
        rb = clone.GetComponent<Rigidbody2D>();
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
            clone.transform.position = Vector2.MoveTowards(clone.transform.position, clonePosition, 5f);

            //clone.transform.position = new Vector2(clonePosition.x + 15f, clone.transform.position.y);
        };

            
        await websocket.Connect();
    }

    async void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ) {

           ;
            Vector2 position = player.transform.position;
            string data = JsonUtility.ToJson(position);
            Debug.Log(position);
            websocket.SendText(data);
           
            
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

