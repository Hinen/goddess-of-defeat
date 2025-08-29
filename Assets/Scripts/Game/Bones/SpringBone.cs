using System;
using System.Collections.Generic;
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
        
        protected override Constants.BoneType BoneType => Constants.BoneType.Spring;
        protected override Color CircleColor => Color.yellow;

        private readonly Dictionary<SkeletonBone, Vector3> _setupParentDistance = new();
        private Vector3 _setupSkeletonPosition;
        
        private Vector3 _velocity;
        public Vector3 Delta { get; private set; }
        
        private void Start() {
            foreach (var parent in BoneParentConnector.mainParent)
                _setupParentDistance[parent] = parent.SkeletonPosition - SkeletonPosition;
            
            foreach (var parent in BoneParentConnector.subParent)
                _setupParentDistance[parent] = parent.SkeletonPosition - SkeletonPosition;
        }

        protected override void InitParentBoneLineRenderer() {
            base.InitParentBoneLineRenderer();
            
            foreach (var parent in BoneParentConnector.subParent)
                CreateLine(parent, new Color(1f, 0.5f, 0f));
        }

        protected override void LateUpdate() {
            Delta = Vector3.zero;
            
            foreach (var parentBone in BoneParentConnector.mainParent)
                ApplySpringForcePosition(true, parentBone);

            foreach (var parentBone in BoneParentConnector.subParent)
                ApplySpringForcePosition(false, parentBone);
            
            base.LateUpdate();
        }

        private void ApplySpringForcePosition(bool isMain, SkeletonBone parentBone) {
            var springData = isMain ? mainSpringData : subSpringData;
            
            var originDistance = _setupParentDistance[parentBone];
            var currentDistance = parentBone.SkeletonPosition - SkeletonPosition;
      
            var displacement = originDistance - currentDistance;
            var springForce = -springData.Stiffness * displacement;
            var dampingForce = springData.Damping * _velocity;
            var force = springForce - dampingForce;
            var acceleration = springData.Mass != 0f ? force / springData.Mass : force;
            _velocity += acceleration * Time.deltaTime;
            
            var delta = _velocity * Time.deltaTime;
            Delta += delta;
            SkeletonPosition += delta;
        }
    }
}
