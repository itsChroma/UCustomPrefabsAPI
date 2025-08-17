using System.Collections.Generic;
using System.Linq;
using UCustomPrefabsAPI.Peak.CustomActions;
using UCustomPrefabsAPI.PhotonUtils.Networking;
using UnityEngine;

namespace UCustomPrefabsAPI.Peak.Utils
{
    static class CustomTemplateUtils
    {
        public static List<string> Fetch_Peak_Custom_Template_IDs()
        => TemplateRegistry.GetTemplatesWithCustomActions<Peak_CustomHelper>();
        public static Dictionary<string, Peak_Custom_Template> Fetch_Peak_Custom_Templates(PeakTemplateType type)
        {
            Dictionary<string, Peak_Custom_Template> characterTemplates = new();
            foreach (var templateID in Fetch_Peak_Custom_Template_IDs())
            {
                if (!TemplateRegistry.TryGetTemplate(templateID, out var template))
                    continue;
                var customActionTemplate = template.CustomActionsTemplates.FirstOrDefault(cat => cat is Peak_Custom_Template);
                if (!customActionTemplate)
                    continue;
                var customTemplate = (Peak_Custom_Template)customActionTemplate;
                if (customTemplate.TemplateType == type)
                    characterTemplates[templateID] = customTemplate;
            }
            return characterTemplates;
        }
        //
        public static Texture Get_Peak_Custom_Template_Icon(string templateID)
        {
            Texture icon = null;
            if (TemplateRegistry.TryGetTemplate(templateID, out var template))
            {
                var helper = (Peak_Custom_Template)template.CustomActionsTemplates.FirstOrDefault(a => a is Peak_Custom_Template);
                if (helper != null)
                {
                    icon = helper.PassportIcon;
                }
            }
            return icon;
        }
        //TODO Simplify This later vvv
        public static string Get_Peak_Custom_Template_DisplayName(string templateID)
        {
            string displayName = null;
            if (TemplateRegistry.TryGetTemplate(templateID, out var template))
            {
                var helper = (Peak_Custom_Template)template.CustomActionsTemplates.FirstOrDefault(a => a is Peak_Custom_Template);
                if (helper != null)
                {
                    displayName = helper.PassportDisplayName;
                }
            }
            return displayName;
        }
        //
        public const string Default_Template = "Default";
        public const string NoPreference_Template = "NoPreference";
        public static readonly List<string> Special_TemplateNames = [Default_Template, NoPreference_Template];
        //
        public static bool IsSpecialTemplate(string templateID)
        {
            return Special_TemplateNames.Contains(templateID);
        }
        public static string Evaluate_Peak_Custom_Template(PlayerConfigHelper config, PeakTemplateType type, string templateID)
        {
            //If we're using the default template, we can just ignore this ->
            if (templateID == Default_Template)
                return Default_Template;
            //Prepare for work!
            var templates = Fetch_Peak_Custom_Templates(type);
            //Ensure our template ID actually exists!
            if (!IsSpecialTemplate(templateID))
                if (string.IsNullOrWhiteSpace(templateID) || !templates.TryGetValue(templateID, out var template))
                    templateID = NoPreference_Template;
            //If no templates are used, check our Character template!
            if (templateID == NoPreference_Template)
                templateID = Fetch_Preferred_Peak_Custom_Template(templates, config, type, templateID);
            return templateID;
        }
        private static string Fetch_Preferred_Peak_Custom_Template(Dictionary<string, Peak_Custom_Template> templates, PlayerConfigHelper config, PeakTemplateType type, string templateID)
        {
            //Character doesn't rely on other template types, let it go immediately
            if (type == PeakTemplateType.Character)
                return Default_Template;
            var characterTemplates = Fetch_Peak_Custom_Templates(PeakTemplateType.Character);
            //Verify Character Template
            if (characterTemplates.TryGetValue(config.CurrentCharacterTemplate, out var characterTemplate))
                switch (type)
                {
                    //TODO Simplify this ->//
                    case PeakTemplateType.Skeleton:
                        if (characterTemplate.PreferredSkeletonID == Default_Template)
                            templateID = Default_Template;
                        else
                        if (templates.TryGetValue(characterTemplate.PreferredSkeletonID, out var _))
                            templateID = characterTemplate.PreferredSkeletonID;
                        else
                            templateID = Default_Template;
                        break;
                    case PeakTemplateType.Chicken:
                        if (characterTemplate.PreferredChickenID == Default_Template)
                            templateID = Default_Template;
                        else
                        if (templates.TryGetValue(characterTemplate.PreferredChickenID, out var _))
                            templateID = characterTemplate.PreferredChickenID;
                        else
                            templateID = Default_Template;
                        break;
                }
            return templateID;
        }
    }
}
