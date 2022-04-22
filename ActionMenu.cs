using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ActionMenuApi;
using ActionMenuApi.Api;
using TeleporterVR.Logic;
using TeleporterVR.Utils;
using MelonLoader;

namespace TeleporterVR
{
    public class ActionMenu
    {
        private static readonly string[] AmApiOutdatedVersions = { "0.1.0" , "0.1.2", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.3.0", "0.3.1", "0.3.2", "0.3.3", "0.3.4" };
        // Tested versions to be good => 0.3.5
        public static bool hasAMApiInstalled, AMApiOutdated, hasStarted;
        private static PedalOption VRTP, TP2Name, TP2Coord, Save, Load;

        public static void InitUi()
        {
            if (MelonHandler.Mods.Any(m => m.Info.Name.Equals("ActionMenuApi"))) {
                hasAMApiInstalled = true;
                if (!Main.ActionMenuApiIntegration.Value) return;
                if (MelonHandler.Mods.Single(m => m.Info.Name.Equals("ActionMenuApi")).Info.Version.Equals(AmApiOutdatedVersions))
                {
                    AMApiOutdated = true;
                    Main.Logger.Warning("ActionMenuApi Outdated. older versions are not supported, please update the other mod.");
                } else BuildActionMenu();
            }
        }

        private static void BuildActionMenu()
        {
            AMSubMenu.subMenu = VRCActionMenuPage.AddSubMenu(ActionMenuPage.Main, "<color=#13cf13>TeleporterVR</color>", () =>
                {
                    VRTP = CustomSubMenu.AddToggle("VR " + Language.theWord_Teleport, VRUtils.active, VRUtils.ToggleVRTeleport, ResourceManager.AMVRTP);

                    TP2Name = CustomSubMenu.AddButton(Language.TPtoName_Text, Menu.OpenKeyboardForPlayerTP, ResourceManager.AMMain);

                    TP2Coord = CustomSubMenu.AddButton(Language.TPtoCoord_Text, Menu.OpenKeyboardForCoordTP, ResourceManager.AMMain);

                    Save = CustomSubMenu.AddSubMenu(Language.SavePos, () =>
                    {
                        CustomSubMenu.AddButton(Language.SavePos, () => Menu.SaveAction(1), ResourceManager.AMSL1);
                        CustomSubMenu.AddButton(Language.SavePos, () => Menu.SaveAction(2), ResourceManager.AMSL2);
                        CustomSubMenu.AddButton(Language.SavePos, () => Menu.SaveAction(3), ResourceManager.AMSL3);
                        CustomSubMenu.AddButton(Language.SavePos, () => Menu.SaveAction(4), ResourceManager.AMSL4);
                    }, ResourceManager.AMSave);

                    Load = CustomSubMenu.AddSubMenu(Language.LoadPos, () =>
                    {
                        CustomSubMenu.AddButton(Language.LoadPos, () => Menu.LoadAction(1), ResourceManager.AMSL1);
                        CustomSubMenu.AddButton(Language.LoadPos, () => Menu.LoadAction(2), ResourceManager.AMSL2);
                        CustomSubMenu.AddButton(Language.LoadPos, () => Menu.LoadAction(3), ResourceManager.AMSL3);
                        CustomSubMenu.AddButton(Language.LoadPos, () => Menu.LoadAction(4), ResourceManager.AMSL4);
                    }, ResourceManager.AMSave);
                }, ResourceManager.AMMain);
                AMSubMenu.subMenu.locked = true;
                hasStarted = true;
                if (Main.IsDebug) Main.Logger.Msg(ConsoleColor.Green, "Finished creating ActionMenu");
        }

        public static IEnumerator UpdateIcon(bool ignoreWait = true)
        {
            if (!ignoreWait) yield return new WaitForSeconds(1f);
            try { AMSubMenu.subMenu.icon = CheckWorldAllowed.RiskyFunctionAllowed ? ResourceManager.AMMain : ResourceManager.AMBad; }
            catch { if (hasAMApiInstalled) Main.Logger.Error("Failed to change subMenu Icon"); }
            try { AMUtils.RefreshActionMenu(); } catch { if (hasAMApiInstalled) Main.Logger.Error("Failed to Refresh ActionMenu"); }
            yield break;
        }

        public static void CheckForRiskyFunctions(bool locked)
        {
            try { AMSubMenu.subMenu.locked = locked; } 
            catch { if (hasAMApiInstalled) Main.Logger.Error("ActionMenu subMenu could not be locked"); }
        }
    }
}
