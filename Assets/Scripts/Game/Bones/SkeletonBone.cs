using Core;
using UnityEngine;

namespace Game.Bones {
    public class SkeletonBone : BoneBase {
        public enum PositionMixType {
            Additive,
            Mean,
            Override,
            None
        }
        
        [SerializeField]
        private PositionMixType positionMixType;
        
        protected override Constants.BoneType BoneType => Constants.BoneType.Skeleton;
        
        private AnimationBone _animationBone;
        private SpringBone _springBone;
        
        public override Vector3 Position {
            protected set {
                base.Position = value;
                transform.position = value;
            }
        }

        protected override void Awake() {
            base.Awake();
            
            _animationBone = GetComponent<AnimationBone>();
            _springBone = GetComponent<SpringBone>();
        }
        
        protected override void LateUpdate() {
            if (positionMixType == PositionMixType.Additive)
                Position = _animationBone.Position + _springBone.SetupOffset;
            else if (positionMixType == PositionMixType.Mean)
                Position = Vector3.Lerp(_springBone.Position, _animationBone.Position, 0.5f);
            else if (positionMixType == PositionMixType.Override)
                Position = _springBone.Position;
            else if (positionMixType == PositionMixType.None)
                Position = _animationBone.Position;
            
            base.LateUpdate();
        }
    }
}