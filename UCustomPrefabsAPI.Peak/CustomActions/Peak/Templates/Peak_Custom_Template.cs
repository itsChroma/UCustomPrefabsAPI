using System;
using UCustomPrefabsAPI.RuntimeExtras;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak.CustomActions
{
    public enum PeakTemplateType
    {
        Character,
        Skeleton,
        Chicken
    }
    [RequireComponent(typeof(ShaderFix_Template))]
    public class Peak_Custom_Template : CustomActionsTemplate
    {
        public PeakTemplateType TemplateType
        {
            get
            {
                var type = PeakTemplateType.Character;
                try
                {
                    type = (PeakTemplateType)Enum.Parse(typeof(PeakTemplateType), templateType);
                }
                catch { }
                return type;
            }
        }
        public Texture PassportIcon = null;
        public string PassportDisplayName = null;
        public string templateType;
        public string PreferredSkeletonID = string.Empty;
        public string PreferredChickenID = string.Empty;
        public override Type RegisterCustomActionsBaseType() => typeof(Peak_CustomHelper);
    }
}