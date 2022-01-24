using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;
using VRC;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Menus;
using VRC.DataModel.Core;
using Object = UnityEngine.Object;

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
        
        private static SelectedUserMenuQM _selectedUserMenuQM;
        public static APIUser GetSelectedAPIUser() {
            if (_selectedUserMenuQM == null)
                _selectedUserMenuQM = Object.FindObjectOfType<SelectedUserMenuQM>();

            if (_selectedUserMenuQM != null) {
                DataModel<APIUser> user = _selectedUserMenuQM.field_Private_IUser_0.Cast<DataModel<APIUser>>();
                return user.field_Protected_TYPE_0;
            }

            Main.Logger.Error("Unable to get SelectedUserMenuQM component!");
            return null;
        }

        public static Player GetPlayer(this PlayerManager instance, string UserID) {
            Il2CppSystem.Collections.Generic.List<Player> allPlayers = instance.field_Private_List_1_Player_0;
            Player result = null;
            foreach (Player all in allPlayers)
                if (all.field_Private_APIUser_0.id == UserID)
                    result = all;
            return result;
        }
        
        public static VRCPlayer SelVRCPlayer() {
            return PlayerManager.field_Private_Static_PlayerManager_0.GetPlayer(GetSelectedAPIUser().id)._vrcplayer;
        }
    }
}
