using System;
using System.Net;
using System.Reflection;
using MelonLoader;

namespace TeleporterVR.Utils {
    public class ReMod_Core_Downloader {
        internal static bool failed;
        public static void LoadReModCore(out Assembly loadedAssembly) {
            byte[] bytes = null;
            var wc = new WebClient();
            try {
                bytes = wc.DownloadData("https://github.com/RequiDev/ReMod.Core/releases/latest/download/ReMod.Core.dll");
                loadedAssembly = Assembly.Load(bytes);
                Main.Log("Successfully Loaded ReMod.Core", Main.isDebug);
            }
            catch (WebException e) {
                failed = true;
                Main.Logger.Error($"Unable to Load Core Dep ReModCore: {e}");
            }
            catch (BadImageFormatException e) {
                failed = true;
                loadedAssembly = null;
            }
            loadedAssembly = null;
        }
    }
}