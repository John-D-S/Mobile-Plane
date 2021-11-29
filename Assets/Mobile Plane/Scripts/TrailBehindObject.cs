using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that causes the GameObject it is attatched to to trail behind another object
/// </summary>
public class TrailBehindObject : MonoBehaviour
{
    [SerializeField, Tooltip("The rigidbody to trail behind")] private Rigidbody targetRigidbody;
    private Transform targetTransform => targetRigidbody.transform;
    [SerializeField, Tooltip("How far to trail behind")] private float distanceToFollow = 5f;
    [SerializeField, Tooltip("how slow should the target be going before this object will automatically move behind it")] private float minVelocityBeforeRotateBehindTarget = 1f;
    [SerializeField, Tooltip("The lerp speed when the object is not moving")] private float zeroVelocityLerpSpeed = 0.1f;

    private void Start()
    {
        if(targetTransform)
        {
            transform.position = targetTransform.position - targetTransform.forward * distanceToFollow;
        }
    }

    private void FixedUpdate()
    {
        float rotLerpAmount = zeroVelocityLerpSpeed * Mathf.Clamp01( minVelocityBeforeRotateBehindTarget - targetRigidbody.velocity.magnitude / minVelocityBeforeRotateBehindTarget);
        transform.position = targetTransform.position + Quaternion.Lerp(Quaternion.identity, Quaternion.FromToRotation((transform.position - targetTransform.position).normalized, - targetTransform.forward), rotLerpAmount) * (transform.position - targetTransform.position);
        transform.position = targetTransform.position - (targetTransform.position - transform.position).normalized * distanceToFollow;
        transform.rotation = Quaternion.LookRotation((targetTransform.position - transform.position).normalized, Vector3.up);
    }
}
