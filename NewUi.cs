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
    public static class NewUi {
        internal static IEnumerator OnQuickMenu() {
            while (UIManager.field_Private_Static_UIManager_0 == null) yield return null;
            while (GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null) yield return null;
            ReMod.Core.Unity.EnableDisableListener.RegisterSafe();
            BuildUi();
        }

        private static ReCategoryPage _teleportVr;
        private static ReMenuCategory _mainCat, _waypointsCat;
        private static ReMenuButton _tpKeyboard, _tpCoord, _wp1, _wp2, _wp3, _wp4, _wpl1, _wpl2, _wpl3, _wpl4, _userSelTp;
        private static ReMenuToggle _handChange, _tpAction;
        private static bool _built;
        
        private static void BuildUi() {
            _teleportVr = new ReCategoryPage("TeleporterVR", true);
            _teleportVr.GameObject.SetActive(false);

            ReTabButton.Create("TPVRTab", "Open the TeleporterVR Menu", "TeleporterVR", ResourceManager.Tab);

            _mainCat = _teleportVr.AddCategory("TPVR Actions", false);
            _tpKeyboard = _mainCat.AddButton(Language.TPtoName_Text, $"Open keyboard to {Language.TPtoName_Text}", OpenKeyboardForPlayerTP, ResourceManager.keyboard);
            _tpCoord = _mainCat.AddButton(Language.TPtoCoord_Text, $"Open keyboard to {Language.TPtoCoord_Text}", OpenKeyboardForCoordTP, ResourceManager.mapmarker);
            _handChange = _mainCat.AddToggle(Language.preferedHanded_Text_ON, $"Toggle {Language.preferedHanded_Text_ON} or {Language.preferedHanded_Text_OFF}", b => {
                MelonPreferences.GetEntry<bool>(Main.melon.Identifier, Main.PreferRightHand.Identifier).Value = b;
            });
            _handChange.Toggle(Main.PreferRightHand.Value);
            _tpAction = _mainCat.AddToggle(Language.theWord_Teleport, $"Activate {Language.theWord_Teleport}", b => {
                if (!CheckWorldAllowed.RiskyFunctionAllowed) return;
                VRUtils.active = !VRUtils.active;
                TPLocationIndicator.Toggle();
            });
            //TPAction.Toggle(VRUtils.active);

            _waypointsCat = _teleportVr.AddCategory("Waypoints");
            _wp1 = _waypointsCat.AddButton(Language.SavePos, $"{Language.SavePos} 1", () => { SaveAction(1); },
                ResourceManager.one);
            _wp2 = _waypointsCat.AddButton(Language.SavePos, $"{Language.SavePos} 2", () => { SaveAction(2); },
                ResourceManager.two);
            _wp3 = _waypointsCat.AddButton(Language.SavePos, $"{Language.SavePos} 3", () => { SaveAction(3); },
                ResourceManager.three);
            _wp4 = _waypointsCat.AddButton(Language.SavePos, $"{Language.SavePos} 4", () => { SaveAction(4); },
                ResourceManager.four);
            
            _wpl1 = _waypointsCat.AddButton(Language.LoadPos, $"{Language.LoadPos} 1", () => { LoadAction(1); },
                ResourceManager.one);
            _wpl2 = _waypointsCat.AddButton(Language.LoadPos, $"{Language.LoadPos} 2", () => { LoadAction(2); },
                ResourceManager.two);
            _wpl3 = _waypointsCat.AddButton(Language.LoadPos, $"{Language.LoadPos} 3", () => { LoadAction(3); },
                ResourceManager.three);
            _wpl4 = _waypointsCat.AddButton(Language.LoadPos, $"{Language.LoadPos} 4", () => { LoadAction(4); },
                ResourceManager.four);

            var t = _teleportVr.AddCategory("Information");
            t.AddButton($"Mod v{BuildInfo.Version}", "", () => Main.Log("You're Cute!"), ResourceManager.ver);
            t.AddButton("VRCMG", "Opens webpage to VRChat Modding Group's Discord Invite", () => OpenWebpage("https://discord.gg/7EQCmgrUnH"), ResourceManager.DiscordLogo);
            t.AddButton("GitHub", "Opens a webpage to the Mod\'s GitHub project", () => OpenWebpage(BuildInfo.DownloadLink), ResourceManager.GitHubLogo);
            
            BuildUserSelectMenu();
        }

        private static ReMenuCategory _userSelectCategory;
        private static void BuildUserSelectMenu() {
            var menuSelectedUserLocal = QuickMenuEx.Instance.field_Public_Transform_0.Find("Window/QMParent/Menu_SelectedUser_Local");
            var theSelectedUserMenu = menuSelectedUserLocal.Find("ScrollRect/Viewport/VerticalLayoutGroup");
            _userSelectCategory = new ReMenuCategory("TPVR", theSelectedUserMenu);

            _userSelTp = _userSelectCategory.AddButton(Language.theWord_Teleport, $"{Language.theWord_Teleport} to Selected User", () => {
                if (CheckWorldAllowed.RiskyFunctionAllowed) 
                    PlayerActions.Teleport(PlayerActions.SelVRCPlayer());
            }, ResourceManager.mapmarker);
            _built = true;
        }

        internal static void OnPrefSave() {
            if (!_built) return;
            if (_teleportVr != null && _handChange != null)
                _handChange.Toggle(Main.PreferRightHand.Value);
            
            if (_userSelectCategory != null && _userSelTp != null) {
                _userSelectCategory.Active = Main.VRTeleportVisible.Value;
                _userSelTp.Active = Main.VRTeleportVisible.Value;
            }
        }

        internal static void UpdateWorldActions(bool status) {
            if (_tpAction != null)
                _tpAction.Interactable = status;
            if (_tpKeyboard != null)
                _tpKeyboard.Interactable = status;
            if (_tpCoord != null)
                _tpCoord.Interactable = status;
            
            if (_wp1 != null)
                _wp1.Interactable = status;
            if (_wp2 != null)
                _wp2.Interactable = status;
            if (_wp3 != null)
                _wp3.Interactable = status;
            if (_wp4 != null)
                _wp4.Interactable = status;
            
            if (_wpl1 != null)
                _wpl1.Interactable = status;
            if (_wpl2 != null)
                _wpl2.Interactable = status;
            if (_wpl3 != null)
                _wpl3.Interactable = status;
            if (_wpl4 != null)
                _wpl4.Interactable = status;
        }
    }
}