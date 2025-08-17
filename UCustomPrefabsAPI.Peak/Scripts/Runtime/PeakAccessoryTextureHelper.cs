using System.Collections.Generic;
using UnityEngine;
using UCustomPrefabsAPI.Extras.Utility;
using System;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    [Serializable]
    public class PeakAccessoryTextureHelper : TaggedBehaviour
    {
        public string TargetTexture = "_MainTex";
        public List<int> TargetMaterials = new();
        public List<Texture> DefaultTextures;
        public void Reset()
        {
            DefaultTextures = null;
        }
        public void Register_Default_Textures()
        {
            if (DefaultTextures != null)
                return;
            DefaultTextures = new();
            var renderer = GetComponent<Renderer>();
            var materials = renderer.sharedMaterials;
            foreach (var index in TargetMaterials)
                DefaultTextures.Add(materials[index].GetTexture(TargetTexture));
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
                for (int i = 0; i < DefaultTextures.Count; i++)
                {
                    materials[TargetMaterials[i]].SetTexture(TargetTexture, DefaultTextures[i]);
                }
            }
            else
            {
                foreach (var index in TargetMaterials)
                {
                    materials[index].SetTexture(TargetTexture, texture);
                }
            }
            //may not be required
            //renderer.materials = materials;
        }
    }
}