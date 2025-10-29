using System;
using Core;
using Game.Bones;
using UnityEngine;

namespace Game.OldBones {
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
        
        private AnimationBone _animationBone;
        private Vector3 _setupSkeletonPosition;
        private Vector3 _velocity;

        public Vector3 SetupOffset { get; private set; }

        private void Start() {
            if (!IsAnchorBone)
                BoneParentConnector.Init(this);

            _animationBone = GetComponent<AnimationBone>();
            _setupSkeletonPosition = SkeletonPosition;
        }

        protected override void InitParentBoneLineRenderer() {
            base.InitParentBoneLineRenderer();
            
            foreach (var parent in BoneParentConnector.subParents)
                CreateLine(parent, new Color(1f, 0.5f, 0f));
        }

        protected override void LateUpdate() {
            if (!IsAnchorBone)
                BoneParentConnector.DivideFromMainParent();
            
            SetupOffset = SkeletonPosition - _setupSkeletonPosition;
            base.LateUpdate();
        }
        
        public void ApplyJobResult(Vector3 velocity, Vector3 result) {
            _velocity = velocity;
            Position += result;
        }
        
        /*
        public MainSpringBoneAccess GetMainSpringBoneAccess() {
            var isFoo = BoneParentConnector.mainParent.GetBone(this).IsAnchorBone;
            //isFoo = false;
            
            return new MainSpringBoneAccess {
                Data = mainSpringData,
                SetupParentSkeletonPositionDistance = 
                    isFoo ? BoneParentConnector.mainParent.GetBone(this).SkeletonPosition - _animationBone.SkeletonPosition
                        : BoneParentConnector.SetupParentBoneSkeletonPositionDistances[BoneParentConnector.mainParent],
                ParentSkeletonPosition = BoneParentConnector.mainParent.GetBone(this).SkeletonPosition,
                SubSpringBoneCount = SubParentCount,
                SkeletonPosition = SkeletonPosition,
                Velocity = _velocity
            };
        }
        
        public SubSpringBoneAccess GetSubSpringBoneAccess(int subIndex) {
            return new SubSpringBoneAccess {
                Data = subSpringData,
                SetupParentSkeletonPositionDistance = BoneParentConnector.SetupParentBoneSkeletonPositionDistances[BoneParentConnector.subParents[subIndex]],
                ParentSkeletonPosition = BoneParentConnector.subParents[subIndex].GetBone(this).SkeletonPosition,
            };
        }
        */
    }
}
