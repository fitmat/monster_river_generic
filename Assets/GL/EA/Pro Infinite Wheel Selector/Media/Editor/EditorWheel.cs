using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace InfiniteWheel
{

    [CustomEditor(typeof(InfiniteWheelController))]
    public class EditorWheel : Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (InfiniteWheelController)target;

            if (myTarget.premadeWheelConfig)
            {
                if (EditorCurve.IsCurveConfigSettingIncorrect(myTarget.premadeWheelConfig, false))
                {

                    string name_var = NameOf.nameof(() => myTarget.premadeWheelConfig);

                    EditorGUILayout.HelpBox("The variable (" + name_var + ") contains incorrect settings.", MessageType.Warning);
                    EditorCurve.IsCurveConfigSettingIncorrect(myTarget.premadeWheelConfig, true);
                }
            }
            DrawDefaultInspector();
        }
    }

    [CustomEditor(typeof(CurveConfig))]
    public class EditorCurve : Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (CurveConfig)target;

            if (IsCurveConfigSettingIncorrect(myTarget, false))
            {
                EditorGUILayout.HelpBox("Contains incorrect settings.", MessageType.Warning);
                IsCurveConfigSettingIncorrect(myTarget, true);
            }
            DrawDefaultInspector();

        }

        public static bool IsCurveConfigSettingIncorrect(CurveConfig curveConfig, bool show_warning)
        {
            bool incorrect = false;

            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.position_x), curveConfig.position_x, show_warning)) incorrect = true;
            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.position_y), curveConfig.position_y, show_warning)) incorrect = true;
            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.position_z), curveConfig.position_z, show_warning)) incorrect = true;
            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.rotation_x), curveConfig.rotation_x, show_warning)) incorrect = true;
            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.rotation_y), curveConfig.rotation_y, show_warning)) incorrect = true;
            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.rotation_z), curveConfig.rotation_z, show_warning)) incorrect = true;
            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.itemScale), curveConfig.itemScale, show_warning)) incorrect = true;
            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.colliderActivate), curveConfig.colliderActivate, show_warning)) incorrect = true;
            if (IsCurveIncorrect(NameOf.nameof(() => curveConfig.itemActivate), curveConfig.itemActivate, show_warning)) incorrect = true;

            return incorrect;
        }

        public static bool IsCurveIncorrect(string name_var, AnimationCurve curve, bool show_warning)
        {
            if (curve.keys.Length > 1)
            {
                bool range_incorrect = false;

                if (curve.keys[0].time != 0) { range_incorrect = true; }

                if (curve.keys[curve.keys.Length - 1].time != 2) { range_incorrect = true; }

                if (range_incorrect)
                {
                    if (show_warning)
                    {
                        EditorGUILayout.HelpBox("Curve:(" + name_var + ")-> Make sure that the (time) variable is in the strict range [0, 2].", MessageType.Warning);
                    }
                    return true;
                }
            }
            else
            {
                if (show_warning)
                {
                    EditorGUILayout.HelpBox("Curve contains insufficient keys.", MessageType.Warning);
                }
                return true;
            }
            return false;
        }

    }



    public static class NameOf
    {
        public static String nameof<T, TT>(this Expression<Func<T, TT>> accessor)
        {
            return nameof(accessor.Body);
        }

        public static String nameof<T>(this Expression<Func<T>> accessor)
        {
            return nameof(accessor.Body);
        }

        public static String nameof<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor)
        {
            return nameof(propertyAccessor.Body);
        }

        private static String nameof(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = expression as MemberExpression;
                if (memberExpression == null)
                    return null;
                return memberExpression.Member.Name;
            }
            return null;
        }
    }
}

