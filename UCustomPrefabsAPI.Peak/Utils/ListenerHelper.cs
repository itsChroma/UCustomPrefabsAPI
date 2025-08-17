using System;
using System.Collections.Generic;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak.ActionUtils
{
    //Consider using functions? Func void <- params []
    public class ListenerHelper<A, B> where A : class
    {
        private Dictionary<A, List<Action<B>>> listeners = new();
        public void Listen(A target, Action<B> callback)
        {
            if (target == null || callback == null)
                return;
            if (!listeners.TryGetValue(target, out var listenerList))
            {
                listenerList = new List<Action<B>>();
                listeners[target] = listenerList;
            }
            if (!listenerList.Contains(callback))
                listenerList.Add(callback);
        }
        public void Un_Listen(A target, Action<B> callback)
        {
            if (target == null || callback == null)
                return;
            if (!listeners.TryGetValue(target, out var listenerList))
                return;
            listenerList.Remove(callback);
        }
        public void Clear_Listeners()
        {
            listeners.Clear();
        }
        public void Invoke(A target, B args)
        {
            if (target == null)
                return;
            Validate_Listeners();
            if (!listeners.TryGetValue(target, out var listenerList))
                return;
            for (int i = 0; i < listenerList.Count; i++)
            {
                var listener = listenerList[i];
                if (listener != null)
                    try
                    {
                        listener.Invoke(args);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.LogWarning("This error can be ignored -->");
                        Debug.LogError(ex);
#endif
                        listenerList.RemoveAt(i--);
                    }
            }
            //Clean-up//
            if (listenerList.Count == 0)
                listeners.Remove(target);
        }
        public void Validate_Listeners()
        {
            bool rebuild = false;
            foreach (var key in listeners.Values)
            {
                if (key == null)
                {
                    rebuild = true;
                    break;
                }
            }
            if (!rebuild)
                return;
            Debug.LogWarning("Some Listeners have become invalid, Rebuilding Listeners.");
            Dictionary<A, List<Action<B>>> newListeners = new();
            foreach (var pair in listeners)
            {
                if (pair.Key == null)
                    continue;
                var listenerList = new List<Action<B>>();
                foreach (var listener in listenerList)
                {
                    if (listener == null)
                        continue;
                    listenerList.Add(listener);
                }
                newListeners[pair.Key] = listenerList;
            }
            listeners = newListeners;
        }
    }
}
