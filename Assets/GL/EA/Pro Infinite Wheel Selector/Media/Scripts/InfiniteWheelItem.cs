/*******************************************************
 * 													   *
 * Asset:		 	Pro Infinite Wheel                 *
 * Script:		 	InfiniteWheelItem.cs  			   *
 *                                                     *
 * Page: https://www.facebook.com/NLessStudio/         *
 * 													   *
 *******************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace InfiniteWheel
{
    public class InfiniteWheelItem : MonoBehaviour
    {
        public bool isActive = true;

        public InfiniteWheelCollider itemCollider;
        public GameObject itemBody;

        public UnityEvent onClick;


        public void test(GameObject d)
        {

        }

        public void ActivateCollider(bool active)
        {
            itemCollider.gameObject.SetActive(active);
        }

        public void ActivateItem(bool active)
        {
            itemBody.SetActive(active);
        }

        public void Action()
        {
            if (isActive)
            {
                onClick.Invoke();
            }
        }

        public void SetLocalScale(Vector3 scale)
        {
            itemBody.transform.localScale = scale;
        }

    }
}