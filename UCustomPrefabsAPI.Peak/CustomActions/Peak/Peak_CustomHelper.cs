using System;
using System.Collections.Generic;
using UCustomPrefabsAPI.Peak.Utils;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak.CustomActions
{
    public partial class Peak_CustomHelper : CustomActionsBase
    {
        public override void RegisterActions()
        {
            AddOnStateChanged(DoInit);
            AddOnUpdate(OnUpdate);
            AddOnDestroy(OnDestroy);
        }
        private Dictionary<Type, Peak_Module> _modules = new();

        public void RegisterModule<T>() where T : Peak_Module
        {
            if (_modules.ContainsKey(typeof(T)))
                return;
            var module_type = typeof(T);
            var module = (Peak_Module)Activator.CreateInstance(module_type);
            module.instance = this;
            _modules[module_type] = module;
        }
        public bool TryGetModule<T>(out T module) where T : Peak_Module
        {
            var success = _modules.TryGetValue(typeof(T), out var foundModule);
            module = (T)foundModule;
            return success;
        }
        private void Init_Modules()
        {
            foreach (var module in _modules.Values)
                try
                {
                    module.Init();
                }
                catch (Exception ex) { Debug.LogError(ex); }
        }
        private void Update_Modules()
        {
            foreach (var module in _modules.Values)
                try
                {
                    module.Update();
                }
                catch (Exception ex) { Debug.LogError(ex); }
        }
        private void Reset_Modules()
        {
            foreach (var module in _modules.Values)
                try
                {
                    module.Reset();
                }
                catch (Exception ex) { Debug.LogError(ex); }
        }
        private void Destroy_Modules()
        {
            foreach (var module in _modules.Values)
                try
                {
                    module.Destroy();
                }
                catch (Exception ex) { Debug.LogError(ex); }
        }
        public void DoInit(string last, string state)
        {
            Reset_Modules();
            switch (CustomTemplateUtils.GetHandlerType(Handler))
            {
                case PeakTemplateType.Character:
                    {
                        if (CharacterDummy != null)
                        {
                            Do_Dummy_Init();
                        }
                        else
                        {
                            Do_Character_Init();
                        }
                    }
                    break;
                    //TODO Maybe Make a Chicken_Init, with specific Modules <-- IE Peak_Human, Peak_Chicken
                    //Instead of a bool check? I dunno. Could be worth looking into.
                    //Also, Consider if it's worth spawning chicken template on Own Character...
                case PeakTemplateType.Chicken:
                    Do_Character_Init();
                    break;
                    //TODO Other Template Types?
            }
            Init_Modules();
        }
        public void OnUpdate()
        {
            Update_Modules();
        }
        public void OnDestroy()
        {
            Debug.LogWarning("PlayerHelper OnDestroy");
            Reset_Modules();
            Destroy_Modules();
        }
        public void Do_Character_Init()
        {
            RegisterModule<Peak_LayerFix>();
            //
            RegisterModule<Peak_HideTheBody>();
            RegisterModule<Peak_SkinToneHelper>();
            //
            RegisterModule<Peak_SteamAvatar>();
            RegisterModule<Peak_TextureHelper>();
            RegisterModule<Peak_TextureSwapper>();
            //
            RegisterModule<Peak_Constraints>();
            //
            RegisterModule<Peak_ObjectToggler>();
            //TweenStuff?
            RegisterModule<Peak_Chicken>();
            RegisterModule<Peak_PulseStatus>();
        }
        public void Do_Dummy_Init()
        {
            RegisterModule<Peak_LayerFix>();
            //
            //RegisterModule<Peak_HideTheBody>();
            RegisterModule<Peak_SkinToneHelper>();
            //
            RegisterModule<Peak_SteamAvatar>();
            RegisterModule<Peak_TextureHelper>();
            RegisterModule<Peak_TextureSwapper>();
            //
            RegisterModule<Peak_Constraints>();
            RegisterModule<Peak_ObjectToggler>();
        }
    }
}