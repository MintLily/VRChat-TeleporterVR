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
        private static Ray ray;
        private static bool readyR;
        private static bool readyL;
        public static bool active;

        private static GameObject ControllerLeft, ControllerRight;

        public static bool perferRightHand;

        public static void Init()
        {
            if (Environment.CurrentDirectory.Contains("vrchat-vrchat")) // Oculus Check came from emmVRC (Thanks Emmy)
            {
                ControllerRight = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/RightHandAnchor/PointerOrigin (1)");
                ControllerLeft = GameObject.Find("/_Application/TrackingVolume/TrackingOculus(Clone)/OVRCameraRig/TrackingSpace/LeftHandAnchor/PointerOrigin (1)");
            }
            else
            {
                ControllerRight = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (right)/PointerOrigin");
                ControllerLeft = GameObject.Find("/_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Controller (left)/PointerOrigin");
            }

            MelonLoader.MelonCoroutines.Start(TeleportLoop());
        }

        private static IEnumerator TeleportLoop()
        {
            while (true)
            {
                if (!active)// && !Main.VRTeleportVisible.Value)
                    yield break;
                if (active)
                {
                    if (perferRightHand)
                    {
                        try
                        {
                            if (Input.GetAxis(InputInfo.RightTrigger) > 0.75f && !readyR)
                            {
                                ray = new Ray(ControllerRight.transform.position, ControllerRight.transform.forward);
                                RaycastHit raycastHit;
                                if (Physics.Raycast(ray, out raycastHit))
                                {
                                    PlayerActions.GetLocalVRCPlayer().transform.position = raycastHit.point;
                                }
                                readyR = true;
                            }
                            else if (readyR)
                            {
                                readyR = false;
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            if (Input.GetAxis(InputInfo.LeftTrigger) > 0.75f && !readyL)
                            {
                                ray = new Ray(ControllerLeft.transform.position, ControllerLeft.transform.forward);
                                RaycastHit raycastHit;
                                if (Physics.Raycast(ray, out raycastHit))
                                {
                                    PlayerActions.GetLocalVRCPlayer().transform.position = raycastHit.point;
                                }
                                readyL = true;
                            }
                            else if (readyL)
                            {
                                readyL = false;
                            }
                        }
                        catch { }
                    }
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
