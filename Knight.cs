using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections),typeof(Damageable))]

public class Knight : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public float followRange = 10f;  // Range within which the knight will start following
    public DetectionZonee attackZone;
    public DetectionZonee cliffDetectionZone;
    public Transform player; // Reference to the player's transform

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;

    public enum WalkableDirection { Right, Left }

    public WalkableDirection _walkDirection;

    public Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);

        }
    }

    public int AttackCooldown { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is within follow range
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        HasTarget = distanceToPlayer <= followRange; // Start following if player is within range

        if (distanceToPlayer > followRange)
        {
            HasTarget = false; // Stop following if player is out of range
        }

        if (AttackCooldown > 0)
        {

        }
    }

    private void FixedUpdate()
    {
        // If the knight has a target (the player), move toward the player
        if (HasTarget)

        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            if (touchingDirections.IsGrounded)
            {
                // Smooth movement towards the player
                rb.velocity = new Vector2(Mathf.Clamp(directionToPlayer.x * walkAcceleration * Time.fixedDeltaTime, -maxSpeed, maxSpeed), rb.velocity.y);

                // Flip the knight based on player position
                if (directionToPlayer.x > 0 && WalkDirection == WalkableDirection.Left)
                {
                    FlipDirection();
                }
                else if (directionToPlayer.x < 0 && WalkDirection == WalkableDirection.Right)
                {
                    FlipDirection();
                }
            }
        }
        else
        {
            // Knight stops moving when there's no target
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Unexpected walk direction.");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
