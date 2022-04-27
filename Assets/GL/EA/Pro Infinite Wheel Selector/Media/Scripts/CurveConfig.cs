/*******************************************************
 * 													   *
 * Asset:		 	Pro Infinite Wheel                 *
 * Script:		 	CurveConfig.cs  				   *
 *                                                     *
 * Page: https://www.facebook.com/NLessStudio/         *
 * 													   *
 *******************************************************/
using UnityEngine;

namespace InfiniteWheel
{
    public enum TypeScroll { horizontal, vertical }

    public class CurveConfig : MonoBehaviour
    {
        [Header("Behaviour")]
        [Header("Position")]
        public AnimationCurve position_x;
        [Range(-30, 30)]
        public float amplitud_x = 10;
        public AnimationCurve position_y;
        [Range(-30, 30)]
        public float amplitud_y = 10;
        public AnimationCurve position_z;
        [Range(-30, 30)]
        public float amplitud_z = 10;

        [Header("Rotation")]
        public bool enableItemRotation = false;
        public AnimationCurve rotation_x;
        public AnimationCurve rotation_y;
        public AnimationCurve rotation_z;

        [Header("Scale")]
        public bool enableItemScale = false;
        public AnimationCurve itemScale;

        [Header("Collider Activation")]
        public AnimationCurve colliderActivate;

        [Header("Item Activation")]
        public bool enableItemActivate = false;
        public AnimationCurve itemActivate;

        [Header("Scroll Orientation")]
        public TypeScroll typeScroll = TypeScroll.horizontal;
        public bool invertir = false;


        public void Setting()
        {
            var wrap_mode = WrapMode.Loop;

            position_x.preWrapMode = position_x.postWrapMode = wrap_mode;
            position_y.preWrapMode = position_y.postWrapMode = wrap_mode;
            position_z.preWrapMode = position_z.postWrapMode = wrap_mode;

            rotation_x.preWrapMode = rotation_x.postWrapMode = wrap_mode;
            rotation_y.preWrapMode = rotation_y.postWrapMode = wrap_mode;
            rotation_z.preWrapMode = rotation_z.postWrapMode = wrap_mode;

            itemScale.preWrapMode = itemScale.postWrapMode = wrap_mode;

            colliderActivate.preWrapMode = colliderActivate.postWrapMode = wrap_mode;
            itemActivate.preWrapMode = itemActivate.postWrapMode = wrap_mode;
        }

        public void EvaluatePosition(float position, Transform item)
        {
            item.localPosition = new Vector3(
                                      position_x.Evaluate(position) * amplitud_x,
                                      position_y.Evaluate(position) * amplitud_y,
                                      position_z.Evaluate(position) * amplitud_z
                                      );
        }

        public void EvaluateRotation(float position, Transform iten)
        {
            if (enableItemRotation)
                iten.localEulerAngles = new Vector3(
                                      rotation_x.Evaluate(position) * 360f,
                                      rotation_y.Evaluate(position) * 360f,
                                      rotation_z.Evaluate(position) * 360f
                                      );
            else
                iten.transform.localEulerAngles = Vector3.zero;
        }

        public void EvaluateScale(float position, InfiniteWheelItem item)
        {
            if (enableItemScale)
            {
                item.SetLocalScale(new Vector3(
                                      itemScale.Evaluate(position),
                                      itemScale.Evaluate(position),
                                      itemScale.Evaluate(position)
                                      ));
            }
            else
                item.SetLocalScale(Vector3.one);
        }


        public void EvaluateActivateIten(float position, InfiniteWheelItem iten)
        {
            iten.ActivateItem(!(enableItemActivate && !((int)itemActivate.Evaluate(position) == 1)));
        }
    }
}