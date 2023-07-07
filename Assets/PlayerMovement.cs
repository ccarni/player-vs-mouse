using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //CONSTANTS
    public KeyCode jumpButton;
    public KeyCode rightStrafeButton;
    public KeyCode leftStrafeButton;
    public KeyCode dashButton;

    public Rigidbody2D rigidBodyReference;
    public Transform transformReference;

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
    private bool playerIsTouchingGround;
    public Transform groundChecker;
    public float touchingGroundRadius;
    public LayerMask whatIsGround;

    private bool playerIsJumping;
    public float howLongPlayerCanJump;
    private float jumpTimer;
    //These are the checks we use for wall jumping and sliding.
    public bool enableWallSliding;
    private bool slidingOnWall;
    private bool playerIsTouchingWall;
    public Transform wallTouchChecker;
    public float touchingWallRadius;
    public LayerMask whatIsWall;
    public float wallSlidingSpeed;
    //Variables specifically for wall jumping, the wallsliding variables are also needed for this to work.
    public bool enableWallJumping;
    private bool isWallJumping;
    public float horizontalWallJumpForce;
    public float verticalWallJumpForce;
    public float howLongPlayerCanWallJump;
    private bool wallJumpingLeft;
    private bool wallJumpingRight;

    

    // Start is called before the first frame update.
    void Start()
    {
        
    }
    // Update is called once per frame.
    void Update()
    {
        playerIsTouchingWall = Physics2D.OverlapCircle(wallTouchChecker.position, touchingWallRadius, whatIsWall);
        //creates a circle that checks if the player is with in distance of the ground, if so set isTouchingGround to true
        playerIsTouchingGround = Physics2D.OverlapCircle(groundChecker.position, touchingGroundRadius, whatIsGround);
        //moves player right when they press d.
        if(Input.GetKey(rightStrafeButton)){
            if(transformReference.rotation.y != 0f && !isWallJumping){
                transformReference.Rotate(0f, -180f, 0f);
            }
            rigidBodyReference.velocity = new Vector2(horizontalPlayerSpeed, rigidBodyReference.velocity.y);      
        }
        //moves player left when they press a.
        if(Input.GetKey(leftStrafeButton)){
            rigidBodyReference.velocity = new Vector2(-horizontalPlayerSpeed, rigidBodyReference.velocity.y);
            if(transformReference.rotation.y == 0f && !isWallJumping)
                transformReference.Rotate(0f, 180f, 0f);
        }
        //stops player's movement when they press neither a or d.
        if(!Input.GetKey(leftStrafeButton) && !Input.GetKey(rightStrafeButton)){
            rigidBodyReference.velocity = new Vector2(0f, rigidBodyReference.velocity.y);  
        }
        //enables the character to jump when they press space and are on the ground.
        //also sets a timer for how long a player can jump, thi is usefull for the holding jump feature.
        if(Input.GetKeyDown(jumpButton) && playerIsTouchingGround){
            rigidBodyReference.velocity = new Vector2(rigidBodyReference.velocity.x, verticalPlayerSpeed);
            playerIsJumping = true;
            jumpTimer = howLongPlayerCanJump;
        }
        //this basically checks if the player is holding jump in the air, if so keep the player moving up, but if the jump timer runs out or they stop pressing jump send the player back down.
        //this lets the player control how high they jump by how long they press the button, but there is a limit.
        if(Input.GetKey(jumpButton) && playerIsJumping == true){
            
            if(jumpTimer > 0){
                rigidBodyReference.velocity = new Vector2(rigidBodyReference.velocity.x, verticalPlayerSpeed);
                jumpTimer -= Time.deltaTime;
            }
        }
        //if the player lets off the jump button, make them start falling.
        if(Input.GetKeyUp(jumpButton)){
            playerIsJumping = false;
        }
        //this makes the jump available when the player hits the ground, this makes it so the player can not infinitely dash midair, making platforming less complex.
        if(playerIsTouchingGround && dashIsAvailable == false){
            Invoke("setDashAvailable", dashCoolDown);
        }
        //this just checks if dashing is enabled, this makes dashing easily disablable through unity.
        if(enableDashing){
        //if the player stops dashing, reset the dash variables.
        if(dashTime <= 0){
            dashTime = 0;
            dashingLeft = false;
            dashingRight = false;
            playerIsDashing = false;
        //if the player is currently dashing right move the player right at the dash speed and move down the dash counter.
        } else if(dashingRight){
            if(Input.GetKey(jumpButton)){
                rigidBodyReference.velocity = new Vector2(playerDashSpeed, verticalDashSpeed);
            } else {
                rigidBodyReference.velocity = new Vector2(playerDashSpeed, rigidBodyReference.velocity.y);
            }
            dashTime -= Time.deltaTime;
        //same thing but for when the player dashes left.
        } else if(dashingLeft){
            if(Input.GetKey(jumpButton)){
                rigidBodyReference.velocity = new Vector2(-playerDashSpeed, verticalDashSpeed);
            } else {
                rigidBodyReference.velocity = new Vector2(-playerDashSpeed, rigidBodyReference.velocity.y);
            }
            dashTime -= Time.deltaTime;
        } else if(dashingUp){
            rigidBodyReference.velocity = new Vector2(rigidBodyReference.velocity.x, verticalDashSpeed);
            dashTime -= Time.deltaTime;
        }
        //checks if the dash button is pressed and it is available, if so set up some variables to make the dash work and run the first frame of the dash.
        if(Input.GetKeyDown(dashButton) && dashIsAvailable){
            dashIsAvailable = false;
            playerIsDashing = true;
            dashTime = playerDashTime;
        if(rigidBodyReference.velocity.x < 0){
            dashingLeft = true;
            if(Input.GetKey(jumpButton)){
                rigidBodyReference.velocity = new Vector2(-playerDashSpeed, verticalDashSpeed);
            } else {
                rigidBodyReference.velocity = new Vector2(-playerDashSpeed, rigidBodyReference.velocity.y);
            }
            dashTime -= Time.deltaTime;
        }  else if(Input.GetKey(jumpButton) && rigidBodyReference.velocity.x == 0){
            dashingUp = true;
            rigidBodyReference.velocity = new Vector2(rigidBodyReference.velocity.x, verticalDashSpeed);
            dashTime -= Time.deltaTime;
        } else {
            dashingRight = true;
            if(Input.GetKey(jumpButton)){
                rigidBodyReference.velocity = new Vector2(playerDashSpeed, verticalDashSpeed);
            } else{
                rigidBodyReference.velocity = new Vector2(playerDashSpeed, rigidBodyReference.velocity.y);
            }
            dashTime -= Time.deltaTime;
        }
        } //this closes the loop checking if the dash is available and setting it up.
        }//this closes the loop checking if dashing is enabled.

        if(playerIsTouchingWall && !playerIsTouchingGround && rigidBodyReference.velocity.x != 0){
            slidingOnWall = true;
        } else {
            slidingOnWall = false;
        }

        if(slidingOnWall && enableWallSliding){
            rigidBodyReference.velocity = new Vector2(rigidBodyReference.velocity.x, Mathf.Clamp(rigidBodyReference.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if(enableWallJumping){
        if(Input.GetKeyDown(jumpButton) && slidingOnWall){
            isWallJumping = true;
            Invoke("setWallJumpingToFalse", howLongPlayerCanWallJump);
            if(Input.GetKey(leftStrafeButton)){
                rigidBodyReference.velocity = new Vector2(horizontalWallJumpForce, verticalWallJumpForce);
                transformReference.Rotate(0f, -180f, 0f);
                wallJumpingRight = true;
            } else {
                rigidBodyReference.velocity = new Vector2(-horizontalWallJumpForce, verticalWallJumpForce);
                wallJumpingLeft = true;
                transformReference.Rotate(0f, 180f, 0f);
            }
        }

        if(isWallJumping){
            if(Input.GetKey(jumpButton)){
                if(wallJumpingRight){
                rigidBodyReference.velocity = new Vector2(horizontalWallJumpForce, verticalWallJumpForce);
            } else {
                rigidBodyReference.velocity = new Vector2(-horizontalWallJumpForce, verticalWallJumpForce);
            }
            } else {
                CancelInvoke("setWallJumpingToFalse");
                setWallJumpingToFalse(); 
            }
        }    
        }//This closes the loop checking if walljumping is enabled

    }//this closes the update function loop.

    public void setWallJumpingToFalse(){
        isWallJumping = false;
        wallJumpingLeft = false;
        wallJumpingRight = false;
    }

    public void setDashAvailable(){
        dashIsAvailable = true; 
    }

    //ROOM FOR OTHER FUNCTIONS

}//this closes the loop for the entire class.