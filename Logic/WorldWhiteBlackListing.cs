using System.Collections;
using System.Linq;
using System.Net;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace TeleporterVR.Logic
{
    public class GetWorlds
    {
        public string WorldName;
        public string WorldID;
        public int buttonNumber;
        public string Reason;
    }

    internal class GetSetWorld
    {
        private static string ParsedWorldList;
        public static GetWorlds[] Worlds { get; internal set; }
        private static bool faulted = false;

        public static void Init() { MelonCoroutines.Start(SummomList()); }

        private static IEnumerator SummomList()
        {
            string url = "https://raw.githubusercontent.com/KortyBoi/VRChat-TeleporterVR/master/Logic/Worlds.json";
            WebClient WorldList = new WebClient();
            try { ParsedWorldList = WorldList.DownloadString(url); } catch { MelonLogger.Error("Could not get URL from Webhost (probable 404)"); }

            yield return new WaitForSeconds(0.5f);

            try { Worlds = JsonConvert.DeserializeObject<GetWorlds[]>(ParsedWorldList); } catch { MelonLogger.Error("Could not parse JSON data"); faulted = true; }
            yield break;
        }

        public static IEnumerator DelayedLoad()
        {
            yield return new WaitForSecondsRealtime(1);
            if (faulted) yield break;
            try
            {
                if (RoomManager.field_Internal_Static_ApiWorld_0 != null) {
                    if (Worlds.Any(x => x.WorldID.Equals(RoomManager.field_Internal_Static_ApiWorld_0.id))) {
                        MelonLogger.Msg(System.ConsoleColor.Red, "You have entered a protected world. Some buttons will not be toggleable.");
                             if (Worlds.Any(x => x.buttonNumber.Equals(1))) DoAction(1);
                        else if (Worlds.Any(x => x.buttonNumber.Equals(2))) DoAction(2);
                        else if (Worlds.Any(x => x.buttonNumber.Equals(3))) DoAction(3);
                    }
                    else DoAction(99);
                }
            }
            catch { MelonLogger.Error("Failed to Apply Actions or Read from list of worlds"); }
            yield break;
        }

        internal static void DoAction(int identifier)
        {
            switch (identifier)
            {
                case 1: // allowed
                    Utils.WorldActions.WorldAllowed = true;
                    if (Main.isDebug)
                        MelonLogger.Msg(System.ConsoleColor.Cyan, "Force Allowed");
                    Menu.userSel_TPto.Disabled(false);
                    Menu.VRTeleport.Disabled(false);
                    MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
                    break;
                case 2: // disallowed
                    Utils.WorldActions.WorldAllowed = false;
                    if (Main.isDebug)
                        MelonLogger.Msg(System.ConsoleColor.Red, "Force Disallowed");
                    Menu.userSel_TPto.Disabled(true);
                    Menu.VRTeleport.Disabled(true);
                    MelonCoroutines.Start(Menu.UpdateMenuIcon(false));
                    break;
                case 3: // only disable VRTP
                    Menu.VRTeleport.Disabled(true);
                    break;
                default: break; // Do nothing extra
            }
        }
    }
}
