using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isMe = true;

    [SerializeField] KeyCode rightKey = KeyCode.D;
    [SerializeField] KeyCode lefttKey = KeyCode.A;

    public float speed = 5.0f;
    Rigidbody2D rb;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    public float jumpForce = 5.0f;

   
    private GameObject gameManagment;
    private bool isStay = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManagment = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }   
    }

    void Move()
    {
        if (isMe)
        {
            if (Input.GetKey(rightKey))
            {
                gameManagment.GetComponent<WebsocketHandler>().playerMovement("walk", "right", transform.position);
                //rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else if (Input.GetKey(lefttKey))
            {
                gameManagment.GetComponent<WebsocketHandler>().playerMovement("walk", "left", transform.position);
                //rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else
            {
                if (!isStay)
                {
                    gameManagment.GetComponent<WebsocketHandler>().playerMovement("walk", "stay", transform.position);
                }   
               //rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
   
    public void Move(string input)
    {
        if (input.ToLower() == "right")
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            isStay = false;
            gameManagment.GetComponent<WebsocketHandler>().playerMoved("walk", "right", transform.position);
        }
        else if (input.ToLower() == "left")
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            isStay = false;
            gameManagment.GetComponent<WebsocketHandler>().playerMoved("walk", "left", transform.position);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            isStay = true;
            gameManagment.GetComponent<WebsocketHandler>().playerMoved("walk", "stay", transform.position);
        }
    }

    void Jump()
    {

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    
    public bool isGrounded()
    {
        if(Physics2D.BoxCast(transform.position, boxSize, 0f,-transform.up,castDistance,groundLayer))
        {
            return true;
        }else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up* castDistance, boxSize);
    }
    }

