using System;
using System.Collections.Generic;
using Core;
using Game.Bones.Job;
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
        
        private Vector3 _setupSkeletonPosition;
        private Vector3 _velocity;

        public Vector3 SetupOffset { get; private set; }

        private void Start() {
            if (!IsAnchorBone)
                BoneParentConnector.Init(this);
            
            _setupSkeletonPosition = ToSkeletonPosition(Position);
        }

        protected override void InitParentBoneLineRenderer() {
            base.InitParentBoneLineRenderer();
            
            foreach (var parent in BoneParentConnector.subParents)
                CreateLine(parent, new Color(1f, 0.5f, 0f));
        }

        protected override void LateUpdate() {
            if (!IsAnchorBone)
                BoneParentConnector.DivideFromMainParent();
            
            SetupOffset = ToSkeletonPosition(Position) - _setupSkeletonPosition;
            base.LateUpdate();
        }
        
        public void ApplyJobResult(Vector3 velocity, Vector3 position) {
            _velocity = velocity;
            Position = position;
        }
        
        public MainSpringBoneAccess GetMainSpringBoneAccess() {
            return new MainSpringBoneAccess {
                Data = mainSpringData,
                SetupParentDistance = BoneParentConnector.SetupParentBoneDistances[BoneParentConnector.mainParent],
                ParentPosition = BoneParentConnector.mainParent.GetBone(this).Position,
                Velocity = _velocity,
                SubSpringBoneCount = SubParentCount,
                Position = Position
            };
        }
        
        public SubSpringBoneAccess GetSubSpringBoneAccess(int subIndex) {
            return new SubSpringBoneAccess {
                Data = subSpringData,
                SetupParentDistance = BoneParentConnector.SetupParentBoneDistances[BoneParentConnector.subParents[subIndex]],
                ParentPosition = BoneParentConnector.subParents[subIndex].GetBone(this).Position,
            };
        }
    }
}
