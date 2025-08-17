using DG.Tweening;
using UCustomPrefabsAPI.Peak.Patches.Listeners;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak
{
    public class Peak_PulseStatus : Peak_Module
    {
        public override void Init()
        {
            CharacterCustomization_Listeners.PulseStatus_Postfix.Listen(instance.Character, DoPulseStatus);
        }
        public override void Destroy()
        {
            CharacterCustomization_Listeners.PulseStatus_Postfix.Un_Listen(instance.Character, DoPulseStatus);
        }
        public void DoPulseStatus(Color color)
        {
            foreach (var renderer in instance.Template_Renderers)
            {
                var materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    //Mimics CharacterCustomization.PulseStatus
                    if (materials[i].HasColor(CharacterCustomization.StatusColor))
                    {
                        materials[i].SetColor(CharacterCustomization.StatusColor, color);
                    }
                    if (materials[i].HasFloat(CharacterCustomization.StatusGlow))
                    {
                        materials[i].SetFloat(CharacterCustomization.StatusGlow, 1f);
                        materials[i].DOFloat(0f, CharacterCustomization.StatusGlow, 0.5f);
                    }
                }
            }
        }
    }
}
