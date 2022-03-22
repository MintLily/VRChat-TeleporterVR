using System.Collections;
using MelonLoader;
using ReMod.Core.UI.QuickMenu;
using ReMod.Core.VRChat;
using TeleporterVR.Logic;
using TeleporterVR.Rendering;
using TeleporterVR.Utils;
using UnityEngine;
using VRC.UI.Core;
using static TeleporterVR.Menu;

namespace TeleporterVR {
    public class NewUi {
        internal static IEnumerator OnQuickMenu() {
            while (UIManager.field_Private_Static_UIManager_0 == null) yield return null;
            while (GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null) yield return null;
            ReMod.Core.Unity.EnableDisableListener.RegisterSafe();
            BuildUi();
        }

        private static ReCategoryPage TPVR;
        public static ReMenuCategory MainCat, Waypoints;
        internal static ReMenuButton TPName, TPCoord, wp1, wp2, wp3, wp4, wpl1, wpl2, wpl3, wpl4, UserSelTP;
        internal static ReMenuToggle HandChange, TPAction;
        private static bool built;
        
        private static void BuildUi() {
            TPVR = new ReCategoryPage("TeleporterVR", true);
            TPVR.GameObject.SetActive(false);

            ReTabButton.Create("TPVRTab", "Open the TeleporterVR Menu", "TeleporterVR", ResourceManager.Tab);

            MainCat = TPVR.AddCategory("TPVR Actions", false);
            TPName = MainCat.AddButton(Language.TPtoName_Text, $"Open keyboard to {Language.TPtoName_Text}", OpenKeyboardForPlayerTP, ResourceManager.keyboard);
            TPCoord = MainCat.AddButton(Language.TPtoCoord_Text, $"Open keyboard to {Language.TPtoCoord_Text}", OpenKeyboardForCoordTP, ResourceManager.mapmarker);
            HandChange = MainCat.AddToggle(Language.preferedHanded_Text_ON, $"Toggle {Language.preferedHanded_Text_ON} or {Language.preferedHanded_Text_OFF}", b => {
                MelonPreferences.GetEntry<bool>(Main.melon.Identifier, Main.preferRightHand.Identifier).Value = b;
            });
            HandChange.Toggle(Main.preferRightHand.Value);
            TPAction = MainCat.AddToggle(Language.theWord_Teleport, $"Activate {Language.theWord_Teleport}", b => {
                if (WorldActions.WorldAllowed) {
                    VRUtils.active = !VRUtils.active;
                    TPLocationIndicator.Toggle();
                }
            });
            //TPAction.Toggle(VRUtils.active);

            Waypoints = TPVR.AddCategory("Waypoints");
            wp1 = Waypoints.AddButton(Language.SavePos, $"{Language.SavePos} 1", () => { SaveAction(1); },
                ResourceManager.one);
            wp2 = Waypoints.AddButton(Language.SavePos, $"{Language.SavePos} 2", () => { SaveAction(2); },
                ResourceManager.two);
            wp3 = Waypoints.AddButton(Language.SavePos, $"{Language.SavePos} 3", () => { SaveAction(3); },
                ResourceManager.three);
            wp4 = Waypoints.AddButton(Language.SavePos, $"{Language.SavePos} 4", () => { SaveAction(4); },
                ResourceManager.four);
            
            wpl1 = Waypoints.AddButton(Language.LoadPos, $"{Language.LoadPos} 1", () => { LoadAction(1); },
                ResourceManager.one);
            wpl2 = Waypoints.AddButton(Language.LoadPos, $"{Language.LoadPos} 2", () => { LoadAction(2); },
                ResourceManager.two);
            wpl3 = Waypoints.AddButton(Language.LoadPos, $"{Language.LoadPos} 3", () => { LoadAction(3); },
                ResourceManager.three);
            wpl4 = Waypoints.AddButton(Language.LoadPos, $"{Language.LoadPos} 4", () => { LoadAction(4); },
                ResourceManager.four);

            var t = TPVR.AddCategory("Information");
            t.AddButton($"Mod v{BuildInfo.Version}", "", () => Main.Logger.Msg("You're Cute!"), ResourceManager.ver);
            t.AddButton("VRCMG", "Opens webpage to VRChat Modding Group's Discord Invite", () => OpenWebpage("https://discord.gg/7EQCmgrUnH"), ResourceManager.DiscordLogo);
            t.AddButton("GitHub", "Opens a webpage to the Mod\'s GitHub project", () => OpenWebpage(BuildInfo.DownloadLink), ResourceManager.GitHubLogo);
            
            BuildUserSelectMenu();
        }

        private static ReMenuCategory userSelectCategory;
        private static void BuildUserSelectMenu() {
            var _Menu_SelectedUser_Local = QuickMenuEx.Instance.field_Public_Transform_0.Find("Window/QMParent/Menu_SelectedUser_Local");
            var TheSelectedUserMenu = _Menu_SelectedUser_Local.Find("ScrollRect/Viewport/VerticalLayoutGroup");
            userSelectCategory = new ReMenuCategory("TPVR", TheSelectedUserMenu);

            UserSelTP = userSelectCategory.AddButton(Language.theWord_Teleport, $"{Language.theWord_Teleport} to Selected User", () => {
                if (WorldActions.WorldAllowed) 
                    PlayerActions.Teleport(PlayerActions.SelVRCPlayer());
            }, ResourceManager.mapmarker);
            built = true;
        }

        internal static void OnPrefSave() {
            if (!built) return;
            if (TPVR != null && HandChange != null)
                HandChange.Toggle(Main.preferRightHand.Value);
            
            if (userSelectCategory != null && UserSelTP != null) {
                userSelectCategory.Active = Main.VRTeleportVisible.Value;
                UserSelTP.Active = Main.VRTeleportVisible.Value;
            }
        }

        internal static void UpdateWorldActions(bool status) {
            if (TPAction != null)
                TPAction.Interactable = status;
            if (TPName != null)
                TPName.Interactable = status;
            if (TPCoord != null)
                TPCoord.Interactable = status;
            
            if (wp1 != null)
                wp1.Interactable = status;
            if (wp2 != null)
                wp2.Interactable = status;
            if (wp3 != null)
                wp3.Interactable = status;
            if (wp4 != null)
                wp4.Interactable = status;
            
            if (wpl1 != null)
                wpl1.Interactable = status;
            if (wpl2 != null)
                wpl2.Interactable = status;
            if (wpl3 != null)
                wpl3.Interactable = status;
            if (wpl4 != null)
                wpl4.Interactable = status;
        }
    }
}