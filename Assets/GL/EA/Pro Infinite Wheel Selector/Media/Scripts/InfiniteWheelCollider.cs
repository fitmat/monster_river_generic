/*******************************************************
 * 													   *
 * Asset:		 	Pro Infinite Wheel                 *
 * Script:		 	InfiniteWheelCollider.cs           *
 *                                                     *
 * Page: https://www.facebook.com/NLessStudio/         *
 * 													   *
 *******************************************************/
using UnityEngine.EventSystems;
using UnityEngine;

namespace InfiniteWheel
{

    public class InfiniteWheelCollider : MonoBehaviour
    {
        public InfiniteWheelItem ruletaItem;

        private void OnMouseUpAsButton()
        {
            ruletaItem.Action();
        }
    }
}