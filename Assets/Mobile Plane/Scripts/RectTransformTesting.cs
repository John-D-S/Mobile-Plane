using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformTesting : MonoBehaviour
{
    [SerializeField] private RectTransform backgroundTransform;
    [SerializeField] private RectTransform handleTransform;
    [SerializeField] private Vector2 handleTransformPosition;
    private Vector2 Axis;

    private void OnValidate()
    {
        Vector2 handlePos = handleTransformPosition;
        handlePos = handlePos.magnitude < backgroundTransform.rect.width * 0.5f ? handlePos : backgroundTransform.rect.width * 0.5f * handlePos.normalized;
        handleTransform.anchoredPosition = handlePos;
    }
    
    private void Update()
    {
        bool mouseIsDownOnJoystick;
        if(Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Debug.Log(currentMousePosition);
            //Axis = Vector2.ClampMagnitude((currentMousePosition - JoystickCenter) / JoystickMaxWidth * 2, clampAmount);
            //Axis = (Axis.magnitude < deadzone) ? Vector2.zero : Axis;
            //handleTransform.anchoredPosition
        }
        else
        {
            Axis = Vector2.zero;
        }
    }
}
