using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Bones {
    public class SkeletonBone : MonoBehaviour {
        private struct SetupData {
            public Vector3 SkeletonPosition;
            public Quaternion Rotation;
        }
        
        public enum PositionMixType {
            Additive,
            Mean,
            Override,
            None
        }
        
        [SerializeField]
        private PositionMixType positionMixType;
        
        [SerializeField]
        private bool isApplyRotationToParentBone = true;

        private SetupData _parentSetupData;
        private SetupData _setupData;

        private Skeleton.Skeleton _skeleton;
        private AnimationBoneManager _animationBoneManager;
        private SpringBone _springBone;
        private AnimationBone AnimationBone => _animationBoneManager.Mapping.GetValueOrDefault(gameObject);
        private Vector3 ToSkeletonPosition(Vector3 worldPosition) => _skeleton.transform.InverseTransformPoint(worldPosition);
        
        public Vector3 Position {
            set => transform.position = value;
        }

        private void Awake() {
            _skeleton = GetComponentInParent<Skeleton.Skeleton>();
            _animationBoneManager = _skeleton.GetComponent<AnimationBoneManager>();
            _springBone = GetComponent<SpringBone>();

            _parentSetupData = new SetupData {
                SkeletonPosition = ToSkeletonPosition(transform.parent.position),
                Rotation = transform.parent.rotation
            };
            
            _setupData = new SetupData {
                SkeletonPosition = ToSkeletonPosition(transform.position),
                Rotation = transform.rotation
            };
        }

        private void LateUpdate() {
            var position = positionMixType switch {
                PositionMixType.Additive => AnimationBone.Position + _springBone.SetupOffset,
                PositionMixType.Mean => Vector3.Lerp(_springBone.Position, AnimationBone.Position, 0.5f),
                PositionMixType.Override => _springBone.Position,
                PositionMixType.None => AnimationBone.Position,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (isApplyRotationToParentBone && positionMixType != PositionMixType.None)
                ParentBoneRotation(ToSkeletonPosition(position));
            
            Position = position;
        }
        
        private void ParentBoneRotation(Vector3 skeletonPosition) {
            var setupDirection = _setupData.SkeletonPosition - _parentSetupData.SkeletonPosition;
            var setupRotation = Quaternion.Inverse(_parentSetupData.Rotation) * setupDirection;
            var setupAngle = Mathf.Atan2(setupRotation.y, setupRotation.x) * Mathf.Rad2Deg;
            
            // current
            var direction = skeletonPosition - _parentSetupData.SkeletonPosition;
            var rotation = Quaternion.Inverse(_parentSetupData.Rotation) * direction;
            var angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            var delta = Mathf.DeltaAngle(setupAngle, angle);
            transform.parent.rotation = _parentSetupData.Rotation * Quaternion.Euler(0f, 0f, delta);
        }
    }
}