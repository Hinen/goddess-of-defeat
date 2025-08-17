using System;
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
            Damping = 30f
        };
        
        [SerializeField]
        private SpringBoneData subSpringData = new() {
            Mass = 5f,
            Stiffness = 100f,
            Damping = 15f
        };
        
        private Vector3 _setupPosition;
        private Vector3 _velocity;

        private void Start() {
            _setupPosition = transform.localPosition;
        }
        
        public void ApplySpringForcePosition(bool isMain, Vector3 positionDiff) {
            var springData = isMain ? mainSpringData : subSpringData;
            var oldPosition = transform.localPosition;
            
            var position = oldPosition + positionDiff;
            var displacement = position - _setupPosition;
            var springForce = -springData.Stiffness * displacement;
            var dampingForce = springData.Damping * _velocity;
            var force = springForce - dampingForce;
            var acceleration = springData.Mass != 0f ? force / springData.Mass : force;

            _velocity += acceleration * Time.deltaTime;
            position += _velocity * Time.deltaTime;
            transform.localPosition = position;
            
            var diff = position - oldPosition;
            ApplySpringForcePositionToChildren(diff);
        }
    }
}
