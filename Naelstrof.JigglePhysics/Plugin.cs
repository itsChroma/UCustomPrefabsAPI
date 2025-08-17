using BepInEx;
namespace Naelstrof.JigglePhysics.Plugin
{
    public static class PluginInfo
    {
        public const string GUID = "Naelstrof.JigglePhysics";
        public const string NAME = "Naelstrof.JigglePhysics";
        public const string VERSION = "10.3.1";
        public const string WEBSITE = "https://github.com/naelstrof/UnityJigglePhysics";
    }
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
    }
}
