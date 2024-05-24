using System;
using UnityEngine;

public class Playermovement : MonoBehaviour
{

    private Rigidbody2D rb;

    private Animator anim;

    private float dirX;

    private float horizontal;

    private BoxCollider2D boxCol;


    private SpriteRenderer sprite;
    private bool canDash = true;
    private bool isDashing;
    private float activespeed;
    public float dashspeed;
    public float dashlength = .5f, dashcooldown = 1f;
    private float dashcounter;
    private float dashcoolcounter;

    private float dashingPower = 24f;
    private float dashTime = 0.2f;
    private KeyCode rollKey = KeyCode.V;
    private bool isVisible = false;


    private enum MovementState { idle, running, jumping, falling, double_jumping, wall_jumping, hurt, Roll }

    private MovementState state = MovementState.idle;

    [SerializeField] private int JumpForce = 15;
    [SerializeField] private int moveSpeed = 10;
    [SerializeField] private int ExtraJumps = 1;
    [SerializeField] private int MaxJumps = 1;
    [SerializeField] private int MaxRoll = 2;
    [SerializeField] private int ExtraRoll = 2;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallcheck;


    private float wallslidespeed = 2f;
    private bool walljumping;
    private float wallJumpCooldown;

    private float walljumpdirection;
    private float walljumptime = 20.2f;

    private float walljumpcounter;
    private float walljumpduration = 1f;
    private Vector2 walljumppower = new Vector2(20f, 20f);








    private void Jump()
    {
        if (isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
        else if (Wall() && !isGrounded())
        {
            if (horizontal == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            }
            else
            {
                wallJumpCooldown = 0;
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
        }
    }

    private void DoubleJump()
    {
        jumpSoundEffect.Play();

        rb.velocity = new Vector2(rb.velocity.x, JumpForce);

    }

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSoundEffect;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();


        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        activespeed = moveSpeed;
        Invoke("SpawnDelay", 1);
        sprite.enabled = false;
        ExtraRoll = MaxRoll;

        Invoke("ContinueExecution", 2f);

    }

    private void Roll()
    {



        if (dirX > -1f && ExtraRoll > 0)
        {
            anim.SetTrigger("Roll");
            Invoke("ContinueExecution", 2f);
            ExtraRoll = MaxRoll;
            ExtraRoll--;

            transform.position = transform.position + new Vector3(3, 0, 0);
            Invoke("SpawnDelay", 0.1f);

            sprite.flipX = false;
        }


        if (dirX > 0f && ExtraRoll > 0)
        {
            anim.SetTrigger("Roll");
            Invoke("ContinueExecution", 2f);

            ExtraRoll = MaxRoll;
            ExtraRoll--;

            transform.position = transform.position + new Vector3(3, 0, 0);
            Invoke("SpawnDelay", 1);

            sprite.flipX = false;


        }
        else if (dirX < 0 && ExtraRoll > 0)
        {
            Invoke("ContinueExecution", 2f);
            ExtraRoll = MaxRoll;
            ExtraRoll--;

            sprite.flipX = true;
            transform.position = transform.position + new Vector3(-3, 0, 0);


        }
        else
        {


        }


    }

    void ContinueExecution()
    {

    }
    // Update is called once per frame

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        //Flip player when moving left-right

        if (Input.GetKeyDown(rollKey))
        {
            Roll();
        }






        if (Input.GetKeyDown(rollKey))
        {
            if (dashcoolcounter <= 0 && dashcounter <= 0 && ExtraRoll>0)
            {
                activespeed = dashspeed;
                dashcounter = dashlength;
            }
        }
        if (dashcounter > 0 && ExtraRoll > 0)
        {
            dashcounter -= Time.deltaTime;
            if (dashcounter <= 0)
            {
                activespeed = moveSpeed;
                dashcoolcounter = dashcooldown;
            }
            if (dashcoolcounter > 0)
            {
                dashcoolcounter -= Time.deltaTime;
            }
        }
        if (dashcoolcounter > 0 && ExtraRoll > 0)
        {
            dashcoolcounter -= Time.deltaTime;
        }

        dirX = Input.GetAxisRaw("Horizontal");
        UpdateAnimationState();



        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Jump();
            anim.SetBool("grounded", isGrounded());


        }

        else if (Input.GetButtonDown("Jump") && ExtraJumps > 0)
        {
            ExtraJumps--;
            DoubleJump();
        }

        if (isGrounded())
        {
            ExtraJumps = MaxJumps;
        }


        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Jump();
            anim.SetBool("grounded", isGrounded());
        }
        else if (Input.GetButtonDown("Jump") && ExtraJumps > 0)
        {
            ExtraJumps--;
            DoubleJump();
        }

      





        if (wallJumpCooldown > 0.2f)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            if (Wall() && !isGrounded())
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallslidespeed, float.MaxValue));
            }
            else
            {
                rb.gravityScale = 3;
            }

            if (Input.GetButtonDown("Jump") && Wall())
            {
                wallJumpCooldown = 0;
                walljumping = true;
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * walljumppower.x, walljumppower.y);
                sprite.flipX = !sprite.flipX;
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }








    }

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
        if (Wall() == true)
        {
            sprite.flipX = false;

            state = MovementState.wall_jumping;

        }





        anim.SetInteger("State", (int)state);


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            state = MovementState.wall_jumping;
        }
    }



    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool Wall()
    {
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(transform.localScale.x, 0), -0.1f, wallLayer);
        if (raycastHitLeft.collider != null || raycastHitRight.collider != null) { return true; }
        else
        {
            return false;
        }





    }

}

/*Finish*/ 