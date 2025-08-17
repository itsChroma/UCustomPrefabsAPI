using System.Collections.Generic;
using UCustomPrefabsAPI.Peak.Patches.Listeners;
using UCustomPrefabsAPI.RuntimeExtras;

namespace UCustomPrefabsAPI.Peak
{
    public class Peak_TextureHelper : Peak_Module
    {
        public override void Init()
        {
            RegisterTextureHelpers();
            CharacterCustomization_Listeners.OnPlayerDataChange_Postfix.Listen(instance.Character, UpdateTextureHelpers);
        }
        public override void Reset()
        {
            textureHelpers.Clear();
        }
        public override void Destroy()
        {
            CharacterCustomization_Listeners.OnPlayerDataChange_Postfix.Un_Listen(instance.Character, UpdateTextureHelpers);
        }
        //Texture Helper//
        private List<PeakAccessoryTextureHelper> textureHelpers = new();
        public void RegisterTextureHelpers()
        {
            foreach (var tagged in instance.Handler.GetTagsInTemplates("PeakAccessoryTextureHelper"))
            {
                if (tagged is not PeakAccessoryTextureHelper helper)
                    continue;
                helper.Reset();
                textureHelpers.Add(helper);
            }
            UpdateTextureHelpers(instance.CharacterCustomization);
        }
        public void UpdateTextureHelpers(CharacterCustomization customization)
        {
            foreach (var helper in textureHelpers)
            {
                helper.Set_Texture(instance.Fetch_PeakAccessoryType_Texture(PeakAccessoryType.Outfit));
            }
        }
    }
}
