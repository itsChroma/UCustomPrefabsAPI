using Steamworks;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak.Scripts
{
    public static class LobbyAnalyzer
    {
        public static void Analyze()
        {
#if DEBUG
            Debug.Log("Steam Lobby Info:");
#endif
            CSteamID steamIDLobby;
            if (GameHandler.GetService<SteamLobbyHandler>().InSteamLobby(out steamIDLobby))
            {
                var numSteamLobby = SteamMatchmaking.GetNumLobbyMembers(steamIDLobby);
                for (int i = 0; i < numSteamLobby; i++)
                {
                    var lobbymember = SteamMatchmaking.GetLobbyMemberByIndex(steamIDLobby, i);
#if DEBUG
                    Debug.Log($"{lobbymember}:{SteamFriends.GetFriendPersonaName(lobbymember)}");
#endif
                }
            }
#if DEBUG
            Debug.Log("Photon Lobby Info:");
#endif
            foreach (var player in PlayerHandler.GetAllPlayers())
            {
#if DEBUG
                Debug.Log($"{player.photonView.Owner.ActorNumber}:{player.photonView.Owner.UserId}:{player.photonView.Owner.NickName}");
#endif
            }
        }
    }
}
