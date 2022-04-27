using UnityEngine;
using UnityEngine.Events;
using Lean.Common;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lean.Touch
{
	/// <summary>This component allows you to detect when a finger is on top of the current GameObject without using the standard LeanSelectable system.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanSelectSelf")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Select Self")]
	public class LeanSelectSelf : LeanSelectBase
	{
		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}

		/// <summary>This event will be invoked when the specified finger touches this GameObject.</summary>
		public LeanFingerEvent OnFinger { get { if (onFinger == null) onFinger = new LeanFingerEvent(); return onFinger; } } [SerializeField] private LeanFingerEvent onFinger;

		/// <summary>This event will be invoked when the specified finger touches this GameObject, and tell you the world space position it touched.</summary>
		public Vector3Event OnWorld { get { if (onWorld == null) onWorld = new Vector3Event(); return onWorld; } } [SerializeField] private Vector3Event onWorld;

		protected override void TrySelect(LeanFinger finger, Component component, Vector3 worldPosition)
		{
			// Stores the component we will search for
			var selectSelf = default(LeanSelectSelf);

			// Was a component found?
			if (component != null)
			{
				switch (Search)
				{
					case SearchType.GetComponent:           selectSelf = component.GetComponent          <LeanSelectSelf>(); break;
					case SearchType.GetComponentInParent:   selectSelf = component.GetComponentInParent  <LeanSelectSelf>(); break;
					case SearchType.GetComponentInChildren: selectSelf = component.GetComponentInChildren<LeanSelectSelf>(); break;
				}

				// Discard if tag doesn't match
				if (selectSelf != null && string.IsNullOrEmpty(RequiredTag) == false && selectSelf.tag != RequiredTag)
				{
					selectSelf = null;
				}
			}

			if (selectSelf == this)
			{
				if (onFinger != null)
				{
					onFinger.Invoke(finger);
				}

				if (onWorld != null)
				{
					onWorld.Invoke(worldPosition);
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch
{
	[CustomEditor(typeof(LeanSelectSelf))]
	public class LeanSelectSelf_Inspector : LeanSelectBase_Inspector<LeanSelectSelf>
	{
		protected override void DrawInspector()
		{
			base.DrawInspector();

			EditorGUILayout.Separator();

			Draw("onFinger");
			Draw("onWorld");
		}
	}
}
#endif