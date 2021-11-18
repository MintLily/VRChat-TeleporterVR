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
        public static Sprite goodIcon, badIcon, DiscordLogo, GitHubLogo;
        public static Texture2D AMMain, AMBad, AMVRTP, AMSave, AMLoad, AMSL1, AMSL2, AMSL3, AMSL4;

		private static Sprite LoadSprite(string sprite)
		{
			Sprite sprite2 = Bundle.LoadAsset_Internal(sprite, Il2CppType.Of<Sprite>()).Cast<Sprite>();
			sprite2.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			sprite2.hideFlags = HideFlags.HideAndDontSave;
			return sprite2;
		}

        private static Texture2D LoadTexture(string Texture)
        {
            Texture2D Texture2 = Bundle.LoadAsset_Internal(Texture, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            Texture2.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            Texture2.hideFlags = HideFlags.HideAndDontSave;
            return Texture2;
        }

        private static GameObject LoadPrefab(string go) {
            GameObject go2 = Bundle.LoadAsset_Internal(go, Il2CppType.Of<GameObject>()).Cast<GameObject>();
            go2.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            go2.hideFlags = HideFlags.HideAndDontSave;
            return go2;
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
                    try { goodIcon = LoadSprite("people-solid.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: people-solid.png"); }
                    try { badIcon = LoadSprite("invalid.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: invalid.png"); }
                    try { DiscordLogo = LoadSprite("DiscordLogo.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: DiscordLogo.png"); }
                    try { GitHubLogo = LoadSprite("GitHubLogo.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: GitHubLogo.png"); }

                    try { Logic.CustomToggle.ToggleCanvas = LoadPrefab("TPVRToggleCanvas.prefab"); } catch { MelonLogger.Error("Failed to load image from asset bundle: TPVRToggleCanvas.prefab"); }

                    // Added with ActionMenuApi
                    try { AMMain = LoadTexture("people-solid-tex.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: people-solid-tex.png"); }
                    try { AMBad = LoadTexture("invalid-tex.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: invalid-tex.png"); }
                    try { AMVRTP = LoadTexture("vrtp-icon.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: vrtp-icon.png"); }
                    try { AMSave = LoadTexture("save-icon.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: save-icon.png"); }
                    try { AMLoad = LoadTexture("load-icon.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: load-icon.png"); }
                    try { AMSL1 = LoadTexture("Option1.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: Option1.png"); }
                    try { AMSL2 = LoadTexture("Option2.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: Option2.png"); }
                    try { AMSL3 = LoadTexture("Option3.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: Option3.png"); }
                    try { AMSL4 = LoadTexture("Option4.png"); } catch { MelonLogger.Error("Failed to load image from asset bundle: Option4.png"); }
                    // - End -
                }
            }

            if (Main.isDebug)
                MelonLogger.Msg("Finihsed with Asset Bundle Resource Managment");
            yield break;
        }
	}
}
