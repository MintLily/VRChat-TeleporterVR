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
using ActionMenuApi.Pedals;
using TeleporterVR.Logic;
using TeleporterVR.Utils;
using MelonLoader;

namespace TeleporterVR
{
    public class ActionMenu
    {
        private static readonly string[] AMApiOutdatedVersions = { "0.1.0" , "0.1.2"};
        public static bool hasAMApiInstalled, AMApiOutdated, hasStarted;
        public static PedalOption VRTP, TP2Name, TP2Coord, Save, Load;
        internal static PedalSubMenu subMenu;

        public static void InitUi()
        {
            if (MelonHandler.Mods.Any(m => m.Info.Name.Equals("ActionMenuApi")))
            {
                hasAMApiInstalled = true;
                if (MelonHandler.Mods.Single(m => m.Info.Name.Equals("ActionMenuApi")).Info.Version.Equals(AMApiOutdatedVersions))
                {
                    AMApiOutdated = true;
                    MelonLogger.Warning("ActionMenuApi Outdated. older verions are not supported, please update the other mod.");
                    return;
                }
            } else return;
            if (!Main.ActionMenuApiIntegration.Value) return;

            subMenu = VRCActionMenuPage.AddSubMenu(ActionMenuPage.Main, "<color=#13cf13>TeleporterVR</color>", () =>
            {
                VRTP = CustomSubMenu.AddToggle("VR " + Language.theWord_Teleport, false,
                (bool choice) => Menu.VRTeleport.setToggleState(choice, true), ResourceManager.AMVRTP);

                TP2Name = CustomSubMenu.AddButton(Language.TPtoName_Text, () => Menu.OpenKeyboardForPlayerTP(), ResourceManager.AMMain);

                TP2Coord = CustomSubMenu.AddButton(Language.TPtoCoord_Text, () => Menu.OpenKeyboardForCoordTP(), ResourceManager.AMMain);

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
            subMenu.locked = true;
            hasStarted = true;
            if (Main.isDebug) MelonLogger.Msg(ConsoleColor.Green, "Finished creating ActionMenu");
        }

        public static IEnumerator UpdateIcon(bool ignoreWait = true)
        {
            if (!ignoreWait) yield return new WaitForSeconds(1f);
            subMenu.icon = WorldActions.WorldAllowed ? ResourceManager.AMMain : ResourceManager.AMBad;
            yield break;
        }
    }
}
