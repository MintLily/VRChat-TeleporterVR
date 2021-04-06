using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;
using TeleporterVR;
using TeleporterVR.Utils;

// Came from https://github.com/Psychloor/PlayerRotater/blob/master/PlayerRotater/ModPatches.cs
namespace TeleporterVR.Logic
{
    internal static class Patches
    {
        private static HarmonyInstance instance = HarmonyInstance.Create("TeleporterVRPatch");

        private static HarmonyMethod GetLocalPatch(string name)
        {
            return new HarmonyMethod(typeof(Patches).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static));
        }

        public static void Init()
        {
            try
            {
                // Left room
                instance.Patch(typeof(NetworkManager).GetMethod("OnLeftRoom", BindingFlags.Public | BindingFlags.Instance),
                    postfix: GetLocalPatch("LeftWorld"));
            }
            catch (Exception e)
            {
                MelonLogger.Error("Failed to patch OnLeftRoom\n" + e.Message);
            }

            try
            {
                // Faded to and joined and initialized room
                IEnumerable<MethodInfo> fadeMethods = typeof(VRCUiManager).GetMethods().Where(m =>
                m.Name.StartsWith("Method_Public_Void_String_Single_Action_") && m.GetParameters().Length == 3);

                foreach (MethodInfo fadeMethod in fadeMethods)
                {
                    instance.Patch(fadeMethod, postfix: GetLocalPatch("JoinedRoomPatch"));
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error("Failed to patch FadeTo Initialized room\n" + e.Message);
            }

            if (Main.isDebug)
                MelonLoader.MelonLogger.Msg("Finished with Patches");
        }

        private static void LeftWorldPatch()
        {
            if (Main.isDebug)
                MelonLogger.Msg("Left World Patch");
            WorldActions.OnLeftWorld();
        }

        private static void JoinedRoomPatch(string __0, float __1)
        {
            if (__0.Equals("BlackFade") && __1.Equals(0f) && RoomManager.field_Internal_Static_ApiWorldInstance_0 != null)
            {
                MelonCoroutines.Start(WorldActions.CheckWorld());
                if (Main.isDebug)
                    MelonLogger.Msg("Joined Room Patch");
            }
        }
    }
}
