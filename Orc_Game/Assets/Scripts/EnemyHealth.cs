using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
using Random = System.Random;
*/

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public ParticleSystem bloodSplatter;
    public ParticleSystem deathGush;
    
    private float death_time = 5f;
    private bool dead = false;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private EnemyAI_Grunt enemyScript;
    private AudioSource _audioSource;
    public List<AudioClip> audioClips;

    public AudioClip suchBrutality;
    public float suchBrutalityProbability;
    // Unity Functions //
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyScript = GetComponent<EnemyAI_Grunt>();
        _audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (dead) 
        {
            if (death_time <= 0) { Destroy(gameObject); }
            else { death_time -= Time.deltaTime; }
        }
    }

    // private void LateUpdate()
    // {
    //     enemyScript._animator.SetBool("isHurt", false);
    // }

    public void TakeDamage(float dmg)
    {
        int x = Random.Range(0, 2) / 2;

        _audioSource.clip = audioClips[x];
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        // enemyScript._animator.SetBool("isHurt", true);
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            dead = true;
            enemyScript.enabled = false;
            
            Die();
        }
    }


    private void Die()
    {
        if (Random.value < suchBrutalityProbability)
        {
            _audioSource.clip = suchBrutality;
            _audioSource.Play();
        }
    
        deathGush.Play();
        boxCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(Vector2.up * 500f);
        rb.AddTorque(180f);
    }
}
