using System;
using HarmonyLib;
using TeleporterVR.Utils;
using Log = MelonLoader.MelonLogger;

// Came from https://github.com/Psychloor/PlayerRotater/blob/master/PlayerRotater/ModPatches.cs
//     &     https://github.com/ddakebono/BTKSANameplateFix/blob/master/BTKSANameplateMod.cs
namespace TeleporterVR.Patches;

internal static class NewPatches {
    public static bool IsQmOpen, IsAmOpen;

    public static void SetupPatches() {
        Main.Log("Applying Patches . . .");

        ApplyPatches(typeof(LeftRoomPatches));
        ApplyPatches(typeof(QuickMenuPatches));

        Main.Debug("Finished with Patches");
    }

    private static void ApplyPatches(Type type) {
        try {
            Main.Debug($"Attempting {type.Name} Patches...");
            HarmonyLib.Harmony.CreateAndPatchAll(type, "TeleporterVR");
        } catch (Exception e) {
            Main.Error($"Failed while patching {type.Name}!\n{e}");
        }
    }
}


// Patch Methods

[HarmonyPatch(typeof(VRC.UI.Elements.QuickMenu))]
internal class QuickMenuPatches {
    [HarmonyPostfix]
    [HarmonyPatch("OnEnable")]
    private static void OnQuickMenuEnable() => NewPatches.IsQmOpen = true;

    [HarmonyPostfix]
    [HarmonyPatch("OnDisable")]
    private static void OnQuickMenuDisable() => NewPatches.IsQmOpen = false;
}
    
[HarmonyPatch(typeof(NetworkManager))]
internal class LeftRoomPatches {
    [HarmonyPostfix]
    [HarmonyPatch("OnLeftRoom")]
    static void Yeet() => CheckWorldAllowed.OnWorldLeave();
}
