using System;
using UnityEngine;
using VRCModLoader;
using VRC;
using VRCMenuUtils;
using VRChat.UI;
using VRChat.UI.QuickMenuUI;
using System.Collections;
using System.Reflection;
using System.Linq;
using VRC.Core;

namespace Mod.Teleport
{
	[VRCModInfo("TeleporterVR", "3.0.0", "Janni9009")] //Thank you Slaynash and Yoshifan for saving me from a mental breakdown
	public class Teleport : VRCMod
	{
        #region INIT
        public void OnApplicationStart()
        {
            ModManager.StartCoroutine(Setup());
            VRCTools.ModPrefs.RegisterCategory("TPVR-R", "TeleporterVR");
            VRCTools.ModPrefs.RegisterPrefBool("TPVR-R", "Enable fuchsia text", true);
            if (VRCTools.ModPrefs.GetBool("TPVR-R", "Enable fuchsia text") == true)
            {
                ReloadedText = "\n<color=fuchsia>Reloaded</color>";
            }
            VRCModLogger.Log("[TeleporterVR] TeleporterVR usage guide:\n >Press Shift+T to enable keybinds\n >>Press F to store your position\n >>>Press R to load the stored position\n >>Press G to open the input window\n Credits: Author - Janni9009, UI page help - yoshifan, Playermanager foreach loop - Slaynash");
#if (DEBUG)
            Debug = true;
            VRCModLogger.Log("[TeleporterVR] Debug mode enabled!");
#endif
        }

        private IEnumerator Setup()
        {
            yield return VRCMenuUtilsAPI.WaitForInit();

            //VRCMenuUtilsAPI.AddQuickMenuButton("internal name", "button name", "tooltip bar", method);
            var OpenQM = new VRCEUiQuickButton("OpenQM", new Vector2(-1050f, 1890f), "Teleporter\nVR" + ReloadedText, "Opens the TPVR submenu", QuickMenu.transform.Find("ShortcutMenu"));
            OpenQM.OnClick += ShowTPVRRQM;
            var TP2P = new VRCEUiQuickButton("TPtoP", new Vector2(-600f, 200f), "Teleport", "Teleports to selected player", QuickMenu.transform.Find("UserInteractMenu"));
            TP2P.OnClick += TP2PAction;
            VRCEUiQuickMenu TPVRRQM = new VRCEUiQuickMenu("TPVRR", false);
            var TEST = new VRCEUiQuickButton("TEST", new Vector2(-630, 1050f), "Teleporter\nVR\nVersion3.0", "VR Compliant, Teleport Requests soon(tm)", TPVRRQM.Control.transform);
            TEST.OnClick += TestA;
            var NTP = new VRCEUiQuickButton("NTP1", new Vector2(-210, 1050f), "Teleport\n(Name)", "Teleport by a section of a playername", TPVRRQM.Control.transform);
            NTP.OnClick += TPName;
            var Sav1 = new VRCEUiQuickButton("Sav1", new Vector2(210, 1050f), "Save\nPosition 1", "Save primary position", TPVRRQM.Control.transform);
            Sav1.OnClick += SavP1;
            var Ld1 = new VRCEUiQuickButton("Load1", new Vector2(630, 1050f), "Load\nPosition 1", "Teleport to primary stored position", TPVRRQM.Control.transform);
            Ld1.OnClick += LoadP1;
            var Back = new VRCEUiQuickButton("ExitSub", new Vector2(-630, 630f), "<color=yellow>Back</color>", "Return to the shortcut menu", TPVRRQM.Control.transform);
            Back.OnClick += ExitSM;
            var CTP = new VRCEUiQuickButton("Coords1", new Vector2(-210, 630f), "Teleport\n(Coords)", "Teleport to a coordinate triple/vector3", TPVRRQM.Control.transform);
            CTP.OnClick += TPCoords;
            var Sav2 = new VRCEUiQuickButton("Sav2", new Vector2(210, 630f), "Save\nPosition 2", "Save secondary position", TPVRRQM.Control.transform);
            Sav2.OnClick += SavP2;
            var Ld2 = new VRCEUiQuickButton("Load2", new Vector2(630, 630f), "Load\nPosition 2", "Teleport to secondary stored position", TPVRRQM.Control.transform);
            Ld2.OnClick += LoadP2;
            var KeysToggle = new VRCEUiQuickButton("ToggleKeys", new Vector2(-630, 210f), "<color=red>Not yet\nImplemented</color>", "Enabled/Disables Keyboard shortcuts", TPVRRQM.Control.transform);
            var RequestsToggle = new VRCEUiQuickButton("ToggleRequests", new Vector2(-210, 210f), "<color=red>Not yet\nImplemented</color>", "Enabled/Disables Teleport-requests", TPVRRQM.Control.transform);
            var Sav3 = new VRCEUiQuickButton("Sav3", new Vector2(210, 210f), "Save\nPosition 2", "Save tertiary position", TPVRRQM.Control.transform);
            Sav3.OnClick += SavP3;
            var Ld3 = new VRCEUiQuickButton("Load3", new Vector2(630, 210f), "Load\nPosition 2", "Teleport to tertiary stored position", TPVRRQM.Control.transform);
            Ld3.OnClick += LoadP3;
        }
        
        public QuickMenu QuickMenu
        {
            get
            {
                if (_quickMenu == null)
                    _quickMenu = ((QuickMenu)typeof(QuickMenu).GetMethod("get_Instance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null));
                return _quickMenu;
            }
        }

        public APIUser GetSelectedU()
        {
            return (APIUser)selectedUserField.GetValue(QuickMenu);
        }

        public Player GetSelectedP()
        {
            if (this.GetSelectedU() != null)
            {
                return PlayerManager.GetPlayer(this.GetSelectedU().id);
            }
            return null;
        }
        #endregion
        #region ActionToMethod
        public void TP2PAction()
        {
            TeleportVRCPlayer(GetSelectedP().vrcPlayer);
        }

        public void TPName()
        {
            InputB.InputAttempt("NTP");
        }

        public void SavP1()
        {
            StorePos(out pos1, out rot1);
        }

        public void LoadP1()
        {
            LoadPos(pos1, rot1);
        }

        public void TPCoords()
        {
            InputB.InputAttempt("CoordTP");
        }

        public void SavP2()
        {
            StorePos(out pos2, out rot2);
        }

        public void LoadP2()
        {
            LoadPos(pos2, rot2);
        }

        public void SavP3()
        {
            StorePos(out pos3, out rot3);
        }

        public void LoadP3()
        {
            LoadPos(pos3, rot3);
        }

        public void TestA()
        {
            VRCModLogger.Log("TEST1");
        }

        public void ShowTPVRRQM()
        {
            VRCMenuUtilsAPI.ShowQuickMenuPage("TPVRR");
        }

        public void ExitSM()
        {
            VRCMenuUtilsAPI.ShowQuickMenuPage("ShortcutMenu");
        }
        #endregion
        #region TheActualCode
        public void StorePos(out Vector3 position, out Quaternion rotation)
		{
			position = VRCPlayer.Instance.transform.position;
			rotation = VRCPlayer.Instance.transform.rotation;
		}

		public void LoadPos(Vector3 position, Quaternion rotation)
		{
			VRCPlayer.Instance.transform.position = position;
			VRCPlayer.Instance.transform.rotation = rotation;
		}

        public void TeleportVRCPlayer(VRCPlayer player)
        {
            VRCPlayer.Instance.transform.position = player.transform.position;
            VRCPlayer.Instance.transform.rotation = player.transform.rotation;
        }
        #endregion
#if (!DEBUG) //temporary fix before implementing proper build settings next time
        #region LegacyButtons
        public void OnUpdate()
		{
			if (Event.current.shift && Input.GetKeyDown(KeyCode.T))
			{
				EnableKeys = !EnableKeys;
                if (Debug)
                {
                    VRCModLogger.Log("[TeleporterVR] Keys toggled!");
                }
			}
            if (EnableKeys)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    StorePos(out pos, out rot);
                    if (Debug)
                    {
                        VRCModLogger.Log("[TeleporterVR] Position store key pressed!");
                        if (pos == new Vector3(0f, 0f, 0f))
                        {
                            VRCModLogger.LogError("[TeleporterVR] There may have been an issue saving your position!");
                        }
                        if (rot == new Quaternion(0f, 0f, 0f, 0f))
                        {
                            VRCModLogger.LogError("[TeleporterVR] There may have been an issue saving your rotation!");
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    LoadPos(pos, rot);
                    if (Debug)
                    {
                        VRCModLogger.Log("[TeleporterVR] Teleport to saved key pressed!");
                        if (VRCPlayer.Instance.transform.position != pos)
                        {
                            VRCModLogger.LogError("[TeleporterVR] Error adjusting position!");
                        }
                        if (VRCPlayer.Instance.transform.rotation != rot)
                        {
                            VRCModLogger.LogError("[TeleporterVR] Error adjusting rotation!");
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    InputB.InputAttempt("NTP");
                    if (Debug)
                    {
                        VRCModLogger.Log("[TeleporterVR] Teleport to player key pressed!");
                        VRCModLogger.Log("[TeleporterVR] Received " + InputB.inputcheck);
                        VRCModLogger.Log("[TeleporterVR] Teleporting to " + InputB.namecheck);
                    }
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    InputB.InputAttempt("CoordTP");
                    if (Debug)
                    {
                        VRCModLogger.Log("[TeleporterVR] Teleport to coordinthan- wait no, coordinates key pressed!");
                    }
                }
            }
        }
        #endregion
#endif

        public bool Debug = false;
		private Vector3 pos1;
		private Quaternion rot1;
        private Vector3 pos2;
        private Quaternion rot2;
        private Vector3 pos3;
        private Quaternion rot3;
        private bool EnableKeys;
        public FieldInfo selectedUserField = typeof(QuickMenu).GetFields(BindingFlags.Instance | BindingFlags.NonPublic).First((FieldInfo f) => f.FieldType == typeof(APIUser));
        private QuickMenu _quickMenu;
        private string ReloadedText;
    }
}
