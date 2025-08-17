using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak.CustomActions
{
    public partial class Peak_CustomHelper : CustomActionsBase
    {
        Character _character;
        public Character Character
        {
            get
            {
                if (!_character)
                {
                    _character = Handler.GetComponent<Character>();
                    if (!_character && CharacterDummy)
                    {
                        _character = Character.localCharacter;
                    }

                }
                return _character;
            }
        }
        CharacterCustomization _characterCustomization;
        public CharacterCustomization CharacterCustomization
        {
            get
            {
                if (!_characterCustomization)
                {
                    _characterCustomization = Character?.GetComponent<CharacterCustomization>();
                    //_characterCustomization = Handler.GetComponent<CharacterCustomization>();
                }
                return _characterCustomization;
            }
        }
        PlayerCustomizationDummy CharacterDummy
        {
            get
            {
                return Handler.GetComponent<PlayerCustomizationDummy>();
            }
        }
        public PhotonView PhotonView => Character?.player.view;
        //Probably Optimize this list of renderers vvv to only update during cosmetic/state change
        //TODO this May not be required, This was from the old player model helper
        //Individual modules can handle this <-
        public List<Renderer> Template_Renderers
        {
            get
            {
                var renderers = new List<Renderer>();
                foreach (var target in Templates)
                    renderers.AddRange(target.GetComponentsInChildren<Renderer>(true));
                return renderers;
            }
        }
        public IEnumerable<PrefabTemplate> Templates
        {
            get
            {
                return Handler.LoadedTemplates.Values;
            }
        }
        //TODO probably do this a little better...!
        public PeakTemplateType TemplateType
        {
            get
            {
                if (TemplateRegistry.TryGetTemplate(Handler.Instance.TemplateUID, out var template))
                    foreach (var customAction in template.CustomActionsTemplates)
                    {
                        if (customAction is not Peak_Custom_Template helper)
                            continue;
                        return helper.TemplateType;
                    }
                return PeakTemplateType.Character;
            }
        }
        public bool IsChickenTemplate
        {
            get
            {
                return TemplateType == PeakTemplateType.Chicken;
            }
        }
    }
}