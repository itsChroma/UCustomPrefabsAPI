using System;
using System.Collections.Generic;
using UnityEngine;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public class MeshHider_Template : CustomActionsTemplate
    {
        [Serializable]
        public enum HiderMethod
        {
            Directory,
            Name,
            ChildrenOfDirectory,
            ChildrenOfName,
            Everything,
            SearchUtilTokens//TODO Varify search util token find works.
        }
        [SerializeField][HideInInspector] public string HiderMethod_data;
        [SerializeField] public List<string> MeshesHidden = new();
        [SerializeField] public bool AlwaysCheck = false;
#if UNITY_EDITOR
        //Handle Editor-Serialization
        [SerializeField] public HiderMethod hiderMethod = HiderMethod.Name;
        public void OnValidate()
        {
            HiderMethod_data = hiderMethod.ToString();
        }
        public override Type RegisterCustomActionsBaseType() => throw new NotImplementedException();
#else
        public override object[] PrepareTemplateData()
        {
            return [Enum.Parse(typeof(HiderMethod), HiderMethod_data), MeshesHidden, AlwaysCheck];
        }
        public override Type RegisterCustomActionsBaseType() => typeof(MeshHider);
#endif
    }
}