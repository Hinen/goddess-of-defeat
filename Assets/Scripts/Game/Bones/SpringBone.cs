using System;
using System.Collections.Generic;
using Game.Bones.Job;
using UnityEngine;

namespace Game.Bones {
    public class SpringBone : MonoBehaviour, ISpringBoneParent {
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
        private GameObject mainParent;
        
        [SerializeField]
        private GameObject[] subParent;
        
        private struct ParentInfo {
            public ISpringBoneParent Parent;
            public Vector3 SetupSkeletonPositionDistance;
        }
        
        private ParentInfo mainSpringParentInfo;
        private ParentInfo[] subSpringParentInfos;
        public int SubParentCount => subParent.Length;

        private Skeleton.Skeleton _skeleton;
        private AnimationBoneManager _animationBoneManager;

        public Vector3 Position { get; private set; }
        private Vector3 _setupSkeletonPosition;
        
        private Vector3 _velocity;
        public Vector3 SetupOffset { get; private set; }
        
        public Vector3 SkeletonPosition => ToSkeletonPosition(Position);
        private Vector3 ToSkeletonPosition(Vector3 worldPosition) => _skeleton.transform.InverseTransformPoint(worldPosition);

        private void Awake() {
            _skeleton = GetComponentInParent<Skeleton.Skeleton>();
            _animationBoneManager = _skeleton.GetComponent<AnimationBoneManager>();
            
            Position = transform.position;
            _setupSkeletonPosition = SkeletonPosition;
        }

        private void Start() {
            mainSpringParentInfo = CreateParentInfo(mainParent);
            subSpringParentInfos = new ParentInfo[subParent.Length];
            for (var i = 0; i < subParent.Length; i++)
                subSpringParentInfos[i] = CreateParentInfo(subParent[i]);
        }

        private ParentInfo CreateParentInfo(GameObject parent) {
            var parentSpringBone = parent.GetComponent<SpringBone>();
            if (parentSpringBone != null)
                return new ParentInfo {
                    Parent = parentSpringBone,
                    SetupSkeletonPositionDistance = parentSpringBone.SkeletonPosition - SkeletonPosition
                };

            var animationBone = _animationBoneManager.Mapping.GetValueOrDefault(gameObject);
            if (animationBone == null)
                return default;

            return new ParentInfo {
                Parent = animationBone,
                SetupSkeletonPositionDistance = animationBone.SkeletonPosition - SkeletonPosition
            };
        }
        
        private void LateUpdate() {
            SetupOffset = SkeletonPosition - _setupSkeletonPosition;
        }

        public void ApplyJobResult(Vector3 velocity, Vector3 result) {
            _velocity = velocity;
            Position += result;
        }
        
        public MainSpringBoneAccess GetMainSpringBoneAccess() {
            return new MainSpringBoneAccess {
                Data = mainSpringData,
                SetupParentSkeletonPositionDistance = mainSpringParentInfo.SetupSkeletonPositionDistance,
                ParentSkeletonPosition = mainSpringParentInfo.Parent.SkeletonPosition,
                SubSpringBoneCount = SubParentCount,
                SkeletonPosition = SkeletonPosition,
                Velocity = _velocity
            };
        }
        
        public SubSpringBoneAccess GetSubSpringBoneAccess(int subIndex) {
            return new SubSpringBoneAccess {
                Data = subSpringData,
                SetupParentSkeletonPositionDistance = subSpringParentInfos[subIndex].SetupSkeletonPositionDistance,
                ParentSkeletonPosition = subSpringParentInfos[subIndex].Parent.SkeletonPosition
            };
        }
    }
}