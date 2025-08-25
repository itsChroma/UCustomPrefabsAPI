using Photon.Pun;
using ExitGames.Client.Photon;
using UCustomPrefabsAPI.Peak;
using UnityEngine;
using UCustomPrefabsAPI.Peak.CustomActions;
using UCustomPrefabsAPI.Peak.Utils;
namespace UCustomPrefabsAPI.PhotonUtils.Networking
{
    public class PlayerConfigHelper : MonoBehaviourPunCallbacks
    {
        public const string UCustomPrefabPrefix = "ucp_";
        public const string CharacterTag = $"{UCustomPrefabPrefix}char";
        public const string SkeletonTag = $"{UCustomPrefabPrefix}skel";
        public const string ChickenTag = $"{UCustomPrefabPrefix}chkn";
        public UCustomPrefabHandler CharacterHandler;
        public UCustomPrefabHandler PassportDummy_Handler;
        public UCustomPrefabHandler ChickenHandler;
        public static bool TryGetConfigHandler(Player player, out PlayerConfigHelper config)
        {
            config = player.character.GetComponent<PlayerConfigHelper>();
            return config;
        }
        public static bool TryGetConfigHandler(Character character, out PlayerConfigHelper config)
        {
            config = character.GetComponent<PlayerConfigHelper>();
            return config;
        }
        public static void UpdatePlayerData()
        {
            var hash = new Hashtable
            {
                { CharacterTag , Plugin.PreferredCharacterConfig.Value },
                { SkeletonTag , Plugin.PreferredSkeletonConfig.Value },
                { ChickenTag , Plugin.PreferredChickenConfig.Value }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer != photonView.Owner)
                return;
            object data;
            //var props = changedProps;// <- For individual changes
            // TODO Fix MeshHider issues, Old Method vvv
            // Issue being when Chicken template changes or Character without the other, the stored info
            // from Meshhider gets mis-aligned and meshes stay permanently hidden.
            // Either fix those order issues, Or create a per-character helper script that correctly
            // tracks which meshes are hidden despite of which order are loaded in.
            // *For now, we'll just update everything.

            PurgeInstance(ChickenHandler);
            PurgeInstance(PassportDummy_Handler);
            PurgeInstance(CharacterHandler);

            var props = targetPlayer.CustomProperties; //Everything Always happens//
            if (props.TryGetValue(CharacterTag, out data))
                if (data is string token)
                    UpdateCharacterTemplate(targetPlayer, token);
            if (props.TryGetValue(SkeletonTag, out data))
                if (data is string token)
                    UpdateSkeletonTemplate(targetPlayer, token);
            if (props.TryGetValue(ChickenTag, out data))
                if (data is string token)
                    UpdateChickenTemplate(targetPlayer, token);

            //Not Sure If Neccessary to update our Passport//
            PassportManager.instance?.SetActiveButton();
        }
        public string CurrentCharacterTemplate
        {
            get
            {
                if (photonView.Owner.CustomProperties.TryGetValue(CharacterTag, out var data))
                    return (string)data;
                return CustomTemplateUtils.NoPreference_Template;
            }
        }
        public static void SetCharacterTemplate(string template)
        {
            Plugin.PreferredCharacterConfig.Value = template;
            var hash = new Hashtable
            {
                { CharacterTag , Plugin.PreferredCharacterConfig.Value }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            //UpdatePlayerData();
        }
        public string CurrentSkeletonTemplate
        {
            get
            {
                if (photonView.Owner.CustomProperties.TryGetValue(SkeletonTag, out var data))
                    return (string)data;
                return CustomTemplateUtils.NoPreference_Template;
            }
        }
        public static void SetSkeletonTemplate(string template)
        {
            Plugin.PreferredSkeletonConfig.Value = template;
            var hash = new Hashtable
            {
                { SkeletonTag , Plugin.PreferredSkeletonConfig.Value }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            //UpdatePlayerData();
        }
        public string CurrentChickenTemplate
        {
            get
            {
                if (photonView.Owner.CustomProperties.TryGetValue(ChickenTag, out var data))
                    return (string)data;
                return CustomTemplateUtils.NoPreference_Template;
            }
        }
        public static void SetChickenTemplate(string template)
        {
            Plugin.PreferredChickenConfig.Value = template;
            var hash = new Hashtable
            {
                { ChickenTag , Plugin.PreferredChickenConfig.Value }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            //UpdatePlayerData();
        }
        private void UpdateCharacterTemplate(Photon.Realtime.Player targetPlayer, string token)
        {
            //Cleanup first!//
            PurgeInstance(CharacterHandler);
            //Fetch actual templateID//
            var templateID = CustomTemplateUtils.Evaluate_Peak_Custom_Template(this, PeakTemplateType.Character, token);
            //Verify if we need to use a template//
            if (!CustomTemplateUtils.IsSpecialTemplate(templateID))
            {
                var TemplateUID = $"{CharacterTag}:{targetPlayer.ActorNumber}:{templateID}";
                RegisterInstance(TemplateUID, templateID, transform, out CharacterHandler);
            }
            //Passport Stuff
            if (!photonView.IsMine)
                return;
            PurgeInstance(PassportDummy_Handler);
            if (!CustomTemplateUtils.IsSpecialTemplate(templateID))
            {
                var DummyTemplateUID = $"dummy:{targetPlayer.ActorNumber}:{templateID}";
                RegisterInstance(DummyTemplateUID, templateID, PassportDummy.transform, out PassportDummy_Handler);
            }
        }
        public PlayerCustomizationDummy PassportDummy
        {
            get
            {
                var passport = PassportManager.instance;
                return passport ? passport.dummy : null;
            }
        }
        private void UpdateSkeletonTemplate(Photon.Realtime.Player targetPlayer, string token)
        {
            //TODO Handle Passport Preview Stuff//
        }
        public void CreateSkeletonInstance(Skelleton skeleton)
        {
            var templateID = CustomTemplateUtils.Evaluate_Peak_Custom_Template(this, PeakTemplateType.Skeleton, CurrentSkeletonTemplate);
            //Verify if we need to use a template//
            if (CustomTemplateUtils.IsSpecialTemplate(templateID))
                return;
            var TemplateUID = $"{SkeletonTag}:{photonView.OwnerActorNr}:{templateID}";
            InstanceManager.Register(TemplateUID, templateID, skeleton.transform);
        }
        private void UpdateChickenTemplate(Photon.Realtime.Player targetPlayer, string token)
        {
            //Cleanup first!//
            PurgeInstance(ChickenHandler);
            //Fetch actual templateID//
            var templateID = CustomTemplateUtils.Evaluate_Peak_Custom_Template(this, PeakTemplateType.Chicken, token);
            //Verify if we need to use a template//
            if (CustomTemplateUtils.IsSpecialTemplate(templateID))
                return;
            var TemplateUID = $"{ChickenTag}:{targetPlayer.ActorNumber}:{templateID}";
            RegisterInstance(TemplateUID, templateID, transform, out ChickenHandler);
            //TODO Handle Passport Preview Stuff//
        }
        public static void PurgeInstance(UCustomPrefabHandler handler)
        {
            if (!handler)
                return;
            InstanceManager.Remove(handler.Instance.ID);
        }
        public static bool RegisterInstance(string uid, string template_uid, Transform target, out UCustomPrefabHandler handler)
        {
            handler = null;
            if (VerifyTemplate(template_uid))
            {
                if (InstanceManager.Register(uid, template_uid, target))
                {
                    InstanceManager.TryGetInstance(uid, out var instanceInfo);
                    handler = instanceInfo.Handler;
                    return true;
                }
            }
            return false;
        }
        public static bool VerifyTemplate(string templateID)
        {
            if (string.IsNullOrWhiteSpace(templateID)) return false;
            return TemplateRegistry.TryGetTemplate(templateID, out var _);
        }
        public void Start()
        {
            if (photonView.IsMine)
                UpdatePlayerData();
            OnPlayerPropertiesUpdate(photonView.Owner, photonView.Owner.CustomProperties);
        }
        public void Update()
        {
            Update_Debug_Model_Selection();
        }
        public void Update_Debug_Model_Selection()
        {
            if (!photonView.IsMine)
                return;
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                PassportManager.instance.OpenTab((Customization.Type)PassportHelper.Character_Enum);
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                PassportManager.instance.OpenTab(Customization.Type.Skin);
            }
        }
    }
}