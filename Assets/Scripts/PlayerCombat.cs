using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Playercombat : MonoBehaviour
{
    private Rigidbody2D rb;

    public Animator animator;
    private Animator anim;
    private float horizontal;

    [SerializeField] private int JumpForce = 7;
    [SerializeField] private int moveSpeed = 10;
    [SerializeField] private int ExtraJumps = 1;
    [SerializeField] private int MaxJumps = 1;
    private KeyCode meleeAttackKey = KeyCode.T;
    private KeyCode heavyAttackKey = KeyCode.R;
    public Transform attackpoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public int attackdamage = 80;
    public int heavydamage = 100;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    private SpriteRenderer sprite;
    private float dirX;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxCol;


    private MovementState state = MovementState.idle;

    private enum MovementState { idle, running, jumping, falling, double_jumping, wall_jummping, hurt, Roll }



    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private void Jump()
    {
        if (isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
        else if (!isGrounded())
        {
            if (horizontal == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            }
            else
            rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
        }

    }
    void Update()
    { 
        if(Time.time > nextAttackTime) {
            if (Input.GetKeyDown(meleeAttackKey))
            {
                Attack();
                SlashSoundEffect.Play();

                nextAttackTime = Time.time + 1f/attackRate;
            }
            if (Input.GetKeyDown(heavyAttackKey))
            {
                heavyattack();
                AttackSoundEffect.Play();

                nextAttackTime = Time.time + 1f/attackRate;
            }
            if (dirX > 0f)
            {
                state = MovementState.running;
                sprite.flipX = false;
                FlipAttackPoint(false);

            }
            else if (dirX < 0)
            {
                state = MovementState.running;
                sprite.flipX = true;
                FlipAttackPoint(true);
            }
        }

    }
    [Header("Audio")]
    [SerializeField] private AudioSource AttackSoundEffect;
    [SerializeField] private AudioSource SlashSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;

    private void UpdateAnimationState()
    {
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0)
        {
            sprite.flipX = true;
            state = MovementState.running;

        }
        else
        {
            state = MovementState.idle;
        }
        if (rb.velocity.y > .1f && ExtraJumps == 0)
        {
            state = MovementState.double_jumping;
        }
        else if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitenemies=Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitenemies)
        {
            enemy.GetComponent<Enemymovement>().TakeDamage(attackdamage);

        }
    }
    void heavyattack()
    {
        animator.SetTrigger("heavyattack");

        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitenemies)
        {
            enemy.GetComponent<Enemymovement>().TakeDamage(heavydamage);

        }
    }

    private void FlipAttackPoint(bool flip)
    {
        if (flip)
        {
           
            attackpoint.localScale = new Vector3(-4f, 1f, 1f); 
        }
        else
        {
            
            attackpoint.localScale = new Vector3(1f, 1f, 1f); 
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackpoint==null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackpoint.position, attackRange);
    }
}
