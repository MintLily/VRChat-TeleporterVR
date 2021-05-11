using System;
using System.Collections;
using UnityEngine;
using MelonLoader;

namespace TeleporterVR.Utils
{
    public static class InputInfo
    {
        public const string RightTrigger = "Oculus_CrossPlatform_SecondaryIndexTrigger";
        public const string LeftTrigger = "Oculus_CrossPlatform_PrimaryIndexTrigger";
    }

    class VRUtils
    {
        private static bool _oculus = false;
        private static Ray ray;
        public static bool active, preferRightHand;
        private static GameObject ControllerLeft, ControllerRight;

        private static bool InputDown {
            get {
                return Input.GetButtonDown(preferRightHand ? InputInfo.RightTrigger : InputInfo.LeftTrigger) ||
                    Input.GetAxisRaw(preferRightHand ? InputInfo.RightTrigger : InputInfo.LeftTrigger) != 0 ||
                    Input.GetAxis(preferRightHand ? InputInfo.RightTrigger : InputInfo.LeftTrigger) >= 0.75f;
            }
        }

        public static void Init()
        {
            if (Environment.CurrentDirectory.Contains("vrchat-vrchat")) _oculus = true; // Oculus Check came from emmVRC (Thanks Emmy)
            AssignBindings();
            MelonCoroutines.Start(UpdateVRTP());
        }

        private static void AssignBindings()
        {
            if (_oculus) {
                ControllerRight = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/RightHandAnchor/PointerOrigin (1)");
                ControllerLeft = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/LeftHandAnchor/PointerOrigin (1)");
                if (Main.isDebug) MelonLogger.Msg(ConsoleColor.Blue, "Binds set: Oculus");
            } else {
                ControllerRight = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (right)/PointerOrigin");
                ControllerLeft = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (left)/PointerOrigin");
                if (Main.isDebug) MelonLogger.Msg(ConsoleColor.Blue, "Binds set: SteamVR");
            }
        }

        public static IEnumerator UpdateVRTP()
        {
            while (active) {
                if (ControllerLeft == null || ControllerRight == null) AssignBindings();
                if (InputDown) {
                    ray = preferRightHand ? new Ray(ControllerRight.transform.position, ControllerRight.transform.forward) :
                        new Ray(ControllerLeft.transform.position, ControllerLeft.transform.forward);
                    if (Physics.Raycast(ray, out RaycastHit raycastHit))
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = raycastHit.point;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
