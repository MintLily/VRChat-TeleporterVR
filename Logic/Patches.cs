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
using VRC.Core;

// Came from https://github.com/Psychloor/PlayerRotater/blob/master/PlayerRotater/ModPatches.cs
//     &     https://github.com/ddakebono/BTKSANameplateFix/blob/master/BTKSANameplateMod.cs
namespace TeleporterVR.Patches
{
    internal static class NewPatches
    {
        public static bool IsQMOpen, IsActionMenuOpen;
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
            applyPatches(typeof(ActionMenuPatches));
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

    [HarmonyPatch]
    class ActionMenuPatches
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(ActionMenuOpener).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name.Contains("Method_Public_Void_Boolean")).Cast<MethodBase>();
        }

        static void Postfix(bool __0) => NewPatches.IsActionMenuOpen = __0;
    }

    [HarmonyPatch]
    class FadePatches
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(VRCUiBackgroundFade).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name.Contains("Method_Public_Void_Single_Action") && 
            !x.Name.Contains("PDM")).Cast<MethodBase>();
        }

        static void Postfix() => MelonCoroutines.Start(WorldActions.CheckWorld());
    }


    /*
    internal static class _040Patches // For MelonLoader v0.4.0
    {
        private static OnLeftRoom origOnLeftRoom;
        private static FadeTo origFadeTo;
        private static closeQuickMenu closeQM;
        private static openQuickMenu openQM;

        private static void FadeToPatch(IntPtr instancePtr, IntPtr fadeNamePtr, float fade, IntPtr actionPtr, IntPtr stackPtr)
        {
            if (instancePtr == IntPtr.Zero) return;
            origFadeTo(instancePtr, fadeNamePtr, fade, actionPtr, stackPtr);

            if (!IL2CPP.Il2CppStringToManaged(fadeNamePtr).Equals("BlackFade", StringComparison.Ordinal)
                || !fade.Equals(0f) || RoomManager.field_Internal_Static_ApiWorldInstance_0 == null) return;

            MelonCoroutines.Start(WorldActions.CheckWorld());
        }

        private static void OnLeftRoomPatch(IntPtr instancePtr)
        {
            if (Main.isDebug)
                MelonLogger.Msg("Left World Patch");
            WorldActions.OnLeftWorld();
            origOnLeftRoom(instancePtr);
        }

        public static bool isQMOpened;
        public static void QMOpened(IntPtr instancePtr) {
            isQMOpened = true;
            openQM(instancePtr);
        }
        public static void QMClosed(IntPtr instancePtr) {
            isQMOpened = false;
            closeQM(instancePtr);
        }

        internal static bool PatchMethods()
        {
            try {
                // Left room
                MethodInfo onLeftRoomMethod = typeof(NetworkManager).GetMethod(
                    nameof(NetworkManager.OnLeftRoom),
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly,
                    null, Type.EmptyTypes, null);
                origOnLeftRoom = Patch<OnLeftRoom>(onLeftRoomMethod, GetDetour(nameof(OnLeftRoomPatch)));
            }
            catch (Exception e) {
                MelonLogger.Error("Failed to patch OnLeftRoom\n" + e.Message);
                return false;
            }

            try {
                // Faded to and joined and initialized room
                MethodInfo fadeMethod = typeof(VRCUiManager).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).First(
                    m => m.Name.StartsWith("Method_Public_Void_String_Single_Action_")
                         && m.Name.IndexOf("PDM", StringComparison.OrdinalIgnoreCase) == -1
                         && m.GetParameters().Length == 3);
                origFadeTo = Patch<FadeTo>(fadeMethod, GetDetour(nameof(FadeToPatch)));
            }
            catch (Exception e) {
                MelonLogger.Error("Failed to patch FadeTo\n" + e.Message);
                return false;
            }

            try {
                // Close Quick Menu patch
                MethodInfo closeQMMethod = typeof(QuickMenu).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(
                    mb => mb.Name.StartsWith("Method_Public_Void_Boolean_") && mb.Name.Length <= 29 && 
                    !mb.Name.Contains("PDM") && CheckUsed(mb, "Method_Public_Void_EnumNPublicSealedvaUnWoAvSoSeUsDeSaCuUnique_Boolean")).FirstOrDefault();
                closeQM = Patch<closeQuickMenu>(closeQMMethod, GetDetour(nameof(QMClosed)));
            }
            catch (Exception e) {
                MelonLogger.Error("Failed to patch closeQM\n" + e.Message);
                return false;
            }

            try
            {
                // Open Quick Menu patch
                MethodInfo openQMMethod = typeof(QuickMenu).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(
                    mb => mb.Name.StartsWith("Method_Public_Void_Boolean_") && mb.Name.Length <= 29 &&
                    !mb.Name.Contains("PDM") && CheckUsed(mb, "Method_Public_Void_24")).FirstOrDefault();
                openQM = Patch<openQuickMenu>(openQMMethod, GetDetour(nameof(QMOpened)));
            }
            catch (Exception e) {
                MelonLogger.Error("Failed to patch openQM\n" + e.Message);
                return false;
            }

            return true;
        }

        private static unsafe TDelegate Patch<TDelegate>(MethodBase originalMethod, IntPtr patchDetour)
        {
            IntPtr original = *(IntPtr*)UnhollowerSupport.MethodBaseToIl2CppMethodInfoPointer(originalMethod);
            MelonUtils.NativeHookAttach((IntPtr)(&original), patchDetour);
            return Marshal.GetDelegateForFunctionPointer<TDelegate>(original);
        }


        private static IntPtr GetDetour(string name)
        {
            return typeof(_040Patches).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)!.MethodHandle.GetFunctionPointer();
        }

        private static bool CheckUsed(MethodBase methodBase, string methodName)
        {
            try {
                return XrefScanner.UsedBy(methodBase).Where(instance => 
                instance.TryResolve() != null && instance.TryResolve().Name.Contains(methodName)).Any();
            } catch { }
            return false;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void OnLeftRoom(IntPtr instancePtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void closeQuickMenu(IntPtr instancePtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void openQuickMenu(IntPtr instancePtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FadeTo(IntPtr instancePtr, IntPtr fadeNamePtr, float fade, IntPtr actionPtr, IntPtr stackPtr);
    }

    internal static class Patches
    {
        private static HarmonyInstance instance = HarmonyInstance.Create("TeleporterVRPatch");

        private static HarmonyMethod GetLocalPatch(string name)
        {
            return new HarmonyMethod(typeof(Patches).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static));
        }

        public static MethodInfo closeQuickMenu, openQuickMenu;

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

            closeQuickMenu = typeof(QuickMenu).GetMethods()
                    .Where(mb => mb.Name.StartsWith("Method_Public_Void_Boolean_") && mb.Name.Length <= 29 && !mb.Name.Contains("PDM") && CheckUsed(mb, "Method_Public_Void_EnumNPublicSealedvaUnWoAvSoSeUsDeSaCuUnique_Boolean")).FirstOrDefault();

            openQuickMenu = typeof(QuickMenu).GetMethods()
                .Where(mb => mb.Name.StartsWith("Method_Public_Void_Boolean_") && mb.Name.Length <= 29 && !mb.Name.Contains("PDM") && CheckUsed(mb, "Method_Public_Void_24")).FirstOrDefault();

            if (closeQuickMenu == null)
                MelonLogger.Warning("CloseQuickMenu function was not found!");
            if (openQuickMenu == null)
                MelonLogger.Warning("OpenQuickMenu function was not found!");
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
    }*/
}
