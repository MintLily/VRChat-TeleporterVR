using System;
using System.Linq;
using System.Reflection;
using Il2CppSystem.Collections.Generic;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.UI;

// Came from https://github.com/knah/VRCMods/blob/master/UIExpansionKit/ScanningReflectionCache.cs
//     &     https://github.com/knah/VRCMods/blob/master/UIExpansionKit/API/BuiltinUiUtils.cs

namespace TeleporterVR.Utils
{
    public static class PopupManager
    {
        public static void HideCurrentPopup(this VRCUiPopupManager vrcUiPopupManager) => VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP");

        public static void ShowInputPopup(string title, string initialText, InputField.InputType inputType, bool isNumeric,
            string confirmButtonText, Action<string, List<KeyCode>, Text> onComplete, Action onCancel = null,
            string placeholderText = "Enter text...", bool closeAfterInput = true, Action<VRCUiPopup> onPopupShown = null,
            bool showLimitLabel = false, int textLengthLimit = 0)
        {
            ShowUiInputPopup(title, initialText, inputType, isNumeric, confirmButtonText, onComplete, onCancel, placeholderText, closeAfterInput, onPopupShown, showLimitLabel, textLengthLimit);
        }

        internal delegate void ShowUiInputPopupAction(string title, string initialText, InputField.InputType inputType,
            bool isNumeric, string confirmButtonText, Il2CppSystem.Action<string, List<KeyCode>, Text> onComplete,
            Il2CppSystem.Action onCancel, string placeholderText = "Enter text...", bool closeAfterInput = true,
            Il2CppSystem.Action<VRCUiPopup> onPopupShown = null, bool bUnknown = false, int charLimit = 0);

        private static ShowUiInputPopupAction ourShowUiInputPopupAction;

        internal static ShowUiInputPopupAction ShowUiInputPopup
        {
            get
            {
                if (ourShowUiInputPopupAction != null) return ourShowUiInputPopupAction;

                var candidates = typeof(VRCUiPopupManager)
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(it =>
                        it.Name.StartsWith("Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_Boolean_Int32_")
                        && !it.Name.EndsWith("_PDM"))
                    .ToList();

                var targetMethod = candidates.SingleOrDefault(it => XrefScanner.XrefScan(it).Any(jt =>
                    jt.Type == XrefType.Global &&
                    jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/InputPopup"));
                
                if (targetMethod == null) 
                    targetMethod = typeof(VRCUiPopupManager).GetMethod(nameof(VRCUiPopupManager.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_Boolean_Int32_0),
                    BindingFlags.Instance | BindingFlags.Public);

                ourShowUiInputPopupAction = (ShowUiInputPopupAction) Delegate.CreateDelegate(typeof(ShowUiInputPopupAction), VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0, targetMethod);

                return ourShowUiInputPopupAction;
            }
        }
    }
}
