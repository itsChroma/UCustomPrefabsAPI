using System.Collections.Generic;
using UnityEngine;
using UCustomPrefabsAPI.Extras.Utility;
using Steamworks;
using Photon.Pun;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public class SteamAvatarHelper : TaggedBehaviour
    {
        public string TargetTexture = "_MainTex";
        [SerializeField, HideInInspector] public List<int> TargetMaterials = new List<int>();
        public void Load_Player_Avatar(PhotonView view)
        {
            if (ourSteamID != default)
                Peak_SteamUtils.Fetch_SteamID_Avatar(ourSteamID, SteamAvatarLoaded);
            else
                Peak_SteamUtils.Fetch_SteamID(view, SteamID_Loaded);
        }
        private CSteamID ourSteamID = default;
        private void SteamID_Loaded(CSteamID steamID)
        {
            ourSteamID = steamID;
            Peak_SteamUtils.Fetch_SteamID_Avatar(ourSteamID, SteamAvatarLoaded);
        }
        private void SteamAvatarLoaded(Texture2D avatar)
        {
            var renderer = GetComponent<Renderer>();
            if (!renderer)
                return;
            var materials = renderer.materials;
            foreach (var index in TargetMaterials)
            {
                materials[index].SetTexture(TargetTexture, avatar);
            }
            //may not be required
            //renderer.materials = materials;
        }
    }
}