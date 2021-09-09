using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneController : MonoBehaviour
{
	[SerializeField] private Transform cameraTransform;
	[SerializeField] private float maxTurnAngle = 80;
	[SerializeField] private float torque = 1;
	[SerializeField] private float force = 5;

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void ApplyControlTorque(Vector2 _axis)
	{
		//Debug.Log(_axis);
		if(_axis.magnitude > 0.001)
		{
			//calculating the rotation:
			Vector3 targetRotUpdirection = cameraTransform.rotation * new Vector3(-_axis.x, -_axis.y);
			Debug.DrawLine(transform.position, transform.position + targetRotUpdirection, Color.green);
			Vector3 targetRotForwardDirection = (cameraTransform.rotation * Quaternion.Euler(_axis.y * maxTurnAngle, _axis.x * -maxTurnAngle, 0)) * Vector3.forward;
			Quaternion targetRot = Quaternion.LookRotation(targetRotForwardDirection, targetRotUpdirection);
			
			//torque, with help from https://answers.unity.com/questions/171859/figuring-out-the-correct-amount-of-torque-to-apply.html:
			Vector3 torqueF = OrientTorque(Quaternion.FromToRotation(transform.forward, targetRot * Vector3.forward).eulerAngles);
			Vector3 torqueR = OrientTorque(Quaternion.FromToRotation(transform.right, targetRot * Vector3.right).eulerAngles);
			Vector3 torqueU = OrientTorque(Quaternion.FromToRotation(transform.up, targetRot * Vector3.up).eulerAngles);
			
			Vector3 targetTorque = torqueF + torqueR + torqueU;
            
			rb.AddTorque(targetTorque * torque);
		}
	}
	
	private Vector3 OrientTorque(Vector3 _torque)
	{
		// Quaternion's Euler conversion results in (0-360)
		// For torque, we need -180 to 180.
 
		return new Vector3
		(
			180.0f < _torque.x ? _torque.x - 360.0f : _torque.x,
			180.0f < _torque.y ? _torque.y - 360.0f : _torque.y,
			180.0f < _torque.z ? _torque.z - 360.0f : _torque.z
		);
	}
	
	private void FixedUpdate()
	{
		rb.AddForce(transform.forward * force);
		if(TouchInputHandler.theTouchInputHandler)
		{
			ApplyControlTorque(TouchInputHandler.theTouchInputHandler.Axis);
		}
	}
}
