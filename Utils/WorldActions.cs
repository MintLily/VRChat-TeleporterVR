using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using VRC.Core;
using System.Net;
using System.Threading.Tasks;

namespace TeleporterVR.Utils
{
    internal static class WorldActions
    {
        internal static bool WorldAllowed;

        internal static void Yes()
        {
            if (Main.isDebug)
                MelonLogger.Msg(ConsoleColor.Cyan, "EmmVRC Allowed");
            Menu.userSel_TPto.Disabled(false);
            Menu.VRTeleport.Disabled(false);
            MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
            if (Main.ActionMenuApiIntegration.Value && ActionMenu.hasAMApiInstalled)
            {
                ActionMenu.CheckForRiskyFunctions(false);
                MelonCoroutines.Start(ActionMenu.UpdateIcon(false));
            }
        }

        internal static void No()
        {
            if (Main.isDebug)
                MelonLogger.Msg(ConsoleColor.Red, "EmmVRC Disallowed");
            Menu.userSel_TPto.Disabled(true);
            MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
            Menu.VRTeleport.setToggleState(false, true);
            if (Main.ActionMenuApiIntegration.Value && ActionMenu.hasAMApiInstalled)
            {
                ActionMenu.CheckForRiskyFunctions(true);
                MelonCoroutines.Start(ActionMenu.UpdateIcon(false));
            }
            VRUtils.active = false;
        }

        // Came from https://github.com/Psychloor/PlayerRotater/blob/master/PlayerRotater/Utilities.cs
        internal static IEnumerator CheckWorld()
        {
            if (Main.isDebug)
                MelonLogger.Msg("Checking World");

            string worldId;
            if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                worldId = RoomManager.field_Internal_Static_ApiWorld_0.id;
            else if (RoomManager.field_Internal_Static_ApiWorldInstance_0 != null)
                worldId = RoomManager.field_Internal_Static_ApiWorldInstance_0.id;
            else
            {
                WorldAllowed = false;
                yield break;
            }

            WorldAllowed = false;

            // Allow world creators more choice over Risky Functions without relying on our whitelist, we are looking for "eVRCRiskFuncDisable" or "eVRCRiskFuncEnable"
            // If these are present, they will completely override our choice from tags and the online list, and manually disable or enable Risky Functions
            GameObject[] allWorldGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            if (allWorldGameObjects.Any(a => a.name == "eVRCRiskFuncDisable") || allWorldGameObjects.Any(a => a.name == "TPVRActionDisable")) {
                if (Main.isDebug) MelonLogger.Msg(ConsoleColor.Green, "GameObject found: Disabling Fucntions");
                WorldAllowed = false;
                No();
                yield break;
            }
            else if (allWorldGameObjects.Any(a => a.name == "eVRCRiskFuncEnable") || allWorldGameObjects.Any(a => a.name == "TPVRActionEnable")) {
                if (Main.isDebug) MelonLogger.Msg(ConsoleColor.Green, "GameObject found: Enabling Fucntions");
                WorldAllowed = true;
                Yes();
                yield break;
            }

            // Check if black/whitelisted from EmmVRC - thanks Emilia and the rest of EmmVRC Staff
            string url = $"https://dl.emmvrc.com/riskyfuncs.php?worldid={worldId}";
            WebClient www = new WebClient();
            //WWW www = new WWW(url, null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
            while (www.IsBusy) yield return new WaitForEndOfFrame();
            string result = www.DownloadString(url)?.Trim().ToLower();
            www.Dispose();
            if (!string.IsNullOrWhiteSpace(result))
                switch (result)
                {
                    case "allowed":
                        WorldAllowed = true;
                        Yes();
                        yield break;

                    case "denied":
                        WorldAllowed = false;
                        No();
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
                        if (worldTag.IndexOf("game", StringComparison.OrdinalIgnoreCase) >= 0) {
                            WorldAllowed = false;
                            No();
                            return;
                        }

                    WorldAllowed = true;
                    Yes();
                    return;
                }
                else MelonLogger.Error("Failed to cast ApiModel to ApiWorld");
            }), disableCache: false);

            // If all else fails, or is errored return false
            WorldAllowed = false;
        }

        internal static void OnLeftWorld()
        {
            WorldAllowed = false;
            Menu.VRTeleport.setToggleState(false, true);
            VRUtils.active = false;
        }
    }
}
