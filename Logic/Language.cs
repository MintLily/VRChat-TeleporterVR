using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppSystem.Globalization;

namespace TeleporterVR.Logic
{
    public class Language
    {
        public static CultureInfo InstalledUICulture { get; }
        public static string TPtoName_Text;
        public static string TPtoName_Tooltip;
        public static string TPtoCoord_Text;
        public static string TPtoCoord_Tooltip;
        public static string SavePos;
        public static string SavePos_ToolTip;
        public static string LoadPos;
        public static string LoadPos_Tooltip;
        public static string perferedHand_Tooltip;
        public static string theWord_Teleport;
        public static string preferedHanded_Text_ON;
        public static string preferedHanded_Text_OFF;

        public static void InitLanguageChange()
        {
            CultureInfo ci = CultureInfo.CurrentUICulture;
            ci = CultureInfo.CurrentUICulture;
            string loc = ci.Name.ToString();

            string @override = MelonLoader.MelonPreferences.GetEntryValue<string>(Main.melon.Identifier, Main.OverrideLanguage.Identifier);

            if ((loc.Contains("en") && @override.Equals("off")) || @override.Equals("en") || @override.Equals("off"))
            {
                TPtoName_Text = "Teleport To\n(Input Name)";
                TPtoName_Tooltip = "Opens a keyboard to input the name of the player you'd like to teleport to.";

                TPtoCoord_Text = "Teleport To\n(Coordinates)";
                TPtoCoord_Tooltip = "Opens a keyboard to input the \"X Y Z\" Coordinates in the world.";

                SavePos = "Save Position";
                LoadPos = "Load Position";

                SavePos_ToolTip = "Save your current position in the world.";
                LoadPos_Tooltip = "Teleport to the saved position.";

                perferedHand_Tooltip = Main.preferRightHand.Value ?
                    "TOGGLE: Pressing Right Trigger, you will teleport to the end of your right controller pointer (same as VRChat laser cursor)" :
                    "TOGGLE: Pressing Left Trigger, you will teleport to the end of your left controller pointer (same as VRChat laser cursor)";

                theWord_Teleport = "Teleport"; 

                preferedHanded_Text_ON = "Right Handed";
                preferedHanded_Text_OFF = "Left Handed";
            }
            if ((loc.Contains("fr") && @override.Equals("off")) || @override.Equals("fr"))
            {
                TPtoName_Text = "Se téléporter à\n(Nom Saisi)";
                TPtoName_Tooltip = "Ouvre un clavier pour saisir le nom du joueur sur lequel vous souhaitez vous téléporter";
                
                TPtoCoord_Text = "Se téléporter à\n(Coordonées)";
                TPtoCoord_Tooltip = "Ouvre un clavier pour saisir les coordonées \"X Y Z\" dans le monde.";

                SavePos = "Sauvegarder la position";
                LoadPos = "Charger la position";

                SavePos_ToolTip = "Sauvegarde votre position actuelle dans le monde.";
                LoadPos_Tooltip = "Téléporte à la position sauvegardée.";

                perferedHand_Tooltip = Main.preferRightHand.Value ? 
                    "TOGGLE: En appuyant sur la gâchette droite, vous vous téléporterez à la fin du pointeur de votre contrôleur droit (identique au curseur laser VRChat)" :
                    "TOGGLE: En appuyant sur la gâchette gauche, vous vous téléporterez à la fin du pointeur de votre contrôleur gauche (identique au curseur laser VRChat)";

                theWord_Teleport = "Téléportation";

                preferedHanded_Text_ON = "Droitier";
                preferedHanded_Text_OFF = "Gaucher";
            }
            if ((loc.Contains("de") && @override.Equals("off")) || @override.Equals("de"))
            {
                TPtoName_Text = "Teleportiere zu\n(Name eingeben)";
                TPtoName_Tooltip = "Öffnet eine Tastatur in der du den Namen eingeben kannst, zu der du teleportieren möchtest";

                TPtoCoord_Text = "Teleportiere zu\n(Koordinaten)";
                TPtoCoord_Tooltip = "Öffnet eine Tastatur in der du die \"X Y Z\" Koordinaten eingeben kannst.";

                SavePos = "Speicher Position";
                LoadPos = "Lade Position";

                SavePos_ToolTip = "Speichert deine derzeitige Position.";
                LoadPos_Tooltip = "Teleportiere zur gespeicherten Position.";

                perferedHand_Tooltip = Main.preferRightHand.Value ?
                    "TOGGLE: Durch Drücken des rechten Auslösers teleportieren Sie zum Ende Ihres rechten Controller-Zeigers (wie beim VRChat-Lasercursor)." :
                    "TOGGLE: Durch Drücken des linken Auslösers teleportieren Sie zum Ende Ihres linken Controller-Zeigers (wie beim VRChat-Lasercursor).";

                theWord_Teleport = "Teleportieren";

                preferedHanded_Text_ON = "Rechtshändig";
                preferedHanded_Text_OFF = "Linkshändig";
            }
            if ((loc.Contains("ja") && @override.Equals("off")) || @override.Equals("ja"))
            {
                TPtoName_Text = "\u30c6\u30ec\u30dd\u30fc\u30c8\u5148\n\uff08\u5165\u529b\u540d\uff09";
                TPtoName_Tooltip = "\u30ad\u30fc\u30dc\u30fc\u30c9\u3092\u958b\u3044\u3066\u3001\u30c6\u30ec\u30dd\u30fc\u30c8\u3057\u305f\u3044\u30d7\u30ec\u30a4\u30e4\u30fc\u306e\u540d\u524d\u3092\u5165\u529b\u3057\u307e\u3059";

                TPtoCoord_Text = "\u30c6\u30ec\u30dd\u30fc\u30c8\u5148\n\uff08\u5ea7\u6a19\uff09";
                TPtoCoord_Tooltip = "\u30ad\u30fc\u30dc\u30fc\u30c9\u3092\u958b\u3044\u3066\u3001\u4e16\u754c\u306e\u300cX Y Z\u300d\u5ea7\u6a19\u3092\u5165\u529b\u3057\u307e\u3059\u3002";

                SavePos = "\u4f4d\u7f6e\u3092\u4fdd\u5b58";
                LoadPos = "\u8377\u91cd\u4f4d\u7f6e";

                SavePos_ToolTip = "\u4e16\u754c\u3067\u3042\u306a\u305f\u306e\u73fe\u5728\u306e\u4f4d\u7f6e\u3092\u4fdd\u5b58\u3057\u307e\u3059\u3002";
                LoadPos_Tooltip = "\u4fdd\u5b58\u3057\u305f\u4f4d\u7f6e\u306b\u30c6\u30ec\u30dd\u30fc\u30c8\u3057\u307e\u3059\u3002";

                perferedHand_Tooltip = Main.preferRightHand.Value ?
                    "トグル：右トリガーを押すと、右コントローラーポインターの端にテレポートします（VRChatレーザーカーソルと同じ）" :
                    "トグル：左トリガーを押すと、左コントローラーポインターの端にテレポートします（VRChatレーザーカーソルと同じ）";

                theWord_Teleport = "テレポート";

                preferedHanded_Text_ON = "右利き";
                preferedHanded_Text_OFF = "左利き";
            }
            if ((loc.Contains("no") && @override.Equals("off")) || @override.Equals("no"))
            {
                TPtoName_Text = "Teleporter Til\n(Spiller navn)";
                TPtoName_Tooltip = "Viser et tastatur hvor du kan skrive navnet til spilleren du ønsker å teleportere til.";

                TPtoCoord_Text = "Teleporter Til\n(Koordinater)";
                TPtoCoord_Tooltip = "Viser et tastatur hvor du kan skrive in \"X Y Z\" koordinater du ønsker å teleportere til.";

                SavePos = "Lagre din plassering";
                LoadPos = "Last in plassering";

                SavePos_ToolTip = "Lagre din plassering i verdenen";
                LoadPos_Tooltip = "Teleporter til den lagra plasseringen";

                perferedHand_Tooltip = Main.preferRightHand.Value ?
                    "TOGGLE: Ved å trykke på høyre utløser vil du teleportere til slutten av høyre kontrollpeker (samme som VRChat-laserpekeren)" :
                    "TOGGLE: Ved å trykke på venstre utløser vil du teleportere til slutten av venstre kontrollerpeker (samme som VRChat-laserpekeren)";

                theWord_Teleport = "Teleportere";

                preferedHanded_Text_ON = "Høyrehendt";
                preferedHanded_Text_OFF = "Venstrehendt";
            }
            if ((loc.Contains("ru") && @override.Equals("off")) || @override.Equals("ru"))
            {
                TPtoName_Text = "Телепортироваться к\n(по нику)";
                TPtoName_Tooltip = "Открывает клавиатуру для ввода ника игрока, к которому вы хотите телепортироваться";

                TPtoCoord_Text = "Телепортироваться к\n(по координатам)";
                TPtoCoord_Tooltip = "Открывает клавиатуру для ввода координат \"X Y Z\" в мире.";

                SavePos = "Сохранить позицию";
                LoadPos = "Восстановить позицию";

                SavePos_ToolTip = "Сохранить вашу текущую позицию в мире.";
                LoadPos_Tooltip = "Телепортироваться на сохранённую позицию.";

                perferedHand_Tooltip = Main.preferRightHand.Value ?
                    "ПЕРЕКЛЮЧЕНИЕ: Нажав правый триггер, вы телепортируетесь к концу указателя правого контроллера (так же, как лазерный курсор VRChat)." :
                    "ПЕРЕКЛЮЧЕНИЕ: Нажав левый триггер, вы телепортируетесь к концу указателя левого контроллера (так же, как лазерный курсор VRChat).";

                theWord_Teleport = "Телепорт";

                preferedHanded_Text_ON = "Правша";
                preferedHanded_Text_OFF = "Левша";
            }
            if ((loc.Contains("es") && @override.Equals("off")) || @override.Equals("es"))
            {
                TPtoName_Text = "Teletransportarse a\n(Nombre de entrada)";
                TPtoName_Tooltip = "Abre un teclado para ingresar el nombre del jugador al que te gustar\u00EDa teletransportarte";

                TPtoCoord_Text = "Teletransportarse a\n(Coordenadas)";
                TPtoCoord_Tooltip = "Abre un teclado para ingresar las coordenadas \"X Y Z\" en el mundo.";

                SavePos = "Guardar posicion";
                LoadPos = "Posici\u00F3n de carga";

                SavePos_ToolTip = "Guarde su posici\u00F3n actual en el mundo.";
                LoadPos_Tooltip = "Teletransportarse a la posici\u00F3n guardada.";

                perferedHand_Tooltip = Main.preferRightHand.Value ?
                    "TOGGLE: Al presionar el gatillo derecho, se teletransportará al final del puntero del controlador derecho (igual que el cursor láser VRChat)" :
                    "TOGGLE: Al presionar el gatillo izquierdo, se teletransportará al final del puntero del controlador izquierdo (igual que el cursor láser VRChat)";

                theWord_Teleport = "Teletransportarse";

                preferedHanded_Text_ON = "Diestro";
                preferedHanded_Text_OFF = "Zurdo";
            }
            if ((loc.Contains("po") && @override.Equals("off")) || @override.Equals("po"))
            {
                TPtoName_Text = "Teleportar Para\n(Digitar Nome)";
                TPtoName_Tooltip = "Abre um teclado para digitar o nome do jogador que você gostaria de teleportar para";

                TPtoCoord_Text = "Teleportar Para\n(Coordenadas)";
                TPtoCoord_Tooltip = "Abre um teclado para digitar as coordenadas \"X Y Z\" no mundo.";

                SavePos = "Salvar Posição";
                LoadPos = "Carregar Posição";

                SavePos_ToolTip = "Salva sua posição atual no mundo.";
                LoadPos_Tooltip = "Teleporta para a posição salva.";

                perferedHand_Tooltip = Main.preferRightHand.Value ?
                    "Alternar: Pressionando o gatilho direito, você se teleportará para o final do ponteiro de seu controle direito (o mesmo do laser do cursor do VRChat)" :
                    "Alternar: Pressionando o gatilho esquerdo, você se teleportará para o final do ponteiro de seu controle esquerdo (o mesmo do laser do cursor do VRChat)";
                
                theWord_Teleport = "Teleportar";

                preferedHanded_Text_ON = "Destro";
                preferedHanded_Text_OFF = "Canhoto";
            }
            if ((loc.Contains("sw") && @override.Equals("off")) || @override.Equals("sw"))
            {
                TPtoName_Text = "Teleportera Till\n(Inskrivet Namn)";
                TPtoName_Tooltip = "Öppnar upp ett tangentbord där du kan skriva in namnet till personen du vill teleportera till";

                TPtoCoord_Text = "Teleportera Till\n(Koordinater)";
                TPtoCoord_Tooltip = "Öppnar upp ett tangentbord där du kan skriva in \"X Y Z\" koordinaterna du vill teleporta till";

                SavePos = "Spara Position";
                LoadPos = "Ladda Sparad Position";

                SavePos_ToolTip = "Sparar din nuvarande position.";
                LoadPos_Tooltip = "Teleporterar dig till din sparade position.";

                perferedHand_Tooltip = Main.preferRightHand.Value ?
                    "TOGGLE: Genom att trycka på höger avtryckare teleporterar du till slutet av din högra styrpekare (samma som VRChat-lasermarkören)" :
                    "TOGGLE: Genom att trycka på vänster utlösare teleporterar du till slutet av din vänstra kontrollpekare (samma som VRChat-lasermarkören)";

                theWord_Teleport = "Teleportera";

                preferedHanded_Text_ON = "Högerhänt";
                preferedHanded_Text_OFF = "Vänsterhänt";
            }

            if (Main.isDebug)
                MelonLoader.MelonLogger.Msg(ConsoleColor.Green, "Finished with Languages");
        }
    }
}
