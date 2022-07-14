using UnityEngine;
using UnityEngine.XR;
using TeleporterVR.Patches;

namespace TeleporterVR.Utils;

public static class DesktopUtils {
    private static readonly bool InVR = XRDevice.isPresent;
    private static bool __ = true;

    private static bool InputDown => (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T)) || Input.GetKeyDown(KeyCode.Mouse3);

    public static void OnUpdate() {
        if (!Main.EnableDesktopTp.Value) return;
        if (InVR) return;
        if (!CheckWorldAllowed.RiskyFunctionAllowed) return;
        if (__ && InputDown) {
            if (NewPatches.IsQmOpen) return;
            if (NewPatches.IsAmOpen) return;
            var ray = new Ray(Camera.main!.transform.position, Camera.main!.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
                VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = raycastHit.point;
            __ = false;
        } else if (!__ && !InputDown) __ = true;
    }
}