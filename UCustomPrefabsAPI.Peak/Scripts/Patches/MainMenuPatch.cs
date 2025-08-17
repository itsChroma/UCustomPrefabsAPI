using HarmonyLib;
using UCustomPrefabsAPI.RuntimeExtras;
namespace UCustomPrefabsAPI.Peak
{
    [HarmonyPatch]
    internal static class MainMenuPatch
    {
        [HarmonyPatch(typeof(MainMenu), "Start")]
        [HarmonyPostfix]
        static void Start_Postfix_Patch(ref MainMenu __instance)
        {
            //Just so we can clean up any stored textures//
            Peak_SteamUtils.Clear_Loaded_Avatars();
        }
    }
}
