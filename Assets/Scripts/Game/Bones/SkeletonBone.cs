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

        private BoneChain _boneChain;
        private SpringBone _springBone;
        private AnimationBone AnimationBone => _boneChain.Foo[gameObject];
        
        public Vector3 Position {
            set => transform.position = value;
        }

        private void Awake() {
            _boneChain = GetComponentInParent<BoneChain>();
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