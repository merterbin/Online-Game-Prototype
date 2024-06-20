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
    }


     public void addPlayer(string value)
    {
        if(value == "player1")
        {
            Instantiate(PlayerPrefab, player1Position.transform.position,Quaternion.identity);
            youAre = "player1";
            player1Have = true;
        }
        else
        {
            if(player1Have != true)
            {
                youAre = "player2";
                Instantiate(PlayerPrefab, player1Position.transform.position, Quaternion.identity);
                player1Have = true;
            }
            Instantiate(PlayerPrefab, player2Position.transform.position, Quaternion.identity);
            player2Have = true;

        }
    }

}
