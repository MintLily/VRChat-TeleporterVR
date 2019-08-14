using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRCTools;
using VRC;
using VRCModLoader;

namespace Mod.Teleport
{
    internal class InputB
    {
        public static void InputCancel()
        {
        }

        public static void InputAccept(string str, List<KeyCode> keycodelist, Text text)
        {
            Player tptgt = PlayerUtils.Target(str);
            inputcheck = str;
            namecheck = PlayerUtils.GetAPIUser(tptgt).displayName;
            VRCPlayer.Instance.transform.position = tptgt.transform.position;
            VRCUiManagerUtils.GetVRCUiManager().CloseUi(false);
        }

        public static void InputAttempt(string action)
        {
            InputB.InputCancelAction = new Action(InputB.InputCancel);
            if (action == "Teleport")
            {
                InputB.InputAcceptAction = new Action<string, List<KeyCode>, Text>(InputB.InputAccept);
            }
            VRCUiPopupManagerUtils.GetVRCUiPopupManager().ShowUnityInputPopupWithCancel("Enter player name", "", InputField.InputType.Standard, false, "Teleport", InputB.InputAcceptAction, InputB.InputCancelAction, "Enter text....", true, null);
        }

        public static string namecheck;
        public static string inputcheck;
        public static Action InputCancelAction;
        public static Action<string, List<KeyCode>, Text> InputAcceptAction;
    }

}

