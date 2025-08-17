using System.Collections.Generic;
using UCustomPrefabsAPI.Peak.Patches.Listeners;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak
{
    public class Peak_SkinToneHelper : Peak_Module
    {
        public override void Init()
        {
            RegisterSkinToneHelpers();
            CharacterCustomization_Listeners.OnPlayerDataChange_Postfix.Listen(instance.Character, UpdateSkinTone);
        }
        public override void Reset()
        {
            Registered_SkinToneHelpers.Clear();
        }
        public override void Destroy()
        {
            CharacterCustomization_Listeners.OnPlayerDataChange_Postfix.Un_Listen(instance.Character, UpdateSkinTone);
        }
        public List<PeakSkinToneHelper> Registered_SkinToneHelpers = new List<PeakSkinToneHelper>();
        public void RegisterSkinToneHelpers()
        {
            var tags = instance.Handler.GetTagsInTemplates("SkinTone");
            foreach (var tagged in tags)
            {
                if (tagged is not PeakSkinToneHelper helper)
                    continue;
                Registered_SkinToneHelpers.Add(helper);
            }
            UpdateSkinTone(instance.CharacterCustomization);
        }
        public void UpdateSkinTone(CharacterCustomization customization)
        {
            Color skinColor = customization.refs.mainRenderer.material.GetColor(CharacterCustomization.SkinColor);
            foreach (var helper in Registered_SkinToneHelpers)
            {
                helper.Set_SkinTone(skinColor);
            }
        }
    }
}
