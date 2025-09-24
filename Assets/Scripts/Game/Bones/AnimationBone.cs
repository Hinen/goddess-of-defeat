using Core;
using Game.DebugTools;
using UnityEngine;

namespace Game.Bones {
    public class AnimationBone : BoneBase {
        protected override Constants.BoneType BoneType => Constants.BoneType.Animation;
        protected override Color CircleColor => Color.blue;
        
        private Vector3 _oldTransformPosition;

        private void Update() {
            _oldTransformPosition = transform.position;
        }
        
        protected override void LateUpdate() {
            var delta = transform.position - _oldTransformPosition;
            Position += delta;
            
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