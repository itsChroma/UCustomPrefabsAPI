using HarmonyLib;
namespace UCustomPrefabsAPI.Peak
{
    [HarmonyPatch]
    internal static class GameHandlerPatch
    {
        [HarmonyPatch(typeof(GameHandler), "Initialize")]
        [HarmonyPostfix]
        static void Initialize_Postfix_Patch(ref GameHandler __instance)
        {
            Plugin.BulkLoadTemplates();
        }
    }
}
