using System;
using UnityEngine;
using VRCModLoader;
using VRC;

namespace Mod.Teleport
{
	[VRCModInfo("TeleporterVR", "3.0.0", "Janni9009")] //Thank you Slaynash and Yoshifan for saving me from a mental breakdown
	public class Teleport : VRCMod
	{
        public void OnApplicationStart()
        {
            VRCModLogger.Log("[TeleporterVR] TeleporterVR usage guide:\n >Press Shift+T to enable keybinds\n >>Press F to store your position\n >>>Press R to load the stored position\n >>Press G to open the input window\n Credits: Author - Janni9009, UI page help - yoshifan, Playermanager foreach loop - Slaynash");
#if (DEBUG)
            Debug = true;
            VRCModLogger.Log("[TeleporterVR] Debug mode enabled!");
#endif
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

        public void OnUpdate()
		{
			if (Event.current.shift && Input.GetKeyDown(KeyCode.T))
			{
				EnableKeys = !EnableKeys;
                if (Debug)
                {
                    VRCModLogger.Log("[TeleporterVR] Keys toggled!");
                }
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
            if (EnableKeys && Input.GetKeyDown(KeyCode.G))
            {
                InputB.InputAttempt("Teleport");
                if (Debug)
                {
                    VRCModLogger.Log("[TeleporterVR] Teleport to player key pressed!");
                    VRCModLogger.Log("[TeleporterVR] Received " + InputB.inputcheck);
                    VRCModLogger.Log("[TeleporterVR] Teleporting to " + InputB.namecheck);
                }
            }
        }
        public bool Debug = false;

		private Vector3 pos;

		private Quaternion rot;

		private bool EnableKeys;
	}
}
