using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using VRC.Core;
using ActionMenuApi.Api;

// Came from https://github.com/Psychloor/PlayerRotater/blob/master/PlayerRotater/Utilities.cs
namespace TeleporterVR.Utils
{
    internal static class WorldActions
    {
        internal static bool WorldAllowed;
        internal static IEnumerator CheckWorld()
        {
            if (Main.isDebug)
                MelonLogger.Msg("Checking World");
            string worldId = RoomManager.field_Internal_Static_ApiWorld_0.id;
            WorldAllowed = false;

            // Check if black/whitelisted from EmmVRC - thanks Emilia and the rest of EmmVRC Staff
            WWW www = new WWW($"https://dl.emmvrc.com/riskyfuncs.php?worldid={worldId}", null, new Dictionary<string, string>());
            while (!www.isDone)
                yield return new WaitForEndOfFrame();
            string result = www.text?.Trim().ToLower();
            www.Dispose();
            if (!string.IsNullOrWhiteSpace(result))
                switch (result)
                {
                    case "allowed":
                        WorldAllowed = true;
                        if (Main.isDebug)
                            MelonLogger.Msg(ConsoleColor.Cyan, "EmmVRC Allowed");
                        Menu.userSel_TPto.Disabled(false);
                        Menu.VRTeleport.Disabled(false);
                        ActionMenu.subMenu.locked = false;
                        MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
                        MelonCoroutines.Start(ActionMenu.UpdateIcon(false));
                        AMUtils.RefreshActionMenu();
                        yield break;

                    case "denied":
                        WorldAllowed = false;
                        if (Main.isDebug)
                            MelonLogger.Msg(ConsoleColor.Red, "EmmVRC Disallowed");
                        Menu.userSel_TPto.Disabled(true);
                        Menu.VRTeleport.Disabled(true);
                        ActionMenu.subMenu.locked = true;
                        MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
                        MelonCoroutines.Start(ActionMenu.UpdateIcon(false));
                        Menu.VRTeleport.setToggleState(false, true);
                        AMUtils.RefreshActionMenu();
                        yield break;
                }

            if (Main.isDebug)
                MelonLogger.Msg("Checking World Tags, no response from EmmVRC");

            // no result from server or they're currently down
            // Check tags then. should also be in cache as it just got downloaded
            API.Fetch<ApiWorld>( worldId, new Action<ApiContainer>(container =>
            {
                ApiWorld apiWorld;
                if ((apiWorld = container.Model.TryCast<ApiWorld>()) != null)
                {
                    foreach (string worldTag in apiWorld.tags)
                        if (worldTag.IndexOf("game", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            if (Main.isDebug)
                                MelonLogger.Msg(ConsoleColor.Red, "Found Game Tag(s)");
                            WorldAllowed = false;
                            Menu.userSel_TPto.Disabled(true);
                            Menu.VRTeleport.Disabled(true);
                            ActionMenu.subMenu.locked = true;
                            MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
                            MelonCoroutines.Start(ActionMenu.UpdateIcon(false));
                            Menu.VRTeleport.setToggleState(false, true);
                            AMUtils.RefreshActionMenu();
                            return;
                        }

                    WorldAllowed = true;
                    Menu.userSel_TPto.Disabled(false);
                    Menu.VRTeleport.Disabled(false);
                    ActionMenu.subMenu.locked = false;
                    MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
                    MelonCoroutines.Start(ActionMenu.UpdateIcon(false));
                    AMUtils.RefreshActionMenu();
                }
                else
                {
                    MelonLogger.Error("Failed to cast ApiModel to ApiWorld");
                }
            }), disableCache: false);
        }

        internal static void OnLeftWorld()
        {
            WorldAllowed = false;
            Menu.VRTeleport.setToggleState(false, true);
        }
    }
}
