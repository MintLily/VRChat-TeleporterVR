using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;
using VRC;
using UnityEngine;
using UnityEngine.UI;
using RubyButtonAPIVRT;
using VRC.UI.Elements.Menus;
using VRC.DataModel;

namespace TeleporterVR.Utils
{
    internal static class PlayerActions
    {
        internal static VRCPlayer GetLocalVRCPlayer() { return VRCPlayer.field_Internal_Static_VRCPlayer_0; }
        internal static APIUser GetApiUser(Player player) { return player.prop_APIUser_0; }

        public static Player Target(string username)
        {
            foreach (Player p in GetAllPlayers())
            {
                string user = GetApiUser(p).displayName;
                if (user.ToLower().Contains(username.ToLower()))
                    return p;
            }
            return null;
        }

        public static Il2CppSystem.Collections.Generic.List<Player> GetAllPlayers()
        {
            if (GetPlayerManager() == null) return null;
            return GetPlayerManager().field_Private_List_1_Player_0;
        }

        public static PlayerManager GetPlayerManager() { return PlayerManager.field_Private_Static_PlayerManager_0; }

        public static void Teleport(VRCPlayer player) { GetLocalVRCPlayer().transform.position = player.transform.position; }

        public static VRCPlayer GetSelectedPlayer()
        {
			// This workaround is needed because QuickMenu.field_Private_Player_0 only gets set when clicking on a user with the pointer
			// and not when selecting them from the "Here" tab in QM.
            var userID = GetSelectedIUser().prop_String_0;
            return GetPlayer(userID)._vrcplayer;
        }

        public static IUser GetSelectedIUser() { return GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local").GetComponent<SelectedUserMenuQM>().field_Private_IUser_0; }

        public static Player GetPlayer(string userId)
        {
            foreach (var player in GetPlayerManager().prop_ArrayOf_Player_0)
            {
                if (player == null)
                    continue;

                var apiUser = player.field_Private_APIUser_0;
                if (apiUser == null)
                    continue;

                if (apiUser.id == userId)
                    return player;
            }

            return null;
        }
    }
}
