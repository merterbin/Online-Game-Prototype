using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMovement : MonoBehaviour
{
    public  Vector2 LastPosition;
    public  CloneMovement instance;
    public float speed = 5f;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LastPosition = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        float minDistance = 0.1f;

        Vector2 direction = (LastPosition - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(transform.position, LastPosition);

        if (distance > minDistance)
        {
            rb.velocity = direction * 5; // Yavaşça hızı artırmak için önceki hızı kullanın veya başka bir değer ekleyin.
        }
        else
        {
            rb.velocity = Vector2.zero; // Eğer hedefe ulaşıldıysa hızı sıfıra ayarlayın.
        }
    }

    private void LateUpdate()
    {
        //rb.velocity = (LastPosition - (Vector2)transform.position).normalized * 5;
        //if (Vector2.Distance(transform.position, LastPosition) < 0.1f)
        //{
        //    rb.velocity = (LastPosition - (Vector2)transform.position).normalized * 5;
        //}
    }

    void Awake()
    {
        // Player script'i, instance değişkenine değer olarak kendisini (this) veriyor
        instance = this;
    }
}
