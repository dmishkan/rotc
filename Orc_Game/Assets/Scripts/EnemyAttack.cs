using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour
{
    EnemyAI_Grunt gruntAI;
    private PlayerHealth playerHP;
    private float damage;
    private float knockback;


    void Start()
    {
        gruntAI = transform.parent.GetComponent<EnemyAI_Grunt>();
        playerHP = GameObject.Find("PlayerCharacter").GetComponent<PlayerHealth>();
        damage = gruntAI.damage;
        knockback = gruntAI.knockback;
    }

    // private void LateUpdate()
    // {
    //     gruntAI._animator.SetBool("isAttacking", false);
    // }

    void OnTriggerEnter2D (Collider2D col) 
    {
        if (col.CompareTag("Player"))
        {
            // gruntAI._animator.SetBool("isAttacking", true);
            playerHP.TakeDamage(damage, ((-transform.right + transform.up).normalized) * knockback);
        }
    }
}
