using MelonLoader;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using TeleporterVR.Utils;
using TeleporterVR.Logic;
using UIExpansionKit.API;

namespace TeleporterVR
{
    public static class BuildInfo
    {
        public const string Name = "TeleporterVR";
        public const string Author = "Janni, Lily";
        public const string Company = null;
        public const string Version = "4.1.1";
        public const string DownloadLink = "https://github.com/KortyBoi/TeleporterVR";
        public const string Description = "Easy Utility that allows you to teleport in various different ways while being VR compliant.";
    }

    public class Main : MelonMod
    {
        public static bool isDebug;
        public static MelonPreferences_Category melon;
        public static MelonPreferences_Entry<bool> visible;
        public static MelonPreferences_Entry<int> userSel_x;
        public static MelonPreferences_Entry<int> userSel_y;
        public static MelonPreferences_Entry<bool> preferRightHand;
        public static MelonPreferences_Entry<bool> VRTeleportVisible;
        public static MelonPreferences_Entry<string> OverrideLanguage;
        public static MelonPreferences_Entry<bool> ActionMenuApiIntegration;

        public override void OnApplicationStart()
        {
            if (MelonDebug.IsEnabled() || Environment.CommandLine.Contains("--vrt.debug"))
            {
                isDebug = true;
                MelonLogger.Msg(ConsoleColor.Green, "Debug mode is active");
            }
            
            melon = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            visible = (MelonPreferences_Entry<bool>)melon.CreateEntry("UserInteractTPButtonVisible", true, "Is Teleport Button Visible (on User Select)");
            userSel_x = (MelonPreferences_Entry<int>)melon.CreateEntry("UserInteractTPButtonPositionX", 1, "X-Coordinate (User Selected TPButton)");
            userSel_y = (MelonPreferences_Entry<int>)melon.CreateEntry("UserInteractTPButtonPositionY", 3, "Y-Coordinate (User Selected TPButton)");
            preferRightHand = (MelonPreferences_Entry<bool>)melon.CreateEntry("preferRightHand", true, "Right Handed");
            VRTeleportVisible = (MelonPreferences_Entry<bool>)melon.CreateEntry("VRTeleportVisible", true, "Is VRTeleport Button Visible");
            OverrideLanguage = (MelonPreferences_Entry<string>)melon.CreateEntry("overrideLanguage", "off", "Override Language");
            ExpansionKitApi.RegisterSettingAsStringEnum(melon.Identifier, OverrideLanguage.Identifier, 
                new[] {
                ("off", "Disable Override"),
                ("en", "English"),
                ("fr", "Français"),
                ("de", "Deutsch"),
                ("ja", "日本語"),
                ("no_bm", "Bokmål"),
                ("ru", "русский"),
                ("es", "Español"),
                ("po", "Português"),
                ("sw", "Svensk")
            });
            ActionMenuApiIntegration = (MelonPreferences_Entry<bool>)melon.CreateEntry("ActionMenuApiIntegration", false, "Has ActionMenu Support\n(disable requires game restart)");

            ResourceManager.Init();
            Patches.Init();
            Language.InitLanguageChange();
            ActionMenu.InitUi();

            MelonLogger.Msg("Initialized!");

            if (OverrideLanguage.Value == "no") OverrideLanguage.Value = "no_bm";
        }

        public override void VRChat_OnUiManagerInit()
        {
            Menu.InitUi();
            VRUtils.Init();
            GetSetWorld.Init();
            MelonCoroutines.Start(UiUtils.AllowToolTipTextColor());
        }

        public override void OnPreferencesSaved()
        {
            Menu.UpdateUserSelectTeleportButton();
            Menu.UpdateVRTeleportButton();
            Menu.UpdateLeftRightHandButton();
            Menu.UpdateButtonText();
            preferRightHand.Value = VRUtils.preferRightHand;
            if (ActionMenuApiIntegration.Value // if true
                && !ActionMenu.hasStarted // if has not started yet
                && ActionMenu.hasAMApiInstalled // if gompo's mod is installed
                && !ActionMenu.AMApiOutdated // if gompo's mod is not outdated
                && AMSubMenu.subMenu == null) // if (one) ActionMenu element is not created
            {
                MelonLogger.Msg(ConsoleColor.Yellow, "You may have to change or reload your current world to allow the ActionMenu to show.");
                ActionMenu.InitUi();
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            switch (buildIndex)
            {
                case 0:
                case 1:
                    break;
                default:
                    MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
                    MelonCoroutines.Start(GetSetWorld.DelayedLoad());
                    Menu.VRTeleport.setToggleState(false, true);
                    break;
            }
        }

        public override void OnApplicationQuit() { preferRightHand.Value = VRUtils.preferRightHand; }

        public override void OnUpdate()
        {
            VRUtils.OnUpdate();
            // This check is to keep the menu Disabled in Disallowed worlds, this was super easy to patch into or use UnityExplorer to re-enable the button
            if (!WorldActions.WorldAllowed && Patches.IsQMOpen && (Menu.menu.getMainButton().getGameObject().GetComponent<Button>().enabled || Menu.VRTeleport.getGameObject().GetComponent<Button>().enabled) &&
                Menu.menu.getMainButton().getGameObject().GetComponentInChildren<Image>().sprite == ResourceManager.badIcon)
            {
                Menu.menu.getMainButton().Disabled(true);
                Menu.VRTeleport.Disabled(true);
                Menu.userSel_TPto.Disabled(true);
            }
        }
    }
}