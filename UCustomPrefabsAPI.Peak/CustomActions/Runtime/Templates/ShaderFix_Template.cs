using System;
using UCustomPrefabsAPI.Peak.CustomActions;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public class ShaderFix_Template : CustomActionsTemplate
    {
#if UNITY_EDITOR
        public override Type RegisterCustomActionsBaseType() => throw new NotImplementedException();
#else
        public override Type RegisterCustomActionsBaseType() => typeof(ShaderFix);
#endif
    }
}