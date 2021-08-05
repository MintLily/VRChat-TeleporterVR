# TeleporterVR
Easy Utility to allow you to teleport various different ways while being VR compliant. This mod was originally made by [Janni9009](https://github.com/Janni9009) for VRCModLoader, I have converted this to MelonLoader for all to enjoy.

### MelonLoader
Need to install MelonLoader?<br>
Click [this link](https://melonwiki.xyz/) to get started!

### Prerequisites
MelonLoader: v0.4.3 (Alpha)<br>
Game: VRChat (2021.3.2  [build 1121])<br>
Mod: [UIExpansionKit](https://github.com/knah/VRCMods)

### Optional Prerequisites
Mod: [ActionMenuApi](https://github.com/gompocp/ActionMenuApi) v0.3.1+

### MelonPreferences (Default Values)
```ini
[TeleporterVR]
UserInteractTPButtonVisible = true
UserInteractTPButtonPositionX = 1
UserInteractTPButtonPositionY = 3
preferRightHand = true
VRTeleportVisible = false
overrideLanguage = "off"
ActionMenuApiIntegration = false
EnableTeleportIndicator = true
IndicatorHexColor = "2dff2d"
EnableDesktopTP = false
```
UserInteractTPButtonVisible - User Select Teleport to player is visible<br>
UserInteractTPButtonPositionX - X-Coordinate (User Selected TPButton)<br>
UserInteractTPButtonPositionY - Y-Coordinate (User Selected TPButton)<br>
preferRightHand  - Prefer Right Handed (for VRTeleport)<br>
VRTeleportVisible - VRTeleport Button is visible (next to mute button)<br>
overrideLanguage - force mod into a provided language<br>
ActionMenuApiIntegration - Uses gompo's [ActionMenuApi](https://github.com/gompocp/ActionMenuApi) to add options to your Action Menu<br>
EnableTeleportIndicator - Shows a circle to where you will be teleported to when you press your trigger<br>
IndicatorHexColor - Color the Indicator to your liking, in the Hex Color format #rrggbb<br>
EnableDesktopTP - Allows you to teleport to your cursor (desktop only) [KeyBinds = LeftShift + T or Mouse3]

### Special Features
Dynamic Language Settings - The language of the mod will be determined by your system's local region, you can override this in the settings of the mod. Changing the language will update in real time (when you close the settings window)<br>
Input (partial) Name - input player's full name or partial name and teleport to them<br>
Coordinates - Input the [X Y Z] coords to teleport to that location<br>
Saved Positions - Save a Position in a world to later teleport to (Load Position) [This does not save across game restarts, but will save across world changes]<br>
VRTeleport - There is a Left/Right Hand toggle to then use your VR Laser Cursor to teleport to that location<br>
Oculus Support - Don't use SteamVR, that's okay, this mod was build for both Native Oculus and SteamVR VRChat use<br>
Languages - This mod uses multiple languages to the mod, you can force a language or let let the mod use your system's language<br>
ActionMenu Control - With gompo's [ActionMenuApi](https://github.com/gompocp/ActionMenuApi) mod, you can add the main buttons to your action menu in game

### Preview
![Preview Main Menu](https://kortyboi.com/img/upload/VRChat_Sfa0ZuMDwQ.jpg)<br>
![Preview Menu Content ENG](https://kortyboi.com/img/upload/VRChat_6oSV31AEjG.jpg)
![ActionMenu Preview](https://kortyboi.com/img/upload/ActionMenuApi-Preview.png)

# Credits
* RubyButtonAPI
* * DubyaDude, Emilia, Tritn


* Icons
* * [Font Awesome](https://fontawesome.com/)


* Language Translations
* * French - **Slaynash**
* * German - **Requi**
* * Japanese - N/A (Google Translate)
* * Norwegian (Bokmål) - **Frostbyte**
* * Russian - **Miinc**
* * Spanish - N/A (Google Translate)
* * Portuguese - **nitro.** & **Davi**
* * Swedish - **Psychloor**


* Other Code in the Mod
* * Patches - **Psychloor**
* * emmVRC Risky world / game tag toggling - **Psychloor**
* * Asset Bundle, Keyboard popup input - **knah**
* * TeleportIndicator Scripts - **Davi (d-mageek)**


# Change Log (since Lily's edits)
### v4.4.1
* Fixed Compatibility for VRChat Build 1121 (Unity 2019)
* Updated MelonLoader to v0.4.3
* Updated Dependency for [ActionMenuApi](https://github.com/gompocp/ActionMenuApi) to v0.3.1

### v4.4.0
* Added emmVRC GameObject detection to allow/disallow actions in worlds

### v4.3.1
* Security Fixes - Fixed Teleporting not being disabled properly

### v4.3.0
* Added Desktop Teleporting to cursor (Disabled by default) [KeyBinds = LeftShift + T or Mouse3]
* Fixed null errors on Controller Raycasts

### v4.2.3
* Fixed compatibility for VRChat build 1106
* Fixed various errors regarding the TPIndicator

### v4.2.0
* Added a TeleportIndicator, so you know where you're going
* Added Coloring to the Indicator
* Fixed an error about the ActionMenuApi onPrefSaved
* Update ActionMenuApi Dependency

### v4.1.1
* Fixed an error that would show when you didn't have ActionMenuApi installed
* Fixed the Left/Right Hand button toggle not changing its state on game start
* preferRightHand is now visible in UIX's MelonPref Viewer

### v4.1.0
* Added [ActionMenuApi](https://github.com/gompocp/ActionMenuApi) support
* Fixed VR Teleport

### v4.0.2
* Updated for VRChat build 1088

### v4.0.1
* Updated internal link

### v4.0.0
* Initial Release for MelonLoader

## Stay Updated
Stay update to date with all my mods by joining my [discord server](https://discord.gg/qkycuAMUGS) today.
