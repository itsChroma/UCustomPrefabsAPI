using System.Collections.Generic;
using UCustomPrefabsAPI.Peak.Patches.Listeners;
namespace UCustomPrefabsAPI.Peak
{
    public class Peak_ObjectToggler : Peak_Module
    {
        public override void Init()
        {
            RegisterObjectTogglers();
            CharacterCustomization_Listeners.OnPlayerDataChange_Postfix.Listen(instance.Character, UpdateObjectTogglers);
        }
        public override void Reset()
        {
            instance.Reset_Customization_Renderers();
            togglers.Clear();
        }
        public override void Destroy()
        {
            CharacterCustomization_Listeners.OnPlayerDataChange_Postfix.Un_Listen(instance.Character, UpdateObjectTogglers);
        }
        //Object Toggler
        private List<PeakAccessoryObjectToggler> togglers = new();
        public void RegisterObjectTogglers()
        {
            togglers = new();
            foreach (var tagged in instance.Handler.GetTagsInTemplates("PeakAccessoryObjectToggler"))
            {
                if (tagged is not PeakAccessoryObjectToggler toggler)
                    continue;
                toggler.Rebuild();
                togglers.Add(toggler);
            }
            UpdateObjectTogglers();
        }
        public void UpdateObjectTogglers(CharacterCustomization _ = null)
        {
            HashSet<PeakAccessoryType> HiddenTypes = new HashSet<PeakAccessoryType>();
            foreach (var toggler in togglers)
            {
                toggler.gameObject.SetActive(toggler.HideOnTarget);
                foreach (var target in toggler.ToggleTargetTypes)
                {
                    if (!instance.Is_PeakAccessoryTarget_Active(target))
                        continue;
                    toggler.gameObject.SetActive(!toggler.HideOnTarget);
                    if (toggler.HideTargetAccessory)
                        HiddenTypes.Add(target.type);
                }
            }
            //Move this over to our own script//
            instance.Reset_Customization_Renderers();
            foreach (var type in HiddenTypes)
            {
                instance.Toggle_PeakAccessoryType(type);
            }
        }
    }
}
