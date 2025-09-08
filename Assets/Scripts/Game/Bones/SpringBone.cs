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
        
        private Vector3? _oldSkeletonPosition;
        public Vector3 SkeletonPositionDelta { get; private set; }
        
        private void Start() {
            if (!IsAnchorBone)
                BoneParentConnector.Init(this);
        }

        protected override void InitParentBoneLineRenderer() {
            base.InitParentBoneLineRenderer();
            
            foreach (var parent in BoneParentConnector.subParents)
                CreateLine(parent, new Color(1f, 0.5f, 0f));
        }

        protected override void LateUpdate() {
            if (!IsAnchorBone)
                BoneParentConnector.DivideFromMainParent();
            
            if (_oldSkeletonPosition != null)
                SkeletonPositionDelta = SkeletonPosition - _oldSkeletonPosition.Value;

            _oldSkeletonPosition = SkeletonPosition;
            base.LateUpdate();
        }
        
        public void ApplyJobResult(Vector3 velocity, Vector3 skeletonPosition) {
            _velocity = velocity;
            SkeletonPosition = skeletonPosition;
        }
        
        public MainSpringBoneAccess GetMainSpringBoneAccess() {
            return new MainSpringBoneAccess {
                Data = mainSpringData,
                SetupParentDistance = BoneParentConnector.SetupParentBoneDistances[BoneParentConnector.mainParent],
                ParentSkeletonPosition = BoneParentConnector.mainParent.GetBone(this).SkeletonPosition,
                Velocity = _velocity,
                SubSpringBoneCount = SubParentCount,
                SkeletonPosition = SkeletonPosition
            };
        }
        
        public SubSpringBoneAccess GetSubSpringBoneAccess(int subIndex) {
            return new SubSpringBoneAccess {
                Data = subSpringData,
                SetupParentDistance = BoneParentConnector.SetupParentBoneDistances[BoneParentConnector.subParents[subIndex]],
                ParentSkeletonPosition = BoneParentConnector.subParents[subIndex].GetBone(this).SkeletonPosition,
            };
        }
    }
}
