using UCustomPrefabsAPI.RuntimeExtras;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak
{
    public class Peak_SteamAvatar : Peak_Module
    {
        public override void Init()
        {
            Update_SteamAvatarHelpers();
        }
        //TODO The Async Loading to this module, So we don't have multiple instances all calling for a avatar.
        //Plus we can avoid the weird unity null stuff.
        public void Update_SteamAvatarHelpers()
        {
#if DEBUG
            Debug.Log("Update_SteamAvatarHelpers");
#endif
            foreach (var tagged in instance.Handler.GetTagsInTemplates("SteamAvatar"))
            {
                if (tagged is SteamAvatarHelper helper)
                    helper.Load_Player_Avatar(instance.PhotonView);
            }
        }
    }
}
