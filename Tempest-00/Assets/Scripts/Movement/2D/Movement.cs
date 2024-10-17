//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    // Variables for Movement
    [Header ("Movement")]
    public float moveSpeed = 5f;

    // Variables for Dash
    [Header ("Dashing")]
    public float dashMultiplier = 1.5f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    public KeyCode dashKey = KeyCode.LeftShift;
    
    // Variables for Dash Timer
    private bool isDashing = false;
    private float dashTimer;
    private float dashCooldownTimer;

    // Variables for Jumping
    [Header ("Jumping")]
    public float jumpForce = 5f;
    public float doubleJumpForce = 3.5f;
    public KeyCode jumpKey = KeyCode.Space;
    public LayerMask groundLayer;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private bool isGrounded = false;

    // References
    [Header ("References")]
    private Rigidbody2D rigidBody;
    public Text isGroundedText;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigid body attached to the player
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement Calculations...
        MoveCalculation();
        DashCalculation();
        JumpCalculation();
    }

    private void FixedUpdate()
    {
        if (movement != null) 
        {
            Vector2 movementSpeed = movement * moveSpeed;

            if (isDashing)
            {
                //Dash
                movementSpeed *= dashMultiplier;

                //Stop Dash after fixed time elapsed
                if (Time.time >= dashTimer)
                {
                    isDashing = false;
                }
            }

            // Apply velocity to the rigidbody
            rigidBody.velocity = new Vector2(movementSpeed.x, rigidBody.velocity.y);
        }
    }

    public void MoveCalculation()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2 (moveX, moveY).normalized;
    }

    public void DashCalculation()
    {
        if (Input.GetKeyDown(dashKey) && !isDashing && Time.time >= dashCooldownTimer) 
        {
            isDashing = true;
            dashTimer = Time.time + dashTime;
            dashCooldownTimer = Time.time + dashCooldown;
        }
    }

    public void JumpCalculation()
    {
        CheckJump();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            // Jump
            Debug.Log("Jumping");
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            isDoubleJumping = false;
        }
        else if (Input.GetKeyDown(jumpKey) && !isGrounded && !isDoubleJumping)
        {
            // Double Jump
            Debug.Log("Double Jumping");
            rigidBody.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
            isJumping = false;
            isDoubleJumping = true;
        }
    }

    public void CheckJump()
    {
        // Visualization of groundCheck
        Debug.DrawRay(transform.position, Vector2.down * 0.2f, Color.red);

        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer);

        if (isGrounded) 
        {
            isJumping = false;
            isDoubleJumping = false;
        }

        isGroundedText.text = isGrounded ? "TRUE" : "FALSE";
    }
}
