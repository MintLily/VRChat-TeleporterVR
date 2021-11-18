using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleporterVR.Utils;
using UnityEngine;
using VRC;
using UnityEngine.UI;
using TeleporterVR.Logic;
using TeleporterVR.Rendering;
using System.Diagnostics;

namespace TeleporterVR
{
    public class Menu
    {
        private static Vector3 Pos1, Pos2, Pos3, Pos4;
        private static Quaternion Rot1, Rot2, Rot3, Rot4;

        internal static void OpenWebpage(string site) => Process.Start(site);

        internal static void SaveAction(int slot)
        {
            switch (slot)
            {
                case 1:
                    Pos1 = PlayerActions.GetLocalVRCPlayer().transform.position;
                    Rot1 = PlayerActions.GetLocalVRCPlayer().transform.rotation;
                    break;
                case 2:
                    Pos2 = PlayerActions.GetLocalVRCPlayer().transform.position;
                    Rot2 = PlayerActions.GetLocalVRCPlayer().transform.rotation;
                    break;
                case 3:
                    Pos3 = PlayerActions.GetLocalVRCPlayer().transform.position;
                    Rot3 = PlayerActions.GetLocalVRCPlayer().transform.rotation;
                    break;
                case 4:
                    Pos4 = PlayerActions.GetLocalVRCPlayer().transform.position;
                    Rot4 = PlayerActions.GetLocalVRCPlayer().transform.rotation;
                    break;
            }
        }

        internal static void LoadAction(int slot)
        {
            switch (slot)
            {
                case 1:
                    if (Pos1 == null || Rot1 == null) return;
                    if (!WorldActions.WorldAllowed) return;
                    PlayerActions.GetLocalVRCPlayer().transform.position = Pos1;
                    PlayerActions.GetLocalVRCPlayer().transform.rotation = Rot1;
                    break;
                case 2:
                    if (Pos2 == null || Rot2 == null) return;
                    if (!WorldActions.WorldAllowed) return;
                    PlayerActions.GetLocalVRCPlayer().transform.position = Pos2;
                    PlayerActions.GetLocalVRCPlayer().transform.rotation = Rot2;
                    break;
                case 3:
                    if (Pos3 == null || Rot3 == null) return;
                    if (!WorldActions.WorldAllowed) return;
                    PlayerActions.GetLocalVRCPlayer().transform.position = Pos3;
                    PlayerActions.GetLocalVRCPlayer().transform.rotation = Rot3;
                    break;
                case 4:
                    if (Pos4 == null || Rot4 == null) return;
                    if (!WorldActions.WorldAllowed) return;
                    PlayerActions.GetLocalVRCPlayer().transform.position = Pos4;
                    PlayerActions.GetLocalVRCPlayer().transform.rotation = Rot4;
                    break;
            }
        }

        internal static void OpenKeyboardForPlayerTP()
        {
            PopupManager.ShowInputPopup("Teleport to Player", "", InputField.InputType.Standard, false, "Teleport",
                    (s, __, ___) =>
                    {
                        Player tptgt = PlayerActions.Target(s);
                        PopupManager.HideCurrentPopup(VRCUiPopupManager.prop_VRCUiPopupManager_0);
                        if (tptgt != null)
                            if (WorldActions.WorldAllowed)
                                PlayerActions.GetLocalVRCPlayer().transform.position = tptgt.transform.position;
                    }, null, "Enter (partial) Player Name");
        }

        internal static void OpenKeyboardForCoordTP()
        {
            PopupManager.ShowInputPopup("Teleport to Postition", "", InputField.InputType.Standard, false, "Teleport",
                    (s, __, ___) =>
                    {
                        string[] coords = s.Split(' ');
                        if (coords.Length == 3)
                        {
                            PopupManager.HideCurrentPopup(VRCUiPopupManager.prop_VRCUiPopupManager_0);
                            if (WorldActions.WorldAllowed)
                                PlayerActions.GetLocalVRCPlayer().transform.position = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
                        }
                        else
                            MelonLoader.MelonLogger.Error("Please input the correct coords as => X[space]Y[space]Z");
                    }, null, "Enter coords as X[Space]Y[Space]Z");
        }
    }
}
