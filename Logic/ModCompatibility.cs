using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace TeleporterVR.Logic
{
    class ModCompatibility
    {
        public static bool DiscordMute = false;

        public static void Init()
        {
            if (MelonHandler.Mods.FindIndex((MelonMod i) => i.Info.Name == "DiscordMute") != -1)
                DiscordMute = true;
            if (Main.isDebug && DiscordMute)
                MelonLogger.Msg("Detected DiscordMute");

            if (Main.isDebug)
                MelonLoader.MelonLogger.Msg("Finished with Mod Compatibility");
        }
    }
}
