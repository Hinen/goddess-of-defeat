using UnityEngine;

namespace Game.Bones {
    public class AnimationBone : BoneBase {
        private Vector3 _oldSkeletonPosition;

        private void Update() {
            _oldSkeletonPosition = ToSkeletonSpace(transform.position);
        }
        
        private void LateUpdate() {
            var delta = ToSkeletonSpace(transform.position) - _oldSkeletonPosition;
            SkeletonPosition += delta;
        }
    }
}