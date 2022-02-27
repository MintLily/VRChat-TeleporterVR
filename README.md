<h1>TeleporterVR</h1>
Easy Utility to allow you to teleport various different ways while being VR compliant. This mod was originally made by [Janni9009](https://github.com/Janni9009) for VRCModLoader, I have converted this to MelonLoader for all to enjoy.

<h3>MelonLoader</h3>
Need to install MelonLoader?<br>
Click <a href="https://melonwiki.xyz/">this link</a> to get started!

<h3>Prerequisites</h3>
MelonLoader: v0.5.3+<br>
Game: VRChat build 1169+<br>
Mods:
<ul>
	<li><a href="https://github.com/knah/VRCMods">UIExpansionKit</a></li>
	<li><a href="https://github.com/gompoc/VRChatMods/tree/master/ActionMenuApi">ActionMenuApi</a></li>
</ul>

### MelonPreferences (Default Values)
```ini
[TeleporterVR]
preferRightHand = true
VRTeleportVisible = false
overrideLanguage = "off"
ActionMenuApiIntegration = false
EnableTeleportIndicator = true
IndicatorHexColor = "2dff2d"
EnableDesktopTP = false
UIXTPVR = false
UIXMenu = false
```
preferRightHand  - Prefer Right Handed (for VR Teleporting)<br>
VRTeleportVisible - VRTeleport Button is visible (next to mute button)<br>
overrideLanguage - force mod into a provided language<br>
ActionMenuApiIntegration - Uses gompo's [ActionMenuApi](https://github.com/gompoc/VRChatMods/tree/master/ActionMenuApi) to add options to your Action Menu<br>
EnableTeleportIndicator - Shows a circle to where you will be teleported to when you press your trigger<br>
IndicatorHexColor - Color the Indicator to your liking, in the Hex Color format #rrggbb<br>
EnableDesktopTP - Allows you to teleport to your cursor (desktop only) [KeyBinds = LeftShift + T or Mouse3]<br>
UIXTPVR - Puts a quick VR Teleport toggle on your main UIX Menu<br>
UIXMenu - Would you like to use a UIX Menu or a menu built with [ReMod.Core](https://github.com/RequiDev/ReMod.Core)

<h3>Special Features</h3>
Dynamic Language Settings - The language of the mod will be determined by your system's local region, you can override this in the settings of the mod. Changing the language will update in real time (when you close the settings window)<br>
Input (partial) Name - input player's full name or partial name and teleport to them<br>
Coordinates - Input the [X Y Z] coords to teleport to that location<br>
Saved Positions - Save a Position in a world to later teleport to (Load Position) [This does not save across game restarts, but will save across world changes]<br>
VRTeleport - There is a Left/Right Hand toggle to then use your VR Laser Cursor to teleport to that location<br>
Oculus Support - Don't use SteamVR, that's okay, this mod was build for both Native Oculus and SteamVR VRChat use<br>
Languages - This mod uses multiple languages to the mod, you can force a language or let let the mod use your system's language<br>
ActionMenu Control - With gompo's <a href="https://github.com/gompoc/VRChatMods/tree/master/ActionMenuApi">ActionMenuApi</a> mod, you can add the main buttons to your action menu in game

<h3>Preview</h3>
<img src="https://i.mintlily.lgbt/6sQ1eU5xMyvL.jpg" alt="Preview Menu Content ENG" /><br>
<img src="https://i.mintlily.lgbt/AMApiPreview.jpg" alt="ActionMenu Preview" />

<h2>Credits</h2>
<details>
	<summary>Icons</summary>
	<ul>
		<li><a href="https://fontawesome.com/">Font Awesome</a></li>
		<li><a href="https://thenounproject.com/">The Noun Project</a></li>
	</ul>
</details>

<details>
	<summary>Language Translations</summary>
	<ul>
		<li>French - <b>Slaynash</b></li>
		<li>German - <b>RequiDev</b></li>
		<li>Japanese - N/A (Google Translate)</li>
		<li>Norwegian (Bokmål) - <b>Frostbyte</b></li>
		<li>Russian - <b>Miinc</b></li>
		<li>Spanish - Myself & Google Translate</li>
		<li>Portuguese - <b>nitro.</b> & <b>Davi</b></li>
		<li>Swedish - <b>Psychloor</b></li>
	</ul>
</details>

<details>
	<summary>Other Code in the Mod</summary>
	<ul>
		<li>Patches - <b>DDAkabono</b></li>
		<li>emmVRC Risky world / game tag toggling - <b>Psychloor</b></li>
		<li>Asset Bundle, Keyboard popup input, Enable/Disable Listener - <b>knah</b></li>
		<li>TeleportIndicator Scripts - <b>Davi (d-mageek)</b></li>
		<li>NewUi - <b>RequiDev</b></li>
	</ul>
</details>

<h1>Change Log (since Lily's edits)</h1>
<h3>v4.10.0</h3>
<ul>
	<li>Added option for a menu made with <a href="https://github.com/RequiDev/ReMod.Core">ReMod.Core</a></li>
	<li>Made <a href="https://github.com/gompoc/VRChatMods/tree/master/ActionMenuApi">ActionMenuApi</a> a Prerequisite Mod</li>
</ul>

### v4.9.2
* Fixed Errors showing when changing worlds if AMApi is disabled or null

### v4.9.1
* Revert HttpClient back to WebClient
* Fixed Action Menu always being locked
* Fixed open webpage

### v4.9.0
* Changed MelonLogger to MelonLogger.Instance
* Changed World Check Patches to OnJoin/Leave instead of OnFade (Thanks Bono)
* Fixed ActionMenu VRTP Toggle not being consistent to the actual setting

<details>
	<summary>Past Versions</summary>

### v4.8.1
* Fixed Popup keyboards not showing

### v4.8.0
* Added User Selected Teleport button (with UIX)
* Fixed perfered hand option not being updated properly

### v4.7.0
* Added VRChat build 1151 compatibility
* Removed RubyButtonAPI

### v4.6.0
* Added UIX Menu
* Added VRChat UI Open Beta Detection for less breakage

### v4.5.1
* Added **UniversalRiskyFunc GameObject Toggle**

### v4.5.0
* Changed ActionMenu is Open Listener
* Fixed odd behavior when toggling on teleportation

### v4.4.2
* Fixed issue where ActionMenu would not close

### v4.4.1
* Fixed Compatibility for VRChat Build 1121 (Unity 2019)
* Updated MelonLoader to v0.4.3
* Updated Dependency for [ActionMenuApi](https://github.com/gompoc/VRChatMods/tree/master/ActionMenuApi) to v0.3.1

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
* Added [ActionMenuApi](https://github.com/gompoc/VRChatMods/tree/master/ActionMenuApi) support
* Fixed VR Teleport

### v4.0.2
* Updated for VRChat build 1088

### v4.0.1
* Updated internal link

### v4.0.0
* Initial Release for MelonLoader


</details>