using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UCustomPrefabsAPI.Extras.AssetBundles;
using UCustomPrefabsAPI.Peak.CustomActions;
using UnityEngine;
using UnityEngine.UI;

namespace UCustomPrefabsAPI.Peak.Utils
{
    static class PassportHelper
    {
        public const int Character_Enum = -13377701;
        public const int Skeleton_Enum = -13377702;
        public const int Chicken_Enum = -13377703;
        public static Texture NoPreferenceIcon => AssetBundleRegistry.LoadAsset<Texture>("CustomPassportIcons", "NoPreferenceIcon");
        public static Texture CustomCharacterIcon => AssetBundleRegistry.LoadAsset<Texture>("CustomPassportIcons", "CustomCharacterPassportIcon");
        public static Texture CustomSkeletonIcon => AssetBundleRegistry.LoadAsset<Texture>("CustomPassportIcons", "CustomSkeletonPassportIcon");
        public static Texture CustomChickenIcon => AssetBundleRegistry.LoadAsset<Texture>("CustomPassportIcons", "CustomChickenPassportIcon");
        public static GameObject CustomBookmarkPrefab => AssetBundleRegistry.LoadPrefab("CustomPassportIcons", "BookmarkButton");
        public static Texture EmptyIcon => AssetBundleRegistry.LoadAsset<Texture>("CustomPassportIcons", "Empty");
        public static Texture LockedIcon
        {
            get => PassportManager.instance.buttons[0].GetComponent<PassportButton>().lockedIcon.texture;
        }
        public static bool IsSpecialCustomization(Customization.Type type, out PeakTemplateType templateType)
        {
            templateType = default;
            switch ((int)type)
            {
                case Character_Enum:
                    templateType = PeakTemplateType.Character;
                    return true;
                case Skeleton_Enum:
                    templateType = PeakTemplateType.Skeleton;
                    return true;
                case Chicken_Enum:
                    templateType = PeakTemplateType.Chicken;
                    return true;
            }
            return false;
        }
        public static Customization.Type GetSpecialCustomization(PeakTemplateType type)
        {
            switch (type)
            {
                case PeakTemplateType.Character:
                    return (Customization.Type)Character_Enum;
                case PeakTemplateType.Skeleton:
                    return (Customization.Type)Skeleton_Enum;
                case PeakTemplateType.Chicken:
                    return (Customization.Type)Chicken_Enum;
            }
            return default;
        }
        public class CustomizationOption_Custom : CustomizationOption
        {
            public string TemplateID = string.Empty;
            public string DisplayName = string.Empty;
            public PeakTemplateType TemplateType;
        }
        public static CustomizationOption[] Construct_Customization_List(PeakTemplateType type)
        {
            List<CustomizationOption> list = new List<CustomizationOption>();
            var customType = GetSpecialCustomization(type);
            //
            var noPrefOption = ScriptableObject.CreateInstance<CustomizationOption_Custom>();
            noPrefOption.SetName(CustomTemplateUtils.NoPreference_Template);
            noPrefOption.color = Color.white;
            noPrefOption.texture = NoPreferenceIcon;
            noPrefOption.type = customType;
            noPrefOption.TemplateID = CustomTemplateUtils.NoPreference_Template;
            noPrefOption.DisplayName = "No Preference";
            noPrefOption.TemplateType = type;
            list.Add(noPrefOption);
            //
            var defaultOption = ScriptableObject.CreateInstance<CustomizationOption_Custom>();
            defaultOption.SetName(CustomTemplateUtils.Default_Template);
            defaultOption.color = Color.white;
            defaultOption.texture = EmptyIcon;
            defaultOption.type = customType;
            defaultOption.TemplateID = CustomTemplateUtils.Default_Template;
            defaultOption.DisplayName = "Default";
            defaultOption.TemplateType = type;
            list.Add(defaultOption);
            //
            foreach (var id in Fetch_Template_List(type))
            {
                var customizationOption = ScriptableObject.CreateInstance<CustomizationOption_Custom>();
                customizationOption.SetName(id);
                customizationOption.color = Color.white;
                customizationOption.texture = GetTemplateIcon(id);
                customizationOption.type = customType;
                customizationOption.DisplayName = GetTemplateDisplayName(id);
                customizationOption.TemplateID = id;
                customizationOption.TemplateType = type;
                list.Add(customizationOption);
            }
            return list.ToArray();
        }
        public static Texture GetTemplateIcon(string templateID)
        {
            Texture icon = CustomTemplateUtils.Get_Peak_Custom_Template_Icon(templateID);
            if (icon == null)
                icon = LockedIcon;
            return icon;
        }
        public static string GetTemplateDisplayName(string templateID)
        {
            string displayName = CustomTemplateUtils.Get_Peak_Custom_Template_DisplayName(templateID);
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = templateID;
            return displayName;
        }
        public static List<string> Fetch_Template_List(PeakTemplateType type)
        {
            return CustomTemplateUtils.Fetch_Peak_Custom_Templates(type).Keys.ToList();
        }
        public static Transform OriginalTabContainer { get => PassportManager.instance.tabs[0].transform.parent; }
        public static Transform OriginalButtonContainer { get => PassportManager.instance.buttons[0].transform.parent; }
        public static Vector3 Original_ButtonContainer_Position;
        public static readonly Vector3 ButtonContainer_Name_Offset = new Vector3(0, -20, 0);
        public static List<PassportTab> CustomTabs = new List<PassportTab>();
        public static GameObject CustomContainer;
        public static Button BookmarkButton;
        public static TextMeshProUGUI Custom_Name_Display;
        public static void VerifyAndInitializeCustomTabs()
        {
            Debug.Log("VerifyAndInitializeCustomTabs");
            if (CustomTabs.Count == 0 || CustomTabs.Any(t => !t))
            {
                CustomTabs.Clear();
                //Clone our existing tabs//
                CustomContainer = GameObject.Instantiate(OriginalTabContainer.gameObject, OriginalTabContainer.parent);
                foreach (PassportTab tab in CustomContainer.GetComponentsInChildren<PassportTab>(true))
                {
                    //We just wanna keep the first three//
                    switch (tab.type)
                    {
                        case Customization.Type.Skin:
                            {
                                tab.type = (Customization.Type)Character_Enum;
                                var image = tab.transform.Find("Panel/Icon")?.GetComponent<RawImage>();
                                if (image)
                                    image.texture = CustomCharacterIcon;
                                CustomTabs.Add(tab);
                            }
                            break;
                        case Customization.Type.Accessory:
                            {
                                tab.type = (Customization.Type)Skeleton_Enum;
                                var image = tab.transform.Find("Panel/Icon")?.GetComponent<RawImage>();
                                if (image)
                                    image.texture = CustomSkeletonIcon;
                                CustomTabs.Add(tab);
                            }
                            break;
                        case Customization.Type.Eyes:
                            {
                                tab.type = (Customization.Type)Chicken_Enum;
                                var image = tab.transform.Find("Panel/Icon")?.GetComponent<RawImage>();
                                if (image)
                                    image.texture = CustomChickenIcon;
                                CustomTabs.Add(tab);
                            }
                            break;
                        default:
                            GameObject.DestroyImmediate(tab.gameObject);
                            break;
                    }
                }
                PassportManager.instance.tabs = PassportManager.instance.tabs.AddRangeToArray(CustomTabs.ToArray());

                //BOOKMARK
                try
                {
                    var Bookmark = GameObject.Instantiate(CustomBookmarkPrefab, OriginalTabContainer.parent.parent.parent);
                    Bookmark.transform.localPosition = new Vector3(300, 307, 0);
                    BookmarkButton = Bookmark.GetComponent<Button>();
                    BookmarkButton?.onClick.AddListener(TogglePassportBookmark);
                }
                catch
                {
                    Debug.LogError("Failed to Create Custom Bookmark");
                }
                //Name Display
                try { 
                var NameDisplay = GameObject.Instantiate(PassportManager.instance.nameText.gameObject, OriginalButtonContainer.parent);
                NameDisplay.name = "CustomNameDisplay";
                NameDisplay.transform.localPosition = new Vector3(-193, 95, 0);
                NameDisplay.transform.SetSiblingIndex(0);
                Custom_Name_Display = NameDisplay.GetComponent<TextMeshProUGUI>();
                Custom_Name_Display.color = Color.black;
                Custom_Name_Display.alignment = TextAlignmentOptions.Left;
                Custom_Name_Display.fontStyle = FontStyles.Normal;
                Custom_Name_Display.text = "";
                }
                catch
                {
                    Debug.LogError("Failed to Create Custom Display Text");
                }
                Original_ButtonContainer_Position = OriginalButtonContainer.transform.localPosition;
            }
        }
        public static void TogglePassportBookmark()
        {
            var BlueBookmark = BookmarkButton.transform.Find("RawImage_Blue").GetComponent<RawImage>();
            var RedBookmark = BookmarkButton.transform.Find("RawImage_Red").GetComponent<RawImage>();
            if (IsSpecialCustomization(PassportManager.instance.activeType, out _))
            {
                BlueBookmark.gameObject.SetActive(false);
                RedBookmark.gameObject.SetActive(true);
                BookmarkButton.targetGraphic = RedBookmark;
                PassportManager.instance.OpenTab(Customization.Type.Skin);
            }
            else
            {
                BlueBookmark.gameObject.SetActive(true);
                RedBookmark.gameObject.SetActive(false);
                BookmarkButton.targetGraphic = BlueBookmark;
                PassportManager.instance.OpenTab((Customization.Type)Character_Enum);
            }
        }
        public static void SwitchToCustomTab(Customization.Type type)
        {
            VerifyAndInitializeCustomTabs();
            if (!IsSpecialCustomization(type, out var _))
            {
                CustomContainer.SetActive(false);
                Custom_Name_Display.gameObject.SetActive(false);
                OriginalTabContainer.gameObject.SetActive(true);
                OriginalButtonContainer.transform.localPosition = Original_ButtonContainer_Position;
                return;
            }
            CustomContainer.SetActive(true);
            Custom_Name_Display.gameObject.SetActive(true);
            OriginalTabContainer.gameObject.SetActive(false);
            OriginalButtonContainer.transform.localPosition = ButtonContainer_Name_Offset;
            foreach (var tab in CustomTabs)
            {
                if (tab.type == type)
                {
                    tab.Open();
                }
                else
                {
                    tab.Close();
                }
            }
        }
    }
}
