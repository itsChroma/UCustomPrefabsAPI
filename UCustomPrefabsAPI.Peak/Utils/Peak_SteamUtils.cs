using Photon.Pun;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public class Peak_SteamUtils
    {
        public static Dictionary<CSteamID, Texture2D> LoadedAvatars = new();
        public static void Fetch_SteamID(PhotonView view, Action<CSteamID> callback)
        {
#if DEBUG
            Debug.Log("Fetch_SteamID");
#endif
            if (view == null)
                return;
            var awaiter = Fetch_Photonview_SteamID(view).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                //Weird Unity Object Non-Sense//
                if (callback?.Target is UnityEngine.Object obj && !obj)
                    return;
                callback?.Invoke(awaiter.GetResult());
            });
        }
        public static void Fetch_SteamID_Avatar(CSteamID steamID, Action<Texture2D> callback)
        {
#if DEBUG
            Debug.Log("Fetch_SteamID_Avatar");
#endif
            if (steamID == default)
                return;
            var awaiter = Fetch_SteamAvatar(steamID).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                //Weird Unity Object Non-Sense//
                if (callback?.Target is UnityEngine.Object obj && !obj)
                    return;
                callback?.Invoke(awaiter.GetResult());
            });
        }
        public static void Clear_Loaded_Avatars()
        {
#if DEBUG
            Debug.Log("Clear_Loaded_Avatars");
#endif
            LoadedAvatars.Clear();
        }
        private static async Task<CSteamID> Fetch_Photonview_SteamID(PhotonView view)
        {
            if (view.IsMine)
                return SteamUser.GetSteamID();
            if (view)
                return await Find_Nickname_SteamID(view.Owner.NickName);
            else
                return default;
        }
        private static async Task<CSteamID> Find_Nickname_SteamID(string nickname)
        {
            //Utilize Peak's lobby stuff already. Otherwise I don't know how to get the lobbyID//
            if (!GameHandler.GetService<SteamLobbyHandler>().InSteamLobby(out var lobbyID))
            {
#if DEBUG
                Debug.Log("Unable to get SteamLobbyHandler service.");
#endif
                return default;
            }
            var numMembers = SteamMatchmaking.GetNumLobbyMembers(lobbyID);
            var memberNames = new Dictionary<string, CSteamID>();
            for (int i = 0; i < numMembers; i++)
            {
                var memberID = SteamMatchmaking.GetLobbyMemberByIndex(lobbyID, i);
                await RequestUserInfo(memberID, true);
                var memberName = SteamFriends.GetFriendPersonaName(memberID);
                if (memberName == nickname)
                    return memberID;
            }
            Debug.LogWarning($"Unable to find nickname in steam lobby : {nickname}");
            return default;
        }
        private static async Task<Texture2D> Fetch_SteamAvatar(CSteamID steamID)
        {
            if (LoadedAvatars.TryGetValue(steamID, out var avatar))
                return avatar;
            await RequestUserInfo(steamID);
            int imageId = SteamFriends.GetLargeFriendAvatar(steamID);
            if (imageId == -1)
            {
                Debug.LogWarning($"Failed to GetLargeFriendAvatar {steamID}");
                return null;
            }
            uint width, height;
            bool success = SteamUtils.GetImageSize(imageId, out width, out height);
            if (!success || width == 0 || height == 0)
            {
                Debug.LogWarning($"Failed to GetImageSize {steamID}");
                return null;
            }
            byte[] image = new byte[4 * (int)width * (int)height];
            success = SteamUtils.GetImageRGBA(imageId, image, image.Length);
            if (!success)
            {
                Debug.LogWarning($"Failed to GetImageRGBA {steamID}");
                return null;
            }
            FlipImageHorizontally(ref image, width, height);
            avatar = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
            avatar.LoadRawTextureData(image);
            avatar.Apply();
            LoadedAvatars.Add(steamID, avatar);
            return avatar;
        }
        private static void FlipImageHorizontally(ref byte[] image, uint width, uint height)
        {
            //Working Values
            byte r, g, b, a;
            int ti, bi, topOffset, bottomOffset;
            //
            int rowSize = (int)width * 4;
            // In-place vertical flip using int temp vars (swap rows)
            int halfHeight = (int)height / 2;
            for (int y = 0; y < halfHeight; y++)
            {
                topOffset = y * rowSize;
                bottomOffset = ((int)height - 1 - y) * rowSize;
                for (int x = 0; x < rowSize; x += 4)
                {
                    ti = topOffset + x;
                    bi = bottomOffset + x;

                    // Swap 4 bytes (RGBA)
                    r = image[ti];
                    g = image[ti + 1];
                    b = image[ti + 2];
                    a = image[ti + 3];

                    image[ti] = image[bi];
                    image[ti + 1] = image[bi + 1];
                    image[ti + 2] = image[bi + 2];
                    image[ti + 3] = image[bi + 3];

                    image[bi] = r;
                    image[bi + 1] = g;
                    image[bi + 2] = b;
                    image[bi + 3] = a;
                }
            }
        }
        private const int RequestUserInfo_Interval = 500;
        private static async Task RequestUserInfo(CSteamID steamID, bool NameOnly = false)
        {
            while (SteamFriends.RequestUserInformation(steamID, NameOnly))
                await Task.Delay(RequestUserInfo_Interval);
        }
    }
}
