using System;
using System.Collections.Generic;
using UnityEngine;
namespace UCustomPrefabsAPI.RuntimeExtras
{
    public enum ConstraintTemplatePathingType
    {
        ContainerChildren,
        AllChildren,
        LastChildren,
        FirstChildren,
        Manual
    }
    [Serializable]
    public enum TargetMethod
    {
        Directory,
        Name
    }
    [Serializable]
    public class ConstraintTemplate : MonoBehaviour
    {
        public Transform TargetObject;
        public string TargetPath = string.Empty;
        public string TargetMethod_data;
        public List<string> Paths = new();
        public List<Vector3> Positions = new();
        public List<Vector3> Scales = new();
        public List<Quaternion> Rotations = new();
        public static void ApplyOffsets(Transform origin, Transform target, in Vector3 pos, in Quaternion rot, in Vector3 scale)
        {
            origin.position = target.position + target.rotation * pos;
            origin.rotation = target.rotation * rot;
            Vector3 targetScale = target.lossyScale;
            Vector3 desiredWorldScale = new Vector3(
                targetScale.x * scale.x,
                targetScale.y * scale.y,
                targetScale.z * scale.z
            );
            Vector3 parentScale = origin.parent.lossyScale;
            origin.localScale = new Vector3(
                desiredWorldScale.x / parentScale.x,
                desiredWorldScale.y / parentScale.y,
                desiredWorldScale.z / parentScale.z
            );
        }
        public struct BakedConstraintTemplate
        {
            public ConstraintTemplate template;
            public Transform[] followers;
            public Transform origin;
            public Transform target;
            //Restore
            public Vector3[] pos;
            public Quaternion[] rot;
            public Vector3[] scl;
            public BakedConstraintTemplate(ConstraintTemplate template, Transform origin, Transform target)
            {
                this.template = template;
                followers = default;
                this.origin = origin;
                this.target = target;
                pos = default;
                rot = default;
                scl = default;
                ReAttach(origin, target);
            }
            public void ReAttach(Transform origin, Transform target)
            {
                this.origin = origin;
                this.target = target;
                FindFollowers();
            }
            private void FindFollowers()
            {
                followers = new Transform[template.Paths.Count];
                pos = new Vector3[template.Paths.Count];
                rot = new Quaternion[template.Paths.Count];
                scl = new Vector3[template.Paths.Count];
                for (int i = 0; i < followers.Length; i++)
                {
                    var follower = origin.Find(template.Paths[i]);
                    followers[i] = follower;
                    if (follower)
                    {
                        pos[i] = follower.localPosition;
                        rot[i] = follower.localRotation;
                        scl[i] = follower.localScale;
                    }
                }
            }
            public void UpdateFollowers()
            {
                for (int i = 0; i < followers.Length; i++)
                {
                    var follower = followers[i];
                    if (!follower)
                        continue;
                    ApplyOffsets(follower, target, template.Positions[i], template.Rotations[i], template.Scales[i]);
                }
            }
            public void RestoreFollowers()
            {
                for (int i = 0; i < followers.Length; i++)
                {
                    var follower = followers[i];
                    if (!follower)
                        continue;
                    follower.localPosition = pos[i];
                    follower.localRotation = rot[i];
                    follower.localScale = scl[i];
                }
            }
        }
    }
}