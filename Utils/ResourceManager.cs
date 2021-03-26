using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using MelonLoader;
using System.Collections;
using System.IO;
using System.Reflection;

namespace TeleporterVR.Utils
{
    class ResourceManager
    {
		private static AssetBundle Bundle;
        public static Sprite goodIcon;
        public static Sprite badIcon;

		private static Sprite LoadSprite(string sprite)
		{
			Sprite sprite2 = Bundle.LoadAsset_Internal(sprite, Il2CppType.Of<Sprite>()).Cast<Sprite>();
			sprite2.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			sprite2.hideFlags = HideFlags.HideAndDontSave;
			return sprite2;
		}

		public static void Init() { MelonCoroutines.Start(LoadResources()); }

		private static IEnumerator LoadResources()
        {
            // Came from UIExpansionKit (https://github.com/knah/VRCMods/blob/master/UIExpansionKit)
            MelonLogger.Msg("Loading AssetBundle...");
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TeleporterVR.Resources.teleportervr.tpvrbundle"))
            {
                using (var memoryStream = new MemoryStream((int)stream.Length))
                {
                    stream.CopyTo(memoryStream);
                    Bundle = AssetBundle.LoadFromMemory_Internal(memoryStream.ToArray(), 0);
                    Bundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                    try { goodIcon = LoadSprite("people-solid.png"); } catch { MelonLogger.Error("Failed to load image 1"); }
                    try { badIcon = LoadSprite("invalid.png"); } catch { MelonLogger.Error("Failed to load image 2"); }
                }
            }

            if (Main.isDebug)
                MelonLoader.MelonLogger.Msg("Finihsed with Asset Bundle Resource Managment");
            yield break;
        }
	}
}
