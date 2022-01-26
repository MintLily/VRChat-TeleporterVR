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
using System.Net.Http;
using System.Threading.Tasks;
using TeleporterVR.Logic;

namespace TeleporterVR.Utils
{
    internal static class WorldActions
    {
        internal static bool WorldAllowed;
        internal static Action<bool> OnWorldAllowedUpdate;

        internal static void CheckCompleted(bool state) {
            WorldAllowed = state;
            OnWorldAllowedUpdate?.Invoke(WorldAllowed);
            AMSubMenu.subMenu.locked = !state;
            Main.Log($"World {(WorldAllowed ? "Allowed" : "Denied")}", Main.isDebug);
        }

        // Came from https://github.com/Psychloor/PlayerRotater/blob/master/PlayerRotater/Utilities.cs
        internal static IEnumerator CheckWorld()
        {
            Main.Log("Checking World", Main.isDebug);

            string worldId;
            if (RoomManager.field_Internal_Static_ApiWorld_0 == null) {
                Main.Log("Checking World => Halted", Main.isDebug);
                yield break;
            }
            worldId = RoomManager.field_Internal_Static_ApiWorld_0.id;
            Main.Log($"Got WorldID: {worldId}", Main.isDebug);

            WorldAllowed = false;

            // Allow world creators more choice over Risky Functions without relying on our whitelist, we are looking for "eVRCRiskFuncDisable" or "eVRCRiskFuncEnable"
            // If these are present, they will completely override our choice from tags and the online list, and manually disable or enable Risky Functions
            GameObject[] allWorldGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            if (allWorldGameObjects.Any(a => a.name == "eVRCRiskFuncDisable") || allWorldGameObjects.Any(a => a.name == "TPVRActionDisable") ||
                allWorldGameObjects.Any(a => a.name == "UniversalRiskyFuncDisable")) {
                Main.Log("GameObject found: Disabling Functions", Main.isDebug);
                CheckCompleted(false);
                yield break;
            }
            else if (allWorldGameObjects.Any(a => a.name == "eVRCRiskFuncEnable") || allWorldGameObjects.Any(a => a.name == "TPVRActionEnable") ||
                allWorldGameObjects.Any(a => a.name == "UniversalRiskyFuncEnable")) {
                Main.Log("GameObject found: Enabling Functions", Main.isDebug);
                CheckCompleted(true);
                yield break;
            }

            // Check if black/whitelisted from EmmVRC - thanks Emilia and the rest of EmmVRC Staff
            string url = $"https://dl.emmvrc.com/riskyfuncs.php?worldid={worldId}";
            WebClient www = new WebClient();
            while (www.IsBusy) yield return new WaitForEndOfFrame();
            string result = www.DownloadString(url)?.Trim().ToLower();
            /*
            var www = new HttpClient();
            var result = www.GetStringAsync(url)?.GetAwaiter().GetResult()?.Trim().ToLower();
            bool temp = false;
            try { while (!www.GetAsync(url).GetAwaiter().IsCompleted) temp = true; }
            catch (Exception e) { Main.Log(e, Main.isDebug, true); }
            if (temp) yield return new WaitForEndOfFrame();
            */
            www.Dispose();
            if (!string.IsNullOrWhiteSpace(result)) {
                switch (result) {
                    case "allowed":
                        CheckCompleted(true);
                        yield break;

                    case "denied":
                        CheckCompleted(false);
                        yield break;
                }
            }

            Main.Log("Checking World Tags, no response from EmmVRC", Main.isDebug);

            // no result from server or they're currently down
            // Check tags then. should also be in cache as it just got downloaded
            API.Fetch<ApiWorld>( worldId, new Action<ApiContainer>(container => {
                ApiWorld apiWorld;
                if ((apiWorld = container.Model.TryCast<ApiWorld>()) != null) {
                    foreach (string worldTag in apiWorld.tags) {
                        if (worldTag.IndexOf("game", StringComparison.OrdinalIgnoreCase) >= 0) {
                            CheckCompleted(false);
                            return;
                        }
                    }

                    CheckCompleted(true);
                }
                else Main.Logger.Error("Failed to cast ApiModel to ApiWorld");
            }), disableCache: false);

            // If all else fails, or is errored return false
            CheckCompleted(WorldAllowed);
        }

        internal static void OnLeftWorld() {
            CheckCompleted(false);
            VRUtils.active = false;
        }
    }
}
