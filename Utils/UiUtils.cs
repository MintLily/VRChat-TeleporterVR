using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TeleporterVR.Utils
{
    class UiUtils
    {
        public static IEnumerator AllowToolTipTextColor()
        {
            try {
                GameObject TooltipText = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_ToolTip/_ToolTipPanel/Text");
                TooltipText.GetComponentInChildren<Text>().supportRichText = true;
            } catch { MelonLoader.MelonLogger.Error("Failed to make ToolipText supportRichText"); }
            try {
                GameObject ALTTooltipText = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Hover/_ToolTipPanel/Text");
                ALTTooltipText.GetComponentInChildren<Text>().supportRichText = true;
            } catch { MelonLoader.MelonLogger.Error("Failed to make ALTTooltipText supportRichText"); }

            if (Main.isDebug) MelonLoader.MelonLogger.Msg("Finshed assigning UiToolTip to suuport rich text");
            yield break;
        }
    }
}
