using HarmonyLib;
using UCustomPrefabsAPI.Peak.ActionUtils;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak.Patches.Listeners
{
    [HarmonyPatch]
    public static class HideTheBody_Listeners
    {
        public static ListenerHelper<Character, HideTheBody> Toggle_Postfix = new();
        [HarmonyPatch(typeof(HideTheBody), "Toggle")]
        [HarmonyPostfix]
        static void Toggle_Postfix_Patch(ref HideTheBody __instance)
        {
#if DEBUG
            Debug.Log("Toggle_Postfix_Patch");
#endif
            Toggle_Postfix.Invoke(__instance.character, __instance);
        }
    }
}
