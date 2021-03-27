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

        public static PlayerManager GetPlayerManager() { return PlayerManager.field_Private_Static_PlayerManager_0; }

        public static void Teleport(VRCPlayer player)
        {
            GetLocalVRCPlayer().transform.position = player.transform.position;
            //GetLocalVRCPlayer().transform.rotation = player.transform.rotation;
        }

        public static VRCPlayer GetSelectedPlayer() { return QMStuff.GetQuickMenuInstance().field_Private_Player_0.field_Internal_VRCPlayer_0; }
    }
}
