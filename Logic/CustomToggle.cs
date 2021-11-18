using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using MelonLoader;
using TeleporterVR.Utils;
using UnhollowerBaseLib.Attributes;
using System.Collections;
using Object = UnityEngine.Object;

namespace TeleporterVR.Logic {
    class SetupCustomToggle {
        public static void Init() {
            bool failed;
            try {
                ClassInjector.RegisterTypeInIl2Cpp<CustomToggle>();
                failed = false;
            }
            catch (Exception e) {
                MelonLogger.Error("Unable to Inject Custom CustomToggle Script!\n" + e.ToString());
                failed = true;
            }
            if (Main.isDebug && !failed)
                MelonLogger.Msg(ConsoleColor.Green, "Finished setting up CustomToggle");
        }

        public static IEnumerator SetPrefabOnQM() {
            while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null)
                yield return null;
            GameObject LaunchPadViewport = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/");
            var g = CustomToggle.ToggleCanvas;
            g.layer = 12;
            g.GetComponent<Canvas>().worldCamera = Camera.main;
            g.transform.SetParent(LaunchPadViewport.transform);
            var rT = g.GetComponent<RectTransform>();
            rT.localPosition = new Vector2(0, 0);
            rT.localScale = new Vector3(1, 1, 1);
            MelonLogger.Msg(ConsoleColor.Green, "Finished setting up Prefab on QM");
        }
    }

    class CustomToggle : MonoBehaviour {
        public CustomToggle(IntPtr obj0) : base(obj0) { }

        public static GameObject ToggleCanvas, Allowed, NotAllowed;
        private static GameObject Button, ToggleON, ActiveOverlay_OFF, ActiveOverlay_ON;
        private static readonly Color ToggleTheme = GeneralUtils.HexToColor(Main.IndicatorHexColor.Value, true);

        void Start() {
            if (ToggleCanvas != null) {
                Button = ToggleCanvas.transform.Find("Panel/Allowed/TRVRP_OverlayButton").gameObject;
                Button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                Button.GetComponent<Button>().onClick.AddListener(new Action(OnPress));

                ToggleON = ToggleCanvas.transform.Find("Panel/Allowed/Toggle_ON").gameObject;

                ActiveOverlay_OFF = ToggleCanvas.transform.Find("Panel/Allowed/Toggle_OFF/ActiveOverlay").gameObject;
                ActiveOverlay_ON = ToggleCanvas.transform.Find("Panel/Allowed/Toggle_ON/ActiveOverlay").gameObject;
                Allowed = ToggleCanvas.transform.Find("Panel/Allowed").gameObject;
                NotAllowed = ToggleCanvas.transform.Find("Panel/NotAllowed").gameObject;
            }
        }

        [HideFromIl2Cpp]
        public static void UpdateColorTheme() {
            if (ActiveOverlay_OFF != null || ActiveOverlay_ON != null) {
                ActiveOverlay_OFF.GetComponent<Image>().color = ToggleTheme;
                ActiveOverlay_ON.GetComponent<Image>().color = ToggleTheme;
            }
        }

        [HideFromIl2Cpp]
        public static void UpdateRiskyObjects(bool allowed) {
            if (Allowed != null || NotAllowed != null) {
                Allowed.SetActive(allowed);
                NotAllowed.SetActive(!allowed);
            }
        }

        [HideFromIl2Cpp]
        public static void UpdateToggleState() {
            if (ToggleON != null)
                ToggleON.SetActive(VRUtils.active);
        }

        [HideFromIl2Cpp]
        private void OnPress() {
            if (!Allowed.activeSelf && NotAllowed.activeSelf && !WorldActions.WorldAllowed)
                return;

            if (ToggleON.activeSelf) {
                ToggleON.SetActive(false);
                UIXMenuReplacement.ToggleVRTeleport(false);
            } else {
                ToggleON.SetActive(true);
                UIXMenuReplacement.ToggleVRTeleport(true);
            }
        }
    }
}
