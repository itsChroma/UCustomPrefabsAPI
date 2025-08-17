using HarmonyLib;
using UCustomPrefabsAPI.PhotonUtils.Networking;
namespace UCustomPrefabsAPI.Peak
{
    [HarmonyPatch]
    internal static class SkeletonPatch
    {
        [HarmonyPatch(typeof(Skelleton), "SpawnSkelly")]
        [HarmonyPostfix]
        static void SpawnSkelly_Postfix_Patch(ref Skelleton __instance, Character target)
        {
            if (!PlayerConfigHelper.TryGetConfigHandler(target, out var config))
                return;
            config.CreateSkeletonInstance(__instance);
        }
    }
}
