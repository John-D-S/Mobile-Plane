using System;
using System.Collections;
using System.Collections.Generic;
using TouchInput;
using UnityEngine;

/// <summary>
/// allows the player to control the plane using physics
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlaneController : MonoBehaviour
{
	[SerializeField] private Transform cameraTransform;
	[SerializeField] private float maxTurnAngle = 80;
	[SerializeField] private float torque = 1;
	[SerializeField] private float force = 5;

	[SerializeField] private List<ParticleSystem> exhaustParticles;

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	/// <summary>
	/// applies torque to reach the given axis
	/// </summary>
	private void ApplyControlTorque(Vector2 _axis)
	{
		//calculating the rotation:
		Vector3 targetRotUpdirection = Vector3.Lerp(cameraTransform.up, cameraTransform.rotation * new Vector3(-_axis.x, -_axis.y), _axis.magnitude);
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
	
	/// <summary>
	/// the euler amount of torque to rotate to the correct rotation
	/// </summary>
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
		if(Input.GetKey(KeyCode.Space))
		{
			rb.AddForce(transform.forward * force);
			for(int i = 0; i < exhaustParticles.Count; i++)
			{
				if(!exhaustParticles[i].isPlaying)
				{
					exhaustParticles[i].Play();
				}
			}
		}
		else
		{
			for(int i = 0; i < exhaustParticles.Count; i++)
			{
				if(exhaustParticles[i].isPlaying)
				{
					exhaustParticles[i].Stop();
				}
			}
		}
		if(TouchInputHandler.theTouchInputHandler)
		{
			ApplyControlTorque(TouchInputHandler.theTouchInputHandler.Axis);
		}
	}
}
