using System;
using UnityEngine;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public class RigHelper_Template : CustomActionsTemplate
    {
        [Serializable]
        public enum TargetMethod
        {
            Directory,
            Name
        }
        [SerializeField][HideInInspector] public string TargetMethod_data;
        [SerializeField] public string RigRootPath = "Hip";
#if UNITY_EDITOR
        //Handle Editor-Serialization
        [SerializeField] public TargetMethod targetMethod = TargetMethod.Name;
        public void OnValidate()
        {
            TargetMethod_data = targetMethod.ToString();
        }
        public override Type RegisterCustomActionsBaseType() => throw new NotImplementedException();
#else
        public override object[] PrepareTemplateData()
        {
            Enum.TryParse<TargetMethod>(TargetMethod_data, out var targetMethod);
            return new object[]{ targetMethod, RigRootPath};
        }
        public override Type RegisterCustomActionsBaseType() => typeof(RigHelper);
#endif
    }
}