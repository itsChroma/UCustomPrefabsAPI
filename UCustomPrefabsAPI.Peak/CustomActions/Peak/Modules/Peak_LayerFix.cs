using UnityEngine;
using Zorro.Core;
namespace UCustomPrefabsAPI.Peak
{
    public class Peak_LayerFix : Peak_Module
    {
        public override void Init()
        {
            Do_Layer_Fix();
            Hide_Shadows();
        }
        public void Do_Layer_Fix()
        {
            foreach (var template in instance.Templates)
            {
                template.gameObject.SetLayerRecursivly(LayerMask.NameToLayer("Character"));
            }
        }
        //TODO Set up managed shadows later on... Please!
        public void Hide_Shadows()
        {
            foreach (var template in instance.Templates)
            {
                foreach (var renderer in template.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
        }
    }
}
