using System;
using UnityEngine;
using VRCModLoader;
using VRC;
using SPS;
using System.Collections;
using VRCMenuUtils;

namespace Mod.Teleport
{
    [VRCModInfo("TeleportVR_VRFriendly", "3.1.0", "VRInterface_By_GhostiL (Original_idea_By)=>Janni9009")] //Thank you Slaynash and Yoshifan for saving me from a mental breakdown
    public class Teleport : VRCMod
    {
        public void OnApplicationStart()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            VRCModLogger.Log("\n[TeleporterVR]\n VR_Friendly_Modification By GhostYiL\n Credits: Author - Janni9009;\n UI page help -yoshifan;\n Playermanager foreach loop - Slaynash\n DubyaDude - ButtonAPI(I didn't forget about you ;D)");
            ModManager.StartCoroutine(this.Setup());
#if (DEBUG)
            Debug = true;
            VRCModLogger.Log("[TeleporterVR] Debug mode enabled!");
#endif
        }
        private IEnumerator Setup()
        {
            yield return VRCMenuUtilsAPI.WaitForInit();
            VRCMenuUtilsAPI.AddQuickMenuButton("Teleport", "Teleport", "Teleports u to a player by name", new Action(this.TELEPORT));
            VRCMenuUtilsAPI.AddQuickMenuButton("SaveCurrent\nPosition", "Save\nCurrent\nPos", "Store ur current world position", new Action(this.SavePosition));
            VRCMenuUtilsAPI.AddQuickMenuButton("Tp-To\nStoredPosition", "Tp-To\nStoredPos", "Teleports u to stored world position", new Action(this.TPToStoredPos));
            yield break;
        }
        public void StorePos(out Vector3 position, out Quaternion rotation)
        {
            position = VRCPlayer.Instance.transform.position;
            rotation = VRCPlayer.Instance.transform.rotation;
        }

        public void LoadPos(Vector3 position, Quaternion rotation)
        {
            VRCPlayer.Instance.transform.position = position;
            VRCPlayer.Instance.transform.rotation = rotation;
        }
       

        public void Start()

        {
            EnableKeys = !EnableKeys;

            {
                VRCModLogger.Log("[TeleporterVR] Keys toggled!");
            }


            if (EnableKeys && Input.GetKeyDown(KeyCode.F))
            {
                StorePos(out pos, out rot);
                if (Debug)
                {
                    VRCModLogger.Log("[TeleporterVR] Position store key pressed!");
                    if (pos == new Vector3(0f, 0f, 0f))
                    {
                        VRCModLogger.LogError("[TeleporterVR] There may have been an issue saving your position!");
                    }
                    if (rot == new Quaternion(0f, 0f, 0f, 0f))
                    {
                        VRCModLogger.LogError("[TeleporterVR] There may have been an issue saving your rotation!");
                    }
                }
            }
            if (EnableKeys && Input.GetKeyDown(KeyCode.R))
            {
                LoadPos(pos, rot);
                if (Debug)
                {
                    VRCModLogger.Log("[TeleporterVR] Teleport to saved key pressed!");
                    if (VRCPlayer.Instance.transform.position != pos)
                    {
                        VRCModLogger.LogError("[TeleporterVR] Error adjusting position!");
                    }
                    if (VRCPlayer.Instance.transform.rotation != rot)
                    {
                        VRCModLogger.LogError("[TeleporterVR] Error adjusting rotation!");
                    }
                }
            }
        }
        public  void TELEPORT()
        {
            InputB.InputAttempt("Teleport");

            {
                VRCModLogger.Log("[TeleporterVR] Teleport to player key pressed!");
                VRCModLogger.Log("[TeleporterVR] Received " + InputB.inputcheck);
                VRCModLogger.Log("[TeleporterVR] Teleporting to " + InputB.namecheck);
            }
        }
        public  void TPToStoredPos()

        {
            LoadPos(pos, rot);

            {
                VRCModLogger.Log("[TeleporterVR] Teleport to saved key pressed!");
                if (VRCPlayer.Instance.transform.position != pos)
                {
                    VRCModLogger.LogError("[TeleporterVR] Error adjusting position!");
                }
                if (VRCPlayer.Instance.transform.rotation != rot)
                {
                    VRCModLogger.LogError("[TeleporterVR] Error adjusting rotation!");
                }
            }
        }

        /*public static void Transmists()
        {
            Teleport.TPToStoredPos();
        }*/

        public void SavePosition()
        
            {

                StorePos(out pos, out rot);
                if (Debug)
                {
                    VRCModLogger.Log("[TeleporterVR] Position store key pressed!");
                    if (pos == new Vector3(0f, 0f, 0f))
                    {
                        VRCModLogger.LogError("[TeleporterVR] There may have been an issue saving your position!");
                    }
                    if (rot == new Quaternion(0f, 0f, 0f, 0f))
                    {
                        VRCModLogger.LogError("[TeleporterVR] There may have been an issue saving your rotation!");
                    }
                }
            }
        



        public bool Debug = false;

        private Vector3 pos;

        private Quaternion rot;

        private bool EnableKeys;
    }
}

