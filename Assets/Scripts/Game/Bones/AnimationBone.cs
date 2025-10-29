using UnityEngine;

namespace Game.Bones {
    public class AnimationBone : MonoBehaviour, ISpringBoneParent {
        private Skeleton _skeleton;
        
        public Vector3 Position => transform.position;
        public Vector3 SkeletonPosition => ToSkeletonPosition(Position);
        private Vector3 ToSkeletonPosition(Vector3 worldPosition) => _skeleton.transform.InverseTransformPoint(worldPosition);

        private void Awake() {
            _skeleton = GetComponentInParent<Skeleton>();
        }
    }
}