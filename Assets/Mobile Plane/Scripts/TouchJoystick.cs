using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchInput
{
    public class TouchJoystick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform handle;
        private Vector3 initialHandlePosition = Vector2.zero;
        [SerializeField] private RectTransform background;

        public static TouchJoystick theTouchJoystick;
        public Vector2 Axis { get; private set; } = Vector2.zero;

        private Vector2 JoystickSize => new Vector2(background.rect.width, background.rect.height);
        private Vector2 JoystickCenter => background.rect.center;
        private float JoystickMaxWidth => JoystickSize.x > JoystickSize.y ? JoystickSize.y : JoystickSize.x; 
        [SerializeField, Range(0, 1)] private float deadzone = .25f;
        [SerializeField, Range(0, 1)] private float clampAmount = 1;

        private void OnValidate()
        {
            if(handle && background)
                handle.anchoredPosition = JoystickCenter;
        }

        public void Start()
        {
            theTouchJoystick = this;
            handle.anchoredPosition = JoystickCenter;
        }

        public void OnDrag(PointerEventData _eventData)
        {
            Debug.Log(_eventData.position);
            // Set the axis of the input according to the position of the touch on the screen.
            Vector2 currentMousePosition = Input.mousePosition;
            Axis = Vector2.ClampMagnitude((currentMousePosition - JoystickCenter) / JoystickMaxWidth * 2, clampAmount);
            Axis = (Axis.magnitude < deadzone) ? Vector2.zero : Axis;
            
            // Apply the deadzone effect after the handle has been placed
            // to prevent the handle from visually being stuck in the deadzone
            Axis = (Axis.magnitude < deadzone) ? Vector2.zero : Axis;
            handle.anchoredPosition = JoystickCenter + Axis * JoystickMaxWidth;
        }

        public void OnEndDrag(PointerEventData _eventData)
        {
            // We have let go so reset the axis and the position of the handle
            Axis = Vector2.zero;
            handle.anchoredPosition = JoystickCenter;
        }

        public void OnPointerDown(PointerEventData _eventData) => OnDrag(_eventData);
        public void OnPointerUp(PointerEventData _eventData) => OnEndDrag(_eventData);

        private void Update()
        {
            if(Input.GetMouseButton(0))
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Axis = Vector2.ClampMagnitude((currentMousePosition - JoystickCenter) / JoystickMaxWidth * 2, clampAmount);
                Axis = (Axis.magnitude < deadzone) ? Vector2.zero : Axis;
            }
            else
            {
                Axis = Vector2.zero;
            } 
            //Debug.Log(Axis);
        }
    }
}
