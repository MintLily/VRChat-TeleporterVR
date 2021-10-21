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
using TeleporterVR.Rendering;
using System.Diagnostics;

namespace TeleporterVR
{
    public class Menu
    {
        public static QMNestedButton menu;
        public static QMSingleButton userSel_TPto;
        private static QMSingleButton TPtoName, TPtoCoords;
        private static QMSingleButton SavePos1, SavePos2, SavePos3, SavePos4;
        private static QMSingleButton LoadPos1, LoadPos2, LoadPos3, LoadPos4;
        internal static QMToggleButton VRTeleport, preferdHand;
        private static QMSingleButton DiscordBtn, GitHubBtn;

        private static Vector3 Pos1, Pos2, Pos3, Pos4;
        private static Quaternion Rot1, Rot2, Rot3, Rot4;

        public static void InitUi()
        {
            menu = new QMNestedButton("ShortcutMenu", 2, -1, string.Empty, "<color=#13cf13>TeleporterVR v" + BuildInfo.Version + "</color>\nVR Compliant Teleporter", new Color?(Color.grey));
            menu.getMainButton().getGameObject().name = BuildInfo.Name + "_MenuButton";
            RectTransform rec = menu.getMainButton().getGameObject().GetComponent<RectTransform>();
            rec.anchoredPosition += new Vector2(-125f, -125f);
            rec.sizeDelta /= new Vector2(2.5f, 2.5f);

            userSel_TPto = new QMSingleButton("UserInteractMenu", Main.userSel_x.Value, Main.userSel_y.Value, Language.theWord_Teleport, () =>
            {
                if (WorldActions.WorldAllowed)
                    PlayerActions.Teleport(PlayerActions.GetSelectedPlayer());
            }, "Teleport to Selected Player");
            userSel_TPto.getGameObject().name = BuildInfo.Name + "_TPToPlayerButton";

            VRTeleport = new QMToggleButton("ShortcutMenu", 3, -2, Language.theWord_Teleport, () =>
            {
                if (WorldActions.WorldAllowed) {
                    VRUtils.active = true;
                    TPLocationIndicator.Toggle();
                }
            }, "OFF", () =>
            {
                if (WorldActions.WorldAllowed) {
                    VRUtils.active = false;
                    TPLocationIndicator.Toggle();
                }
            }, Language.perferedHand_Tooltip);
            VRTeleport.getGameObject().GetComponent<RectTransform>().anchoredPosition -= new Vector2(92f, 66.2f);
            VRTeleport.getGameObject().GetComponentInChildren<Transform>().localScale = new Vector3(0.6f, 0.75f, 1.0f);
            VRTeleport.getGameObject().name = BuildInfo.Name + "_VRTPToggleButton";

            TPtoName = new QMSingleButton(menu, 1, 0, Language.TPtoName_Text, () => OpenKeyboardForPlayerTP(), Language.TPtoName_Tooltip);

            TPtoCoords = new QMSingleButton(menu, 2, 0, Language.TPtoCoord_Text, () => OpenKeyboardForCoordTP(), Language.TPtoCoord_Tooltip);

            preferdHand = new QMToggleButton(menu, 4, 0, Language.preferedHanded_Text_ON, () =>
            {
                VRUtils.preferRightHand = true;
                VRTeleport.setToolTip(Language.perferedHand_Tooltip);
                preferdHand.setToolTip(Language.perferedHand_Tooltip);
            }, Language.preferedHanded_Text_OFF, () =>
            {
                VRUtils.preferRightHand = false;
                VRTeleport.setToolTip(Language.perferedHand_Tooltip);
                preferdHand.setToolTip(Language.perferedHand_Tooltip);
            }, Logic.Language.perferedHand_Tooltip);

            SavePos1 = new QMSingleButton(menu, 1, 1, Language.SavePos + "\n1", () => SaveAction(1), Language.SavePos_ToolTip);

            SavePos2 = new QMSingleButton(menu, 2, 1, Language.SavePos + "\n2", () => SaveAction(2), Language.SavePos_ToolTip);

            SavePos3 = new QMSingleButton(menu, 3, 1, Language.SavePos + "\n3", () => SaveAction(3), Language.SavePos_ToolTip);

            SavePos4 = new QMSingleButton(menu, 4, 1, Language.SavePos + "\n4", () => SaveAction(4), Language.SavePos_ToolTip);

            LoadPos1 = new QMSingleButton(menu, 1, 2, Language.LoadPos + "\n1", () => LoadAction(1), Language.LoadPos_Tooltip);

            LoadPos2 = new QMSingleButton(menu, 2, 2, Language.LoadPos + "\n2", () => LoadAction(2), Language.LoadPos_Tooltip);

            LoadPos3 = new QMSingleButton(menu, 3, 2, Language.LoadPos + "\n3", () => LoadAction(3), Language.LoadPos_Tooltip);

            LoadPos4 = new QMSingleButton(menu, 4, 2, Language.LoadPos + "\n4", () => LoadAction(4), Language.LoadPos_Tooltip);

            DiscordBtn = new QMSingleButton(menu, 5, -1, string.Empty, () => OpenWebpage("https://discord.gg/qkycuAMUGS"), "Invite to Discord Server of Lily's Mods", Color.white);
            DiscordBtn.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2.0f, 2.0f);
            DiscordBtn.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(-105f, 105f);

            GitHubBtn = new QMSingleButton(menu, 5, -1, string.Empty, () => OpenWebpage(BuildInfo.DownloadLink), "Opens browser to GitHub project", Color.white);
            GitHubBtn.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2.0f, 2.0f);
            GitHubBtn.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(-105f, -105f);

            MelonLoader.MelonCoroutines.Start(ApplySocialIconsToButtons());

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
            preferdHand.getGameObject().GetComponentInChildren<Text>().fontSize = 55;

            MelonLoader.MelonCoroutines.Start(UpdateMenuIcon());
            UpdateUserSelectTeleportButton();
            UpdateVRTeleportButton();
            UpdateLeftRightHandButton();
            preferdHand.setToggleState(Main.preferRightHand.Value);

            Main.Log("Finished creating Menus", Main.isDebug);
        }

        public static void UpdateUserSelectTeleportButton() {
            if (userSel_TPto != null) userSel_TPto.setActive(Main.visible.Value);
        }

        public static void UpdateVRTeleportButton() {
            if (VRTeleport != null)
                VRTeleport.setActive(Main.VRTeleportVisible.Value);
        }

        public static void UpdateLeftRightHandButton() {
            if (preferdHand != null) {
                preferdHand.setActive(Main.VRTeleportVisible.Value);
                preferdHand.setToggleState(Main.preferRightHand.Value, true);
            }
        }

        public static IEnumerator UpdateMenuIcon(bool ignoreWait = true)
        {
            if (!ignoreWait) yield return new WaitForSeconds(1f);
            menu.getMainButton().getGameObject().GetComponentInChildren<Image>().sprite = WorldActions.WorldAllowed ? ResourceManager.goodIcon : ResourceManager.badIcon;
            menu.getMainButton().Disabled(!WorldActions.WorldAllowed);
            VRTeleport.Disabled(!WorldActions.WorldAllowed);
            userSel_TPto.Disabled(!WorldActions.WorldAllowed);
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
            SavePos1.setToolTip(Language.SavePos_ToolTip);
            SavePos2.setToolTip(Language.SavePos_ToolTip);
            SavePos3.setToolTip(Language.SavePos_ToolTip);
            SavePos4.setToolTip(Language.SavePos_ToolTip);

            LoadPos1.setButtonText(Language.LoadPos + "\n1");
            LoadPos2.setButtonText(Language.LoadPos + "\n2");
            LoadPos3.setButtonText(Language.LoadPos + "\n3");
            LoadPos4.setButtonText(Language.LoadPos + "\n4");
            LoadPos1.setToolTip(Language.LoadPos_Tooltip);
            LoadPos2.setToolTip(Language.LoadPos_Tooltip);
            LoadPos3.setToolTip(Language.LoadPos_Tooltip);
            LoadPos4.setToolTip(Language.LoadPos_Tooltip);

            userSel_TPto.setButtonText(Language.theWord_Teleport);

            VRTeleport.setOnText(Language.theWord_Teleport);
            VRTeleport.setToolTip(Language.perferedHand_Tooltip);
            preferdHand.setToolTip(Language.perferedHand_Tooltip);
            preferdHand.setOnText(Language.preferedHanded_Text_ON);
            preferdHand.setOffText(Language.preferedHanded_Text_OFF);

            Main.Log("Updated button text and tooltip text", Main.isDebug);
        }

        private static IEnumerator ApplySocialIconsToButtons()
        {
            yield return new WaitForSeconds(4.5f);
            DiscordBtn.getGameObject().GetComponentInChildren<Image>().sprite = ResourceManager.DiscordLogo;
            yield return new WaitForSeconds(0.5f);
            GitHubBtn.getGameObject().GetComponentInChildren<Image>().sprite = ResourceManager.GitHubLogo;
            yield break;
        }

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
