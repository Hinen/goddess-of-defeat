using System;
using System.Collections.Generic;
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
        
        [SerializeField]
        private SkeletonBone[] mainParent;
        
        [SerializeField]
        private SkeletonBone[] subParent;

        private readonly Dictionary<SkeletonBone, Vector3> _setupParentDistance = new();
        private Vector3 _setupSkeletonPosition;
        
        private Vector3 _velocity;
        public Vector3 Delta { get; private set; }

        protected override void Awake() {
            base.Awake();
            InitChildBoneLineRenderer();
        }
        
        public void Start() {
            foreach (var parent in mainParent)
                _setupParentDistance[parent] = parent.SkeletonPosition - SkeletonPosition;
            
            foreach (var parent in subParent)
                _setupParentDistance[parent] = parent.SkeletonPosition - SkeletonPosition;
        }

        private void InitChildBoneLineRenderer() {
            foreach (var parent in mainParent)
                CreateLine(parent, Color.green);
            
            foreach (var parent in subParent)
                CreateLine(parent, Color.yellow);
            
            return;
            void CreateLine(BoneBase parent, Color color) {
                var boneLineRenderer = Resources.Load<BoneLineRenderer>("Prefabs/BoneLineRenderer");
                var line = Instantiate(boneLineRenderer, transform);
                line.Init(this, parent, color);
            }
        }

        private void LateUpdate() {
            Delta = Vector3.zero;
            
            foreach (var parentBone in mainParent)
                ApplySpringForcePosition(true, parentBone);

            foreach (var parentBone in subParent)
                ApplySpringForcePosition(false, parentBone);
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
