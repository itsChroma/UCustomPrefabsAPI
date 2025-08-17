using System.Collections.Generic;
using UCustomPrefabsAPI.Peak.Patches.Listeners;

namespace UCustomPrefabsAPI.Peak
{
    public class Peak_TextureSwapper : Peak_Module
    {
        public override void Init()
        {
            RegisterTextureSwappers();
            CharacterCustomization_Listeners.OnPlayerDataChange_Postfix.Listen(instance.Character, UpdateTextureSwappers);
        }
        public override void Reset()
        {
            textureSwappers.Clear();
        }
        public override void Destroy()
        {
            CharacterCustomization_Listeners.OnPlayerDataChange_Postfix.Un_Listen(instance.Character, UpdateTextureSwappers);
        }
        //Texture Swapper//
        private List<PeakAccessoryTextureSwapper> textureSwappers = new();
        public void RegisterTextureSwappers()
        {
            foreach (var tagged in instance.Handler.GetTagsInTemplates("PeakAccessoryTextureSwapper"))
            {
                if (tagged is not PeakAccessoryTextureSwapper swapper)
                    continue;
                swapper.Reset();
                textureSwappers.Add(swapper);
            }
            UpdateTextureSwappers(instance.CharacterCustomization);
        }
        public void UpdateTextureSwappers(CharacterCustomization customization)
        {
            foreach (var swapper in textureSwappers)
            {
                foreach (var target in swapper.ToggleTargetTypes)
                {
                    if (instance.Is_PeakAccessoryTarget_Active(target))
                        swapper.Set_CurrentTarget(target);
                    else
                        swapper.Set_Texture(null);
                }
            }
        }
    }
}
