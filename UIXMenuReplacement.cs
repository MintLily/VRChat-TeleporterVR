using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using TeleporterVR.Logic;
using TeleporterVR.Rendering;
using TeleporterVR.Utils;
using UIExpansionKit.API;
using UnityEngine;
using UnityEngine.UI;
using static TeleporterVR.Menu;

namespace TeleporterVR {
    class UIXMenuReplacement {
        internal static ICustomShowableLayoutedMenu menu = ExpansionKitApi.CreateCustomQuickMenuPage(LayoutDescription.QuickMenu4Columns);

        static string color(string c, string s) { return $"<color={c}>{s}</color> "; }
        static Dictionary<string, Transform> buttons = new Dictionary<string, Transform>();
        static Dictionary<string, Transform> permbuttons = new Dictionary<string, Transform>();
        internal static GameObject MainMenuBTN, TPVRButton;
        static bool runOnce_start;

        static void UIXButton(int UIXExpandedMenuENUM, string UIXGetMethod, string buttonText, Action action, Action<GameObject> goAction) {
            MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod(UIXGetMethod,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] {
                    UIXExpandedMenuENUM, buttonText, new Action(() => action()), new Action<GameObject>((GameObject obj) => goAction(obj))
                });
        }

        public static void Init() {
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton(color("#13cf13", "TeleporterVR") + "\nMenu", () => {
                if (!runOnce_start) {
                    TheMenu();
                    runOnce_start = true;
                    menu.Show();
                } else if (runOnce_start) {
                    menu.Show();
                    UpdateText();
                    UpdateWorldStatusText();
                }
            }, (obj) => {
                MainMenuBTN = obj;
                obj.SetActive(Main.UIXMenu.Value);
            });

            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton(VRUtils.active ? Language.theWord_Teleport + color("#00ff00", "\nON") : 
                Language.theWord_Teleport + color("red", "\nOFF"), () => {
                    if (WorldActions.WorldAllowed) {
                        VRUtils.active = !VRUtils.active;
                        TPLocationIndicator.Toggle();
                    }
                    UpdateText();
                }, (obj2) => {
                    permbuttons["TPActive_1"] = obj2.transform;
                    TPVRButton = obj2;
                    obj2.SetActive(Main.UIXTPVR.Value);
                });
            Main.Log("Finished creating UIXMenus", Main.isDebug);
        }

        static void TheMenu() {
            buttons.Clear();
            menu.AddSimpleButton(color("red", "Close") + "Menu", () => menu.Hide());
            menu.AddSimpleButton($"World\n{(WorldActions.WorldAllowed ? color("#00ff00", "Allowed") : color("red", "Disallowed"))}", null, (button) => buttons["WorldStatus"] = button.transform);
            menu.AddSpacer();
            menu.AddSimpleButton(VRUtils.active ? Language.theWord_Teleport + color("#00ff00", "\nON") : Language.theWord_Teleport + color("red", "\nOFF"), () => {
                if (WorldActions.WorldAllowed) {
                    VRUtils.active = !VRUtils.active;
                    TPLocationIndicator.Toggle();
                }
                UpdateText();
            }, (button) => buttons["TPActive_2"] = button.transform);

            menu.AddSimpleButton(Language.TPtoName_Text, () => OpenKeyboardForPlayerTP(), (button) => buttons["KeyboardTP"] = button.transform);
            menu.AddSimpleButton(Language.TPtoCoord_Text, () => OpenKeyboardForCoordTP(), (button) => buttons["CoordTP"] = button.transform);
            menu.AddSpacer();
            menu.AddSimpleButton(VRUtils.preferRightHand ? Language.preferedHanded_Text_ON : Language.preferedHanded_Text_OFF, () => {
                VRUtils.preferRightHand = !VRUtils.preferRightHand;
                VRTeleport.setToolTip(Language.perferedHand_Tooltip);
                preferdHand.setToolTip(Language.perferedHand_Tooltip);
                UpdateText();
            }, (button) => buttons["preferRightHand"] = button.transform);

            menu.AddSimpleButton(Language.SavePos + "\n1", () => SaveAction(1), (button) => buttons["Save_1"] = button.transform);
            menu.AddSimpleButton(Language.SavePos + "\n2", () => SaveAction(2), (button) => buttons["Save_2"] = button.transform);
            menu.AddSimpleButton(Language.SavePos + "\n3", () => SaveAction(3), (button) => buttons["Save_3"] = button.transform);
            menu.AddSimpleButton(Language.SavePos + "\n4", () => SaveAction(4), (button) => buttons["Save_4"] = button.transform);

            menu.AddSimpleButton(Language.LoadPos + "\n1", () => LoadAction(1), (button) => buttons["Load_1"] = button.transform);
            menu.AddSimpleButton(Language.LoadPos + "\n2", () => LoadAction(2), (button) => buttons["Load_2"] = button.transform);
            menu.AddSimpleButton(Language.LoadPos + "\n3", () => LoadAction(3), (button) => buttons["Load_3"] = button.transform);
            menu.AddSimpleButton(Language.LoadPos + "\n4", () => LoadAction(4), (button) => buttons["Load_4"] = button.transform);

            menu.AddSpacer();
            menu.AddSimpleButton("Discord", () => OpenWebpage("https://discord.gg/qkycuAMUGS"));
            menu.AddSimpleButton("GitHub", () => OpenWebpage(BuildInfo.DownloadLink));
            menu.AddSpacer();

            Main.Log("Finished creating UIXMenus", Main.isDebug);
        }

        static string text(string buttonName, string text) {
            if (buttons[buttonName] != null)
                return buttons[buttonName].GetComponentInChildren<Text>().text = text;
            else return null;
        }

        internal static void UpdateWorldStatusText() {
            try {
                text("WorldStatus", $"World\n{(WorldActions.WorldAllowed ? color("#00ff00", "Allowed") : color("red", "Disallowed"))}");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }
        }

        public static void UpdateText() {
            if (menu == null) return;
            try {
                if (permbuttons["TPActive_1"] != null)
                    permbuttons["TPActive_1"].GetComponentInChildren<Text>().text = VRUtils.active ? Language.theWord_Teleport + color("#00ff00", "\nON") :
                        Language.theWord_Teleport + color("red", "\nOFF");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("TPActive_2", VRUtils.active ? Language.theWord_Teleport + color("#00ff00", "\nON") : Language.theWord_Teleport + color("red", "\nOFF"));
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("KeyboardTP", Language.TPtoName_Text);
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("CoordTP", Language.TPtoCoord_Text);
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("preferRightHand", VRUtils.preferRightHand ? Language.preferedHanded_Text_ON : Language.preferedHanded_Text_OFF);
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("Save_1", Language.SavePos + "\n1");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("Save_2", Language.SavePos + "\n2");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("Save_3", Language.SavePos + "\n3");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("Save_4", Language.SavePos + "\n4");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("Load_1", Language.LoadPos + "\n1");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("Load_2", Language.LoadPos + "\n2");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("Load_3", Language.LoadPos + "\n3");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }

            try {
                text("Load_4", Language.LoadPos + "\n4");
            } catch (Exception e) { MelonLogger.Error($"{e}"); }
        }
    }
}
