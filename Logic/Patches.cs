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

        public static void SetupPatches()
        {
            bool d = Main.isDebug;
            Log.Msg("Applying Patches . . .");

            applyPatches(typeof(QuickMenuPatches));
            applyPatches(typeof(FadePatches));

            if (d) Log.Msg(ConsoleColor.Green, "Finished with Patches");
        }

        private static void applyPatches(Type type)
        {
            try {
                if (Main.isDebug) Log.Msg($"Attempting {type.Name} Patches...");
                HarmonyLib.Harmony.CreateAndPatchAll(type, "TeleporterVR");
            } catch (Exception e) {
                Log.Error($"Failed while patching {type.Name}!\n{e}");
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

    [HarmonyPatch]
    class FadePatches
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(VRCUiBackgroundFade).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name.Contains("Method_Public_Void_Single_Action") && 
            !x.Name.Contains("PDM")).Cast<MethodBase>();
        }

        static void Postfix()
        {
            try { MelonCoroutines.Start(WorldActions.CheckWorld()); }
            catch (Exception e) {
                Log.Error($"Error checking world for Risky Function checks, returning false\nError:\n{e}");
                WorldActions.WorldAllowed = false;
            }
        }
    }
}
