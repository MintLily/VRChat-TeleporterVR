using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ActionMenuApi;
using TeleporterVR.Logic;
using TeleporterVR.Utils;
using MelonLoader;

namespace TeleporterVR
{
    public class ActionMenu
    {
        private static readonly string[] AMApiOutdatedVersions = { "0.1.0" };//, ""};
        public static bool hasAMApiInstalled, AMApiOutdated, hasStarted;
        public static PedalOption VRTP, TP2Name, TP2Coord, Save, Load;

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

            AMAPI.AddSubMenuToMenu(ActionMenuApi.Types.ActionMenuPageType.Main, "<color=#13cf13>TeleporterVR</color>", () =>
            {
                VRTP = AMAPI.AddTogglePedalToSubMenu("VR" + Language.theWord_Teleport, false, 
                    (bool choice) => Menu.VRTeleport.setToggleState(choice, true), ResourceManager.AMVRTP);

                TP2Name = AMAPI.AddButtonPedalToSubMenu(Language.TPtoName_Text, () => Menu.OpenKeyboardForPlayerTP(), ResourceManager.AMMain);

                TP2Coord = AMAPI.AddButtonPedalToSubMenu(Language.TPtoCoord_Text, () => Menu.OpenKeyboardForCoordTP(), ResourceManager.AMMain);

                Save = AMAPI.AddSubMenuToSubMenu(Language.SavePos, () => 
                {
                    AMAPI.AddButtonPedalToSubMenu(Language.SavePos, () => Menu.SaveAction(1), ResourceManager.AMSL1);
                    AMAPI.AddButtonPedalToSubMenu(Language.SavePos, () => Menu.SaveAction(2), ResourceManager.AMSL2);
                    AMAPI.AddButtonPedalToSubMenu(Language.SavePos, () => Menu.SaveAction(3), ResourceManager.AMSL3);
                    AMAPI.AddButtonPedalToSubMenu(Language.SavePos, () => Menu.SaveAction(4), ResourceManager.AMSL4);
                }, ResourceManager.AMSave);

                Load = AMAPI.AddSubMenuToSubMenu(Language.LoadPos, () => 
                {
                    AMAPI.AddButtonPedalToSubMenu(Language.LoadPos, () => Menu.LoadAction(1), ResourceManager.AMSL1);
                    AMAPI.AddButtonPedalToSubMenu(Language.LoadPos, () => Menu.LoadAction(2), ResourceManager.AMSL2);
                    AMAPI.AddButtonPedalToSubMenu(Language.LoadPos, () => Menu.LoadAction(3), ResourceManager.AMSL3);
                    AMAPI.AddButtonPedalToSubMenu(Language.LoadPos, () => Menu.LoadAction(4), ResourceManager.AMSL4);
                }, ResourceManager.AMLoad);
            }, ResourceManager.AMMain);
            hasStarted = true;
            if (Main.isDebug) MelonLogger.Msg(ConsoleColor.Green, "Finished creating ActionMenu");
        }
    }
}
