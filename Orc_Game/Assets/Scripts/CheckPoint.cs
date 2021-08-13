using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gm;
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("isLit", true);
            gm.lastCheckpoint = transform.position;
        }
    }
}
