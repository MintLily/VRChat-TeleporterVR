using System.Collections;
using System.Linq;
using System.Net;
using MelonLoader;
using UnityEngine;
using ActionMenuApi.Api;
using Newtonsoft.Json;
using System.Threading.Tasks;
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

        public static void Init() => MelonCoroutines.Start(SummomList());

        private static IEnumerator SummomList()
        {
            string url = "https://raw.githubusercontent.com/MintLily/VRChat-TeleporterVR/master/Logic/Worlds.json";
            WebClient WorldList = new WebClient();
            try { ParsedWorldList = WorldList.DownloadString(url); } catch { Main.Logger.Error("Could not get URL from Webhost (probable 404)"); }

            yield return new WaitForSeconds(0.5f);

            try {
                Worlds = JsonConvert.DeserializeObject<GetWorlds[]>(ParsedWorldList);
            }
            catch {
                Main.Logger.Error("Could not parse JSON data");
                faulted = true;
            }
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
                        Main.Logger.Msg(System.ConsoleColor.Red, "You have entered a protected world. Some buttons will not be toggleable.");
                             if (Worlds.Any(x => x.buttonNumber.Equals(1))) DoAction(1);
                        else if (Worlds.Any(x => x.buttonNumber.Equals(2))) DoAction(2);
                        else if (Worlds.Any(x => x.buttonNumber.Equals(3))) DoAction(3);
                    }
                    else DoAction(99);
                }
            }
            catch { Main.Logger.Error("Failed to Apply Actions or Read from list of worlds"); }
            yield break;
        }

        internal static void DoAction(int identifier)
        {
            switch (identifier)
            {
                case 1: // allowed
                    Utils.WorldActions.WorldAllowed = true;
                    if (Main.isDebug)
                        Main.Logger.Msg(System.ConsoleColor.Cyan, "Force Allowed");

                    if (Main.ActionMenuApiIntegration.Value && ActionMenu.hasAMApiInstalled) {
                        ActionMenu.CheckForRiskyFunctions(false);
                        MelonCoroutines.Start(ActionMenu.UpdateIcon(false));
                    }
                    break;
                case 2: // disallowed
                    Utils.WorldActions.WorldAllowed = false;
                    if (Main.isDebug)
                        Main.Logger.Msg(System.ConsoleColor.Red, "Force Disallowed");
                    
                    if (Main.ActionMenuApiIntegration.Value && ActionMenu.hasAMApiInstalled) {
                        ActionMenu.CheckForRiskyFunctions(true);
                        MelonCoroutines.Start(ActionMenu.UpdateIcon(false));
                    }
                    Utils.VRUtils.active = false;
                    break;
                case 3: // only disable VRTP
                    Utils.VRUtils.active = false;
                    break;
                default: break; // Do nothing extra
            }
        }
    }
}
