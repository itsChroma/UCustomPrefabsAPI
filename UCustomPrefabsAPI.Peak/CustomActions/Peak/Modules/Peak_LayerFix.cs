using UnityEngine;
using Zorro.Core;
namespace UCustomPrefabsAPI.Peak
{
    public class Peak_LayerFix : Peak_Module
    {
        public override void Init()
        {
            Do_Layer_Fix();
        }
        public void Do_Layer_Fix()
        {
            foreach (var template in instance.Templates)
            {
                template.gameObject.SetLayerRecursivly(LayerMask.NameToLayer("Character"));
            }
        }
    }
}
