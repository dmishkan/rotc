using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    EnemyAI_Grunt gruntAI;

    void Start()
    {
        gruntAI = transform.parent.GetComponent<EnemyAI_Grunt>();
    }

    // Detect player in enemy's vision area
    void OnTriggerEnter2D (Collider2D col) 
    {
        // PlayerCharacter was given tag "Player" in editor
        if (col.CompareTag("Player")) 
        {
            // Activate follow mode if it sees the player, even if player leaves vision
            gruntAI.SawPlayer();
        }
    }

    // Detect player in enemy's vision area
    void OnTriggerExit2D (Collider2D col) 
    {
        if (col.CompareTag("Player")) 
        {
            gruntAI.TurnAround();
        }
    }
}
