using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Reflection;
using UCustomPrefabsAPI.Peak.CustomActions;
using UCustomPrefabsAPI.RuntimeExtras;
using UCustomPrefabsAPI.Extras.AssetBundles;
using UCustomPrefabsAPI.Extras.CustomActions;
namespace UCustomPrefabsAPI.Peak
{
    //TODO fix UNT0008 potential issues with null-checks, Also stop using null checks everywhere,
    //currentOption?.isSkirt ?? false for example, can be currentOption != null ? currentOption.isSkirt : false
    public static class PluginInfo
    {
        public const string GUID = "UCustomPrefabsAPI.Peak";
        public const string NAME = "UCustomPrefabsAPI.Peak";
        public const string VERSION = "0.0.1";
        public const string WEBSITE = "https://github.com/ScottyFox/UCustomPrefabsAPI/tree/peak";
    }
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    [BepInDependency("Naelstrof.JigglePhysics", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigFile config;
        public static ConfigEntry<string> TemplatesFileFinderName;
        public static ConfigEntry<bool> EveryoneUsePreferredConfig;
        public static ConfigEntry<string> PreferredCharacterConfig;
        public static ConfigEntry<string> PreferredSkeletonConfig;
        public static ConfigEntry<string> PreferredChickenConfig;
        internal static Harmony instance = new(PluginInfo.GUID);
        private void Awake()
        {
            try
            {
                SetUpConfig();
                RegisterAssetBundles();
                RegisterCustomActions();
                RegisterCustomTemplates();
                //BulkLoadTemplates(); // Moved over to GameHandler Patch, to allow for other mods to load first. //
                instance.PatchAll(Assembly.GetExecutingAssembly());
                Logger.LogInfo($"Plugin {PluginInfo.GUID} is loaded!");
            }
            catch (Exception exception)
            {
                Logger.LogInfo($"Plugin {PluginInfo.GUID} failed to load...");
                Logger.LogError(exception);
            }
        }
        public void SetUpConfig()
        {
            config = base.Config;;
            TemplatesFileFinderName = Plugin.config.Bind("UCustomPrefabs Config", "Templates Folder Finder File", "ucustomprefabs.templates.txt");
            EveryoneUsePreferredConfig = Plugin.config.Bind<bool>("Player Config", "Everyone Uses Preferred Templates", false);
            PreferredCharacterConfig = Plugin.config.Bind("Player Config", "Preferred Character Template", "");
            PreferredSkeletonConfig = Plugin.config.Bind("Player Config", "Preferred Skeleton Template", "");
            PreferredChickenConfig = Plugin.config.Bind("Player Config", "Preferred Chicken Template", "");
        }
        public static void RegisterAssetBundles()
        {
            //In-case we need to load any utility assetbundles, you can ignore this//
            AssetBundleRegistry.RegisterEmbedded<Plugin>("custompassporticons","CustomPassportIcons");
        }
        public static void RegisterCustomActions()
        {
            //Register Custom Actions//
            CustomActionsRegistry.Register<Peak_CustomHelper>("Peak_PlayerHelper");
            CustomActionsRegistry.Register<MeshHider>("MeshHider");
            CustomActionsRegistry.Register<RigHelper>("RigHelper");
            CustomActionsRegistry.Register<ShaderFix>("ShaderFix");
        }
        public static void RegisterCustomTemplates()
        {
            //In-case we want to manually register a custom-template prefab
            //The Template is a gameobject that contains CustomAction-Templates,
            //with child objects that use the PrefabTemplate monobehaviour <-
            //The handler uses a "state" option, that allows the end-user to swap different states
            //THOUGH, I MAY change the behaviour of this to allow for stacking different states, since--
            //it's use is mostly ambiguous , and unless coded specifically for it, is mostly useless.
        }
        public static void BulkLoadTemplates()
        {
            //TODO : Remove BepInEx Requirement
            //TODO : Rework the way we handle files, Instead of checking assetbundles,--
            // Maybe add a TryToLoadTemplateAssetBundlesWithFileTypeFromPath function??? with ".ucustomprefab"
            var Directories = UCustomPrefabFileHelper.FindDirectoriesWithFileName(BepInEx.Paths.PluginPath, TemplatesFileFinderName.Value);
            foreach (var directory in Directories)
            {
                UCustomPrefabFileHelper.TryToLoadTemplateAssetBundlesFromPath<Plugin>(directory);
            }
            //TODO : This is a quick and dirty fix for ensuring templates are all determined after other mods//
            //HOWEVER, I need to figure out how to allow end-users to allow async asset loading.
            //Usually assets are loaded immediately when templates are registered. This is fine, but it does have a
            //Memory impact. It's the easier solution considering the nature of runtime objects being unpredictable--
            //BUT that being said, we do try to only soft-modify objects to allow for hot-swapping.
            //So , TODO , We need to rework a callback for template async-loading for the Handlers ->
            //Handler ->  OnLoad -> TryGrabTemplateASYNC (Set up a bool config?) -> With callback to continue handler.
            TemplateRegistry.CommitLateRegistry();
        }
    }
}
