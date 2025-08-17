using HarmonyLib;
using UCustomPrefabsAPI.Peak.Utils;
using UCustomPrefabsAPI.PhotonUtils.Networking;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak
{
    [HarmonyPatch]
    internal static class PassportPatch
    {
        [HarmonyPatch(typeof(PassportManager), "SetOption")]
        [HarmonyPrefix]
        static bool SetOption_Prefix_Patch(ref PassportManager __instance, CustomizationOption option)
        {
            //TODO add try catch methods for non-performance issue patches, to prevent mod issues.
            Debug.Log("SetOption_Prefix_Patch");
            if (!PassportHelper.IsSpecialCustomization(option.type, out var _))
                return true;
            Debug.Log("Is Custom Typed SetOption");
            if (option is not PassportHelper.CustomizationOption_Custom customOption)
                return true;
            var templateID = customOption.TemplateID;
            Debug.Log($"ID Selected = {templateID}");
            switch (customOption.TemplateType)
            {
                case CustomActions.PeakTemplateType.Character:
                    {
                        PlayerConfigHelper.SetCharacterTemplate(templateID);
                    }
                    break;
                case CustomActions.PeakTemplateType.Skeleton:
                    {
                        PlayerConfigHelper.SetSkeletonTemplate(templateID);
                    }
                    break;
                case CustomActions.PeakTemplateType.Chicken:
                    {
                        PlayerConfigHelper.SetChickenTemplate(templateID);
                    }
                    break;
            }
            //Vanilla Behaviour//
            __instance.SetActiveButton();
            __instance.dummy.UpdateDummy();
            return false;
        }
        //SetActiveButton()
        [HarmonyPatch(typeof(PassportManager), "SetActiveButton")]
        [HarmonyPrefix]
        static bool SetActiveButton_Prefix_Patch(ref PassportManager __instance)
        {
            if (!PassportHelper.IsSpecialCustomization(__instance.activeType, out var templateType))
                return true;
            if (!PlayerConfigHelper.TryGetConfigHandler(Character.localCharacter, out var config))
                return true;
            var list = PassportHelper.Fetch_Template_List(templateType);;
            //TODO ADD PREFERENCE BUTTONS//
            string activeID;
            switch (templateType)
            {
                case CustomActions.PeakTemplateType.Character:
                    activeID = config.CurrentCharacterTemplate;
                    break;
                case CustomActions.PeakTemplateType.Skeleton:
                    activeID = config.CurrentSkeletonTemplate;
                    break;
                case CustomActions.PeakTemplateType.Chicken:
                    activeID = config.CurrentChickenTemplate;
                    break;
                default:
                    activeID = string.Empty;
                    break;
            }
            PassportHelper.CustomizationOption_Custom activeOption = null;
            //TODO add NO-PREFERENCE to a option
            foreach (var button in __instance.buttons)
            {
                if (!button || !button.currentOption)
                    continue;
                if (button.currentOption is not PassportHelper.CustomizationOption_Custom option)
                    continue;
                //if (activeID == button.currentOption.GetName())
                if (activeID == option.TemplateID)
                {
                    activeOption = option;
                    button.border.color = __instance.activeBorderColor;
                }
                else
                    button.border.color = __instance.inactiveBorderColor;
            }
            PassportHelper.Custom_Name_Display.text = activeOption?.DisplayName;
            return false;
        }
        //OpenTab
        [HarmonyPatch(typeof(PassportManager), "OpenTab")]
        [HarmonyPrefix]
        static bool OpenTab_Prefix_Patch(ref PassportManager __instance, Customization.Type type)
        {
            PassportHelper.SwitchToCustomTab(type);
            __instance.activeType = type;
            if (!PassportHelper.IsSpecialCustomization(__instance.activeType, out var templateType))
                return true;
            //Vanilla Behaviour
            __instance.CameraOut();
            __instance.SetButtons();
            __instance.dummy.UpdateDummy();
            return false;
        }
    }
}
