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
            Damping = 40f
        };
        
        [SerializeField]
        private SpringBoneData subSpringData = new() {
            Mass = 5f,
            Stiffness = 100f,
            Damping = 20f
        };
        
        [SerializeField]
        private AnimationBone[] mainParent;
        
        [SerializeField]
        private AnimationBone[] subParent;
        
        private AnimationBone _animationBone;
        private Vector3 _setupSkeletonPosition;
        
        private Vector3 _velocity;
        public Vector3 TotalDiff { get; private set; }

        protected override void Awake() {
            base.Awake();
            
            _animationBone = GetComponent<AnimationBone>();
            _setupSkeletonPosition = SkeletonPosition;
            InitChildBoneLineRenderer();
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
            foreach (var parentBone in mainParent)
                ApplySpringForcePosition(true, parentBone.AnimationDelta);
        }

        private void ApplySpringForcePosition(bool isMain, Vector3 animationDelta) {
            var springData = isMain ? mainSpringData : subSpringData;
            var oldPosition = _animationBone.SkeletonPosition; // 변위 계산은 애니메이션 본의 위치를 기준으로 삼음
            var followPosition = oldPosition + animationDelta;
            
            var displacement = followPosition - _setupSkeletonPosition;
            var springForce = -springData.Stiffness * displacement;
            var dampingForce = springData.Damping * _velocity;
            var force = springForce - dampingForce;
            var acceleration = springData.Mass != 0f ? force / springData.Mass : force;

            _velocity += acceleration * Time.deltaTime;
            var delta = _velocity * Time.deltaTime;
            TotalDiff += delta;
            SkeletonPosition = oldPosition + delta;
        }
    }
}
