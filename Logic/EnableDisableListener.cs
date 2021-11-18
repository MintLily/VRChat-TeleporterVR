using MelonLoader;
using System;
using System.Collections;
using TeleporterVR.Patches;
using TeleporterVR.Utils;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TeleporterVR.Logic
{
    public class CreateListener
    {
        static GameObject AMLeft, AMRight, QuickMenu;

        public static void Init()
        {
            bool failed;
            try { ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>(); failed = false; }
            catch (Exception e) { MelonLogger.Error("Unable to Inject Custom EnableDisableListener Script!\n" + e.ToString()); failed = true; }
            if (Main.isDebug && !failed) MelonLogger.Msg(ConsoleColor.Green, "Finished setting up EnableDisableListener");
        }

        public static void UiInit()
        {
            AMLeft = ActionMenuDriver.prop_ActionMenuDriver_0.field_Public_ActionMenuOpener_0.field_Public_ActionMenu_0.gameObject;
            AMRight = ActionMenuDriver.prop_ActionMenuDriver_0.field_Public_ActionMenuOpener_1.field_Public_ActionMenu_0.gameObject;
            QuickMenu = FindInactiveObjectInActiveRoot("UserInterface/Canvas_QuickMenu(Clone)/Container");

            var listener = AMLeft.GetOrAddComponent<EnableDisableListener>();
            listener.OnEnabled += AMOpenToggle;
            listener.OnDisabled += AMOpenToggle;
            listener = AMRight.GetOrAddComponent<EnableDisableListener>();
            listener.OnEnabled += AMOpenToggle;
            listener.OnDisabled += AMOpenToggle;

            if (Main.isDebug)
                MelonLogger.Msg(ConsoleColor.Green, "Finished creating ActionMenuListener");

            MelonCoroutines.Start(WaitForAndFindQM());
        }

        static IEnumerator WaitForAndFindQM() {
            while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null)
                yield return null;

            QuickMenu = FindInactiveObjectInActiveRoot("UserInterface/Canvas_QuickMenu(Clone)/Container");

            var qmListener = QuickMenu.GetOrAddComponent<EnableDisableListener>();
            qmListener.OnDisabled += QMOpenToggle;
            qmListener.OnEnabled += QMOpenToggle;

            if (Main.isDebug)
                MelonLogger.Msg(ConsoleColor.Green, "Finished creating QuickMenuListener");
        }

        public static GameObject? FindInactiveObjectInActiveRoot(string path) {
            var split = path.Split(new char[] { '/' }, 2);
            var rootObject = GameObject.Find($"/{split[0]}")?.transform;
            if (rootObject == null)
                return null;
            return Transform.FindRelativeTransformWithPath(rootObject, split[1], false)?.gameObject;
        }

        static void AMOpenToggle()
        {
            var leftOpen = AMLeft.activeSelf;
            var rightOpen = AMRight.activeSelf;

            if (leftOpen || rightOpen) NewPatches.IsAMOpen = true;
            else NewPatches.IsAMOpen = false;
        }

        static void QMOpenToggle() {
            var QMOpen = QuickMenu.activeSelf;

            NewPatches.IsQMOpen = QMOpen;
        }
    }

#nullable enable
    // Came from https://github.com/knah/VRCMods/UIExpansionKit/Components/EnableDisableListener.cs
    public class EnableDisableListener : MonoBehaviour
    {
        [method: HideFromIl2Cpp]
        public event Action? OnEnabled;

        [method: HideFromIl2Cpp]
        public event Action? OnDisabled;

        public EnableDisableListener(IntPtr obj0) : base(obj0) { }

        private void OnEnable() => OnEnabled?.Invoke();

        private void OnDisable() => OnDisabled?.Invoke();
    }
}
