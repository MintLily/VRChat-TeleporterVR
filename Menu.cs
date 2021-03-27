using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RubyButtonAPIVRT;
using TeleporterVR.Utils;
using UnityEngine;
using VRC;
using UnityEngine.UI;
using TeleporterVR.Logic;

namespace TeleporterVR
{
    public class Menu
    {
        public static QMNestedButton menu;
        public static QMSingleButton userSel_TPto;
        private static QMSingleButton TPtoName;
        private static QMSingleButton TPtoCoords;
        private static QMSingleButton SavePos1;
        private static QMSingleButton LoadPos1;
        private static QMSingleButton SavePos2;
        private static QMSingleButton LoadPos2;
        private static QMSingleButton SavePos3;
        private static QMSingleButton LoadPos3;
        private static QMSingleButton SavePos4;
        private static QMSingleButton LoadPos4;
        public static QMToggleButton VRTeleport;
        private static QMToggleButton perferdHand;

        private static Vector3 Pos1, Pos2, Pos3, Pos4;
        private static Quaternion Rot1, Rot2, Rot3, Rot4;

        public static void InitUi()
        {
            menu = new QMNestedButton("ShortcutMenu", 2, -1, string.Empty, "<color=#13cf13>TeleporterVR v" + BuildInfo.Version + "</color>\nVR Compliant Teleporter", new Color?(Color.grey));
            menu.getMainButton().getGameObject().name = BuildInfo.Name + "_MenuButton";
            RectTransform rec = menu.getMainButton().getGameObject().GetComponent<RectTransform>();
            rec.anchoredPosition += new Vector2(-125f, -125f);
            rec.sizeDelta /= new Vector2(2.5f, 2.5f);

            if (Main.visible.Value)
                MelonLoader.MelonCoroutines.Start(LoadUserSelectTPButton());
            if (Main.VRTeleportVisible.Value && VRUtils.inVR)
                MelonLoader.MelonCoroutines.Start(LoadVRTPButton());

            TPtoName = new QMSingleButton(menu, 1, 0, Logic.Language.TPtoName_Text, () =>
            {
                PopupManager.ShowInputPopup("Teleport to Player", "", InputField.InputType.Standard, false, "Teleport", 
                    (s, __, ___) =>
                    {
                        Player tptgt = PlayerActions.Target(s);
                        PopupManager.HideCurrentPopup(VRCUiPopupManager.prop_VRCUiPopupManager_0);
                        if (tptgt != null)
                            PlayerActions.GetLocalVRCPlayer().transform.position = tptgt.transform.position;
                    }, null, "Enter (partial) Player Name");
            }, Logic.Language.TPtoName_Tooltip);

            TPtoCoords = new QMSingleButton(menu, 2, 0, Logic.Language.TPtoCoord_Text, () =>
            {
                PopupManager.ShowInputPopup("Teleport to Postition", "", InputField.InputType.Standard, false, "Teleport",
                    (s, __, ___) =>
                    {
                        string[] coords = s.Split(' ');
                        if (coords.Length == 3)
                        {
                            PopupManager.HideCurrentPopup(VRCUiPopupManager.prop_VRCUiPopupManager_0);
                            PlayerActions.GetLocalVRCPlayer().transform.position = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
                        }
                        else
                            MelonLoader.MelonLogger.Error("Please input the correct coords as => X[space]Y[space]Z");
                    }, null, "Enter coords as X[Space]Y[Space]Z");
            }, Logic.Language.TPtoCoord_Tooltip);

            if (VRUtils.inVR)
                perferdHand = new QMToggleButton(menu, 4, 0, "RightHanded", () => { VRUtils.perferRightHand = true; }, "LeftHanded", () => { VRUtils.perferRightHand = false; }, "TOGGLE: Choose wether you use Left or Right hand for VR Raycast Teleporting");

            SavePos1 = new QMSingleButton(menu, 1, 1, Logic.Language.SavePos + "\n1", () =>
            {
                Pos1 = PlayerActions.GetLocalVRCPlayer().transform.position;
                Rot1 = PlayerActions.GetLocalVRCPlayer().transform.rotation;
            }, Logic.Language.SavePos_ToolTip);

            SavePos2 = new QMSingleButton(menu, 2, 1, Logic.Language.SavePos + "\n2", () =>
            {
                Pos2 = PlayerActions.GetLocalVRCPlayer().transform.position;
                Rot2 = PlayerActions.GetLocalVRCPlayer().transform.rotation;
            }, Logic.Language.SavePos_ToolTip);

            SavePos3 = new QMSingleButton(menu, 3, 1, Logic.Language.SavePos + "\n3", () =>
            {
                Pos3 = PlayerActions.GetLocalVRCPlayer().transform.position;
                Rot3 = PlayerActions.GetLocalVRCPlayer().transform.rotation;
            }, Logic.Language.SavePos_ToolTip);

            SavePos4 = new QMSingleButton(menu, 4, 1, Logic.Language.SavePos + "\n4", () =>
            {
                Pos4 = PlayerActions.GetLocalVRCPlayer().transform.position;
                Rot4 = PlayerActions.GetLocalVRCPlayer().transform.rotation;
            }, Logic.Language.SavePos_ToolTip);

            LoadPos1 = new QMSingleButton(menu, 1, 2, Logic.Language.LoadPos + "\n1", () =>
            {
                if (Pos1 == null || Rot1 == null) return;
                PlayerActions.GetLocalVRCPlayer().transform.position = Pos1;
                PlayerActions.GetLocalVRCPlayer().transform.rotation = Rot1;
            }, Logic.Language.LoadPos_Tooltip);

            LoadPos2 = new QMSingleButton(menu, 2, 2, Logic.Language.LoadPos + "\n2", () =>
            {
                if (Pos2 == null || Rot2 == null) return;
                PlayerActions.GetLocalVRCPlayer().transform.position = Pos2;
                PlayerActions.GetLocalVRCPlayer().transform.rotation = Rot2;
            }, Logic.Language.LoadPos_Tooltip);

            LoadPos3 = new QMSingleButton(menu, 3, 2, Logic.Language.LoadPos + "\n3", () =>
            {
                if (Pos3 == null || Rot3 == null) return;
                PlayerActions.GetLocalVRCPlayer().transform.position = Pos3;
                PlayerActions.GetLocalVRCPlayer().transform.rotation = Rot3;
            }, Logic.Language.LoadPos_Tooltip);

            LoadPos4 = new QMSingleButton(menu, 4, 2, Logic.Language.LoadPos + "\n4", () =>
            {
                if (Pos4 == null || Rot4 == null) return;
                PlayerActions.GetLocalVRCPlayer().transform.position = Pos4;
                PlayerActions.GetLocalVRCPlayer().transform.rotation = Rot4;
            }, Logic.Language.LoadPos_Tooltip);

            menu.getMainButton().getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            TPtoName.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            TPtoCoords.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            SavePos1.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            SavePos2.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            SavePos3.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            SavePos4.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            LoadPos1.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            LoadPos2.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            LoadPos3.getGameObject().GetComponentInChildren<Text>().fontSize = 55;
            LoadPos4.getGameObject().GetComponentInChildren<Text>().fontSize = 55;

            MelonLoader.MelonCoroutines.Start(UpdateMenuIcon());
            if (VRTeleport != null) MelonLoader.MelonCoroutines.Start(ShiftButtons());

            if (Main.isDebug)
                MelonLoader.MelonLogger.Msg("Finished creating Menus");
        }

        public static IEnumerator LoadUserSelectTPButton(bool ignoreWait = true)
        {
            if (!ignoreWait) yield return new WaitForSeconds(2f);
            userSel_TPto = new QMSingleButton("UserInteractMenu", Main.userSel_x.Value, Main.userSel_y.Value, Language.theWord_Teleport, () =>
            {
                if (WorldActions.WorldAllowed)
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = QMStuff.GetQuickMenuInstance().field_Private_Player_0.field_Internal_VRCPlayer_0.transform.position;
            }, "Teleport to Selected Player");
            userSel_TPto.getGameObject().name = BuildInfo.Name + "_TPToPlayerButton";
            yield break;
        }

        public static IEnumerator LoadVRTPButton(bool ignoreWait = true)
        {
            if (!ignoreWait) yield return new WaitForSeconds(2f);
            VRTeleport = new QMToggleButton("ShortcutMenu", 0, 2, "VR", () =>
            {
                VRUtils.active = true;
            }, Language.theWord_Teleport, () =>
            {
                VRUtils.active = false;
            }, Language.perferedHand_Tooltip);
            if (ModCompatibility.DiscordMute)
                VRTeleport.getGameObject().GetComponentInChildren<Transform>().localPosition = new Vector3(-1400f, 600.8f, 0.0f);
            else
                VRTeleport.getGameObject().GetComponentInChildren<Transform>().localPosition = new Vector3(-1170.0f, 600.8f, 0.0f);

            VRTeleport.getGameObject().GetComponentInChildren<Transform>().localScale = new Vector3(0.6f, 0.75f, 1.0f);
            VRTeleport.getGameObject().name = BuildInfo.Name + "_VRTPToggleButton";
            yield break;
        }

        public static IEnumerator UpdateMenuIcon(bool ignoreWait = true)
        {
            if (!ignoreWait) yield return new WaitForSeconds(1f);
            if (WorldActions.WorldAllowed)
            {
                menu.getMainButton().getGameObject().GetComponentInChildren<Image>().sprite = ResourceManager.goodIcon;
                menu.getMainButton().Disabled(false);
                if (Main.VRTeleportVisible.Value && VRUtils.inVR)
                    VRTeleport.Disabled(false);
            }
            else
            {
                menu.getMainButton().getGameObject().GetComponentInChildren<Image>().sprite = ResourceManager.badIcon;
                menu.getMainButton().Disabled(true);
                if (Main.VRTeleportVisible.Value && VRUtils.inVR)
                    VRTeleport.Disabled(true);
            }
            yield break;
        }

        private static IEnumerator ShiftButtons()
        {
            yield return new WaitForSecondsRealtime(4f);
            if (Logic.ModCompatibility.DiscordMute)
                VRTeleport.getGameObject().GetComponentInChildren<Transform>().localPosition = new Vector3(-1400f, 600.8f, 0.0f);
            yield break;
        }

        public static void UpdateButtonText()
        {
            Language.InitLanguageChange();

            TPtoName.setButtonText(Language.TPtoName_Text);
            TPtoName.setToolTip(Language.TPtoName_Tooltip);

            TPtoCoords.setButtonText(Language.TPtoCoord_Text);
            TPtoCoords.setToolTip(Language.TPtoCoord_Tooltip);

            SavePos1.setButtonText(Language.SavePos + "\n1");
            SavePos2.setButtonText(Language.SavePos + "\n2");
            SavePos3.setButtonText(Language.SavePos + "\n3");
            SavePos4.setButtonText(Language.SavePos + "\n4");

            LoadPos1.setButtonText(Language.LoadPos + "\n1");
            LoadPos2.setButtonText(Language.LoadPos + "\n2");
            LoadPos3.setButtonText(Language.LoadPos + "\n3");
            LoadPos4.setButtonText(Language.LoadPos + "\n4");

            userSel_TPto.setButtonText(Language.theWord_Teleport);

            if (VRTeleport != null)
            {
                VRTeleport.setOffText(Language.theWord_Teleport);
                VRTeleport.setToolTip(Language.perferedHand_Tooltip);
            }

            if (Main.isDebug)
                MelonLoader.MelonLogger.Msg("Updated button text and tooltip text");
        }
    }
}
