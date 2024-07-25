//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Reference Variables
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody playerRb;

    // Movement Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Sprite Faces")]
    // Sprite Variables (Might not be applicable)
    [HideInInspector] public Sprite frontFace;
    [HideInInspector] public Sprite backFace;
    [HideInInspector] public Sprite forwardFace;
    [HideInInspector] public Sprite backwardFace;

    // Player Variables
    private CharacterController playerController;
    private Vector3 moveDirection = Vector3.zero;
    [HideInInspector] public bool CanMove = true;


    // Start is called before the first frame update
    void Start()
    {
        //-- Lock Cursor --//
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //-- Move based on availability --//
        if (CanMove)
        {
            //-- Calculating Moving Direction and Speed --//
            bool IsRunning = Input.GetKey(KeyCode.LeftShift);
            float ChangeInX = Input.GetAxisRaw("Vertical");
            float ChangeInZ = Input.GetAxisRaw("Horizontal");
            Vector3 MovementDirection = new Vector3(ChangeInX, 0, ChangeInZ).normalized;

            Rotate(ChangeInX, ChangeInZ);
            //Walk(MovementDirection, IsRunning);
        }
    }

    private void Rotate(float x, float z)
    {
        //-- Rotate Player through the orientation empty obj --//
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        Vector3 inputDirection = orientation.forward * x + orientation.right * z;

        if (inputDirection != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    private void Walk(Vector3 Direction, bool IsRunning)
    {
        //-- Jump on command --//
        if (Input.GetButton("Jump"))
        {
            //Direction += Jump();
        }

        // Movement Speed Calculation
        float Speed = IsRunning ? runSpeed : moveSpeed;

        // Rotation Speed Calculation
        Quaternion ToRotation = Quaternion.LookRotation(Direction, Vector3.up);

        // Move and Rotate the player
        transform.rotation = Quaternion.RotateTowards(transform.rotation, ToRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
    }

    private Vector3 Jump()
    {
        // Jump

        // Double-Jump
        return Vector3.zero;
    }

    private void SpriteChanges()
    { 
        
    }
}
