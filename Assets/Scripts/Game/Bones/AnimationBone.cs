using UnityEngine;

namespace Game.Bones {
    public class AnimationBone : BoneBase {
        private Vector3 _oldSkeletonPosition;
        public Vector3 Delta { get; private set; }

        private void Update() {
            _oldSkeletonPosition = ToSkeletonSpace(transform.position);
        }
        
        private void LateUpdate() {
            Delta = ToSkeletonSpace(transform.position) - _oldSkeletonPosition;
            SkeletonPosition += Delta;
        }
    }
}