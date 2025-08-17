using UCustomPrefabsAPI.Peak.Patches.Listeners;
using UnityEngine;

namespace UCustomPrefabsAPI.Peak
{
    public class Peak_HideTheBody : Peak_Module
    {
        public override void Init()
        {
            Update_Renderers_VertexGhost();
            HideTheBody_Listeners.Toggle_Postfix.Listen(instance.Character, Toggle);
        }
        public override void Destroy()
        {
            HideTheBody_Listeners.Toggle_Postfix.Un_Listen(instance.Character, Toggle);
        }
        public void Toggle(HideTheBody hideTheBody)
        {
#if DEBUG
            Debug.Log("HideTheBody_Toggle");
#endif
            Update_Renderers_VertexGhost();
        }
        public bool IsVertexGhostShowing => instance.Character?.refs.hideTheBody.isShowing ?? false;
        public void Update_Renderers_VertexGhost()
        {
            foreach (var renderer in instance.Template_Renderers)
            {
                var materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                    materials[i].SetFloat(CharacterCustomization.VertexGhost, IsVertexGhostShowing ? 0 : 1);
                //may not be required
                //renderer.materials = materials;
            }
        }
    }
}
