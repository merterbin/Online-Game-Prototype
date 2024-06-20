using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject PlayerPrefab;
    public GameObject player1Position;
    public GameObject player2Position;

    private GameObject player1;
    private GameObject player2;
    public string youAre;

    public bool player1Have;
    public bool player2Have;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        


    }

    public void GameHandler(WsResponse res)
    {
        if(res.Type == "login")
        {
            if(res.Status == "success")
            {
                addPlayer(res.Player);
            }
        }
        if(res.Type == "move")
        {
            if (res.Status == "success")
            {
                WsMovement move = new WsMovement();
                JsonUtility.FromJsonOverwrite(res.Data, move);
                movePlayer(move);
            }            
        }
    }

    public void movePlayer(WsMovement move)
    {
        if(move.Player == "player1")
        {
            if(move.Type == "walk")
            {
                player1.GetComponent<PlayerMovement>().Move(move.Direction);
            }  
        }
        else
        {
            if (move.Type == "walk")
            {
                player2.GetComponent<PlayerMovement>().Move(move.Direction);
            }

        }
    }


     public void addPlayer(string value)
     {
        if(value == "player1")
        {
         player1 =  Instantiate(PlayerPrefab, player1Position.transform.position,Quaternion.identity);
            player1.GetComponent<PlayerMovement>().isMe = true;
            player1Have = true;

            youAre = "player1";
        }
        else
        {
            player2 = Instantiate(PlayerPrefab, player2Position.transform.position, Quaternion.identity);
            player2Have = true;
            if (player1Have != true)
            {
                 player1 = Instantiate(PlayerPrefab, player1Position.transform.position, Quaternion.identity);
                player1.GetComponent<PlayerMovement>().isMe = false;
                player2.GetComponent<PlayerMovement>().isMe = true;
                player1Have = true;

                youAre = "player2";
            } 
        }
    }

}
