using MelonLoader;
using System;
using TeleporterVR.Utils;
using TeleporterVR.Logic;
using TeleporterVR.Patches;
using UIExpansionKit.API;
using TeleporterVR.Rendering;

namespace TeleporterVR
{
    public static class BuildInfo
    {
        public const string Name = "TeleporterVR";
        public const string Author = "Janni, Lily";
        public const string Company = null;
        public const string Version = "4.12.0";
        public const string DownloadLink = "https://github.com/MintLily/VRChat-TeleporterVR";
        public const string Description = "Easy Utility that allows you to teleport in various different ways while being VR compliant.";
    }

    public class Main : MelonMod {
        public static bool IsDebug;
        private static TPLocationIndicator _lr;
        public static MelonPreferences_Category Melon;
        public static MelonPreferences_Entry<bool> PreferRightHand, VRTeleportVisible, ActionMenuApiIntegration, EnableTeleportIndicator, EnableDesktopTp;//, UIXTPVR, UIXMenu;
        public static MelonPreferences_Entry<string> OverrideLanguage, IndicatorHexColor;
        internal static readonly MelonLogger.Instance Logger = new (BuildInfo.Name, ConsoleColor.Green);
        private static int _scenesLoaded = 0;

        public override void OnApplicationStart() {
            if (MelonDebug.IsEnabled() || Environment.CommandLine.Contains("--vrt.debug")) {
                IsDebug = true;
                Logger.Msg(ConsoleColor.Green, "Debug mode is active");
            }

            Melon = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            //visible = melon.CreateEntry("UserInteractTPButtonVisible", true, "Is Teleport Button Visible (on User Select)");
            PreferRightHand = Melon.CreateEntry("preferRightHand", true, "Right Handed");
            VRTeleportVisible = Melon.CreateEntry("VRTeleportVisible", true, "Is User Selected Teleport Button visible");
            OverrideLanguage = Melon.CreateEntry("overrideLanguage", "off", "Override Language");
            ExpansionKitApi.RegisterSettingAsStringEnum(Melon.Identifier, OverrideLanguage.Identifier, 
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
            ActionMenuApiIntegration = Melon.CreateEntry("ActionMenuApiIntegration", false, "Has ActionMenu Support\n(disable requires game restart)");
            EnableTeleportIndicator = Melon.CreateEntry("EnableTeleportIndicator", true, "Shows a circle to where you will teleport to");
            IndicatorHexColor = Melon.CreateEntry("IndicatorHEXColor", "2dff2d", "Indicator Color (HEX Value [\"RRGGBB\"])");
            EnableDesktopTp = Melon.CreateEntry("EnableDesktopTP", false, "Allows you to teleport to your cursor (desktop only)\n[LeftShift + T]");
            //UIXTPVR = melon.CreateEntry("ShowUIXTPVRButton", false, "Put TPVR button on UIX Menu");

            ResourceManager.Init();
            NewPatches.SetupPatches();
            Language.InitLanguageChange();
            CreateListener.Init();
            ActionMenu.InitUi();
            RenderingIndicator.Init();
            //if (UIXMenu.Value || ReMod_Core_Downloader.failed)
            //    UIXMenuReplacement.Init();

            Logger.Msg("Initialized!");

            if (OverrideLanguage.Value == "no") OverrideLanguage.Value = "no_bm";
        }

        private void OnUiManagerInit()
        {
            VRUtils.Init();
            GetSetWorld.Init();
            if (EnableTeleportIndicator.Value)
                _lr = GeneralUtils.GetPtrObj().GetOrAddComponent<TPLocationIndicator>();

            if (EnableTeleportIndicator.Value && !_ranOnce) { // null checks for less errors and breakage
                if (_lr == null) _lr = GeneralUtils.GetPtrObj().GetOrAddComponent<TPLocationIndicator>();
                if (_lr != null) TPLocationIndicator.Toggle(false);
                _ranOnce = true;
            }
            CreateListener.UiInit();
            //MelonCoroutines.Start(SetupCustomToggle.SetPrefabOnQM());
            try {
                MelonCoroutines.Start(NewUi.OnQuickMenu());
            }
            catch (Exception e) {
                Error($"Something on the UI (QuickMenu building) method failed! Did ReMod.Core fully load properly?\n{e}");
            }
        }

        public override void OnPreferencesSaved()
        {
            //if (UIXMenuReplacement.runOnce_start && UIXMenu.Value) UIXMenuReplacement.UpdateText();
            //MelonPreferences.GetEntry<bool>(melon.Identifier, preferRightHand.Identifier).Value = VRUtils.preferRightHand;
            if (ActionMenuApiIntegration.Value     // if true
                && !ActionMenu.hasStarted          // if has not started yet
                && ActionMenu.hasAMApiInstalled    // if gompo's mod is installed
                && !ActionMenu.AMApiOutdated)      // if gompo's mod is not outdated
            {
                Logger.Msg(ConsoleColor.Yellow, "You may have to change or reload your current world to allow the ActionMenu to show.");
                ActionMenu.InitUi();
            }

            //if (UIXMenu.Value) {
            //    try { UIXMenuReplacement.TPVRButton.SetActive(UIXTPVR.Value); } catch (Exception e) { Error(e, IsDebug); }
            //    try { UIXMenuReplacement.UserTPButton.SetActive(VRTeleportVisible.Value); } catch (Exception e) { Error(e, IsDebug); }
            //    try { if (UIXMenuReplacement.runOnce_start) UIXMenuReplacement.UpdateText(); } catch (Exception e) { Error(e, IsDebug); }
            //} else
            NewUi.OnPrefSave();
        }

        private bool _ranOnce;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            if (_scenesLoaded <= 2) {
                _scenesLoaded++;
                if (_scenesLoaded == 2)
                    OnUiManagerInit();
            }
            switch (buildIndex) {
                case 0: case 1: break;
                default:
                    MelonCoroutines.Start(GetSetWorld.DelayedLoad());
                    break;
            }
            CheckWorldAllowed.WorldChange(-1);
        }

        //public override void OnApplicationQuit() => preferRightHand.Value = VRUtils.preferRightHand;

        public override void OnUpdate()
        {
            VRUtils.OnUpdate();
            // This check is to keep the menu Disabled in Disallowed worlds, this was super easy to patch into or use UnityExplorer to re-enable the button
            if (!CheckWorldAllowed.RiskyFunctionAllowed && !(NewPatches.IsQmOpen || NewPatches.IsQmOpen))
                VRUtils.active = false;
            DesktopUtils.OnUpdate();
        }

        public static void Log(string s) => Logger.Msg(s);
        
        public static void Error(object s, bool isDebug = false) {
            var c = Console.ForegroundColor;
            Logger.Msg(isDebug ? ConsoleColor.DarkMagenta : c, s);
        }

        public static void Debug(string s) {
            if (!IsDebug) return;
            Logger.Msg(ConsoleColor.DarkMagenta, s);
        }
    }
}
// Janni is a cutie~