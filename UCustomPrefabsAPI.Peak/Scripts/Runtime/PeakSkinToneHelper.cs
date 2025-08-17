using System.Collections.Generic;
using UnityEngine;
using UCustomPrefabsAPI.Extras.Utility;
namespace UCustomPrefabsAPI.Peak
{
    public class PeakSkinToneHelper : TaggedBehaviour
    {
        [SerializeField, HideInInspector] public List<int> TargetMaterials = new List<int>();
        public void Set_SkinTone(Color skinColor)
        {
            var renderer = GetComponent<Renderer>();
            if (!renderer)
                return;
            var materials = renderer.materials;
            foreach (var index in TargetMaterials)
            {
                materials[index].SetFloat("_UseSkinTone", 1);
                materials[index].SetColor(CharacterCustomization.SkinColor, skinColor);
            }
            //May Not be Required///
            //renderer.materials = materials;
        }
    }
}