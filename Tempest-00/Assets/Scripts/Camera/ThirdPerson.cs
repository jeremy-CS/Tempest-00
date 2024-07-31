using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    // Player and Camera References
    public Transform cameraPosition;
    public Transform playerPosition;

    // Camera Sensitivity
    public float cameraSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        cameraPosition.forward = Vector3.Slerp(cameraPosition.forward, playerPosition.forward, Time.deltaTime * cameraSensitivity);
    }
}
