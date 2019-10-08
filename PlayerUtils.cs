using System;
using System.Linq;
using System.Reflection;
using VRC;
using VRC.Core;
using VRCModLoader;

namespace Mod.Teleport
{
	internal static class PlayerUtils
	{
		static PlayerUtils()
		{
			try
			{
				PropertyInfo propertyInfo = typeof(Player).GetProperties().First((PropertyInfo p) => p.PropertyType == typeof(APIUser));
				PlayerUtils.getApiUserMethod = ((propertyInfo != null) ? propertyInfo.GetGetMethod() : null);
			}
			catch (Exception)
			{
#if (DEBUG)
                VRCModLogger.LogError("[TeleporterVR] Hmmm... seems like PlayerUtils failed, you might need to restart your game.");
#endif
            }
		}

		public static APIUser GetAPIUser(this Player player)
		{
			return (APIUser)PlayerUtils.getApiUserMethod.Invoke(player, null);
		}
           
        public static Player Target(string username)
        {
            foreach (Player p in PlayerManager.GetAllPlayers())
            {
                string user = GetAPIUser(p).displayName;
                if (user.ToLower().Contains(username.ToLower()))
                {
                    return p;
                }
            };
            return null;
            
        }
        private static MethodInfo getApiUserMethod;
	}
}
