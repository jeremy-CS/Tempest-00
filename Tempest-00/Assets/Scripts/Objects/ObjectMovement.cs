//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    // Rotation/Pivot Variables
    [Header("Rotation and Pivot Variables")]
    public bool isRotating = false;
    public bool isSelfRotating = false;
    public bool isTargetRotating = false;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private Vector3 rotatingAxis;
    [SerializeField] private GameObject pivotTarget;
    [SerializeField] private float pivotTargetSpeed;
    [SerializeField] private Vector3 pivotTargetAxis;

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
            Rotate();
    }

    private void Rotate()
    {
        //-- Rotate Object based on set preferences --//
        // Rotate around itself
        if (isSelfRotating) 
        {
            transform.Rotate(rotatingSpeed * Time.deltaTime * rotatingAxis);
        }

        // Rotate around a target
        if (isTargetRotating && pivotTarget != null)
        {
            transform.RotateAround(pivotTarget.transform.position, pivotTargetAxis, pivotTargetSpeed * Time.deltaTime); 
        }
    }

    private void Translate()
    {

    }

    private void Oscillate()
    {

    }
}
