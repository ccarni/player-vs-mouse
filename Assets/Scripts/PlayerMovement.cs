using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Buttons")]

    public Animator animator;
    public KeyCode jumpButton;

    // references
    private Rigidbody2D rb;

    [Header("Enable/Disable")]
    public bool enableJump;
    public bool enableDoubleJump;
    public bool canJump;
    public bool canDoubleJump;

    [Header("Inputs")]
    private float x;
    private bool space, s;

    [Header("Speeds")]
    public float xAcceleration = 1000;
    public float xDecceleration = 200;

    public float xMaxSpeed = 8;
    public float yMaxSpeed = 40;

    public float xStoppingThreshold = 0.2f, yStoppingThreshold = 0.2f;

    [Header("Jumping")]
    private bool jumpAvailable;



    //ground check
    private bool grounded;
    public Transform groundChecker;
    public LayerMask whatIsGround;
    public float touchingGroundRadius;

    private bool jumping;
    public float jumpForce = 8f;

    //public float sideJumpTorque = 60;
    //public float topJumpTorque = 125;
    //public float sideJumpForce = 5;
    //public float topJumpForce = 10f;

    public float fallGravity = 1.5f;
    public float earlyFallGravity = 1f;

    public float coyoteTime = 2f;
    private float coyoteTimer;

    private bool extraJump = false;
    private bool canUseExtraJump = false;

    // Start is called before the first frame update.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame.
    void Update()
    {
        GetInput();
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    private void FixedUpdate()
    {
        XMove();
        YMove();
        Coyote();
        Jump();
        CheckGrounded();
    }
    
    private void GetInput()
    {
        grounded = Physics2D.OverlapCircle(groundChecker.position, touchingGroundRadius, whatIsGround);
        x = Input.GetAxisRaw("Horizontal");
        space = Input.GetKey(jumpButton);
    }

    private void XMove()
    {
        //move
        rb.AddForce(Vector2.right * x * xAcceleration, ForceMode2D.Force);

        if (Mathf.Abs(rb.velocity.x) <= xStoppingThreshold && x == 0)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        // slow down
        if ((x == 0 || Mathf.Sign(x) != Mathf.Sign(rb.velocity.x)) && Mathf.Abs(rb.velocity.x) > xStoppingThreshold)
        {
            rb.AddForce(new Vector2(-rb.velocity.normalized.x * xDecceleration, 0f));
        }



        //clamp speed
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -xMaxSpeed, xMaxSpeed), Mathf.Clamp(rb.velocity.y, -yMaxSpeed, yMaxSpeed));

    }

    private void YMove() 
    {
        // snappy falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallGravity * Time.deltaTime;
        } 
        else if (rb.velocity.y > 0 && !space)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * earlyFallGravity * Time.deltaTime;

        }   

    }

    private void Jump()
    {

        if (space && enableJump && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumping = true;
            coyoteTimer = 0f;

        }
        else if (space && enableJump && enableDoubleJump && canUseExtraJump)
        {
            extraJump = false;
            canUseExtraJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumping = true;
            coyoteTimer = 0f;
        }
        
    }

    private void Coyote()
    {

        if (grounded)
        {
            extraJump = true;
            canUseExtraJump = false;
        }

        if (extraJump && !space)
        {
            canUseExtraJump = true;
        }

        // coyote
        if (!grounded && coyoteTimer > 0)
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private void CheckGrounded()
    {
        if (grounded && rb.velocity.y < 0)
        {
            coyoteTimer = coyoteTime;
            jumping = false;
        }
    }

}