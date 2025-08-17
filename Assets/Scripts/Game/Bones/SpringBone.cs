using System;
using Core;
using UnityEngine;

namespace Game.Bones {
    public class SpringBone : BoneBase {
        [Serializable]
        public struct SpringBoneData {
            public float Mass;
            public float Stiffness;
            public float Damping;
        }
        
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
        private Vector3 _velocity;

        private void Start() {
            _setupPosition = SkeletonPosition;
        }
        
        public void ApplySpringForcePosition(bool isMain, Vector3 skeletonPositionDiff) {
            var springData = isMain ? mainSpringData : subSpringData;
            var oldPosition = SkeletonPosition;
            var followPosition = oldPosition + skeletonPositionDiff;
            
            var displacement = followPosition - _setupPosition;
            var springForce = -springData.Stiffness * displacement;
            var dampingForce = springData.Damping * _velocity;
            var force = springForce - dampingForce;
            var acceleration = springData.Mass != 0f ? force / springData.Mass : force;

            _velocity += acceleration * Time.deltaTime;
            var newPosition = oldPosition + _velocity * Time.deltaTime;
            var worldPosition = newPosition.ToWorldSpace(Skeleton);
            transform.position = worldPosition;
            
            var diff = newPosition - oldPosition;
            ApplySpringForcePositionToChildren(diff);
        }
    }
}
