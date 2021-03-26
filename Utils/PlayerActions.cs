using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;
using VRC;
using UnityEngine;
using UnityEngine.UI;

namespace TeleporterVR.Utils
{
    internal static class PlayerActions
    {
        internal static VRCPlayer GetLocalVRCPlayer() { return VRCPlayer.field_Internal_Static_VRCPlayer_0; }
        internal static APIUser GetApiUser(Player player) { return player.field_Private_APIUser_0; }

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

        public static QuickMenu GetQuickMenu() { return QuickMenu.prop_QuickMenu_0; }

        public static Player GetSelectedPlayer(this QuickMenu instance)
        {
            APIUser field_Private_APIUser_ = instance.field_Private_APIUser_0;
            return GetPlayerManager().GetPlayer(field_Private_APIUser_.id);
        }

        public static Il2CppSystem.Collections.Generic.List<Player> GetAllPlayers(this PlayerManager instance) { return instance.field_Private_List_1_Player_0; }

        public static APIUser GetAPIUser(this Player player) { return player.field_Private_APIUser_0; }

        public static Player GetPlayer(this PlayerManager instance, string UserID)
        {
            Il2CppSystem.Collections.Generic.List<Player> allPlayers = instance.GetAllPlayers();
            Player result = null;
            for (int i = 0; i < allPlayers.Count; i++)
            {
                Player player = allPlayers[i];
                if (player.GetAPIUser().id == UserID)
                {
                    result = player;
                }
            }
            return result;
        }

        public static PlayerManager GetPlayerManager() { return PlayerManager.field_Private_Static_PlayerManager_0; }

        public static void Teleport(Player player)
        {
            GetLocalVRCPlayer().transform.position = player.transform.position;
            GetLocalVRCPlayer().transform.rotation = player.transform.rotation;
        }
    }
}
