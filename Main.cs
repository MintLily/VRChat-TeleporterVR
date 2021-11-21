using MelonLoader;
using System;
using System.Collections;
using UnityEngine.UI;
using TeleporterVR.Utils;
using TeleporterVR.Logic;
using TeleporterVR.Patches;
using UIExpansionKit.API;
using TeleporterVR.Rendering;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace TeleporterVR
{
    public static class BuildInfo
    {
        public const string Name = "TeleporterVR";
        public const string Author = "Janni, Lily";
        public const string Company = null;
        public const string Version = "4.7.0";
        public const string DownloadLink = "https://github.com/MintLily/VRChat-TeleporterVR";
        public const string Description = "Easy Utility that allows you to teleport in various different ways while being VR compliant.";
    }

    public class Main : MelonMod
    {
        private MelonMod Instance;
        public static bool isDebug;
        private static TPLocationIndicator LR;
        public static MelonPreferences_Category melon;
        public static MelonPreferences_Entry<bool> /*visible, */preferRightHand, VRTeleportVisible, ActionMenuApiIntegration, EnableTeleportIndicator, EnableDesktopTP, UIXTPVR;
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
            //visible = melon.CreateEntry("UserInteractTPButtonVisible", true, "Is Teleport Button Visible (on User Select)");
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
            EnableDesktopTP = melon.CreateEntry("EnableDesktopTP", false, "Allows you to teleport to your cursor (desktop only)\n[LeftShift + T]");
            UIXTPVR = melon.CreateEntry("ShowUIXTPVRButton", false, "Put TPVR button on UIX Menu");

            ResourceManager.Init();
            NewPatches.SetupPatches();
            Language.InitLanguageChange();
            CreateListener.Init();
            //SetupCustomToggle.Init();
            ActionMenu.InitUi();
            RenderingIndicator.Init();
            UIXMenuReplacement.Init();

            MelonLogger.Msg("Initialized!");

            if (OverrideLanguage.Value == "no") OverrideLanguage.Value = "no_bm";
        }

        private void OnUiManagerInit()
        {
            VRUtils.Init();
            GetSetWorld.Init();
            if (EnableTeleportIndicator.Value)
                LR = GeneralUtils.GetPtrObj().GetOrAddComponent<TPLocationIndicator>();

            if (EnableTeleportIndicator.Value && !runOnce) { // null checks for less errors and breakage
                if (LR == null) LR = GeneralUtils.GetPtrObj().GetOrAddComponent<TPLocationIndicator>();
                if (LR != null) TPLocationIndicator.Toggle(false);
                runOnce = true;
            }
            CreateListener.UiInit();
            //MelonCoroutines.Start(SetupCustomToggle.SetPrefabOnQM());
        }

        public override void OnPreferencesSaved()
        {
            if (UIXMenuReplacement.runOnce_start) UIXMenuReplacement.UpdateText();
            preferRightHand.Value = VRUtils.preferRightHand;
            if (ActionMenuApiIntegration.Value // if true
                && !ActionMenu.hasStarted // if has not started yet
                && ActionMenu.hasAMApiInstalled // if gompo's mod is installed
                && !ActionMenu.AMApiOutdated) // if gompo's mod is not outdated
            {
                MelonLogger.Msg(ConsoleColor.Yellow, "You may have to change or reload your current world to allow the ActionMenu to show.");
                ActionMenu.InitUi();
            }
            //CustomToggle.UpdateColorTheme();

            try { UIXMenuReplacement.TPVRButton.SetActive(UIXTPVR.Value); } catch { }
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
                    MelonCoroutines.Start(GetSetWorld.DelayedLoad());
                    WorldActions.OnLeftWorld();
                    break;
            }
        }

        public override void OnApplicationQuit() => preferRightHand.Value = VRUtils.preferRightHand;

        public override void OnUpdate()
        {
            VRUtils.OnUpdate();
            // This check is to keep the menu Disabled in Disallowed worlds, this was super easy to patch into or use UnityExplorer to re-enable the button
            if (!WorldActions.WorldAllowed && !(NewPatches.IsQMOpen || NewPatches.IsQMOpen))
                VRUtils.active = false;
            DesktopUtils.OnUpdate();
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

        public static void Log(string s, bool isDebug = false, bool ErrorAlt = false) {
            if (isDebug) MelonLogger.Msg(ErrorAlt ? ConsoleColor.Red : ConsoleColor.Green, s);
            else MelonLogger.Msg(s);
        }
    }
}
// Janni is a cutie~