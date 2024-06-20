using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] public int RoomID;
    private WebsocketHandler wsHandler;

    void Start()
    {
        wsHandler = GetComponent<WebsocketHandler>();
    }

    public void FindRoom()
    {
        StartCoroutine(Room());
    }

    public void ExitRoom()
    {
        StartCoroutine(exitRoom());
    }

    IEnumerator Room()
    {   
        WWWForm form = new WWWForm();
        form.AddField("id", 25);
        form.AddField("player1", "hellokity");
        form.AddField("player2", "batman");

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/room/find", form);
        yield return www.SendWebRequest();

        var data = www.downloadHandler.text;
        
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else {
            string id = data.ToString().Split("\"ID\":")[1].Split(",")[0];
            RoomID = Convert.ToInt32(id);
            wsHandler.loginRoom(data.ToString());
            Debug.Log("Room Finds!");
        }
    }

    IEnumerator exitRoom()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", 25);
        form.AddField("player1", "hellokity");
        form.AddField("player2", "batman");

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/room/exit/"+RoomID, form);
        yield return www.SendWebRequest();

        var data = www.downloadHandler.text;

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            wsHandler.exitRoom(data.ToString());
            Debug.Log("Room Finds!");
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
