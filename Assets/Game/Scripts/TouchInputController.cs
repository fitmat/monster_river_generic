using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RunnerGame
{
    public class TouchInputController : MonoBehaviour
    {
		public float DEADZONE = 50f;

        public const float MAX_SWIPE_TIME = 0.5f;
        public const float MIN_SWIPE_DISTANCE = 0.17f;

        public static TouchInputController Instance { set; get; }

		private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown, doubleTap;
		private Vector2 swipeDelta, startTouch;

        private Vector2 fingerDownPos;
        private Vector2 fingerUpPos;

        public bool detectSwipeAfterRelease = false;

        public float SWIPE_THRESHOLD = 20f;

        public bool Tap { get { return tap; } }

		public bool SwipeLeft { get { return swipeLeft; } }

		public bool SwipeRight { get { return swipeRight; } }

		public bool SwipeUp { get { return swipeUp; } }

		public bool SwipeDown { get { return swipeDown; } }

		public bool DoubleTap { get { return doubleTap; } }

        private void OnEnable()
        {
			SwipeManager.OnDoubleTap += OnDoubleTap;
            //SwipeEvents.OnSwipeUp += MoveUp;
            //SwipeEvents.OnSwipeDown += MoveDown;
            //SwipeEvents.OnSwipeRight += MoveRight;
            //SwipeEvents.OnSwipeLeft += MoveLeft;
        }

        private void OnDisable()
        {
			SwipeManager.OnDoubleTap -= OnDoubleTap;
            //SwipeEvents.OnSwipeUp -= MoveUp;
            //SwipeEvents.OnSwipeDown -= MoveDown;
            //SwipeEvents.OnSwipeRight -= MoveRight;
            //SwipeEvents.OnSwipeLeft -= MoveLeft;
        }

        private void Awake()
		{
			Instance = this;
		}

        private void Update()
        {
            // Reset all the booleans
            tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

            // Check for inputs
            #region Standalone Inputs
            if (Input.GetMouseButtonDown(0))
            {
                tap = true;
                startTouch = (Vector2)Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                startTouch = swipeDelta = Vector2.zero;
            }
            #endregion

            #region Mobile Inputs
            if (Input.touches.Length != 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    tap = true;
                    startTouch = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                {
                    startTouch = swipeDelta = Vector2.zero;
                }
            }
            #endregion

            //Calculate distance
            swipeDelta = Vector2.zero;
            if (startTouch != Vector2.zero)
            {

                // Check standalone
                if (Input.GetMouseButton(0))
                {
                    swipeDelta = (Vector2)Input.mousePosition - startTouch;
                }

                // Check mobile
                if (Input.touches.Length != 0)
                {
                    swipeDelta = Input.touches[0].position - startTouch;
                }

                // Check if byond deadzone
                if (swipeDelta.magnitude > DEADZONE)
                {
                    // Complete swipe
                    float x = swipeDelta.x;
                    float y = swipeDelta.y;

                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        // Horizontal
                        if (x < 0)
                        {
                            swipeLeft = true;
                        }
                        else
                        {
                            swipeRight = true;
                        }
                    }
                    else
                    {
                        // Vertical
                        if (y < 0)
                        {
                            swipeDown = true;
                        }
                        else
                        {
                            swipeUp = true;
                        }
                    }

                    startTouch = swipeDelta = Vector2.zero;
                }
            }

            //    foreach (Touch touch in Input.touches)
            //    {
            //        if (touch.phase == TouchPhase.Began)
            //        {
            //            fingerUpPos = touch.position;
            //            fingerDownPos = touch.position;
            //        }

            //        //Detects Swipe while finger is still moving on screen
            //        if (touch.phase == TouchPhase.Moved)
            //        {
            //            if (!detectSwipeAfterRelease)
            //            {
            //                fingerDownPos = touch.position;
            //                DetectSwipe();
            //            }
            //        }

            //        //Detects swipe after finger is released from screen
            //        if (touch.phase == TouchPhase.Ended)
            //        {
            //            fingerDownPos = touch.position;
            //            DetectSwipe();
            //        }
            //    }

            //}

            //void DetectSwipe()
            //{

            //    if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
            //    {
            //        Debug.Log("Vertical Swipe Detected!");
            //        if (fingerDownPos.y - fingerUpPos.y > 0)
            //        {
            //            OnSwipeUp();
            //        }
            //        else if (fingerDownPos.y - fingerUpPos.y < 0)
            //        {
            //            OnSwipeDown();
            //        }
            //        fingerUpPos = fingerDownPos;

            //    }
            //    else if (HorizontalMoveValue() > (SWIPE_THRESHOLD / 3f) && HorizontalMoveValue() > VerticalMoveValue())
            //    {
            //        Debug.Log("Horizontal Swipe Detected!");
            //        if (fingerDownPos.x - fingerUpPos.x > 0)
            //        {
            //            OnSwipeRight();
            //        }
            //        else if (fingerDownPos.x - fingerUpPos.x < 0)
            //        {
            //            OnSwipeLeft();
            //        }
            //        fingerUpPos = fingerDownPos;

            //    }
            //    else
            //    {
            //        Debug.Log("No Swipe Detected!");
            //    }
        }

        float VerticalMoveValue()
        {
            return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
        }

        float HorizontalMoveValue()
        {
            return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
        }

        void OnSwipeUp()
        {
            swipeUp = true;
        }

        void OnSwipeDown()
        {
            swipeDown = true;
        }

        void OnSwipeLeft()
        {
            swipeLeft = true;
        }

        void OnSwipeRight()
        {
            swipeRight = true;
        }

        private void OnDoubleTap()
		{
			doubleTap = true;
		}

		private void MoveUp()
		{
			swipeUp = true;
		}

		private void MoveDown()
		{
			swipeDown = true;
		}

		private void MoveRight()
		{
			swipeRight = true;
		}

		private void MoveLeft()
		{
			swipeLeft = true;
		}
	}
}
