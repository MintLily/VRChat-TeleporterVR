using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using HarmonyLib;
using UnityEngine;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;
using TeleporterVR;
using TeleporterVR.Utils;
using UnhollowerBaseLib;
using Log = MelonLoader.MelonLogger;

// Came from https://github.com/Psychloor/PlayerRotater/blob/master/PlayerRotater/ModPatches.cs
//     &     https://github.com/ddakebono/BTKSANameplateFix/blob/master/BTKSANameplateMod.cs
namespace TeleporterVR.Patches
{
    internal static class NewPatches
    {
        public static bool IsQMOpen, IsAMOpen;
        public static Action OnWorldJoin, OnWorldLeave;

        public static void SetupPatches()
        {
            bool d = Main.isDebug;
            Main.Logger.Msg("Applying Patches . . .");

            applyPatches(typeof(LeftRoomPatches));
            applyPatches(typeof(QuickMenuPatches));

            if (d) Main.Logger.Msg(ConsoleColor.Green, "Finished with Patches");
        }

        private static void applyPatches(Type type)
        {
            try {
                if (Main.isDebug) Main.Logger.Msg($"Attempting {type.Name} Patches...");
                HarmonyLib.Harmony.CreateAndPatchAll(type, "TeleporterVR");
            } catch (Exception e) {
                Main.Logger.Error($"Failed while patching {type.Name}!\n{e}");
            }
        }
    }


    // Patch Methods

    [HarmonyPatch(typeof(VRC.UI.Elements.QuickMenu))]
    class QuickMenuPatches {
        [HarmonyPostfix]
        [HarmonyPatch("OnEnable")]
        private static void OnQuickMenuEnable() {
            NewPatches.IsQMOpen = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnDisable")]
        private static void OnQuickMenuDisable() {
            NewPatches.IsQMOpen = false;
        }
    }
    
    [HarmonyPatch(typeof(NetworkManager))]
    class LeftRoomPatches {
        [HarmonyPostfix]
        [HarmonyPatch("OnLeftRoom")]
        static void Yeet() {
            WorldActions.OnLeftWorld();
            NewPatches.OnWorldLeave?.Invoke();
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnJoinedRoom")]
        static void JoinedRoom() {
            MelonCoroutines.Start(WorldActions.CheckWorld());
            NewPatches.OnWorldJoin?.Invoke();
        }
    }
}
