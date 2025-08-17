using HarmonyLib;
using UCustomPrefabsAPI.Peak.Utils;
namespace UCustomPrefabsAPI.Peak
{
    [HarmonyPatch]
    internal static class CustomizationPatch
    {
        [HarmonyPatch(typeof(Customization), "GetList")]
        [HarmonyPrefix]
        static bool GetList_Prefix_Patch(Customization.Type type, ref object[] __result)
        {
            if (!PassportHelper.IsSpecialCustomization(type, out var templateType))
                return true;
            __result = PassportHelper.Construct_Customization_List(templateType);
            return false;
        }
        public static CustomizationOption[] Test_list()
        {
            return Customization.Instance.GetList((Customization.Type)PassportHelper.Character_Enum);
        }
    }
}
