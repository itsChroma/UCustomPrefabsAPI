using HarmonyLib;
using UCustomPrefabsAPI.Peak.ActionUtils;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak.Patches.Listeners
{
    [HarmonyPatch]
    public static class CharacterCustomization_Listeners
    {
        //CharacterDied
        public static ListenerHelper<Character, CharacterCustomization> CharacterDied_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "CharacterDied")]
        [HarmonyPostfix]
        static void CharacterDied_Postfix_Patch(ref CharacterCustomization __instance)
        {
#if DEBUG
            Debug.Log("CharacterDied_Postfix_Patch");
#endif
            if (__instance)
                CharacterDied_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //CharacterPassedOut
        public static ListenerHelper<Character, CharacterCustomization> CharacterPassedOut_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "CharacterPassedOut")]
        [HarmonyPostfix]
        static void CharacterPassedOut_Postfix_Patch(ref CharacterCustomization __instance)
        {
#if DEBUG
            Debug.Log("CharacterPassedOut_Postfix_Patch");
#endif
            if (__instance)
                CharacterPassedOut_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //OnRevive_RPC
        public static ListenerHelper<Character, CharacterCustomization> OnRevive_RPC_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "OnRevive_RPC")]
        [HarmonyPostfix]
        static void OnRevive_RPC_Postfix_Patch(ref CharacterCustomization __instance)
        {
#if DEBUG
            Debug.Log("OnRevive_RPC_Postfix_Patch");
#endif
            if (__instance)
                OnRevive_RPC_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //OnPlayerDataChange
        public static ListenerHelper<Character, CharacterCustomization> OnPlayerDataChange_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "OnPlayerDataChange")]
        [HarmonyPostfix]
        static void OnPlayerDataChange_Postfix_Patch(ref CharacterCustomization __instance)
        {
#if DEBUG
            Debug.Log("OnPlayerDataChange_Postfix_Patch");
#endif
            if (__instance)
                OnPlayerDataChange_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //PulseStatus
        public static ListenerHelper<Character, Color> PulseStatus_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "PulseStatus")]
        [HarmonyPostfix]
        static void PulseStatus_Postfix_Patch(ref CharacterCustomization __instance, Color c)
        {
#if DEBUG
            Debug.Log("PulseStatus_Postfix_Patch");
#endif
            if (__instance)
                PulseStatus_Postfix.Invoke(__instance.GetComponent<Character>(), c);
        }
        //BecomeChicken
        public static ListenerHelper<Character, CharacterCustomization> BecomeChicken_Prefix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "BecomeChicken")]
        [HarmonyPrefix]
        static void BecomeChicken_Prefix_Patch(ref CharacterCustomization __instance)
        {
            //Internal Check done by Vanilla//
            if (__instance.isCannibalizable)
                return;
#if DEBUG
            Debug.Log("BecomeChicken_Prefix_Patch");
#endif
            if (__instance)
                BecomeChicken_Prefix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //BecomeChicken
        public static ListenerHelper<Character, CharacterCustomization> BecomeChicken_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "BecomeChicken")]
        [HarmonyPostfix]
        static void BecomeChicken_Postfix_Patch(ref CharacterCustomization __instance)
        {
            //Internal Check done by Vanilla//
            if (!__instance.isCannibalizable)
                return;
#if DEBUG
            Debug.Log("BecomeChicken_Postfix_Patch");
#endif
            if (__instance)
                BecomeChicken_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //BecomeHuman
        public static ListenerHelper<Character, CharacterCustomization> BecomeHuman_Prefix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "BecomeHuman")]
        [HarmonyPrefix]
        static void BecomeHuman_Prefix_Patch(ref CharacterCustomization __instance)
        {
            //Internal Check done by Vanilla//
            if (!__instance.isCannibalizable)
                return;
#if DEBUG
            Debug.Log("BecomeHuman_Prefix_Patch");
#endif
            if (__instance)
                BecomeHuman_Prefix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //BecomeHuman
        public static ListenerHelper<Character, CharacterCustomization> BecomeHuman_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "BecomeHuman")]
        [HarmonyPostfix]
        static void BecomeHuman_Postfix_Patch(ref CharacterCustomization __instance)
        {
            //Internal Check done by Vanilla//
            if (__instance.isCannibalizable)
                return;
#if DEBUG
            Debug.Log("BecomeHuman_Postfix_Patch");
#endif
            if (__instance)
                BecomeHuman_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //ShowChicken
        public static ListenerHelper<Character, CharacterCustomization> ShowChicken_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "ShowChicken")]
        [HarmonyPostfix]
        static void ShowChicken_Postfix_Patch(ref CharacterCustomization __instance)
        {
#if DEBUG
            Debug.Log("ShowChicken_Postfix_Patch");
#endif
            if (__instance)
                ShowChicken_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //HideChicken
        public static ListenerHelper<Character, CharacterCustomization> HideChicken_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "HideChicken")]
        [HarmonyPostfix]
        static void HideChicken_Postfix_Patch(ref CharacterCustomization __instance)
        {
#if DEBUG
            Debug.Log("HideChicken_Postfix_Patch");
#endif
            if (__instance)
                HideChicken_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //ShowHuman
        public static ListenerHelper<Character, CharacterCustomization> ShowHuman_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "ShowHuman")]
        [HarmonyPostfix]
        static void ShowHuman_Postfix_Patch(ref CharacterCustomization __instance)
        {
#if DEBUG
            Debug.Log("ShowHuman_Postfix_Patch");
#endif
            if (__instance)
                ShowHuman_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
        //HideHuman
        public static ListenerHelper<Character, CharacterCustomization> HideHuman_Postfix = new();
        [HarmonyPatch(typeof(CharacterCustomization), "HideHuman")]
        [HarmonyPostfix]
        static void HideHuman_Postfix_Patch(ref CharacterCustomization __instance)
        {
#if DEBUG
            Debug.Log("HideHuman_Postfix_Patch");
#endif
            if (__instance)
                HideHuman_Postfix.Invoke(__instance.GetComponent<Character>(), __instance);
        }
    }
}
