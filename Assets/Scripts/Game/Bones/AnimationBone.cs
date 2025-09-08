using Core;
using Game.DebugTools;
using UnityEngine;

namespace Game.Bones {
    public class AnimationBone : BoneBase {
        protected override Constants.BoneType BoneType => Constants.BoneType.Animation;
        protected override Color CircleColor => Color.blue;
        
        private Vector3 _oldSkeletonPosition;
        public Vector3 SkeletonPositionDelta { get; private set; }

        private void Update() {
            _oldSkeletonPosition = ToSkeletonSpace(transform.position);
        }
        
        protected override void LateUpdate() {
            SkeletonPositionDelta = ToSkeletonSpace(transform.position) - _oldSkeletonPosition;
            SkeletonPosition += SkeletonPositionDelta;
            
            base.LateUpdate();
        }

        protected override bool IsCircleRendererActive() {
            var active = base.IsCircleRendererActive();
            if (!active)
                active = BoneVisualizeToggle.BoneTypeToVisibility[Constants.BoneType.Spring] && IsAnchorBone;
            
            return active;
        }
    }
}