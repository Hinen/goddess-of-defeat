using System;
using UnityEngine;

namespace Game.Bones {
    public class SpringBone : BoneBase {
        private enum PositionMixType {
            Additive,
            Mean,
            Override,
            None
        }
        
        [Serializable]
        public struct SpringBoneData {
            public float Mass;
            public float Stiffness;
            public float Damping;
        }

        [SerializeField]
        private PositionMixType positionMixType;
        
        [SerializeField]
        private SpringBoneData mainSpringData = new() {
            Mass = 10f,
            Stiffness = 200f,
            Damping = 40f
        };
        
        [SerializeField]
        private SpringBoneData subSpringData = new() {
            Mass = 5f,
            Stiffness = 100f,
            Damping = 20f
        };
        
        private Vector3 _setupPosition;
        private Vector3 _springPosition;
        
        private Vector3 _velocity;
        private Vector3 _diff;

        private void Start() {
            _setupPosition = _springPosition = SkeletonPosition;
        }

        protected override void LateUpdate() {
            if (positionMixType == PositionMixType.Additive)
                SkeletonPosition += _diff;
            else if (positionMixType == PositionMixType.Mean)
                SkeletonPosition = Vector3.Lerp(SkeletonPosition, _springPosition, 0.5f);
            else if (positionMixType == PositionMixType.Override)
                SkeletonPosition = _springPosition;

            base.LateUpdate();
        }

        public void ApplySpringForcePosition(bool isMain, Vector3 skeletonPositionDiff) {
            var springData = isMain ? mainSpringData : subSpringData;
            var oldPosition = OldSkeletonPosition;
            var followPosition = oldPosition + skeletonPositionDiff;
            
            var displacement = followPosition - _setupPosition;
            var springForce = -springData.Stiffness * displacement;
            var dampingForce = springData.Damping * _velocity;
            var force = springForce - dampingForce;
            var acceleration = springData.Mass != 0f ? force / springData.Mass : force;

            _velocity += acceleration * Time.deltaTime;
            var newPosition = oldPosition + _velocity * Time.deltaTime;
            _springPosition = newPosition;
            
            _diff = newPosition - oldPosition;
            ApplySpringForcePositionToChildren(_diff);
        }
    }
}
