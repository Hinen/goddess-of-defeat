using Core;
using UnityEngine;

namespace Game.Bones {
    public class AnchorBone : BoneBase {
        protected override Color GetCircleRendererColor() {
            return Color.blue;
        }
        
        protected override void LateUpdate() {
            var skeletonPositionDiff = SkeletonPosition - OldSkeletonPosition;
            
            ApplySpringForcePositionToChildren(skeletonPositionDiff);
            base.LateUpdate();
        }
    }
}
