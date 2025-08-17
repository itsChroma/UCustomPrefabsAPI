using System.Collections.Generic;
using UnityEngine;
using UCustomPrefabsAPI.Extras.Utility;
using static UCustomPrefabsAPI.RuntimeExtras.MeshHider_Template;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public class MeshHider : CustomActionsBase
    {
        private static RenderingLayerMask Hidden_Mask = 0;
        Dictionary<Renderer, RenderingLayerMask> _renderers = new();
        HiderMethod HiderMethod = HiderMethod.Everything;
        List<string> MeshesHidden = new();
        bool AlwaysCheck = false;
        public override void RegisterActions()
        {
            AddOnStateChanged(Init);
            AddOnDestroy(Reset);
        }
        public override void HandleTemplateData(object[] data)
        {
            //Convert Template Data Over <--
            HiderMethod = (HiderMethod)data[0];
            MeshesHidden = (List<string>)data[1];
            AlwaysCheck = (bool)data[2];
        }
        public void Init(string last, string state)
        {
            RegisterRenderers();
            HideRenderers();
        }
        public void Reset()
        {
            ResetRenderers();
        }
        //TODO Ensure we aren't targeting Template Renderers!
        public void RegisterRenderers()
        {
            switch (HiderMethod)
            {
                case HiderMethod.Directory:
                    {
                        foreach (string path in MeshesHidden)
                        {
                            var transform = Handler.transform.Find(path);
                            if (!transform)
                                continue;
                            var renderer = transform.GetComponent<Renderer>();
                            if (!renderer)
                                continue;
                            //Add it in...
                            _renderers[renderer] = renderer.renderingLayerMask;
                        }
                    }
                    break;
                case HiderMethod.Name:
                    {
                        var children = new Dictionary<string, Transform>();
                        SearchUtils.IterativelyCollectChildNames(Handler.transform, ref children);
                        foreach (string name in MeshesHidden)
                        {
                            if (!children.TryGetValue(name, out Transform child))
                                continue;
                            var renderer = child.GetComponent<Renderer>();
                            if (!renderer)
                                continue;
                            //Add it in...
                            _renderers[renderer] = renderer.renderingLayerMask;
                        }
                    }
                    break;
                case HiderMethod.ChildrenOfDirectory:
                    {
                        foreach (string path in MeshesHidden)
                        {
                            var transform = Handler.transform.Find(path);
                            if (!transform)
                                continue;
                            var renderers = transform.GetComponents<Renderer>();
                            foreach (var renderer in renderers)
                            {
                                //Add it in...
                                _renderers[renderer] = renderer.renderingLayerMask;
                            }
                        }
                    }
                    break;
                case HiderMethod.ChildrenOfName:
                    {
                        var children = new Dictionary<string, Transform>();
                        SearchUtils.IterativelyCollectChildNames(Handler.transform, ref children);
                        foreach (string name in MeshesHidden)
                        {
                            if (!children.TryGetValue(name, out Transform child))
                                continue;
                            var renderers = child.GetComponents<Renderer>();
                            foreach (var renderer in renderers)
                            {
                                //Add it in...
                                _renderers[renderer] = renderer.renderingLayerMask;
                            }
                        }
                    }
                    break;
                case HiderMethod.Everything:
                    {
                        //May need to place a priority for starting this before others//
                        var renderers = Handler.transform.GetComponentsInChildren<Renderer>(true);
                        foreach (var renderer in renderers)
                        {
                            //Add it in...
                            _renderers[renderer] = renderer.renderingLayerMask;
                        }
                    }
                    break;
                case HiderMethod.SearchUtilTokens:
                    {
                        Debug.LogWarning("SearchUtilTokens Still TODO, This will be Ignored.");
                    }
                    break;
                default:
                    {
                        Debug.LogWarning("Unknown HiderMethod...");
                    }
                    break;
            }
        }
        private Stack<Renderer> _InvalidKeys = new();
        public void DoPostValidation()
        {
            PurgeInvalidRenderers();
        }
        public void PurgeInvalidRenderers()
        {
            while (_InvalidKeys.Count > 0)
                _renderers.Remove(_InvalidKeys.Pop());
        }
        public bool ValidateRenderer(Renderer renderer)
        {
            if (renderer != null && renderer)
                return true;
            _InvalidKeys.Push(renderer);
            return false;
        }
        public void HideRenderers()
        {
            foreach (var pair in _renderers)
            {
                if (ValidateRenderer(pair.Key))
                    pair.Key.renderingLayerMask = Hidden_Mask;
            }
            DoPostValidation();
        }
        public void ResetRenderers()
        {
            foreach (var pair in _renderers)
            {
                if (ValidateRenderer(pair.Key))
                    pair.Key.renderingLayerMask = pair.Value;
            }
            DoPostValidation();
            _renderers.Clear();
        }
        public void Update()
        {
            //TODO Constant Update Enabled//
        }
    }
}
