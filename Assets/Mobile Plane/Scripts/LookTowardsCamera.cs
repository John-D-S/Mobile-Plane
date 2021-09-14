using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LookTowardsCamera : MonoBehaviour
{
    [SerializeField] private bool disappearWhenCameraNear;
    [SerializeField] private Vector2 cameraNearRange;
    
    private SpriteRenderer spriteRenderer = new SpriteRenderer();
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Camera.current)
        {
            transform.rotation = Quaternion.LookRotation(Camera.current.transform.position - transform.position, Vector3.up);
            float cameraDistance = Vector3.Distance(transform.position, Camera.current.transform.position);
            float transparency = Mathf.Lerp(0, 1, (cameraDistance - cameraNearRange.x) / (cameraNearRange.y - cameraNearRange.x));
            spriteRenderer.color = new Color(1, 1, 1, transparency);
        }
    }
}
