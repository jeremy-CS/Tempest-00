//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Camera Objects
    [Header("Cameras")]
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    // Camera Positions
    [Header("Positions")]
    public Transform firstPersonCameraPosition;
    public Transform thirdPersonCameraPosition;

    // Camera Availability Variables
    [HideInInspector] public bool isCameraLocked = false;
    [HideInInspector] public bool isFirstPerson = true;
    [HideInInspector] public bool isThirdPerson = false;

    // Start is called before the first frame update
    void Start()
    {
        if (firstPersonCamera == null)
        {
            thirdPersonCamera.enabled = true;
            isCameraLocked = true;
        }
        else if (thirdPersonCamera == null)
        {
            firstPersonCamera.enabled = true;
            isCameraLocked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //-- Switch between first or third person perspective --//
        if (!isCameraLocked)
        {
            if (Input.GetKey(KeyCode.Alpha1) && !isFirstPerson)
            {
                ChangeToFirstPerson();
            }

            if (Input.GetKey(KeyCode.Alpha2) && !isThirdPerson)
            {
               ChangeToThirdPerson();
            }
        }

        //-- Move Camera based on Player location --//
        MoveCameras();
    }

    private void ChangeToFirstPerson()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        isFirstPerson = true;
        isThirdPerson = false;

        // Should add a transition to other camera instead of snapping
    }
    private void ChangeToThirdPerson()
    {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.enabled = true;
        isFirstPerson = false;
        isThirdPerson = true;
    }

    private void MoveCameras()
    {
        if (isFirstPerson)
        {
            transform.position = firstPersonCameraPosition.position;
        }
        else
        {
            transform.position = thirdPersonCameraPosition.position;
        }
    }
}
