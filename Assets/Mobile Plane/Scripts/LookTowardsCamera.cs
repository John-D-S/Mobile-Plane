using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a script that causes the gameobject to look towards the active camera
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class LookTowardsCamera : MonoBehaviour
{
    [SerializeField, Tooltip("Whether or not to dissappear when near the camera")] private bool disappearWhenCameraNear;
    [SerializeField, Tooltip("How close to the camera before the renderer starts fading")] private Vector2 cameraNearRange;
    
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
