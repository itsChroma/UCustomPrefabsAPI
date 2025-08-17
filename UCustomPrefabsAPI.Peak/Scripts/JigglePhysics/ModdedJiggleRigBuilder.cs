using UnityEngine;
using System.Collections.Generic;
namespace JigglePhysics.Modding
{
    public class ModdedJiggleRigBuilder : JiggleRigBuilder
    {
        //Data to store...!
        [SerializeField, HideInInspector] private int _count = 0;
        [SerializeField, HideInInspector] private List<bool> _animated = new();
        [SerializeField, HideInInspector] private List<ScriptableObject> _settings = new();
        [SerializeField, HideInInspector] private List<Transform> _root = new();
        [SerializeField, HideInInspector] private List<Transform> _ignored = new();
        [SerializeField, HideInInspector] private List<int> _ignored_count = new();
        [SerializeField, HideInInspector] private List<Collider> _colliders = new();
        [SerializeField, HideInInspector] private List<int> _colliders_count = new();
        //Reflection, Should change accessibility of certain variables instead.
        //Only Required In Editor
        //private static FieldInfo _JiggleRig_ignoredTransforms = typeof(JiggleRig).GetField("ignoredTransforms", BindingFlags.NonPublic | BindingFlags.Instance);
        //private static FieldInfo _JiggleRig_colliders = typeof(JiggleRig).GetField("colliders", BindingFlags.NonPublic | BindingFlags.Instance);
        public void ResetData()
        {
            _count = 0;
            _animated.Clear();
            _settings.Clear();
            _root.Clear();
            _ignored.Clear();
            _ignored_count.Clear();
            _colliders.Clear();
            _colliders_count.Clear();
        }
        public void UpdateJiggleRigData(bool clearJiggleRigsData = false)
        {
            //Reset our stored Data//
            ResetData();
            _count = jiggleRigs.Count;
            //Check the JiggleRigs!
            foreach (var rig in jiggleRigs)
            {
                _animated.Add(rig.animated);
                _settings.Add(rig.jiggleSettings);
                _root.Add(rig.GetRootTransform());
                foreach (var transform in rig.ignoredTransforms)
                    _ignored.Add(transform);
                _ignored_count.Add(rig.ignoredTransforms.Count);
                foreach (var collider in rig.colliders)
                    _colliders.Add(collider);
                _colliders_count.Add(rig.colliders.Length);
            }
            //Just for cleanup...!
            if (clearJiggleRigsData)
                jiggleRigs.Clear();
        }
        public void ApplyJiggleRigData(bool clearStoredData = true)
        {
            if (_count == 0)
                return;
            //Make sure we can mess with the rigs in the first place!
            if (jiggleRigs == null)
                jiggleRigs = new List<JiggleRig>();
            //Make sure the rigs dont already exist, in the case of FixedSerialzation is used.
            else if (jiggleRigs.Count > 0)
            {
                if (clearStoredData)
                    ResetData();
                return;
            }
            //Working Indexes
            for (int i = 0, ign = 0, col = 0; i < _count; i++)
            {
                var ignoredList = new List<Transform>();
                for (int j = 0; j < _ignored_count[i]; j++)
                    ignoredList.Add(_ignored[ign + j]);
                //
                var colliderList = new List<Collider>();
                for (int j = 0; j < _colliders_count[i]; j++)
                    colliderList.Add(_colliders[ign + j]);
                //
                var newJiggleRig = new JiggleRig(_root[i], _settings[i] as JiggleSettings, ignoredList, colliderList);
                //
                newJiggleRig.animated = _animated[i];
                //
                jiggleRigs.Add(newJiggleRig);
                //Iterate our Working Indexes
                ign += _ignored_count[i]; //Ignored
                col += _colliders_count[i]; //Collision
            }
            Initialize();
            //Just for cleanup...!
            if (clearStoredData)
            {
                ResetData();
            }
        }
        public void Awake()
        {
            ApplyJiggleRigData();
        }
    }
}