using Core;
using UnityEngine;

namespace Game.Bones {
    public class BoneBase : MonoBehaviour {
        private Skeleton _skeleton;
        
        public virtual Vector3 SkeletonPosition { get; protected set; }

        protected virtual void Awake() {
            _skeleton = GetComponentInParent<Skeleton>();
            SkeletonPosition = ToSkeletonSpace(transform.position);
        }
        
        public Vector3 ToSkeletonSpace(Vector3 worldPosition) {
            return _skeleton.transform.InverseTransformPoint(worldPosition);
        }
        
        public Vector3 ToWorldSpace(Vector3 skeletonPosition) {
            return _skeleton.transform.TransformPoint(skeletonPosition);
        }
    }
}