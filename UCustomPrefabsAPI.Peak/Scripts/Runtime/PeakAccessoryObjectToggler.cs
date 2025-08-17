using System.Collections.Generic;
using UnityEngine;
using UCustomPrefabsAPI.Extras.Utility;
using System;
namespace UCustomPrefabsAPI.Peak
{
    [Serializable]
    public class PeakAccessoryObjectToggler : TaggedBehaviour
    {
        public bool HideOnTarget = false;
        public bool HideTargetAccessory = true;
        public List<string> Hints = new();
        public List<string> Types = new();
        public List<PeakAccessoryTarget> ToggleTargetTypes = new();
        public void Rebuild()
        {
            if (Hints.Count != Types.Count)
            {
                Debug.LogWarning($"Invalid Hint:Types Count, PeakAccessoryTargets in {gameObject.name} : PeakAccessoryObjectToggler");
                return;
            }
            for (int i = 0; i < Hints.Count; i++)
            {
                ToggleTargetTypes.Add(new PeakAccessoryTarget(Types[i], Hints[i]));
            }
        }
    }
}