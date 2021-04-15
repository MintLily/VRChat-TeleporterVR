using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        private static bool readyR, readyL;
        public static bool active;

        private static GameObject ControllerLeft, ControllerRight;

        public static bool preferRightHand;

        public static void Init()
        {
            if (Environment.CurrentDirectory.Contains("vrchat-vrchat")) // Oculus Check came from emmVRC (Thanks Emmy)
                _oculus = true;

            AssignBindings();
        }

        private static void AssignBindings()
        {
            if (_oculus) {
                ControllerRight = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/RightHandAnchor/PointerOrigin (1)");
                ControllerLeft = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/LeftHandAnchor/PointerOrigin (1)");
                if (Main.isDebug) MelonLoader.MelonLogger.Msg("Binds set: Oculus");
            }
            else {
                ControllerRight = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (right)/PointerOrigin");
                ControllerLeft = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (left)/PointerOrigin");
                if (Main.isDebug) MelonLoader.MelonLogger.Msg("Binds set: SteamVR");
            }
        }

        public static /*async*/ void OnUpdate()
        {
            if (!active || !WorldActions.WorldAllowed || (!Main.VRTeleportVisible.Value && Menu.VRTeleport == null)) return;
            if (ControllerLeft == null || ControllerRight == null) AssignBindings();
            if (active && (preferRightHand ? Input.GetButtonDown(InputInfo.RightTrigger) : Input.GetButtonDown(InputInfo.LeftTrigger))) {
                ray = preferRightHand ? new Ray(ControllerRight.transform.position, ControllerRight.transform.forward) : new Ray(ControllerLeft.transform.position, ControllerLeft.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = raycastHit.point;
            }
            //await Task.Delay(50);
        }
    }
}
