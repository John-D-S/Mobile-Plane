using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehindObject : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float distanceToFollow = 5f;

    private void Start()
    {
        if(targetTransform)
        {
            transform.position = targetTransform.position - targetTransform.forward * distanceToFollow;
        }
    }

    private void Update()
    {
        transform.position = targetTransform.position - (targetTransform.position - transform.position).normalized * distanceToFollow;
        transform.rotation = Quaternion.LookRotation((targetTransform.position - transform.position).normalized, Vector3.up);
    }
}
