using System;
using Il2CppSystem.Globalization;

namespace TeleporterVR.Logic;

public static class Language {
    public static string TpToNameText, TpToNameTooltip, TpToCoordText, TpToCoordTooltip, SavePos, SavePosToolTip, LoadPos, LoadPosTooltip, PerferedHandTooltip,
        TheWordTeleport, PreferedHandedTextOn, PreferedHandedTextOff;
    
    public static void InitLanguageChange() {
        var currentUiCulture = CultureInfo.CurrentUICulture;
        var loc = currentUiCulture.Name;

        var @override = MelonLoader.MelonPreferences.GetEntryValue<string>(Main.Melon.Identifier, Main.OverrideLanguage.Identifier);

        if ((loc.Contains("en") && @override.Equals("off")) || @override.Equals("en") || @override.Equals("off")) {
            TpToNameText = "Teleport To\n(Input Name)";
            TpToNameTooltip = "Opens a keyboard to input the name of the player you'd like to teleport to.";

            TpToCoordText = "Teleport To\n(Coordinates)";
            TpToCoordTooltip = "Opens a keyboard to input the \"X Y Z\" Coordinates in the world.";

            SavePos = "Save Position";
            LoadPos = "Load Position";

            SavePosToolTip = "Save your current position in the world.";
            LoadPosTooltip = "Teleport to the saved position.";

            PerferedHandTooltip = $"TOGGLE: Pressing {(Main.PreferRightHand.Value ? "Right" : "Left")} Trigger, you will teleport to the end of your {(Main.PreferRightHand.Value ? "Right" : "Left")} controller pointer (same as VRChat laser cursor)";

            TheWordTeleport = "Teleport"; 

            PreferedHandedTextOn = "Right Handed";
            PreferedHandedTextOff = "Left Handed";
        }
        if ((loc.Contains("fr") && @override.Equals("off")) || @override.Equals("fr")) {
            TpToNameText = "Se téléporter à\n(Nom Saisi)";
            TpToNameTooltip = "Ouvre un clavier pour saisir le nom du joueur sur lequel vous souhaitez vous téléporter";
                
            TpToCoordText = "Se téléporter à\n(Coordonées)";
            TpToCoordTooltip = "Ouvre un clavier pour saisir les coordonées \"X Y Z\" dans le monde.";

            SavePos = "Sauvegarder la position";
            LoadPos = "Charger la position";

            SavePosToolTip = "Sauvegarde votre position actuelle dans le monde.";
            LoadPosTooltip = "Téléporte à la position sauvegardée.";

            PerferedHandTooltip = $"TOGGLE: En appuyant sur la gâchette droite, vous vous téléporterez à la fin du pointeur de votre contrôleur {(Main.PreferRightHand.Value ? "droit" : "gauche")} (identique au curseur laser VRChat)";

            TheWordTeleport = "Téléportation";

            PreferedHandedTextOn = "Droitier";
            PreferedHandedTextOff = "Gaucher";
        }
        if ((loc.Contains("de") && @override.Equals("off")) || @override.Equals("de")) {
            TpToNameText = "Teleportiere zu\n(Name eingeben)";
            TpToNameTooltip = "Öffnet eine Tastatur in der du den Namen eingeben kannst, zu der du teleportieren möchtest";

            TpToCoordText = "Teleportiere zu\n(Koordinaten)";
            TpToCoordTooltip = "Öffnet eine Tastatur in der du die \"X Y Z\" Koordinaten eingeben kannst.";

            SavePos = "Speicher Position";
            LoadPos = "Lade Position";

            SavePosToolTip = "Speichert deine derzeitige Position.";
            LoadPosTooltip = "Teleportiere zur gespeicherten Position.";

            PerferedHandTooltip = $"TOGGLE: Durch Drücken des {(Main.PreferRightHand.Value ? "rechten" : "linken")} Auslösers teleportieren Sie zum Ende Ihres {(Main.PreferRightHand.Value ? "rechten" : "linken")} Controller-Zeigers (wie beim VRChat-Lasercursor).";

            TheWordTeleport = "Teleportieren";

            PreferedHandedTextOn = "Rechtshändig";
            PreferedHandedTextOff = "Linkshändig";
        }
        if ((loc.Contains("ja") && @override.Equals("off")) || @override.Equals("ja")) {
            TpToNameText = "\u30c6\u30ec\u30dd\u30fc\u30c8\u5148\n\uff08\u5165\u529b\u540d\uff09";
            TpToNameTooltip = "\u30ad\u30fc\u30dc\u30fc\u30c9\u3092\u958b\u3044\u3066\u3001\u30c6\u30ec\u30dd\u30fc\u30c8\u3057\u305f\u3044\u30d7\u30ec\u30a4\u30e4\u30fc\u306e\u540d\u524d\u3092\u5165\u529b\u3057\u307e\u3059";

            TpToCoordText = "\u30c6\u30ec\u30dd\u30fc\u30c8\u5148\n\uff08\u5ea7\u6a19\uff09";
            TpToCoordTooltip = "\u30ad\u30fc\u30dc\u30fc\u30c9\u3092\u958b\u3044\u3066\u3001\u4e16\u754c\u306e\u300cX Y Z\u300d\u5ea7\u6a19\u3092\u5165\u529b\u3057\u307e\u3059\u3002";

            SavePos = "\u4f4d\u7f6e\u3092\u4fdd\u5b58";
            LoadPos = "\u8377\u91cd\u4f4d\u7f6e";

            SavePosToolTip = "\u4e16\u754c\u3067\u3042\u306a\u305f\u306e\u73fe\u5728\u306e\u4f4d\u7f6e\u3092\u4fdd\u5b58\u3057\u307e\u3059\u3002";
            LoadPosTooltip = "\u4fdd\u5b58\u3057\u305f\u4f4d\u7f6e\u306b\u30c6\u30ec\u30dd\u30fc\u30c8\u3057\u307e\u3059\u3002";

            PerferedHandTooltip = Main.PreferRightHand.Value ?
                "トグル：右トリガーを押すと、右コントローラーポインターの端にテレポートします（VRChatレーザーカーソルと同じ）" :
                "トグル：左トリガーを押すと、左コントローラーポインターの端にテレポートします（VRChatレーザーカーソルと同じ）";

            TheWordTeleport = "テレポート";

            PreferedHandedTextOn = "右利き";
            PreferedHandedTextOff = "左利き";
        }
        if ((loc.Contains("no") && @override.Equals("off")) || @override.Equals("no")) {
            TpToNameText = "Teleporter Til\n(Spiller navn)";
            TpToNameTooltip = "Viser et tastatur hvor du kan skrive navnet til spilleren du ønsker å teleportere til.";

            TpToCoordText = "Teleporter Til\n(Koordinater)";
            TpToCoordTooltip = "Viser et tastatur hvor du kan skrive in \"X Y Z\" koordinater du ønsker å teleportere til.";

            SavePos = "Lagre din plassering";
            LoadPos = "Last in plassering";

            SavePosToolTip = "Lagre din plassering i verdenen";
            LoadPosTooltip = "Teleporter til den lagra plasseringen";

            PerferedHandTooltip = $"TOGGLE: Ved å trykke på {(Main.PreferRightHand.Value ? "høyre" : "venstre")} utløser vil du teleportere til slutten av {(Main.PreferRightHand.Value ? "høyre" : "venstre")} kontrollpeker (samme som VRChat-laserpekeren)";

            TheWordTeleport = "Teleportere";

            PreferedHandedTextOn = "Høyrehendt";
            PreferedHandedTextOff = "Venstrehendt";
        }
        if ((loc.Contains("ru") && @override.Equals("off")) || @override.Equals("ru")) {
            TpToNameText = "Телепортироваться к\n(по нику)";
            TpToNameTooltip = "Открывает клавиатуру для ввода ника игрока, к которому вы хотите телепортироваться";

            TpToCoordText = "Телепортироваться к\n(по координатам)";
            TpToCoordTooltip = "Открывает клавиатуру для ввода координат \"X Y Z\" в мире.";

            SavePos = "Сохранить позицию";
            LoadPos = "Восстановить позицию";

            SavePosToolTip = "Сохранить вашу текущую позицию в мире.";
            LoadPosTooltip = "Телепортироваться на сохранённую позицию.";

            PerferedHandTooltip = $"ПЕРЕКЛЮЧЕНИЕ: Нажав {(Main.PreferRightHand.Value ? "правый" : "левый")} триггер, вы телепортируетесь к концу указателя {(Main.PreferRightHand.Value ? "правого" : "левого")} контроллера (так же, как лазерный курсор VRChat).";

            TheWordTeleport = "Телепорт";

            PreferedHandedTextOn = "Правша";
            PreferedHandedTextOff = "Левша";
        }
        if ((loc.Contains("es") && @override.Equals("off")) || @override.Equals("es")) {
            TpToNameText = "Teletransportarse a\n(Nombre de entrada)";
            TpToNameTooltip = "Abre un teclado para ingresar el nombre del jugador al que te gustar\u00EDa teletransportarte";

            TpToCoordText = "Teletransportarse a\n(Coordenadas)";
            TpToCoordTooltip = "Abre un teclado para ingresar las coordenadas \"X Y Z\" en el mundo.";

            SavePos = "Guardar posicion";
            LoadPos = "Posici\u00F3n de carga";

            SavePosToolTip = "Guarde su posici\u00F3n actual en el mundo.";
            LoadPosTooltip = "Teletransportarse a la posici\u00F3n guardada.";

            PerferedHandTooltip = $"TOGGLE: Al presionar el gatillo {(Main.PreferRightHand.Value ? "derecho" : "izquierdo")}, se teletransportará al final del puntero del controlador {(Main.PreferRightHand.Value ? "derecho" : "izquierdo")} (igual que el cursor láser VRChat)";

            TheWordTeleport = "Teletransportarse";

            PreferedHandedTextOn = "Diestro";
            PreferedHandedTextOff = "Zurdo";
        }
        if ((loc.Contains("po") && @override.Equals("off")) || @override.Equals("po")) {
            TpToNameText = "Teleportar Para\n(Digitar Nome)";
            TpToNameTooltip = "Abre um teclado para digitar o nome do jogador que você gostaria de teleportar para";

            TpToCoordText = "Teleportar Para\n(Coordenadas)";
            TpToCoordTooltip = "Abre um teclado para digitar as coordenadas \"X Y Z\" no mundo.";

            SavePos = "Salvar Posição";
            LoadPos = "Carregar Posição";

            SavePosToolTip = "Salva sua posição atual no mundo.";
            LoadPosTooltip = "Teleporta para a posição salva.";

            PerferedHandTooltip = $"Alternar: Pressionando o gatilho {(Main.PreferRightHand.Value ? "direito" : "esquerdo")}, você se teleportará para o final do ponteiro de seu controle {(Main.PreferRightHand.Value ? "direito" : "esquerdo")} (o mesmo do laser do cursor do VRChat)";

            TheWordTeleport = "Teleportar";

            PreferedHandedTextOn = "Destro";
            PreferedHandedTextOff = "Canhoto";
        }
        if ((loc.Contains("sw") && @override.Equals("off")) || @override.Equals("sw")) {
            TpToNameText = "Teleportera Till\n(Inskrivet Namn)";
            TpToNameTooltip = "Öppnar upp ett tangentbord där du kan skriva in namnet till personen du vill teleportera till";

            TpToCoordText = "Teleportera Till\n(Koordinater)";
            TpToCoordTooltip = "Öppnar upp ett tangentbord där du kan skriva in \"X Y Z\" koordinaterna du vill teleporta till";

            SavePos = "Spara Position";
            LoadPos = "Ladda Sparad Position";

            SavePosToolTip = "Sparar din nuvarande position.";
            LoadPosTooltip = "Teleporterar dig till din sparade position.";

            PerferedHandTooltip = $"TOGGLE: Genom att trycka på {(Main.PreferRightHand.Value ? "höger" : "vänster")} avtryckare teleporterar du till slutet av din {(Main.PreferRightHand.Value ? "höger styrpekare" : "vänster kontrollpekare")} (samma som VRChat-lasermarkören)";

            TheWordTeleport = "Teleportera";

            PreferedHandedTextOn = "Högerhänt";
            PreferedHandedTextOff = "Vänsterhänt";
        }
        
        if (Main.IsDebug)
            Main.Logger.Msg(ConsoleColor.Green, "Finished with Languages");
    }
}
