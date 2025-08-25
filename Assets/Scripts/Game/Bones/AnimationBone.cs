using Core;
using UnityEngine;

namespace Game.Bones {
    public class AnimationBone : BoneBase {
        protected override Constants.BoneType BoneType => Constants.BoneType.Animation;
        protected override Color CircleColor => Color.blue;
        
        private Vector3 _oldSkeletonPosition;
        public Vector3 Delta { get; private set; }

        private void Update() {
            _oldSkeletonPosition = ToSkeletonSpace(transform.position);
        }
        
        protected override void LateUpdate() {
            Delta = ToSkeletonSpace(transform.position) - _oldSkeletonPosition;
            SkeletonPosition += Delta;
            
            base.LateUpdate();
        }
    }
}