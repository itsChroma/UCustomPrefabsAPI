using System.Collections.Generic;
using UCustomPrefabsAPI.Extras.Animation;
using UCustomPrefabsAPI.Extras.Utility;
using UnityEngine;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public class RigHelper : CustomActionsBase
    {
        //Now that I think about it, Maybe we should have template-specific variables that are stored in the base-template//
        RigHelper_Template.TargetMethod targetMethod = RigHelper_Template.TargetMethod.Name;
        string RigRootPath = "Hip";
        Transform Root = null;
        Dictionary<RigBuilder, BoneRigTracker> Rig_Targets = new Dictionary<RigBuilder, BoneRigTracker>();
        public override void RegisterActions()
        {
            AddOnStateChanged(BuildRigs);
            AddOnUpdate(Update);
            AddOnDestroy(CleanUp);
        }
        //Possibly change object[] to be object[] for parameters instead. UCustomPrefabs
        public override void HandleTemplateData(object[] data)
        {
            //Convert Template Data Over <--
            targetMethod = (RigHelper_Template.TargetMethod)data[0];
            RigRootPath = (string)data[1];
        }
        public void CleanUp()
        {
            Root = null;
            var rigs = Rig_Targets.Keys;
            foreach (var rig in rigs)
                if (rig)
                    Component.DestroyImmediate(rig);
            Rig_Targets.Clear();
        }
        public void BuildRigs(string last, string state)
        {
            CleanUp();
            switch (targetMethod)
            {
                case RigHelper_Template.TargetMethod.Directory:
                    {
                        Root = Handler.transform.Find(RigRootPath);
                    }
                    break;
                case RigHelper_Template.TargetMethod.Name:
                    {
                        Root = SearchUtils.IterativelyFindChild(Handler.transform, RigRootPath);
                    }
                    break;
                default:
                    {
                        Debug.LogWarning("Unknown TargetMethod");
                        return;
                    }
            }
            if (!Root)
                return;
            //Grab and Setup Rigs.
            foreach (var template in Handler.LoadedTemplates)
                foreach (var rig in template.Value.GetComponentsInChildren<RigBuilder>(true))
                    SetUpRig(rig, Root.parent);
            //Pre-Frame Attachment//
            Update();
        }
        public void SetUpRig(RigBuilder rig, Transform hips)
        {
            if (!Root) return;
            //Ensure we don't setup rig again.
            if (Rig_Targets.TryGetValue(rig, out var tracker))
                return;
            tracker = new BoneRigTracker(rig.transform, hips);
            tracker.SetUpRig(rig.Rig);
            Rig_Targets.Add(rig, tracker);
        }
        Stack<RigBuilder> BrokenRigs = new();
        public void Update()
        {
            if (!Root || !Root.gameObject.activeInHierarchy)
                return;
            foreach (var pair in Rig_Targets)
                if (!pair.Key)
                    BrokenRigs.Push(pair.Key);
                else
                    pair.Value.Update();
            ValidateRigs();
        }
        public void ValidateRigs()
        {
            if (BrokenRigs.Count != 0)
                Debug.LogWarning("Removing Broken Rigs.");
            while (BrokenRigs.Count > 0)
                Rig_Targets.Remove(BrokenRigs.Pop());
        }
    }
}
