//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    // Player and Camera References
    [Header("Player Position Reference")]
    [SerializeField] private Transform cameraTarget;

    // Camera Sensitivity Variables
    [Header("Camera Sensitivity")]
    public float cameraSensitivity = 3.0f;

    // Camera Settings Variables
    [Header("Camera Settings")]
    [SerializeField] private float distanceFromTarget = 3.0f;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector2 rotationXMinMax = new Vector2(-40, 40);
    [SerializeField] private bool invertXAxis = false;
    [SerializeField] private bool invertYAxis = true;

    // Camera Control Reference
    [Header("Camera Control")]
    public CameraControl cameraControl;

    // Other Camera Variables for computation
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    private float rotationX;
    private float rotationY;

    // Update is called once per frame
    void Update()
    {
        if (cameraControl.isThirdPerson)
        {
            GatherCameraInput();
            RotateCamera();
        }
    }

    private void GatherCameraInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;

        // Inverting Rotations based on preferences
        if (invertXAxis)
        {

            rotationY += mouseX * -1;
        }
        else
        {
            rotationY += mouseX;
        }

        if (invertYAxis)
        {
            rotationX += mouseY * -1;
        }
        else
        {
            rotationX += mouseY;
        }

        // Apply clamping for x rotation 
        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);
    }

    private void RotateCamera()
    {
        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        // Apply damping between rotation changes
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = cameraTarget.position - transform.forward * distanceFromTarget;
    }
}
