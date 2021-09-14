using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehindObject : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRigidbody;
    private Transform targetTransform => targetRigidbody.transform;
    [SerializeField] private float distanceToFollow = 5f;
    [SerializeField] private float minVelocityBeforeRotateBehindTarget = 1f;
    [SerializeField] private float zeroVelocityLerpSpeed = 0.1f;

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
