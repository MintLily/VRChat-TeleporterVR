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
//     &     https://github.com/ddakebono/BTKSANameplateFix/blob/master/BTKSANameplateMod.cs
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

            MethodInfo closeQuickMenu = typeof(QuickMenu).GetMethods()
                    .Where(mb => mb.Name.StartsWith("Method_Public_Void_Boolean_") && mb.Name.Length <= 29 && !mb.Name.Contains("PDM") && CheckUsed(mb, "Method_Private_Void_String_String_LoadErrorReason_")).First();

            MethodInfo openQuickMenu = typeof(QuickMenu).GetMethods()
                .Where(mb => mb.Name.StartsWith("Method_Public_Void_Boolean_") && mb.Name.Length <= 29 && !mb.Name.Contains("PDM") && CheckUsing(mb, "Method_Public_Static_Boolean_byref_Boolean_0", typeof(VRCInputManager))).First();

            try
            {
                instance.Patch(openQuickMenu, null, new HarmonyMethod(typeof(Patches).GetMethod("QMOpen", BindingFlags.Public | BindingFlags.Static)));
                instance.Patch(closeQuickMenu, null, new HarmonyMethod(typeof(Patches).GetMethod("QMClose", BindingFlags.Public | BindingFlags.Static)));
            }
            catch (Exception e) { MelonLogger.Error("Unable to patch Quickmenu Open/Close functions!\n" + e.ToString()); }

            if (Main.isDebug)
                MelonLogger.Msg(ConsoleColor.Green, "Finished with Patches");
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

        internal static bool IsQMOpen;

        private static void QMOpen() { IsQMOpen = true; }

        private static void QMClose() { IsQMOpen = false; }

        private static bool CheckUsed(MethodBase methodBase, string methodName)
        {
            try
            {
                return UnhollowerRuntimeLib.XrefScans.XrefScanner.UsedBy(methodBase).Where(instance => instance.TryResolve() != null && instance.TryResolve().Name.Contains(methodName)).Any();
            }
            catch { }
            return false;
        }

        public static bool CheckUsing(MethodInfo method, string match, Type type)
        {
            foreach (XrefInstance instance in XrefScanner.XrefScan(method))
                if (instance.Type == XrefType.Method)
                    try
                    {
                        if (instance.TryResolve().DeclaringType == type && instance.TryResolve().Name.Contains(match))
                            return true;
                    }
                    catch
                    {

                    }
            return false;
        }
    }
}
