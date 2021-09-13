using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchInput
{
    public class TouchInputHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public static TouchInputHandler theTouchInputHandler;
        public Vector2 Axis { get; private set; } = Vector2.zero;

        private Vector2 ScreenSize => new Vector2(Screen.width, Screen.height);
        private Vector2 ScreenCenter => new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        private float JoystickMaxWidth => ScreenSize.x > ScreenSize.y ? ScreenSize.y : ScreenSize.x; 
        [SerializeField, Range(0, 1)] private float deadzone = .25f;
        [SerializeField, Range(0, 1)] private float clampAmount = 1;
        [SerializeField] private bool alwaysWork;

        public void Start()
        {
            theTouchInputHandler = this;
        }

        public void OnDrag(PointerEventData _eventData)
        {
            Debug.Log(_eventData.position);
            // Set the axis of the input according to the position of the touch on the screen.
            Axis = Vector2.ClampMagnitude((_eventData.position - ScreenCenter) / JoystickMaxWidth, JoystickMaxWidth);
            
            // Apply the deadzone effect after the handle has been placed
            // to prevent the handle from visually being stuck in the deadzone
            Axis = (Axis.magnitude < deadzone) ? Vector2.zero : Axis;
        }

        public void OnEndDrag(PointerEventData _eventData)
        {
            // We have let go so reset the axis and the position of the handle
            Axis = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData _eventData) => OnDrag(_eventData);
        public void OnPointerUp(PointerEventData _eventData) => OnEndDrag(_eventData);

        private void Update()
        {
            if(Input.GetMouseButton(0) || alwaysWork)
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Axis = Vector2.ClampMagnitude((currentMousePosition - ScreenCenter) / JoystickMaxWidth * 2, clampAmount);
                Axis = (Axis.magnitude < deadzone) ? Vector2.zero : Axis;
            }
            else
            {
                Axis = Vector2.zero;
            }
        }
    }
}
