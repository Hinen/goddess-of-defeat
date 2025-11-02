using System.Collections.Generic;
using UnityEngine;

namespace Game.Bones {
    public class SkeletonBone : MonoBehaviour {
        public enum PositionMixType {
            Additive,
            Mean,
            Override,
            None
        }
        
        [SerializeField]
        private PositionMixType positionMixType;

        private AnimationBoneManager _animationBoneManager;
        private SpringBone _springBone;
        private AnimationBone AnimationBone => _animationBoneManager.Mapping.GetValueOrDefault(gameObject);
        
        public Vector3 Position {
            set => transform.position = value;
        }

        private void Awake() {
            _animationBoneManager = GetComponentInParent<AnimationBoneManager>();
            _springBone = GetComponent<SpringBone>();
        }

        private void LateUpdate() {
            if (positionMixType == PositionMixType.Additive)
                Position = AnimationBone.Position + _springBone.SetupOffset;
            else if (positionMixType == PositionMixType.Mean)
                Position = Vector3.Lerp(_springBone.Position, AnimationBone.Position, 0.5f);
            else if (positionMixType == PositionMixType.Override)
                Position = _springBone.Position;
            else if (positionMixType == PositionMixType.None)
                Position = AnimationBone.Position;
        }
    }
}