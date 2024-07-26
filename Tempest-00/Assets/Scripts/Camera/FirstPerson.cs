//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    // Camera Sensitivity Variables
    [Header("Sensitivity")]
    public float sensitivityX;
    public float sensitivityY;

    // Camera Rotation Variables
    float xRotation;
    float yRotation;

    // Player Orientation Variable
    [Header("Player and Camera Orientations")]
    public Transform cameraOrientation;
    public Transform playerOrientation;

    // Player Control Lock Variable
    bool canMove = true;

    // Camera Control Reference
    [Header("Camera Control")]
    public CameraControl cameraControl;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //-- Move Player according to movement lock --//
        if (canMove && cameraControl.isFirstPerson)
        {
            // Mouse input transformation
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamping rotation only on one axis
            yRotation += mouseX;

            // Apply rotation to camera and player
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            cameraOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }
}
