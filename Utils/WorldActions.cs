using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine;
using VRC.Core;
using UnityEngine.SceneManagement;

namespace TeleporterVR.Utils {
    // Came from https://github.com/RequiDev/ReModCE/blob/master/ReModCE/Managers/RiskyFunctionsManager.cs
    internal class CheckWorldAllowed {
        //public static CheckWorldAllowed Instance;

        public static event Action<bool> OnRiskyFunctionsChanged;

        private static readonly List<string> BlacklistedTags = new() {
            "author_tag_game",
            "author_tag_games",
            "author_tag_club",
            "admin_game"
        };

        public static bool RiskyFunctionAllowed { get; internal set; }

        //public CheckWorldAllowed() { Instance = this; }

        public static void WorldChange(int buildIndex) {
            if (buildIndex == -1) MelonCoroutines.Start(CheckWorld());
        }

        private static IEnumerator CheckWorld() {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                yield return new WaitForEndOfFrame();
            var apiWorld = RoomManager.field_Internal_Static_ApiWorld_0;

            var worldName = apiWorld.name.ToLower();
            var tags = new List<string>();
            foreach (var tag in apiWorld.tags)
                tags.Add(tag.ToLower());

            var hasBlacklistedTag = BlacklistedTags.Any(tag => tags.Contains(tag));
            var riskyFunctionAllowed = !worldName.Contains("club") && !worldName.Contains("game") && !hasBlacklistedTag;

            var instanceAccessType = RoomManager.field_Internal_Static_ApiWorldInstance_0.type;
            riskyFunctionAllowed = !(instanceAccessType == InstanceAccessType.Public || instanceAccessType == InstanceAccessType.FriendsOfGuests);

            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            if (rootGameObjects.Any(go => go.name is "eVRCRiskFuncDisable" or "UniversalRiskyFuncDisable"))
                riskyFunctionAllowed = false;
            else if (rootGameObjects.Any(go => go.name is "eVRCRiskFuncEnable" or "UniversalRiskyFuncEnable"))
                riskyFunctionAllowed = true;

            RiskyFunctionAllowed = riskyFunctionAllowed;
            OnRiskyFunctionsChanged?.Invoke(RiskyFunctionAllowed);

            Main.Log($"RiskyFunctions: {RiskyFunctionAllowed}", Main.IsDebug);
            NewUi.UpdateWorldActions(RiskyFunctionAllowed);
            ActionMenu.CheckForRiskyFunctions(!RiskyFunctionAllowed);
        }

        public static void OnWorldLeave() => RiskyFunctionAllowed = false;
    }
}
