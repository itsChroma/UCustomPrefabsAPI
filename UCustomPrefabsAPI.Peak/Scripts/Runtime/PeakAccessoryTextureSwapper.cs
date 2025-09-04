using System.Collections.Generic;
using UnityEngine;
using UCustomPrefabsAPI.Extras.Utility;
using System;
namespace UCustomPrefabsAPI.Peak
{
    [Serializable]
    public struct PeakAccessoryTextureSwapperPair
    {
        public PeakAccessoryTarget TargetAccessory;
        public Texture2D Texture;
    }
    [Serializable]
    public class PeakAccessoryTextureSwapper : TaggedBehaviour
    {
        public string TargetTexture = "_MainTex";
        public List<string> Hints = new();
        public List<string> Types = new();
        public List<Texture2D> Textures = new();
        public List<int> TargetMaterials = new();
        public List<PeakAccessoryTarget> ToggleTargetTypes = new();
        public void Reset()
        {
            if (Hints.Count != Types.Count)
            {
                Debug.LogWarning($"Invalid Hint:Types Count, PeakAccessoryTargets in {gameObject.name} : PeakAccessoryTextureSwapper");
                return;
            }
            DefaultTextures = null;
            for (int i = 0; i < Hints.Count; i++)
            {
                ToggleTargetTypes.Add(new PeakAccessoryTarget(Types[i], Hints[i]));
            }
        }
        public void Set_CurrentTarget(PeakAccessoryTarget target)
        {
            var index = ToggleTargetTypes.IndexOf(target);
            Set_Texture(Textures[index]);
        }
        public List<Texture> DefaultTextures;
        public void Register_Default_Textures()
        {
            if (DefaultTextures != null)
                return;
            DefaultTextures = new();
            var renderer = GetComponent<Renderer>();
            var materials = renderer.sharedMaterials;
            foreach (var index in TargetMaterials)
            {
                try
                {
                    DefaultTextures.Add(materials[index].GetTexture(TargetTexture));
                }
                catch
                {
#if DEBUG
                    Debug.LogWarning($"Incorrect TargetMaterials in {gameObject.name}");
#endif
                }
            }
        }
        public void Set_Texture(Texture texture)
        {
            Register_Default_Textures();
            var renderer = GetComponent<Renderer>();
            if (!renderer)
                return;
            var materials = renderer.materials;
            if (texture == null)
            {
                for (int i = 0; i < TargetMaterials.Count; i++)
                {
                    try
                    {
                        materials[TargetMaterials[i]].SetTexture(TargetTexture, DefaultTextures[i]);
                    }
                    catch
                    {
#if DEBUG
                        Debug.LogWarning($"Incorrect TargetMaterials in {gameObject.name}");
#endif
                    }
                }
            }
            else
            {
                foreach (var index in TargetMaterials)
                {
                    try
                    {
                        materials[index].SetTexture(TargetTexture, texture);
                    }
                    catch
                    {
#if DEBUG
                        Debug.LogWarning($"Incorrect TargetMaterials in {gameObject.name}");
#endif
                    }
                }
            }
            //may not be required
            //renderer.materials = materials;
        }
    }
}