using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject PlayerPrefab;
    public GameObject player1Position;
    public GameObject player2Position;

    
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
            Debug.Log("Player 1 move");
            if (move.Type == "walk")
            {
                GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerMovement>().Move(move.Direction.ToString());
            }  
        }
        else
        {
       
            if (move.Type == "walk")
            {
                GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>().Move(move.Direction.ToString());
                Debug.Log("Player 2 move");

            }
        }
    }


     public void addPlayer(string value)
     {
        if(value == "player1")
        {
         GameObject player1 =  Instantiate(PlayerPrefab, player1Position.transform.position,Quaternion.identity);
            player1.GetComponent<PlayerMovement>().isMe = true;
            player1.tag = "Player1";
            player1Have = true;

            youAre = "player1";
        }
        else
        {
            GameObject player2 = Instantiate(PlayerPrefab, player2Position.transform.position, Quaternion.identity);
            player2.tag = "Player2";
            player2Have = true;
            if (player1Have != true)
            {
                GameObject player1 = Instantiate(PlayerPrefab, player1Position.transform.position, Quaternion.identity);
                player1.GetComponent<PlayerMovement>().isMe = false;
                player1.tag = "Player1";
                player2.GetComponent<PlayerMovement>().isMe = true;
                player1Have = true;

                youAre = "player2";
            }
            else
            {
                player2.GetComponent<PlayerMovement>().isMe = false;
            }
        }
    }

}
