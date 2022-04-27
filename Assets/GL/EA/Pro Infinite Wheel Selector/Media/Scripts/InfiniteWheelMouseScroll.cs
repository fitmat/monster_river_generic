/*******************************************************
 * 													   *
 * Asset:		 	Pro Infinite Wheel                 *
 * Script:		 	InfiniteWheelMouseScroll.cs  	   *
 *                                                     *
 * Page: https://www.facebook.com/NLessStudio/         *
 * 													   *
 *******************************************************/
using UnityEngine;

namespace InfiniteWheel
{
    public class InfiniteWheelMouseScroll : MonoBehaviour
    {

        public InfiniteWheelController wheelController;

        public float dragForce = 1f;


        private bool lastOrientation = false;
        private bool scrolling = false;
        private Vector2 slider;
        private Vector2 onClickDownPosition;
        private float[] trail;
        private bool multiTouchSwitch = false;

        Vector2 offset_mouse;

        private void Start()
        {
            trail = new float[2];
            //while the roulette is active, deactivates the multitouch
            if (Input.multiTouchEnabled)
            {
                Input.multiTouchEnabled = false;
                multiTouchSwitch = true;
            }
        }

        public bool IsScrolling() { return scrolling; }
        public bool IsLastOrientation() { return lastOrientation; }

        private void StartScroller()
        {
            onClickDownPosition = Input.mousePosition;
            trail[0] = trail[1] = GetMousePositionByConfig();

            scrolling = true;
            wheelController.StartScroll();
        }

        public void EndScroller()
        {
            lastOrientation = trail[1] < GetMousePositionByConfig();
            scrolling = false;
            slider = Vector2.zero;
            wheelController.EndScroll();
        }

        void Update()
        {

            if (Input.GetMouseButtonDown(0)) { StartScroller(); }
            if (Input.GetMouseButtonUp(0)) { EndScroller(); }

            if (scrolling)
            {
                slider.x = (Input.mousePosition.x - onClickDownPosition.x) / Screen.height;
                slider.y = (Input.mousePosition.y - onClickDownPosition.y) / Screen.height;
                offset_mouse = slider * dragForce * ((wheelController.premadeWheelConfig.invertir) ? -1f : 1f);

                saveTrial();
            }
        }

        void saveTrial()
        {
            if (trail[0] != GetMousePositionByConfig())
            {
                trail[1] = trail[0];
                trail[0] = GetMousePositionByConfig();
            }
        }

        public void ResetOffset() { offset_mouse = Vector2.zero; }
        public float GetOffsetX() { return offset_mouse.x; }
        public float GetOffsetY() { return offset_mouse.y; }

        public float GetOffsetByConfig()
        {
            switch (wheelController.premadeWheelConfig.typeScroll)
            {
                case TypeScroll.horizontal:
                    return offset_mouse.x;
                case TypeScroll.vertical:
                    return offset_mouse.y;
            }
            return 0;
        }

        public float GetMousePositionByConfig()
        {
            switch (wheelController.premadeWheelConfig.typeScroll)
            {
                case TypeScroll.horizontal:
                    return Input.mousePosition.x;
                case TypeScroll.vertical:
                    return Input.mousePosition.y;
            }
            return 0;
        }



        private void OnDestroy()
        {
            if (multiTouchSwitch)
                Input.multiTouchEnabled = true;
        }

    }
}