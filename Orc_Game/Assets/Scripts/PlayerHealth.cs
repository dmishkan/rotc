using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public enum HealthState
    {
        Healthy,
        Hurt,
        Dying,
        Dead
    }
    
    public float invincibility = 1f;
    public Vector2 healthyRange;
    public Vector2 hurtRange;
    public Vector2 dyingRange;
    private float invincibilityStart;
    public HealthState healthState = HealthState.Healthy;
    public float maxHealth = 100f;
    private float health;
    private Rigidbody2D rb;
    private PlayerMovement _playerMovement;
    [SerializeField] private AudioClip hit = null;
    [SerializeField] private AudioClip dead = null;
    private AudioSource _audioSource;
    
    // Animator variables
    private bool isHurt = false;
    private bool isDead = false;
    public void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        // init invincibility
        invincibilityStart = invincibility;
        invincibility = 0f;
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        health = maxHealth;
    }

    private void LateUpdate()
    {
        invincibility -= Time.deltaTime;
    }

    public void TakeDamage(float dmg, Vector3 dir)
    {
        if (invincibility <= 0f)
        {
            rb.AddForce((dir));
            health -= dmg;

            if (health <= 0)
            {
                _playerMovement.PlayerDied();
                Die();
            }
            else
            {
                _audioSource.clip = hit;
                _audioSource.Play();
                _playerMovement.PlayerTookDamage();
            }
            if (healthyRange.x <= health && healthyRange.y >= health)
            {
                healthState = HealthState.Healthy;
            }
            else if (hurtRange.x <= health && hurtRange.y >= health)
            {
                healthState = HealthState.Hurt;
            }
            else if (dyingRange.x <= health && dyingRange.y >= health)
            {
                healthState = HealthState.Dying;
            }

            invincibility = invincibilityStart;
        }
    }

    private void Die()
    {
        _audioSource.clip = dead;
        _audioSource.Play();
        healthState = HealthState.Dead;
        _playerMovement.enabled = false;
    }
    
}
