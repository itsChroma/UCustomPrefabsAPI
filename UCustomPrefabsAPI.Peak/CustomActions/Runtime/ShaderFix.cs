using UnityEngine;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public class ShaderFix : CustomActionsBase
    {
        public override void RegisterActions()
        {
            AddOnStateChanged(DoInit);
        }
        public void DoInit(string last, string state)
        {
            foreach (var target in Handler.LoadedTemplates)
                FixShaders(target.Value.transform);
        }
        public void FixShaders(Transform target)
        {
            foreach (var renderer in target.GetComponentsInChildren<Renderer>(true))
            {
                var materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    var newShader = Shader.Find(materials[i].shader.name);
                    if (newShader != null)
                        materials[i].shader = newShader;
                }
                renderer.sharedMaterials = materials;
            }
        }
    }
}