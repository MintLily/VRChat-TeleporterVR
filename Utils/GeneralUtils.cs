using System;
using UnityEngine;
using System.Globalization;

namespace TeleporterVR.Utils
{
    static class GeneralUtils
    {
        // Got from https://github.com/Nirv-git/VRChat-Mods/blob/93e6a872dcec544a5943a73ad65eaacecb1cc066/PortableMirror_Combined/Utils.cs#L72
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null) return gameObject.AddComponent<T>();
            return component;
        }

        public static void DestroyComponent<T>(this GameObject go) where T : Component => Component.Destroy(go.GetComponent<T>());

        // Came form https://github.com/d-mageek/VRC-Mods/blob/e6fc68e4b53be9f69ea05d38ad017d7fc0b6bb0d/BetterPortalPlacement/Utils/Utilities.cs#L32
        public static GameObject GetPtrObj()
        {
            /*if (!XRDevice.isPresent) {
                return TrackingManager.GetComponentInChildren<NeckMouseRotator>()
                    .transform.Find(Environment.CurrentDirectory.Contains("vrchat-vrchat") ? "CenterEyeAnchor" : "Camera (head)/Camera (eye)").gameObject;
            }*/ // Not needed since this is a VR Mod
            return VRCTrackingManager.field_Private_Static_VRCTrackingManager_0.gameObject;
        }

        public static Color HexToColor(string hex, bool hasTrans = false, float TransLevel = 1f)
        {
            if (hex.StartsWith("#")) hex = hex.Replace("#", "");
            var Specifier = NumberStyles.AllowHexSpecifier;
            float R = int.Parse(hex.Substring(0, 2), Specifier) / (float)byte.MaxValue;
            float G = int.Parse(hex.Substring(2, 2), Specifier) / (float)byte.MaxValue;
            float B = int.Parse(hex.Substring(4, 2), Specifier) / (float)byte.MaxValue;
            if (hasTrans) return new Color(R, G, B, TransLevel);
            else return new Color(R, G, B);
        }
    }
}
