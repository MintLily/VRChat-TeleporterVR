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
        public static MethodInfo setMenuIndex;

        public static void SetupPatches()
        {
            bool d = Main.isDebug;
            Log.Msg("Applying Patches . . .");

            if (d) Log.Msg("Attempting setMenuIndex Patches...");
            List<Type> quickMenuNestedEnums = typeof(QuickMenu).GetNestedTypes().Where(type => type.IsEnum).ToList();
            PropertyInfo quickMenuEnumProperty = typeof(QuickMenu).GetProperties().First(pi => pi.PropertyType.IsEnum && quickMenuNestedEnums.Contains(pi.PropertyType));
            setMenuIndex = typeof(QuickMenu).GetMethods().First(mb => mb.Name.StartsWith("Method_Public_Void_Enum") && !mb.Name.Contains("_PDM_") &&
            mb.GetParameters().Length == 1 && mb.GetParameters()[0].ParameterType == quickMenuEnumProperty.PropertyType);

            applyPatches(typeof(QuickMenuOpen));
            applyPatches(typeof(QuickMenuClose));
            //applyPatches(typeof(ActionMenuPatches));
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

    [HarmonyPatch]
    class QuickMenuOpen
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(QuickMenu).GetMethods().Where(mb => mb.Name.StartsWith("Method_Public_Void_Boolean_") && mb.Name.Length <= 29 &&
            XrefThings.CheckUsing(mb, NewPatches.setMenuIndex.Name, typeof(QuickMenu))).Cast<MethodBase>();
        }

        static void Postfix() => NewPatches.IsQMOpen = true;
    }

    [HarmonyPatch]
    class QuickMenuClose
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(QuickMenu).GetMethods().Where(mb => mb.Name.StartsWith("Method_Public_Void_Boolean_") && mb.Name.Length <= 29 &&
            XrefThings.CheckUsed(mb, NewPatches.setMenuIndex.Name)).Cast<MethodBase>();
        }

        static void Postfix() => NewPatches.IsQMOpen = false;
    }

    /*[HarmonyPatch]
    class ActionMenuPatches
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(ActionMenuOpener).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name.Contains("Method_Private_Void_Boolean")).Cast<MethodBase>();
        }

        static void Postfix(bool __0) => NewPatches.IsActionMenuOpen = __0;
    }*/

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
