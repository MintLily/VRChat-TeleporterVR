using System;
using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(TeleporterVR.BuildInfo.Name)]
[assembly: AssemblyDescription(TeleporterVR.BuildInfo.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(TeleporterVR.BuildInfo.Company)]
[assembly: AssemblyProduct(TeleporterVR.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + TeleporterVR.BuildInfo.Author)]
[assembly: AssemblyTrademark(TeleporterVR.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(TeleporterVR.BuildInfo.Version)]
[assembly: AssemblyFileVersion(TeleporterVR.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(TeleporterVR.Main),
    TeleporterVR.BuildInfo.Name,
    TeleporterVR.BuildInfo.Version,
    TeleporterVR.BuildInfo.Author,
    TeleporterVR.BuildInfo.DownloadLink)]
[assembly: MelonOptionalDependencies("ActionMenuApi")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: HarmonyDontPatchAll]