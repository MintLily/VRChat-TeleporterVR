using System;
using System.Collections;
using UnityEngine;
using MelonLoader;
using TeleporterVR.Rendering;
using TeleporterVR.Patches;

namespace TeleporterVR.Utils
{
    public static class InputInfo {
        public const string RightTrigger = "Oculus_CrossPlatform_SecondaryIndexTrigger";
        public const string LeftTrigger = "Oculus_CrossPlatform_PrimaryIndexTrigger";
    }

    public static class VRUtils {
        private static bool _oculus, __ = true; // fear my variable naming scheme
        public static bool Active { get; set; }
        public static GameObject ControllerLeft, ControllerRight;
        public static Ray Ray;

        private static bool InputDown => Input.GetButtonDown(Main.PreferRightHand.Value ? InputInfo.RightTrigger : InputInfo.LeftTrigger) ||
                                         Input.GetAxisRaw(Main.PreferRightHand.Value ? InputInfo.RightTrigger : InputInfo.LeftTrigger) != 0 ||
                                         Input.GetAxis(Main.PreferRightHand.Value ? InputInfo.RightTrigger : InputInfo.LeftTrigger) >= 0.75f;

        public static void Init() {
            if (Environment.CurrentDirectory.Contains("vrchat-vrchat")) _oculus = true; // Oculus Check came from emmVRC (Thanks Emmy)
            AssignBindings();
        }

        private static void AssignBindings() {
            if (_oculus) {
                ControllerRight = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/RightHandAnchor/PointerOrigin (1)");
                ControllerLeft = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/LeftHandAnchor/PointerOrigin (1)");
                if (Main.IsDebug) Main.Logger.Msg(ConsoleColor.Blue, "Binds set: Oculus");
            } else {
                ControllerRight = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (right)/PointerOrigin");
                ControllerLeft = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (left)/PointerOrigin");
                if (Main.IsDebug) Main.Logger.Msg(ConsoleColor.Blue, "Binds set: SteamVR");
            }
        }

        public static void OnUpdate() { // Suggestion from Davi > Only click once at a time to not spam teleport OnUpdate
            if (!Active) return;
            if (ControllerLeft == null || ControllerRight == null) AssignBindings();
            if (NewPatches.IsQmOpen) return; // Temporarily Disables Teleporting if the QuickMenu is currently open
            if (NewPatches.IsAmOpen) return; // Temporarily Disables Teleporting if the ActionMenu is currently open
            if (!CheckWorldAllowed.RiskyFunctionAllowed) return;
            if (__ && InputDown) {
                Ray = Main.PreferRightHand.Value ? new Ray(ControllerRight.transform.position, ControllerRight.transform.forward) :
                        new Ray(ControllerLeft.transform.position, ControllerLeft.transform.forward);
                if (Physics.Raycast(Ray, out RaycastHit raycastHit))
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = raycastHit.point;
                __ = false;
            } else if (!__ && !InputDown) __ = true;
        }

        public static Vector3 GetControllerPos() => Main.PreferRightHand.Value ? ControllerRight.transform.position : ControllerLeft.transform.position;

        public static RaycastHit RaycastVR() {
            Ray = Main.PreferRightHand.Value ? new Ray(ControllerRight.transform.position, ControllerRight.transform.forward) :
                new Ray(ControllerLeft.transform.position, ControllerLeft.transform.forward);
            Physics.Raycast(Ray, out RaycastHit hit, TPLocationIndicator.defaultLength);
            return hit;
        }
        
        internal static void ToggleVRTeleport(bool state) {
            if (!CheckWorldAllowed.RiskyFunctionAllowed) return;
            Active = state;
            TPLocationIndicator.Toggle();
        }
    }
}
