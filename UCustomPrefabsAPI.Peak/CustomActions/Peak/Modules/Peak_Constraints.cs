using System;
using System.Collections.Generic;
using static UCustomPrefabsAPI.RuntimeExtras.ConstraintTemplate;
using UCustomPrefabsAPI.Extras.Utility;
using UCustomPrefabsAPI.RuntimeExtras;
using UnityEngine;
using UCustomPrefabsAPI.Peak.Patches.Listeners;

namespace UCustomPrefabsAPI.Peak
{
    public class Peak_Constraints : Peak_Module
    {
        //Dirty Hack To Manage Reset Order
        //TODO move this to main Peak_PlayerHelper??? Just to handle future order issues//
        public static Dictionary<Transform, Stack<Peak_Constraints>> Shared_Constraints = new();
        public bool IsPrimaryConstraint = false;
        public override void Init()
        {
            RebuildConstraints();
            CharacterCustomization_Listeners.BecomeChicken_Prefix.Listen(instance.Character, BecomeChicken);
            CharacterCustomization_Listeners.BecomeHuman_Prefix.Listen(instance.Character, BecomeHuman);
        }
        public override void Update()
        {
            UpdateConstraints();
        }
        public override void Reset()
        {
            //Dirty Hack To Manage Reset Order
            if (IsPrimaryConstraint)
            {
                var target = instance.Handler.transform;
                if (!Shared_Constraints.TryGetValue(target, out var sharedConstraints))
                {
                    Debug.LogError("Error Getting Shared Constraints");
                }
                if (!Shared_Constraints.Remove(instance.Handler.transform))
                {
                    Debug.LogError("Error Removing Shared Constraints");
                }
                //Reset Constraints starting from the back//
                while (sharedConstraints.Count > 0)
                {
                    sharedConstraints.Pop()?.Reset_Constraints();
                }
            }
        }
        public override void Destroy()
        {
            CharacterCustomization_Listeners.BecomeChicken_Prefix.Un_Listen(instance.Character, BecomeChicken);
            CharacterCustomization_Listeners.BecomeHuman_Prefix.Un_Listen(instance.Character, BecomeHuman);
        }
        //CONSTRAINTS
        private List<BakedConstraintTemplate> BakedConstraints = new();
        public void RebuildConstraints()
        {
            foreach (var loadedTemplate in instance.Handler.LoadedTemplates)
            {
                var split = loadedTemplate.Key.Split(':');
                var templateKey = split[0];
                var templateIndex = int.Parse(split[1]);
#if DEBUG
                Debug.Log($"{templateKey} Looking Up Template Key.");
#endif
                if (TemplateRegistry.TryGetTemplate(templateKey, out var template))
                {
                    var currentTemplate = template.Templates[templateIndex];
#if DEBUG
                    Debug.Log($"{templateKey} Template Key Found.");
#endif
                    foreach (var constraintTemplate in template.Container.GetComponentsInChildren<ConstraintTemplate>(true))
                    {
                        RegisterConstraints(loadedTemplate.Value.transform, currentTemplate.transform, constraintTemplate);
                    }
                }
            }
            //Dirty Hack To Manage Reset Order
            if (!Shared_Constraints.TryGetValue(instance.Handler.transform, out var active))
            {
                active = new Stack<Peak_Constraints>();
                Shared_Constraints[instance.Handler.transform] = active;
                IsPrimaryConstraint = true;
            }
            active.Push(this);
            //
            var isCurrentlyChicken = instance.CharacterCustomization.isCannibalizable;
            if (isCurrentlyChicken)
                Toggle_Constraints(instance.IsChickenTemplate);
            else
                Toggle_Constraints(!instance.IsChickenTemplate);
            //
            UpdateConstraints();
        }
        public bool ConstraintsAreActive = true;
        public void Toggle_Constraints(bool isActive = true)
        {
            ConstraintsAreActive = isActive;
            if (!isActive && BakedConstraints.Count != 0)
                for (int i = BakedConstraints.Count - 1; i >= 0; i--)
                {
                    BakedConstraints[i].RestoreFollowers();
                }
        }
        public void Reset_Constraints()
        {
            if (BakedConstraints.Count != 0)
                for (int i = BakedConstraints.Count - 1; i >= 0; i--)
                {
                    BakedConstraints[i].RestoreFollowers();
                }
            BakedConstraints.Clear();
        }
        public void RegisterConstraints(Transform loadedTemplate, Transform refTemplate, ConstraintTemplate constraintTemplate)
        {
            Transform target = null;
            TargetMethod targetMethod = (TargetMethod)Enum.Parse(typeof(TargetMethod), constraintTemplate.TargetMethod_data);
            switch (targetMethod)
            {
                case TargetMethod.Directory:
                    {
                        target = instance.Handler.transform.Find(constraintTemplate.TargetPath);
                    }
                    break;
                case TargetMethod.Name:
                    {
                        target = SearchUtils.IterativelyFindChild(instance.Handler.transform, constraintTemplate.TargetPath);
                    }
                    break;
            }
            if (!refTemplate || !constraintTemplate?.TargetObject)
            {
                Debug.LogWarning("Invalid Template Reference, Unable to build constraints.");
                return;
            }
            var refPath = SearchUtils.FindPath(refTemplate, constraintTemplate.TargetObject);
            var root = loadedTemplate.Find(refPath);//SearchUtils.IterativelyFindChild(loadedTemplate, refPath);
            if (!root)
            {
                Debug.LogWarning("Invalid Template Root, Unable to build constraints.");
                return;
            }
            var bakedConstraint = new BakedConstraintTemplate(constraintTemplate, target, root);
            BakedConstraints.Add(bakedConstraint);
        }
        public void UpdateConstraints()
        {
            if (ConstraintsAreActive)
                foreach (var constraint in BakedConstraints)
                    constraint.UpdateFollowers();
        }
        private void BecomeChicken(CharacterCustomization customization)
        {
            Toggle_Constraints(instance.IsChickenTemplate);
        }
        private void BecomeHuman(CharacterCustomization customization)
        {
            Toggle_Constraints(!instance.IsChickenTemplate);
        }
    }
}
