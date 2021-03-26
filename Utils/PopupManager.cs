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
        public static void HideCurrentPopup(this VRCUiPopupManager vrcUiPopupManager)
        {
            VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP");
        }

        public static void ShowInputPopup(string title, string initialText, InputField.InputType inputType, bool isNumeric,
            string confirmButtonText, Action<string, List<KeyCode>, Text> onComplete, Action onCancel = null,
            string placeholderText = "Enter text...", bool closeAfterInput = true, Action<VRCUiPopup> onPopupShown = null)
        {
            ShowUiInputPopup(title, initialText, inputType, isNumeric, confirmButtonText, onComplete, onCancel, placeholderText, closeAfterInput, onPopupShown);
        }

        internal delegate void ShowUiInputPopupAction(string title, string initialText, InputField.InputType inputType,
            bool isNumeric, string confirmButtonText, Il2CppSystem.Action<string, List<KeyCode>, Text> onComplete,
            Il2CppSystem.Action onCancel, string placeholderText = "Enter text...", bool closeAfterInput = true,
            Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        private static ShowUiInputPopupAction ourShowUiInputPopupAction;

        internal static ShowUiInputPopupAction ShowUiInputPopup
        {
            get
            {
                if (ourShowUiInputPopupAction != null) return ourShowUiInputPopupAction;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods((BindingFlags.Instance | BindingFlags.Public))
                    .Where(it => it.GetParameters().Length == 10 &&
                                  it.Name.StartsWith("Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup") &&
                                  XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/InputPopup")).OrderBy(it => -it.GetCustomAttribute<CallerCountAttribute>().Count).First();

                ourShowUiInputPopupAction = (ShowUiInputPopupAction)Delegate.CreateDelegate(typeof(ShowUiInputPopupAction), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourShowUiInputPopupAction;
            }
        }
    }
}
