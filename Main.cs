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
        public static MelonPreferences_Entry<bool> visible, preferRightHand, VRTeleportVisible, ActionMenuApiIntegration, EnableTeleportIndicator, EnableDesktopTP, UIXMenu, UIXTPVR, UIXTPToPlayer;
        public static MelonPreferences_Entry<int> userSel_x, userSel_y;
        public static MelonPreferences_Entry<string> OverrideLanguage, IndicatorHexColor;
        internal static int VRCBuildNumber = 1134; // aka target game version

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
            EnableDesktopTP = melon.CreateEntry("EnableDesktopTP", false, "Allows you to teleport to your cursor (desktop only)\n[LeftShift + T]");
            UIXMenu = melon.CreateEntry("ShowUIXMenuButton", false, "Use a Menu built by UIExpansionKit");
            UIXTPVR = melon.CreateEntry("ShowUIXTPVRButton", false, "Put TPVR button on UIX Menu");
            UIXTPToPlayer = melon.CreateEntry("ShowUIXTPToPlayerButton", false, "Put TP button on UIX User Selected Menu");

            ResourceManager.Init();
            NewPatches.SetupPatches();
            Language.InitLanguageChange();
            CreateListener.Init();
            ActionMenu.InitUi();
            RenderingIndicator.Init();
            UIXMenuReplacement.Init();

            MelonLogger.Msg("Initialized!");

            if (OverrideLanguage.Value == "no") OverrideLanguage.Value = "no_bm";
        }

        private void OnUiManagerInit()
        {
            try { VRCBuildNumber = Resources.FindObjectsOfTypeAll<VRCApplicationSetup>().First().field_Public_Int32_0; } catch {
                MelonLogger.Error("VRCApplicationSetup = MonoBehaviourPublicApStInStBoGaBoInObStUnique is most likely null in this update");
            }
            try { Menu.InitUi(); } catch (Exception e) {
                if (VRCBuildNumber > 1134) {
                    MelonLogger.Warning("This Mod does not have a dedicated UI for the UI Update");
                    Log($"{e}", isDebug, true);
                } else MelonLogger.Error($"{e}");
            }
            VRUtils.Init();
            GetSetWorld.Init();
            try { MelonCoroutines.Start(UiUtils.AllowToolTipTextColor()); } catch (Exception t) {
                if (VRCBuildNumber > 1134) {
                    MelonLogger.Warning("This Mod does not have a dedicated UI for the UI Update");
                    MelonLogger.Error($"{t}");
                } else MelonLogger.Error($"{t}");
            }
            if (EnableTeleportIndicator.Value)
                LR = GeneralUtils.GetPtrObj().GetOrAddComponent<TPLocationIndicator>();

            if (EnableTeleportIndicator.Value && !runOnce) { // null checks for less errors and breakage
                if (LR == null) LR = GeneralUtils.GetPtrObj().GetOrAddComponent<TPLocationIndicator>();
                if (LR != null) TPLocationIndicator.Toggle(false);
                runOnce = true;
            }
            CreateListener.UiInit();
        }

        public override void OnPreferencesSaved()
        {
            if (VRCBuildNumber <= 1134) {
                Menu.UpdateUserSelectTeleportButton();
                Menu.UpdateVRTeleportButton();
                Menu.UpdateLeftRightHandButton();
                Menu.UpdateButtonText();
            }
            if (UIXMenuReplacement.menu != null && UIXMenu.Value) UIXMenuReplacement.UpdateText();
            preferRightHand.Value = VRUtils.preferRightHand;
            if (ActionMenuApiIntegration.Value // if true
                && !ActionMenu.hasStarted // if has not started yet
                && ActionMenu.hasAMApiInstalled // if gompo's mod is installed
                && !ActionMenu.AMApiOutdated) // if gompo's mod is not outdated
            {
                MelonLogger.Msg(ConsoleColor.Yellow, "You may have to change or reload your current world to allow the ActionMenu to show.");
                ActionMenu.InitUi();
            }

            try { UIXMenuReplacement.MainMenuBTN.SetActive(UIXMenu.Value); } catch { }
            try { UIXMenuReplacement.TPVRButton.SetActive(UIXTPVR.Value); } catch { }
            try { UIXMenuReplacement.TPToPlayerButton.SetActive(UIXTPToPlayer.Value); } catch { }
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

        public override void OnApplicationQuit() => preferRightHand.Value = VRUtils.preferRightHand;

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
                VRUtils.active = false;
            }
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