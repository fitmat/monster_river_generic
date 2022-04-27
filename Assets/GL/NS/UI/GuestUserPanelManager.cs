using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLib.UI.PanelManagers
{
    public class GuestUserPanelManager : MonoBehaviour
    {
        // required variables
        [SerializeField] YipliConfig currentYipliConfig = null;

        public void GetMatButtonFunction()
        {
            if (currentYipliConfig.getMatUrlIn == null || currentYipliConfig.getMatUrlUS == null)
            {
                if (System.Globalization.RegionInfo.CurrentRegion.ToString().Equals("IN", System.StringComparison.OrdinalIgnoreCase))
                {
                    Application.OpenURL(currentYipliConfig.getMatUrlIn);
                }
                else
                {
                    Application.OpenURL(currentYipliConfig.getMatUrlUS);
                }
            }
            else
            {
                if (System.Globalization.RegionInfo.CurrentRegion.ToString().Equals("IN", System.StringComparison.OrdinalIgnoreCase))
                {
                    Application.OpenURL(ProductMessages.GetMatUrlIn);
                }
                else
                {
                    Application.OpenURL(ProductMessages.GetMatUrlUS);
                }
            }
        }
    }
}
