using System.Collections.Generic;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak.Scripts
{
    public static class MaterialEvaluator
    {
        public struct MaterialInfoOutput
        {
            public Material materialRef;
            public Dictionary<string, float> float_dict;
            public Dictionary<string, Vector4> vector_dict;
            public Dictionary<string, Texture> texture_dict;
            public MaterialInfoOutput(Material material)
            {
                materialRef = material;
                var float_props = material.GetPropertyNames(MaterialPropertyType.Float);
                var vector_props = material.GetPropertyNames(MaterialPropertyType.Vector);
                var texture_props = material.GetPropertyNames(MaterialPropertyType.Texture);
                float_dict = new Dictionary<string, float>();
                vector_dict = new Dictionary<string, Vector4>();
                texture_dict = new Dictionary<string, Texture>();
                foreach (var f in float_props)
                    float_dict[f] = material.GetFloat(f);
                foreach (var v in vector_props)
                    vector_dict[v] = material.GetVector(v);
                foreach (var t in texture_props)
                    texture_dict[t] = material.GetTexture(t);
            }
            public void Retarget(Material material)
            {
                foreach (var f in float_dict.Keys)
                {
                    material.SetFloat(f, float_dict[f]);
                }
                foreach (var v in vector_dict.Keys)
                {
                    material.SetVector(v, vector_dict[v]);
                }
                foreach (var t in texture_dict.Keys)
                {
                    material.SetTexture(t, texture_dict[t]);
                }
            }
            public void Reapply()
            {
                foreach (var f in float_dict.Keys)
                {
                    materialRef.SetFloat(f, float_dict[f]);
                }
                foreach (var v in vector_dict.Keys)
                {
                    materialRef.SetVector(v, vector_dict[v]);
                }
                foreach (var t in texture_dict.Keys)
                {
                    materialRef.SetTexture(t, texture_dict[t]);
                }
            }
        }
        public static MaterialInfoOutput EvaluateMaterial(Material material)
        {
            return new MaterialInfoOutput(material);
        }
    }
}
