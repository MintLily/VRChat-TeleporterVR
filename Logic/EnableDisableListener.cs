using MelonLoader;
using System;
using TeleporterVR.Patches;
using TeleporterVR.Utils;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace TeleporterVR.Logic
{
    public class CreateListener
    {
        static GameObject AMLeft, AMRight;

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

            var listener = AMLeft.GetOrAddComponent<EnableDisableListener>();
            listener.OnEnabled += AMOpenToggle;
            listener.OnDisabled += AMOpenToggle;
            listener = AMRight.GetOrAddComponent<EnableDisableListener>();
            listener.OnEnabled += AMOpenToggle;
            listener.OnDisabled += AMOpenToggle;

            if (Main.isDebug)
                MelonLogger.Msg(ConsoleColor.Green, "Finished creating ActionMenuListener");
        }

        static void AMOpenToggle()
        {
            var leftOpen = AMLeft.activeSelf;
            var rightOpen = AMRight.activeSelf;

            if (leftOpen || rightOpen) NewPatches.IsAMOpen = true;
            else NewPatches.IsAMOpen = false;
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
