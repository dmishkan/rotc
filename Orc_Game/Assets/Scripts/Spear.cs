using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public enum SpearState
    {
        Holding,
        Thrown,
        Landed,
        Returning
    };

    public SpearState spearState = SpearState.Landed;
    public PlayerMovement character;
    public float spearDamage;
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public ParticleSystem impactSystem;
    public float speedDivider;
    public float recoil;
    
    private void Start()
    {
        if (spearState == SpearState.Landed)
        {
            box.enabled = true;
        }
        else
        {
            box.enabled = false;
        }
    }

    private void Update()
    {
        if (this == character._spear)
        {
            if (Input.GetKey(KeyCode.Mouse1) && (spearState == SpearState.Thrown || spearState == SpearState.Returning) )
            {
                Vector3 dir = character.transform.position - transform.position;
                rb.velocity = rb.velocity.magnitude * dir.normalized;
                float AngleRad = Mathf.Atan2 (dir.y - transform.position.y, dir.x - transform.position.x);
                float angle = (180 / Mathf.PI) * AngleRad;
                /*
                transform.eulerAngles = new Vector3(0f, 0f, angle);
                */
                spearState = SpearState.Returning;

                transform.right = dir;
            }
            if (Input.GetKey(KeyCode.Mouse1) && spearState == SpearState.Landed)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                box.enabled = false;
                Vector3 dir = character.transform.position - transform.position;
                float AngleRad = Mathf.Atan2 (dir.y - transform.position.y, dir.x - transform.position.x);
                float angle = (180 / Mathf.PI) * AngleRad;
                /*
                transform.eulerAngles = new Vector3(0f, 0f, angle);
                */
                

                transform.right = dir;

                rb.velocity = dir.normalized * character.throw_force / speedDivider;
                spearState = SpearState.Returning;
                character.attackMode = PlayerMovement.AttackMode.Melee;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && (spearState == SpearState.Thrown || spearState == SpearState.Returning) )
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.velocity = Vector2.zero;
            spearState = SpearState.Landed;
            box.enabled = true;

            impactSystem.Play();

        }
        if (other.CompareTag("Enemy") && (spearState == SpearState.Thrown || spearState == SpearState.Returning))
        {
            //TODO: enemies take damage
            other.GetComponent<EnemyHealth>().TakeDamage(spearDamage);
            other.attachedRigidbody.AddForce(rb.velocity * 10f);
        }
        /*if (other.CompareTag("Player") && spearState == SpearState.Returning)
        {
            character.EquipSpear(this);
            Vector3 dir = character.transform.position - transform.position;
            character._rigidbody.AddForce(dir * recoil);
        }*/
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        // so it doesnt get stuck if thrown while inside a wall
        if (other.CompareTag("Ground") && (spearState == SpearState.Thrown || spearState == SpearState.Returning))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.velocity = Vector2.zero;
            spearState = SpearState.Landed;
            if (Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Player")))
            {
                box.enabled = false;
            }
            else
            {
                box.enabled = true;
            }
            impactSystem.Play();
        }
        if (other.CompareTag("Enemy") && (spearState == SpearState.Thrown || spearState == SpearState.Returning))
        {
            //TODO: enemies take damage
            other.GetComponent<EnemyHealth>().TakeDamage(spearDamage);
            other.attachedRigidbody.AddForce(rb.velocity * 10f);

        }        
        /*if (other.CompareTag("Player") && spearState == SpearState.Returning)
        {
            character.EquipSpear(this);
            Vector3 dir = character.transform.position - transform.position;
            character._rigidbody.AddForce(dir * recoil);

        }*/

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (box.enabled == false)
            {
                box.enabled = true;
            }
        }

    }
}
