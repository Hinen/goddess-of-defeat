using Core;
using UnityEngine;

namespace Game.Bones {
    public class SkeletonBone : BoneBase {
        public enum PositionMixType {
            Additive,
            Mean,
            Override,
            None
        }

        [SerializeField]
        private Color circleColor = Color.red;
        
        [SerializeField]
        private PositionMixType positionMixType;
        
        private AnimationBone _animationBone;
        private SpringBone _springBone;

        private CircleRenderer _boneCircleRenderer;
        private Vector3 _additivePosition;

        public override Vector3 SkeletonPosition {
            get => ToSkeletonSpace(transform.position);
            protected set {
                base.SkeletonPosition = value;
                transform.position = ToWorldSpace(value);
            }
        }

        protected override void Awake() {
            base.Awake();
            
            _animationBone = GetComponent<AnimationBone>();
            _springBone = GetComponent<SpringBone>();
            InitCircleRenderer();
            _additivePosition = SkeletonPosition;
        }

        private void InitCircleRenderer() {
            var lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = Resources.Load<Material>("Materials/Sprites-Default");
            lineRenderer.startColor = circleColor;
            lineRenderer.endColor = circleColor;
            lineRenderer.widthMultiplier = 10f;
            lineRenderer.sortingLayerName = "Gizmo";
            
            _boneCircleRenderer = gameObject.AddComponent<CircleRenderer>();
            _boneCircleRenderer.lineRenderer = lineRenderer;
            _boneCircleRenderer.segments = 32;
            _boneCircleRenderer.radius = 25f;
        }

        private void LateUpdate() {
            if (_springBone != null)
                _additivePosition += _animationBone.Delta + _springBone.Delta;

            if (positionMixType == PositionMixType.Additive)
                SkeletonPosition = _additivePosition;
            else if (positionMixType == PositionMixType.Mean)
                SkeletonPosition = Vector3.Lerp(_springBone.SkeletonPosition, _animationBone.SkeletonPosition, 0.5f);
            else if (positionMixType == PositionMixType.Override)
                SkeletonPosition = _springBone.SkeletonPosition;
            else if (positionMixType == PositionMixType.None)
                SkeletonPosition = _animationBone.SkeletonPosition;

            _boneCircleRenderer.UpdateCircle(transform.position);
        }
    }
}