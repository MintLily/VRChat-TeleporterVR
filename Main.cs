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
        public const string Version = "4.0.0";
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

        public override void OnApplicationStart()
        {
            if (MelonDebug.IsEnabled() || Environment.CommandLine.Contains("--vrt.debug"))
            {
                isDebug = true;
                MelonLogger.Msg("Debug mode is active");
                VRUtils.inVR = true;
            }
            
            melon = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            visible = (MelonPreferences_Entry<bool>)melon.CreateEntry("UserInteractTPButtonVisible", true, "Is Teleport Button Visible (on User Select)");
            userSel_x = (MelonPreferences_Entry<int>)melon.CreateEntry("UserInteractTPButtonPositionX", 1, "X-Coordinate (User Selected TPButton)");
            userSel_y = (MelonPreferences_Entry<int>)melon.CreateEntry("UserInteractTPButtonPositionY", 3, "Y-Coordinate (User Selected TPButton)");
            preferRightHand = (MelonPreferences_Entry<bool>)melon.CreateEntry("perferRightHand", true, "Right Handed", true);
            VRTeleportVisible = (MelonPreferences_Entry<bool>)melon.CreateEntry("VRTeleportVisible", false, "Is VRTeleport Button Visible");
            OverrideLanguage = (MelonPreferences_Entry<string>)melon.CreateEntry("overrideLanguage", "off", "Override Language");
            ExpansionKitApi.RegisterSettingAsStringEnum(melon.Identifier, OverrideLanguage.Identifier, 
                new[] {
                ("off", "Disable Override"),
                ("en", "English"),
                ("fr", "Français"),
                ("de", "Deutsch"),
                ("ja", "日本語"),
                ("no", "Bokmål"),
                ("ru", "русский"),
                ("es", "Español"),
                ("po", "Português"),
                ("sw", "Svensk")
            });

            Patches.Init();
            Language.InitLanguageChange();
            ModCompatibility.Init();

            MelonLogger.Msg("Initialized!");
        }

        public override void VRChat_OnUiManagerInit()
        {
            Menu.InitUi();
            ResourceManager.Init();
            if (VRUtils.inVR)
                VRUtils.Init();
            MelonCoroutines.Start(UiUtils.AllowToolTipTextColor());
        }

        public override void OnUpdate() { VRUtils.OnUpdate(); }

        public override void OnPreferencesSaved()
        {
            if (Menu.userSel_TPto != null && !visible.Value) Menu.userSel_TPto.DestroyMe();
            else if (Menu.userSel_TPto == null && visible.Value) MelonCoroutines.Start(Menu.LoadUserSelectTPButton(false));

            if (VRUtils.inVR)
                if (Menu.VRTeleport != null && !VRTeleportVisible.Value) Menu.VRTeleport.DestroyMe();
                else if (Menu.VRTeleport == null && VRTeleportVisible.Value) MelonCoroutines.Start(Menu.LoadVRTPButton(false));

            Menu.UpdateButtonText();

            preferRightHand.Value = VRUtils.perferRightHand;
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
                    break;
            }
        }

        public override void OnApplicationQuit() { preferRightHand.Value = VRUtils.perferRightHand; }
    }
}