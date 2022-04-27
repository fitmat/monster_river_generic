/*******************************************************
 * 													   *
 * Asset:		 	Pro Infinite Wheel                 *
 * Script:		 	InfiniteWheelController.cs  	   *
 *                                                     *
 * Page: https://www.facebook.com/NLessStudio/         *
 * 													   *
 *******************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteWheel
{
    [ExecuteInEditMode]
    public class InfiniteWheelController : MonoBehaviour
    {
        [Tooltip("Current index of the element, useful to perform operations on the current element.")]
        public int Index;

        [Tooltip("See the changes in real time.")]
        public bool UpdateOnEditor = false;

        [Space(10), Tooltip("Enable save of the last selection.")]
        public bool saveState = true;

        [Tooltip("Wheel identifier to save your last selection if saveState is enable.")]
        public string ID_WHEEL = "wheel_id";


        [Space(10)] [Tooltip("Wheel configuration, drag the prefab created here.")]
        public CurveConfig premadeWheelConfig;

        [Space(10)] public InfiniteWheelMouseScroll wheelMouseScroll;


        [HideInInspector] public float offset = 0;

        public float clearance_clicked = 0.02f;

        public float velocityRecoveryConf = 6;


        private bool isClickable = true;
        private float displacement = 0;
        private float offset_destination = 0;
        private float velocityRecovery = 1;

        public InfiniteWheelItem[] items;

        public void SetitemsList(List<InfiniteWheelItem> allButtons) {
            items = allButtons.ToArray();
        }

        private void Start()
        {


            Setting();

            CalculateInit();
        }

        private void Setting()
        {
            premadeWheelConfig.Setting();

            displacement = 2f / items.Length;
            //displacement = 2f / items.Count;
            velocityRecovery = velocityRecoveryConf / items.Length;
            //velocityRecovery = velocityRecoveryConf / items.Count;
        }

        private void CalculateInit()
        {
            int index = (saveState) ? PlayerPrefs.GetInt(ID_WHEEL, 0) : 0;

            offset = (0 - (displacement * index));
            offset_destination = offset;
        }


        public void StartScroll()
        {
            isClickable = true;
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                Setting();

                if (UpdateOnEditor)
                {
                    MoveByScroller();
                }

                return;
            }

            ValidateScroll();

            MoveByScroller();

            if (!wheelMouseScroll.IsScrolling())
            {
                offset = Mathf.MoveTowards(offset, offset_destination, Time.deltaTime * velocityRecovery);
            }
        }

        private void ValidateScroll()
        {
            if (isClickable)
            {
                if (Mathf.Abs(wheelMouseScroll.GetOffsetX()) > clearance_clicked ||
                    Mathf.Abs(wheelMouseScroll.GetOffsetY()) > clearance_clicked)
                {
                    isClickable = false;
                }
            }
        }

        private float GetOffsetByMouse()
        {
            return wheelMouseScroll.GetOffsetByConfig() / items.Length;
            //return wheelMouseScroll.GetOffsetByConfig() / items.Count;
        }


        private void MoveByScroller()
        {
            float position = offset + GetOffsetByMouse();
            for (int i = 0; i < items.Length; i++)
            //for (int i = 0; i < items.Count; i++)
            {
                premadeWheelConfig.EvaluatePosition(position, items[i].transform);

                premadeWheelConfig.EvaluateRotation(position, items[i].transform);

                premadeWheelConfig.EvaluateScale(position, items[i]);

                EvaluateActivateCollider(position, items[i], i);

                premadeWheelConfig.EvaluateActivateIten(position, items[i]);

                position += displacement;
            }
        }


        private void EvaluateActivateCollider(float position, InfiniteWheelItem item, int index)
        {
            if (Math.Abs(Mathf.Clamp01(Mathf.RoundToInt(premadeWheelConfig.colliderActivate.Evaluate(position))) - 1) < 0.01f)
            {
                if (isClickable || !wheelMouseScroll.IsScrolling())
                {
                    if (Application.isPlaying)
                    {
                        if (saveState)
                            PlayerPrefs.SetInt(ID_WHEEL, index);
                    }

                    item.ActivateCollider(true);
                    Index = index;
                }
                else
                {
                    item.ActivateCollider(false);
                }
            }
            else
            {
                item.ActivateCollider(false);
            }
        }


        public void EndScroll()
        {
            if (isClickable)
            {
                isClickable = false;
            }
            else
            {
                offset += GetOffsetByMouse();
                offset_destination = offset;

                var restForward = Mathf.Abs(offset % displacement);
                var restBack = displacement - (restForward);

                if (offset >= 0)
                {
                    if (wheelMouseScroll.IsLastOrientation())
                    {
                        offset_destination += restBack;
                    }
                    else
                    {
                        offset_destination -= restForward;
                    }
                }
                else
                {
                    if (!wheelMouseScroll.IsLastOrientation())
                    {
                        offset_destination -= restBack;
                    }
                    else
                    {
                        offset_destination += restForward;
                    }
                }
            }

            wheelMouseScroll.ResetOffset();
        }

        public void Next()
        {
            offset_destination -= displacement;
        }

        public void Back()
        {
            offset_destination += displacement;
        }
    }
}