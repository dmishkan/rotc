using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    private GameManager gm;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gm.lastCheckpoint = new Vector2(-1.5f, 2);
        gm.RestartLevel();
    }
}
