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

        public static void InputAcceptTeleport(string str, List<KeyCode> keycodelist, Text text)
        {
            Player tptgt = PlayerUtils.Target(str);
            inputcheck = str;
            namecheck = PlayerUtils.GetAPIUser(tptgt).displayName;
            VRCPlayer.Instance.transform.position = tptgt.transform.position;
            VRCUiManagerUtils.GetVRCUiManager().CloseUi(false);
        }

        public static void InputAcceptCoords(string str, List<KeyCode> keycodelist, Text text)
        {
            string[] coords = str.Split(' ');
            if (coords.Length == 3)
            {
                VRCPlayer.Instance.transform.position = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
                PassedInputStageWithoutErrors = true;
            }
            if (coords.Length > 3)
            {
                VRCPlayer.Instance.transform.position = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
                PassedInputStageWithoutErrors = true;
                EasterEggTripped = true;
            }
            else
            {
                PassedInputStageWithoutErrors = false;
            }
            VRCUiManagerUtils.GetVRCUiManager().CloseUi(false);
        }

        public static void InputAttempt(string action)
        {
            InputCancelAction = new Action(InputCancel);
            string raction = "";
            if (action == "NTP")
            {
                InputAcceptAction = new Action<string, List<KeyCode>, Text>(InputAcceptTeleport);
                TypeOfInput = "Enter (partial) playername:";
                raction = "Teleport";
            }
            if (action == "CoordTP")
            {
                InputAcceptAction = new Action<string, List<KeyCode>, Text>(InputAcceptCoords);
                TypeOfInput = "Enter coords as X[Space]Y[Space]Z";
                raction = "Teleport";
            }
            VRCUiPopupManagerUtils.GetVRCUiPopupManager().ShowUnityInputPopupWithCancel("TPVR-R Input:", "", InputField.InputType.Standard, false, raction, InputAcceptAction, InputCancelAction, TypeOfInput, true, null);
        }

        public static bool EasterEggTripped = false; //Extra input? Hey, just like most of my code! :D
        public static bool PassedInputStageWithoutErrors = false;
        public static string TypeOfInput;
        public static string namecheck;
        public static string inputcheck;
        public static Action InputCancelAction;
        public static Action<string, List<KeyCode>, Text> InputAcceptAction;
    }

}

