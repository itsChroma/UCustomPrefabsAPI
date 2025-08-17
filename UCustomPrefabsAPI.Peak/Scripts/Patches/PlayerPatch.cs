using HarmonyLib;
using UCustomPrefabsAPI.PhotonUtils.Networking;
namespace UCustomPrefabsAPI.Peak
{
    [HarmonyPatch]
    internal static class PlayerPatch
    {
        [HarmonyPatch(typeof(Character), "Start")]
        [HarmonyPostfix]
        static void Start_Postfix_Patch(ref Character __instance)
        {
            //Verify if we are in-fact a Scout model
            if (__instance.isBot || !__instance.transform.Find("Scout"))
                return;
            if (__instance.GetComponent<PlayerConfigHelper>())
                return;
            __instance.gameObject.AddComponent<PlayerConfigHelper>();
        }
    }
}
