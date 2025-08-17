using DG.Tweening;
using System.Collections.Generic;
using UCustomPrefabsAPI.Peak.Patches.Listeners;
using UCustomPrefabsAPI.RuntimeExtras;
using UnityEngine;

namespace UCustomPrefabsAPI.Peak
{
    public class Peak_Chicken : Peak_Module
    {
        public override void Init()
        {
            ToggleChickenInit();
            CharacterCustomization_Listeners.BecomeChicken_Postfix.Listen(instance.Character, BecomeChicken);
            CharacterCustomization_Listeners.BecomeHuman_Postfix.Listen(instance.Character, BecomeHuman);
            //
            /*
            //For More Specific Control <-
            //Need to sync with our Cosmetic module somehow
            CharacterCustomization_Listeners.ShowChicken_Postfix.Listen(instance.Character, ShowChicken);
            CharacterCustomization_Listeners.HideChicken_Postfix.Listen(instance.Character, HideChicken);
            CharacterCustomization_Listeners.ShowHuman_Postfix.Listen(instance.Character, ShowHuman);
            CharacterCustomization_Listeners.HideHuman_Postfix.Listen(instance.Character, HideHuman);
            */
        }
        public override void Reset()
        {
            OpacityLibrary = new();
            KillTweeners();
        }
        public override void Destroy()
        {
            CharacterCustomization_Listeners.BecomeChicken_Postfix.Un_Listen(instance.Character, BecomeChicken);
            CharacterCustomization_Listeners.BecomeHuman_Postfix.Un_Listen(instance.Character, BecomeHuman);
        }
        public bool TryFindMeshHider(out MeshHider hider)
        {
            return instance.Handler.TryGetCustomActions(out hider);
        }
        public void ToggleChickenInit()
        {
            var isCurrentlyChicken = instance.CharacterCustomization.isCannibalizable;
            //TODO Move this behaviour somewhere else//
            //var hasHider = TryFindMeshHider(out var hider);
            if (isCurrentlyChicken)
            {
                SetOpacity(instance.IsChickenTemplate ? 1 : 0);
                //TODO SIMPLIFY THIS//
                /*
                if (hasHider)
                    if (instance.IsChickenTemplate)
                        hider.Init(null, null);
                    else
                        hider.Reset();
                */
            }
            else
            {
                SetOpacity(instance.IsChickenTemplate ? 0 : 1);
                //TODO SIMPLIFY THIS//
                /*
                if (hasHider)
                    if (instance.IsChickenTemplate)
                        hider.Reset();
                    else
                        hider.Init(null, null);
                */
            }
        }
        List<Tweener> _activeTweens = new List<Tweener>();
        private void BecomeChicken(CharacterCustomization customization)
        {
            DoFadeAnimation(customization, instance.IsChickenTemplate ? 1 : 0);
            //TODO SIMPLIFY THIS//
            /*
            var hasHider = TryFindMeshHider(out var hider);
            if (hasHider)
                if (instance.IsChickenTemplate)
                    hider.Init(null, null);
                else
                    hider.Reset();
            */
        }
        private void BecomeHuman(CharacterCustomization customization)
        {
            DoFadeAnimation(customization, instance.IsChickenTemplate ? 0 : 1);
            //TODO SIMPLIFY THIS//
            /*
            var hasHider = TryFindMeshHider(out var hider);
            if (hasHider)
                if (instance.IsChickenTemplate)
                    hider.Reset();
                else
                    hider.Init(null, null);
            */
        }
        private Dictionary<Renderer, List<float>> OpacityLibrary = new();
        private List<float> GetOpacityLibrary(Renderer renderer)
        {
            if (OpacityLibrary.TryGetValue(renderer, out var list))
                return list;
            list = new List<float>();
            OpacityLibrary[renderer] = list;
            var materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].HasFloat(CharacterCustomization.Opacity))
                {
                    list.Add(materials[i].GetFloat(CharacterCustomization.Opacity));
                }
                else
                {
                    list.Add(1);
                }
            }
            return list;
        }
        private void DoFadeAnimation(CharacterCustomization customization, float opacity)
        {
            KillTweeners();
            foreach (var renderer in instance.Template_Renderers)
            {
                var materials = renderer.materials;
                var opacityLib = GetOpacityLibrary(renderer);
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i].HasFloat(CharacterCustomization.Opacity))
                    {
                        AddTweener(materials[i].DOFloat(opacityLib[i] * opacity, CharacterCustomization.Opacity, 1f));
                    }
                }
            }
            /*
            var materials = renderer.materials;
            foreach (var material in materials)
            {
                //Mimic peak vanilla behaviour//
                AddTweener(material.DOFloat(opacity, CharacterCustomization.Opacity, 1f));
            }*/
        }
        private void SetOpacity(float opacity)
        {
            KillTweeners();
            foreach (var renderer in instance.Template_Renderers)
            {
                var materials = renderer.materials;
                var opacityLib = GetOpacityLibrary(renderer);
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i].HasFloat(CharacterCustomization.Opacity))
                    {
                        materials[i].SetFloat(CharacterCustomization.Opacity, opacityLib[i] * opacity);
                    }
                }
            }
        }
        private void AddTweener(Tweener tweener)
        {
            _activeTweens.Add(tweener);
        }
        private void KillTweeners()
        {
            foreach (var t in _activeTweens)
                t?.Kill();
            _activeTweens.Clear();
        }
    }
}
