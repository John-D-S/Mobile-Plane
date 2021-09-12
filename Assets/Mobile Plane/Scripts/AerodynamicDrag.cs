using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AerodynamicDrag : MonoBehaviour
{
    [SerializeField] private Vector3 directionalDragFactor = Vector3.zero;

    private Rigidbody rb;    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void ApplyAerodynamicDrag()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        Vector3 dragForce = Vector3.zero;
        dragForce.x = - directionalDragFactor.x * localVelocity.x;
        dragForce.y = - directionalDragFactor.y * localVelocity.y;
        dragForce.z = - directionalDragFactor.z * localVelocity.z;
        dragForce = transform.TransformDirection(dragForce);
        Debug.DrawLine(transform.position, transform.position + dragForce, Color.red);
        Debug.DrawLine(transform.position, transform.position + rb.velocity, Color.green);
        rb.AddForce(dragForce, ForceMode.Acceleration);
    }

    private void FixedUpdate()
    {
        ApplyAerodynamicDrag();
    }
}
