using Core;
using UnityEngine;

namespace Game.Bones {
    public class AnchorBone : BoneBase {
        private Vector3 _oldSkeletonPosition;

        protected override Color GetCircleRendererColor() {
            return Color.blue;
        }

        public void Update() {
            _oldSkeletonPosition = SkeletonPosition;
        }
        
        protected override void LateUpdate() {
            var skeletonPositionDiff = SkeletonPosition - _oldSkeletonPosition;
            
            ApplySpringForcePositionToChildren(skeletonPositionDiff);
            base.LateUpdate();
        }
    }
}
