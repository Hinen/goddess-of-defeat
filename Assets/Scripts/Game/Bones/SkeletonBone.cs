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
        
        private Vector3 _additivePosition;
        
        public override Vector3 SkeletonPosition {
            get => ToSkeletonSpace(transform.position);
            protected set {
                base.SkeletonPosition = value;
                transform.position = ToWorldSpace(value);
            }
        }

        protected override void Awake() {
            base.Awake();
            
            _animationBone = GetComponent<AnimationBone>();
            _springBone = GetComponent<SpringBone>();
            _additivePosition = SkeletonPosition;
        }

        protected override void LateUpdate() {
            if (_springBone != null)
                _additivePosition += _animationBone.Delta + _springBone.Delta;

            if (positionMixType == PositionMixType.Additive)
                SkeletonPosition = _additivePosition;
            else if (positionMixType == PositionMixType.Mean)
                SkeletonPosition = Vector3.Lerp(_springBone.SkeletonPosition, _animationBone.SkeletonPosition, 0.5f);
            else if (positionMixType == PositionMixType.Override)
                SkeletonPosition = _springBone.SkeletonPosition;
            else if (positionMixType == PositionMixType.None)
                SkeletonPosition = _animationBone.SkeletonPosition;

            base.LateUpdate();
        }
    }
}