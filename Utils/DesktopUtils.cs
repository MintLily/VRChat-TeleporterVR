using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Runtime.InteropServices;
using TeleporterVR.Logic;
using MelonLoader;
using UIExpansionKit.API;
using TeleporterVR.Patches;
using System.Linq;

namespace TeleporterVR.Utils
{
    class DesktopUtils
    {
        private static readonly bool InVR = XRDevice.isPresent;
        private static bool __ = true;

        private static bool InputDown { 
            get {
                if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T)) || Input.GetKeyDown(KeyCode.Mouse3))
                    return true;
                return false;
            }
        }

        public static void OnUpdate()
        {
            if (!Main.EnableDesktopTP.Value) return;
            if (InVR) return;
            if (__ && InputDown) {
                if (NewPatches.IsQMOpen) return;
                if (NewPatches.IsAMOpen) return;
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = raycastHit.point;
                __ = false;
            } else if (!__ && !InputDown) __ = true;
        }
    }
}
