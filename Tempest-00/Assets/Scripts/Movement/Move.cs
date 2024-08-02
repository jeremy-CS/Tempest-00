//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    // Reference Variables
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    private Rigidbody playerRb;
    [SerializeField] private Camera thirdPersonCamera;


    // Movement Variables
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rotationSpeed;

    // Jump Variables
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    public KeyCode jumpKey;
    bool readyToJump = true;
    //bool readyToDoubleJump = false;

    // Ground Check Variables
    [Header("Ground Checks")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask groundLayerMask;
    bool isGrounded;

    [Header("Sprite Faces")]
    // Sprite Variables (Might not be applicable)
    [HideInInspector] public Sprite frontFace;
    [HideInInspector] public Sprite backFace;
    [HideInInspector] public Sprite forwardFace;
    [HideInInspector] public Sprite backwardFace;

    // Camera Control Reference
    [Header("Camera Control")]
    public CameraControl cameraControl;

    // Debug Variables
    [Header("Debug")]
    public Text boolDebugText;

    // Player Variables
    private Vector3 moveDirection = Vector3.zero;
    [HideInInspector] public bool CanMove = true;

    // Input Variables
    float changeInZ = 0f;
    float changeInX = 0f;
    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        //-- Lock Cursor --//
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //-- Getting Player Rigid Body and locking its rotation --//
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //-- Move based on availability --//
        if (CanMove)
        {
            // Ground Check
            isGrounded = Physics.Raycast(orientation.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayerMask);
            //Debug Ground Check
            Debug.DrawRay(orientation.position, Vector3.down, Color.green);

            // Gather Player Inputs
            PlayerInputs();

            // Control Player Speed
            SpeedControl();

            //-- Calculating and applying Drag to player --//
            if (isGrounded)
            {
                playerRb.drag = groundDrag;
                readyToJump = true;
            }
            else
            {
                playerRb.drag = 0f;
                readyToJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            Rotate();
            Walk();
        }
    }

    private void PlayerInputs()
    {
        //-- Calculating Moving Direction and Speed --//
        isRunning = Input.GetKey(KeyCode.LeftShift);
        changeInZ = Input.GetAxisRaw("Vertical");
        changeInX = Input.GetAxisRaw("Horizontal");

        if (cameraControl.isFirstPerson)
        {
            moveDirection = orientation.forward * changeInZ + orientation.right * changeInX;
        }
        else 
        {
            moveDirection = Quaternion.Euler(0, thirdPersonCamera.transform.eulerAngles.y, 0) * new Vector3(changeInX, 0, changeInZ);
        }

        moveDirection = moveDirection.normalized;

        //-- Handling Jumping Input --//
        if (Input.GetKeyDown(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump(); // PROBLEM! can only jump once. Might be an issue with isGrounded or readyToJump values //

            Invoke("ResetJump", jumpCooldown);
        }

        //-- DEBUG MESSAGES --//
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
        }
        boolDebugText.text = isGrounded.ToString() + "\n" + readyToJump.ToString();
    }

    private void Rotate()
    {
        //-- Rotate Player through the orientation empty obj IF in third person mode --//
        if (cameraControl.isThirdPerson)
        {
            if (moveDirection != Vector3.zero)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

                playerObj.rotation = Quaternion.Slerp(playerObj.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void Walk()
    {
        // Movement Speed Calculation
        float speed = isRunning ? runSpeed : walkSpeed;

        // Move the player (note: 10f is used to have even more speed -> could be merged with speed)
        if (isGrounded)
        {
            playerRb.AddForce(moveDirection * speed * 10f, ForceMode.Force);
        }
        else // In the air movement
        {
            playerRb.AddForce(moveDirection * speed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        //-- Control Player Max Speed for Walking or Running --//
        if (!isRunning)
        {
            // Adjust player speed if exceeding max value
            if (flatVelocity.magnitude > walkSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * walkSpeed;
                playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
            }
        }
        else
        {
            // Adjust player speed if exceeding max value
            if (flatVelocity.magnitude > runSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * runSpeed;
                playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump()
    {
        // Reset Y-velocity to ensure consistency in jump height
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        //-- Jump --//
        playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        

        //-- Double-Jump --//
        /// TODO
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpriteChanges()
    { 
        
    }
}
