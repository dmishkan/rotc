using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    PlayerHealth playerHP;
    public float damage;
    public float knockback;


    void Start()
    {
        playerHP = GameObject.Find("PlayerCharacter").GetComponent<PlayerHealth>();
    }


    void OnCollisionStay2D (Collision2D col) 
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerHP.TakeDamage(damage, (transform.up) * knockback);
        }
    }
}
