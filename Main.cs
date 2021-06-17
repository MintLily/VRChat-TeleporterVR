using MelonLoader;
using System;
using System.Collections;
using UnityEngine.UI;
using TeleporterVR.Utils;
using TeleporterVR.Logic;
using UIExpansionKit.API;
using TeleporterVR.Rendering;
using System.Reflection;
using System.Linq;

namespace TeleporterVR
{
    public static class BuildInfo
    {
        public const string Name = "TeleporterVR";
        public const string Author = "Janni, Lily";
        public const string Company = null;
        public const string Version = "4.2.3";
        public const string DownloadLink = "https://github.com/MintLily/VRChat-TeleporterVR";
        public const string Description = "Easy Utility that allows you to teleport in various different ways while being VR compliant.";
    }

    public class Main : MelonMod
    {
        private static MelonMod Instance;
        public static bool isDebug;
        private static TPLocationIndicator LR;
        public static MelonPreferences_Category melon;
        public static MelonPreferences_Entry<bool> visible, preferRightHand, VRTeleportVisible, ActionMenuApiIntegration, EnableTeleportIndicator;
        public static MelonPreferences_Entry<int> userSel_x, userSel_y;
        public static MelonPreferences_Entry<string> OverrideLanguage, IndicatorHexColor;

        public override void OnApplicationStart()
        {
            Instance = this;
            if (MelonDebug.IsEnabled() || Environment.CommandLine.Contains("--vrt.debug")) {
                isDebug = true;
                MelonLogger.Msg(ConsoleColor.Green, "Debug mode is active");
            }

            MelonCoroutines.Start(GetAssembly());

            melon = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            visible = melon.CreateEntry("UserInteractTPButtonVisible", true, "Is Teleport Button Visible (on User Select)");
            userSel_x = melon.CreateEntry("UserInteractTPButtonPositionX", 1, "X-Coordinate (User Selected TPButton)");
            userSel_y = melon.CreateEntry("UserInteractTPButtonPositionY", 3, "Y-Coordinate (User Selected TPButton)");
            preferRightHand = melon.CreateEntry("preferRightHand", true, "Right Handed");
            VRTeleportVisible = melon.CreateEntry("VRTeleportVisible", true, "Is VRTeleport Button Visible");
            OverrideLanguage = melon.CreateEntry("overrideLanguage", "off", "Override Language");
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
            ActionMenuApiIntegration = melon.CreateEntry("ActionMenuApiIntegration", false, "Has ActionMenu Support\n(disable requires game restart)");
            EnableTeleportIndicator = melon.CreateEntry("EnableTeleportIndicator", true, "Shows a circle to where you will teleport to");
            IndicatorHexColor = melon.CreateEntry("IndicatorHEXColor", "2dff2d", "Indicator Color (HEX Value [\"RRGGBB\"])");

            ResourceManager.Init();
            NewPatches.SetupPatches();
            Language.InitLanguageChange();
            ActionMenu.InitUi();
            RenderingIndicator.Init();

            MelonLogger.Msg("Initialized!");

            if (OverrideLanguage.Value == "no") OverrideLanguage.Value = "no_bm";
        }

        private void OnUiManagerInit()
        {
            Menu.InitUi();
            VRUtils.Init();
            GetSetWorld.Init();
            MelonCoroutines.Start(UiUtils.AllowToolTipTextColor());
            if (EnableTeleportIndicator.Value)
                LR = GeneralUtils.GetPtrObj().GetOrAddComponent<TPLocationIndicator>();

            if (EnableTeleportIndicator.Value && !runOnce) { // null checks for less errors and breakage
                if (LR == null) LR = GeneralUtils.GetPtrObj().GetOrAddComponent<TPLocationIndicator>();
                if (LR != null) TPLocationIndicator.Toggle(false);
                runOnce = true;
            }
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
                && !ActionMenu.AMApiOutdated) // if gompo's mod is not outdated
            {
                MelonLogger.Msg(ConsoleColor.Yellow, "You may have to change or reload your current world to allow the ActionMenu to show.");
                ActionMenu.InitUi();
            }
        }

        bool runOnce;

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
                    WorldActions.OnLeftWorld();
                    break;
            }
        }

        public override void OnApplicationQuit() { preferRightHand.Value = VRUtils.preferRightHand; }

        public override void OnUpdate()
        {
            VRUtils.OnUpdate();
            // This check is to keep the menu Disabled in Disallowed worlds, this was super easy to patch into or use UnityExplorer to re-enable the button
            if (!WorldActions.WorldAllowed && NewPatches.IsQMOpen && 
                (Menu.menu.getMainButton().getGameObject().GetComponent<Button>().enabled || Menu.VRTeleport.getGameObject().GetComponent<Button>().enabled) &&
                Menu.menu.getMainButton().getGameObject().GetComponentInChildren<Image>().sprite == ResourceManager.badIcon)
            {
                Menu.menu.getMainButton().Disabled(true);
                Menu.VRTeleport.Disabled(true);
                Menu.userSel_TPto.Disabled(true);
            }
            
        }

        private IEnumerator GetAssembly()
        {
            Assembly assemblyCSharp = null;
            while (true) {
                assemblyCSharp = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");
                if (assemblyCSharp == null)
                    yield return null;
                else
                    break;
            }

            MelonCoroutines.Start(WaitForUiManagerInit(assemblyCSharp));
        }

        private IEnumerator WaitForUiManagerInit(Assembly assemblyCSharp)
        {
            Type vrcUiManager = assemblyCSharp.GetType("VRCUiManager");
            PropertyInfo uiManagerSingleton = vrcUiManager.GetProperties().First(pi => pi.PropertyType == vrcUiManager);
            while (uiManagerSingleton.GetValue(null) == null) yield return null;
            OnUiManagerInit(); // Run UI
        }
    }
}