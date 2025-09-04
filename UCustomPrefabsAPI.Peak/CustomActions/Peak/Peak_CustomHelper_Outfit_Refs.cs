using System.Collections.Generic;
using UnityEngine;

namespace UCustomPrefabsAPI.Peak.CustomActions
{
    public partial class Peak_CustomHelper : CustomActionsBase
    {
        CharacterCustomizationData CharacterCustomizationData
        {
            get
            {
                PersistentPlayerDataService service = GameHandler.GetService<PersistentPlayerDataService>();
                var playerData = service?.GetPlayerData(Character.view.Owner);
                return playerData.customizationData;
            }
        }
        Material FitMaterial
        {
            get
            {
                return Current_Customization(Customization.Type.Fit).fitMaterial;
                //return CharacterCustomization?.refs.mainRenderer.sharedMaterials[1];
            }
        }
        Material FitHatMaterial
        {
            get
            {
                return Current_Customization(Customization.Type.Fit).fitHatMaterial;
                //return CharacterCustomization?.refs.mainRenderer.sharedMaterials[2];
            }
        }
        Material FitPantsMaterial
        {
            get
            {
                return Current_Customization(Customization.Type.Fit).fitPantsMaterial;
                //return CharacterCustomization?.refs.mainRenderer.sharedMaterials[2];
            }
        }
        Material FitShoesMaterial
        {
            get
            {
                return Current_Customization(Customization.Type.Fit).fitMaterialShoes;
                //return CharacterCustomization?.refs.mainRenderer.sharedMaterials[2];
            }
        }
        bool FitHasShorts
        {
            get
            {
                //TODO check if conflicts with Mesa update
                return !Current_Customization(Customization.Type.Fit).isSkirt;
                //return CharacterCustomization?.refs.shorts.gameObject.activeSelf ?? true;
            }
        }
        bool FitHasNoPants
        {
            get
            {
                //TODO check if conflicts with Mesa update
                return Current_Customization(Customization.Type.Fit).noPants;
            }
        }
        /* Will have to rethink this vvv certain fits don't even have shoes, but it can't be directly determined. 
        bool FitHasShoes
        {
            get
            {
                //return CharacterCustomization?.refs.shorts.gameObject.activeSelf ?? true;
            }
        }
        */
        //TODO Rework these functionality to support custom cosmetics + layered cosmetics//
        public CustomizationOption Soft_Accessory_Search(string type, string hint)
        {
            //TODO Add more flexible options for layered cosmetics//
            var data = CharacterCustomizationData;
            var customization = Customization.Instance;
            if (data != null && customization != null)
                switch (type)
                {
                    case "Skins":
                        return Soft_Accessory_Name_Search(customization.skins, hint);
                    case "Eyes":
                        return Soft_Accessory_Name_Search(customization.eyes, hint);
                    case "Outfit":
                        return Soft_Accessory_Name_Search(customization.fits, hint);
                    case "Fit":
                        return Soft_Accessory_Name_Search(customization.fits, hint);
                    case "Hat":
                        return Soft_Accessory_Name_Search(customization.hats, hint);
                    case "Mouth":
                        return Soft_Accessory_Name_Search(customization.mouths, hint);
                    case "Accessory":
                        return Soft_Accessory_Name_Search(customization.accessories, hint);
                    case "Shorts":
                        return Current_Customization(Customization.Type.Fit);
                    case "Skirt":
                        return Current_Customization(Customization.Type.Fit);
                }
            return null;
        }
        private CustomizationOption Soft_Accessory_Name_Search(IEnumerable<CustomizationOption> options, string hint)
        {
            foreach (CustomizationOption option in options)
                if (string.Compare(option.name, hint, true) == 0)
                    return option;
            return null;
        }
        public bool Is_PeakAccessoryTarget_Active(PeakAccessoryTarget accessoryTarget)
        {
            CustomizationOption currentOption = null;
            switch (accessoryTarget.type)
            {
                case PeakAccessoryType.Accessory:
                    currentOption = Current_Customization(Customization.Type.Accessory);
                    break;
                case PeakAccessoryType.Eyes:
                    currentOption = Current_Customization(Customization.Type.Eyes);
                    break;
                case PeakAccessoryType.Outfit:
                    currentOption = Current_Customization(Customization.Type.Fit);
                    break;
                case PeakAccessoryType.Hat:
                    currentOption = Current_Customization(Customization.Type.Hat);
                    break;
                case PeakAccessoryType.Mouth:
                    currentOption = Current_Customization(Customization.Type.Mouth);
                    break;
                case PeakAccessoryType.Shorts:
                    currentOption = Current_Customization(Customization.Type.Fit);
                    return !(currentOption?.isSkirt ?? false);
                case PeakAccessoryType.Skirt:
                    currentOption = Current_Customization(Customization.Type.Fit);
                    return (currentOption?.isSkirt ?? false);
                    //TODO actually implement these, thanks!
                case PeakAccessoryType.NoPants:
                    currentOption = Current_Customization(Customization.Type.Fit);
                    return (currentOption?.noPants ?? true);
                case PeakAccessoryType.ScoutRank:
                    return accessoryTarget.hint == CharacterCustomizationData.currentSash.ToString();
                default:
                    break;
            }
#if DEBUG
            //Debug.Log($"Check = {accessoryTarget.hint} =?= {currentOption?.GetName()}");
#endif
            var valid = string.Equals(accessoryTarget.hint, currentOption?.GetName(), System.StringComparison.OrdinalIgnoreCase);
            return valid;
        }
        public Material Fetch_PeakAccessoryType_Material(PeakAccessoryType type)
        {
            CustomizationOption currentOption = null;
            switch (type)
            {
                case PeakAccessoryType.Accessory:
                    return CharacterCustomization?.refs.accessoryRenderer.material;
                case PeakAccessoryType.Eyes:
                    return CharacterCustomization?.refs.EyeRenderers[0].material;
                case PeakAccessoryType.Outfit:
                    currentOption = Current_Customization(Customization.Type.Fit);
                    return currentOption.fitMaterial;
                case PeakAccessoryType.Hat:
                    currentOption = Current_Customization(Customization.Type.Hat);
                    return currentOption.fitHatMaterial;
                case PeakAccessoryType.Mouth:
                    return CharacterCustomization?.refs.mouthRenderer.material;
                case PeakAccessoryType.Shorts:
                    currentOption = Current_Customization(Customization.Type.Fit);
                    return currentOption.fitMaterial;
                case PeakAccessoryType.Skirt:
                    currentOption = Current_Customization(Customization.Type.Fit);
                    return currentOption.fitMaterial;
            }
            return null;
        }
        public Texture Fetch_PeakAccessoryType_Texture(PeakAccessoryType type)
        {
            var material = Fetch_PeakAccessoryType_Material(type);
            return material?.mainTexture;
        }
        public List<Renderer> Fetch_PeakAccessoryType_Renderers(PeakAccessoryType type)
        {
            List<Renderer> renderers = new();
            if (!CharacterCustomization)
                return renderers;
            CustomizationRefs refs;
            if (!CharacterDummy)
                refs = CharacterCustomization.refs;
            else
                refs = CharacterDummy.refs;
            switch (type)
            {
                case PeakAccessoryType.Accessory:
                    renderers.Add(refs.accessoryRenderer);
                    break;
                case PeakAccessoryType.Eyes:
                    renderers.AddRange(refs.EyeRenderers);
                    break;
                case PeakAccessoryType.Outfit:
                    renderers.Add(refs.mainRenderer);
                    break;
                case PeakAccessoryType.Hat:
                    renderers.AddRange(refs.playerHats);
                    break;
                case PeakAccessoryType.Mouth:
                    renderers.Add(refs.mouthRenderer);
                    break;
                case PeakAccessoryType.Shorts:
                    renderers.Add(refs.shorts);
                    break;
                case PeakAccessoryType.Skirt:
                    renderers.Add(refs.skirt);
                    break;
            }
            return renderers;
        }
        public void Toggle_PeakAccessoryType(PeakAccessoryType type, bool Hide = true)
        {
            List<Renderer> renderers = Fetch_PeakAccessoryType_Renderers(type);
            if (Hide)
                foreach (Renderer renderer in renderers)
                {
                    Hide_Customization_Renderer(renderer);
                }
            else
                foreach (Renderer renderer in renderers)
                {
                    Show_Customization_Renderer(renderer);
                }
        }
        public CustomizationOption Current_Customization(Customization.Type type)
        {
            //TODO Add more flexible options for layered cosmetics//
            var data = CharacterCustomizationData;
            var customization = Customization.Instance;
            if (data != null && customization != null)
                switch (type)
                {
                    case Customization.Type.Skin:
                        return customization.skins[data.currentSkin];
                    case Customization.Type.Accessory:
                        return customization.accessories[data.currentAccessory];
                    case Customization.Type.Eyes:
                        return customization.eyes[data.currentEyes];
                    case Customization.Type.Mouth:
                        return customization.mouths[data.currentMouth];
                    case Customization.Type.Fit:
                        return customization.fits[data.currentOutfit];
                    case Customization.Type.Hat:
                        return customization.hats[data.currentHat];
                }
            return null;
        }
        private static RenderingLayerMask Hidden_Mask = 0;
        Dictionary<Renderer, RenderingLayerMask> _customization_renderers = new();
        public void Toggle_Customization(Customization.Type type, bool Hide = true)
        {
            var data = CharacterCustomizationData;
            var customization = Customization.Instance;
            if (data == null && customization == null)
                return;
            CustomizationRefs refs;
            if (!CharacterDummy)
                refs = CharacterCustomization.refs;
            else
                refs = CharacterDummy.refs;
            List<Renderer> workingRenderers = new List<Renderer>();
            switch (type)
            {
                case Customization.Type.Skin:
                    workingRenderers.AddRange(refs.PlayerRenderers);
                    break;
                case Customization.Type.Accessory:
                    workingRenderers.Add(refs.accessoryRenderer);
                    break;
                case Customization.Type.Eyes:
                    workingRenderers.AddRange(refs.EyeRenderers);
                    break;
                case Customization.Type.Mouth:
                    workingRenderers.Add(refs.mouthRenderer);
                    break;
                case Customization.Type.Fit:
                    workingRenderers.Add(refs.mouthRenderer);
                    break;
                case Customization.Type.Hat:
                    workingRenderers.AddRange(refs.playerHats);
                    break;
            }
            if (Hide)
                foreach (Renderer renderer in workingRenderers)
                {
                    Hide_Customization_Renderer(renderer);
                }
            else
                foreach (Renderer renderer in workingRenderers)
                {
                    Show_Customization_Renderer(renderer);
                }
        }
        //TODO fix this : Possibly move to own module...? or variable on CustomHelper instance
        public void Hide_Customization_Renderer(Renderer renderer)
        {
            if (_customization_renderers.ContainsKey(renderer))
                return;
            _customization_renderers.Add(renderer, renderer.renderingLayerMask);
            renderer.renderingLayerMask = Hidden_Mask;
        }
        public void Show_Customization_Renderer(Renderer renderer)
        {
            if (!_customization_renderers.TryGetValue(renderer, out var mask))
                return;
            renderer.renderingLayerMask = mask;
            _customization_renderers.Remove(renderer);
        }
        public void Reset_Customization_Renderers()
        {
            foreach (var pair in _customization_renderers)
                pair.Key.renderingLayerMask = pair.Value;
            _customization_renderers.Clear();
        }
    }
}