﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //CONSTANTS

    public KeyCode jumpButton;
    public KeyCode spinStopButton = KeyCode.S;

    private Rigidbody2D rb;

    private float x;
    private bool space, s;


    public float xAcceleration = 1000;
    public float xDecceleration = 200;

    public float xMaxSpeed = 8;
    public float yMaxSpeed = 40;
    public float spinMaxSpeed = 900;

    //controls x and y speed when using wasd.
    public float horizontalPlayerSpeed;
    public float verticalPlayerSpeed;

    public bool enableDashing;
    public float playerDashSpeed;
    public float verticalDashSpeed;
    private bool dashIsAvailable;
    private float dashTime;
    public float playerDashTime;
    public float dashCoolDown;
    private bool playerIsDashing;
    private bool dashingRight;
    private bool dashingLeft;
    private bool dashingUp;

    //this part is being used to check for the ground.
    private bool grounded, rightGrounded, leftGrounded, topGrounded;
    private bool canJumpNormal, canJumpRight, canJumpLeft, canJumpTop;
    public Transform groundChecker, rightChecker, leftChecker, topChecker;
    public float touchingGroundRadius;
    public float touchingGroundSideRadius;
    public LayerMask whatIsGround;

    // jumping
    public float jumpForce = 700;
    private bool jumping, leftSpinJumping, rightSpinJumping;

    public float sideJumpTorque = 60;
    public float topJumpTorque = 125;
    public float sideJumpForce = 5;
    public float topJumpForce = 10f;

    public float fallGravity = 1.5f;
    public float earlyFallGravity = 1f;

    public float coyoteTime = 2f;
    public float coyoteTimer;

    // spinning & combos
    public float comboJumpBoost = 1;
    public float comboSpinBoost = 15;
    public float maxJumpBoost = 3;
    public float spinStopTime = 1f;

    // Start is called before the first frame update.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame.
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
        Coyote();
        Jump();
        CheckGrounded();
    }
    
    private void GetInput()
    {
        grounded = Physics2D.OverlapCircle(groundChecker.position, touchingGroundSideRadius, whatIsGround);
        rightGrounded = Physics2D.OverlapCircle(rightChecker.position, touchingGroundSideRadius, whatIsGround);
        leftGrounded = Physics2D.OverlapCircle(leftChecker.position, touchingGroundSideRadius, whatIsGround);
        topGrounded = Physics2D.OverlapCircle(topChecker.position, touchingGroundSideRadius, whatIsGround);

        x = Input.GetAxisRaw("Horizontal");
        space = Input.GetKey(jumpButton);
        s = Input.GetKey(spinStopButton);
        
    }

    private void Move()
    {
        rb.AddForce(Vector2.right * x * xAcceleration, ForceMode2D.Force);

        if (x == 0 || Mathf.Sign(x) != Mathf.Sign(rb.velocity.x))
        {
            rb.AddForce(new Vector2(-rb.velocity.normalized.x * xDecceleration, 0f));
        }
         
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -xMaxSpeed, xMaxSpeed), Mathf.Clamp(rb.velocity.y, -yMaxSpeed, yMaxSpeed));

        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -spinMaxSpeed, spinMaxSpeed);

        // falling
        if (rb.velocity.y < 0 && jumping)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallGravity * Time.deltaTime;
        } 
        else if (rb.velocity.y > 0 && !Input.GetKey(jumpButton) && jumping)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * earlyFallGravity * Time.deltaTime;
        }


        // stop spin
        if (s)
        {
           rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0, spinStopTime);
        }
    }

    private void Jump()
    {
        if (!space) return;


        if (canJumpNormal)
        {
            // rb.AddForce(transform.up * jumpForce, ForceMode2D.Force);
            float boost = 0;
            if (leftSpinJumping || rightSpinJumping)
            {
                boost += Mathf.Min(maxJumpBoost, comboJumpBoost * Mathf.Abs(rb.angularVelocity));
                rb.AddTorque(rb.angularVelocity * comboSpinBoost);
            }

            rb.velocity = new Vector2(rb.velocity.x, 8f + boost);
            jumping = true;

            coyoteTimer = 0f;
        }

        else if (canJumpLeft)
        {
            if (rb.velocity.x < 0)
            {
                rb.AddTorque(sideJumpTorque);
                leftSpinJumping = true;
            }

            else
            {
                rb.AddTorque(-sideJumpTorque);
                rightSpinJumping = true;
            }
            rb.AddForce(Vector2.up * sideJumpForce, ForceMode2D.Impulse);
            coyoteTimer = 0f;
        }

        else if (canJumpRight)
        {
            if (rb.velocity.x > 0)
            {
                rb.AddTorque(-sideJumpTorque);
                rightSpinJumping = true;
            }

            else
            {
                rb.AddTorque(sideJumpTorque);
                leftSpinJumping = true;
            }
            rb.AddForce(Vector2.up * sideJumpForce, ForceMode2D.Impulse);
            coyoteTimer = 0f;
        }

        else if (canJumpTop)
        {
            if (rb.velocity.x < 0)
            {
                rb.AddTorque(topJumpTorque);
                leftSpinJumping = true;
            }
            else
            {
                rb.AddTorque(-topJumpTorque);
                leftSpinJumping = true;
            }
            rb.AddForce(Vector2.up * topJumpForce, ForceMode2D.Impulse);
            coyoteTimer = 0f;
        }

        
    }

    private void Coyote()
    {

        if (grounded) canJumpNormal = true;
        else if (coyoteTimer <= 0) canJumpNormal = false;

        if (leftGrounded) canJumpLeft = true;
        else if (coyoteTimer <= 0) canJumpLeft = false;

        if (rightGrounded) canJumpRight = true;
        else if (coyoteTimer <= 0) canJumpRight = false;

        if (topGrounded) canJumpTop = true;
        else if (coyoteTimer <= 0) canJumpTop = false;

        
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private void CheckGrounded()
    {
        if ((grounded || leftGrounded || rightGrounded || topGrounded) && rb.velocity.y < 0)
        {
            coyoteTimer = coyoteTime;
            jumping = false;
            leftSpinJumping = false;
            rightSpinJumping = false;
        }
    }

    //ROOM FOR OTHER FUNCTIONS

}//this closes the loop for the entire class.