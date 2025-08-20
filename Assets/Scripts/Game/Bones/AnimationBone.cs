using UnityEngine;

namespace Game.Bones {
    public class AnimationBone : BoneBase {
        private Vector3 _oldSkeletonPosition;
        public Vector3 AnimationDelta => SkeletonPosition - _oldSkeletonPosition;

        protected override void Awake() {
            base.Awake();
            
            _oldSkeletonPosition = SkeletonPosition = ToSkeletonSpace(transform.position);
        }
        
        private void Update() {
            _oldSkeletonPosition = SkeletonPosition;
        }
        
        private void LateUpdate() {
            SkeletonPosition = ToSkeletonSpace(transform.position);
            transform.position = ToWorldSpace(_oldSkeletonPosition);
        }
    }
}