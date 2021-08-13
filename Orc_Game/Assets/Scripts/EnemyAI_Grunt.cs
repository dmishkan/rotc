using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Grunt : MonoBehaviour
{
    Rigidbody2D rb;

    BoxCollider2D hitbox;
    Transform wallDetector;
    Transform groundDetector;
    [HideInInspector]public float damage = 15;
    public float knockback = 1000f;
    public float normalMoveSpeed;
    public float chasingMoveSpeed;
    public float maxVelocity;
    public float jumpForce;
    public bool sawPlayer;
    public bool grounded;
    public float turnTime;
    public Vector2 randomJumpRange;
    private float time;
    private float randomJumpTimer;

    public enum Mode {Standing, StandAndTurn, Patrolling, Chasing};
    public Mode mode;
    public enum Direction {Left, Right};
    public Direction direction;
    [HideInInspector] public Animator _animator;


    // Unity Functions
    void Start()
    {
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        wallDetector = transform.GetChild(2);
        groundDetector = transform.GetChild(3);
        sawPlayer = false;
        grounded = true;
        randomJumpTimer = Random.Range(randomJumpRange.x,randomJumpRange.y);

        // All enemies start by standing, then adjust in Update()
        Stand();
    }


    void FixedUpdate() 
    {
        time += Time.deltaTime;
        
        // Test surroundings
        Collider2D groundInfo = Physics2D.OverlapCircle(groundDetector.position, 0.1f, LayerMask.GetMask("Ground"));
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetector.position, Vector2.left, 1, LayerMask.GetMask("Ground", "Enemy", "Spear"));
        if (direction == Direction.Right) {
            wallInfo = Physics2D.Raycast(wallDetector.position, Vector2.right, 1, LayerMask.GetMask("Ground"));
        }

        // Check if grounded to allow jumping
        if (groundInfo == true) {
            grounded = true;
        }
        else {
            grounded = false;
        }

        // Regardless of inital mode, follow and attack player forever if they are seen
        if (sawPlayer) {
            // Do a little white flash animation to show alert
            if (mode != Mode.Chasing) {
                //animator.;
                mode = Mode.Chasing;
            }
            else {
                Chasing(groundInfo, wallInfo);
            }
        }

        // If mode is Standing, do nothing here.
        // Stand in place, switching directions every X seconds
        if (mode == Mode.StandAndTurn) {
            StandAndTurn();
        }
        // Patrol, starting with a given direction, and change directions when seeing a wall/pit
        else if (mode == Mode.Patrolling) {
            Patrol(groundInfo, wallInfo);
        }
    }



    // Public Functions //
    public void SawPlayer()
    {
        sawPlayer = true;
    }

    public void CantSeePlayer()
    {
        sawPlayer = false;
    }
    

    public void TurnAround()
    {
        if (direction == Direction.Left) {
                transform.eulerAngles = new Vector3(0, 180, 0);
                direction = Direction.Right;
        }
        else {
            transform.eulerAngles = new Vector3(0, 0, 0);
            direction = Direction.Left;
        }
    }



    // Private Functions //
    private void Stand()
    {
        if (direction == Direction.Left) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }


    private void StandAndTurn() 
    {
        // Alternate between left and right directions at constant rate
        if (time >= turnTime) {
            TurnAround();
            time = 0;  
        }
    }


    private void Patrol(Collider2D groundInfo, RaycastHit2D wallInfo) 
    {
        // Move in a given direction
        transform.Translate(Vector2.left * normalMoveSpeed * Time.deltaTime);
        
        _animator.SetBool("isWalking", true);
        _animator.SetBool("isIdle", false);
        
        // If a pit or a wall is detected ahead, turn around
        if (groundInfo == false || wallInfo.collider == true) {
            TurnAround();
        }
    }


    private void Chasing(Collider2D groundInfo, RaycastHit2D wallInfo) 
    {
        _animator.SetBool("isWalking", true);
        _animator.SetBool("isIdle", false);
        
        if (Mathf.Abs(rb.velocity.x) < maxVelocity)
        {
            if (direction == Direction.Left) {
                rb.AddForce( new Vector2(-chasingMoveSpeed, 0f) * Time.deltaTime);
            }
            else {
                rb.AddForce( new Vector2(chasingMoveSpeed, 0f) * Time.deltaTime);
            }
        }
        if (Mathf.Abs(rb.velocity.y) > maxVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxVelocity * rb.velocity.normalized.y);
        }

        // Randomly jump a little while chasing
        if (grounded && randomJumpTimer < 0) {
            randomJumpTimer = Random.Range(randomJumpRange.x,randomJumpRange.y);
            rb.AddForce(Vector2.up * jumpForce);
        }
        else {
            randomJumpTimer -= Time.deltaTime;
        }

        // If a wall is detected ahead, try to jump (can cause enemies to fall into pits but it's funny)
        if (wallInfo.collider != null && wallInfo.collider.gameObject.CompareTag("Ground") && grounded) {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }
    

}
