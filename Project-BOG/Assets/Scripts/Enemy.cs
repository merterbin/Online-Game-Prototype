using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public GameObject floatingTextPrefab;

    public void TakeDamage(int damage)
    {
        health -= damage;
        ShowFlotingText();

        if (health <= 0)
        {
            Die();


        }
    }
    void ShowFlotingText()
    {
        if (floatingTextPrefab && health > 0)
        {
            
            var floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
            floatingText.GetComponent<TextMesh>().text = health.ToString();
        }
    }
   
    public void Die()
    {
        Destroy(gameObject);
    }
}