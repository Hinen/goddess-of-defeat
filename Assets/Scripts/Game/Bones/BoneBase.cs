using Core;
using Game.DebugTools;
using UnityEngine;

namespace Game.Bones {
    public abstract class BoneBase : MonoBehaviour {
        protected abstract Constants.BoneType BoneType { get; }
        
        private Skeleton _skeleton;
        private CircleRenderer _boneCircleRenderer;
        protected BoneParentConnector BoneParentConnector;
        
        protected virtual Color CircleColor => Color.red;
        public virtual Vector3 SkeletonPosition { get; protected set; }
        public Vector3 WorldPosition => ToWorldSpace(SkeletonPosition);

        protected virtual void Awake() {
            _skeleton = GetComponentInParent<Skeleton>();
            SkeletonPosition = ToSkeletonSpace(transform.position);
            BoneParentConnector = GetComponent<BoneParentConnector>();

            InitCircleRenderer();
            InitParentBoneLineRenderer();
        }
        
        private void InitCircleRenderer() {
            var boneCircleRenderer = Resources.Load<CircleRenderer>("Prefabs/BoneCircleRenderer");
            _boneCircleRenderer = Instantiate(boneCircleRenderer, transform);
            _boneCircleRenderer.lineRenderer.startColor = CircleColor;
            _boneCircleRenderer.lineRenderer.endColor = CircleColor;
        }
        
        private void InitParentBoneLineRenderer() {
            foreach (var parent in BoneParentConnector.mainParent)
                CreateLine(parent, CircleColor);
            
            foreach (var parent in BoneParentConnector.subParent)
                CreateLine(parent, new Color(CircleColor.r * 0.5f, CircleColor.g * 0.5f, CircleColor.b * 0.5f, 0.8f));
            
            return;
            void CreateLine(BoneBase parent, Color color) {
                var boneLineRenderer = Resources.Load<BoneLineRenderer>("Prefabs/BoneLineRenderer");
                var line = Instantiate(boneLineRenderer, transform);
                line.Init(this, parent, BoneType, color);
            }
        }

        protected virtual void LateUpdate() {
            _boneCircleRenderer.lineRenderer.enabled = BoneVisualizeToggle.BoneTypeToVisibility[BoneType];
            _boneCircleRenderer.UpdateCircle(ToWorldSpace(SkeletonPosition));
        }

        protected Vector3 ToSkeletonSpace(Vector3 worldPosition) {
            return _skeleton.transform.InverseTransformPoint(worldPosition);
        }

        protected Vector3 ToWorldSpace(Vector3 skeletonPosition) {
            return _skeleton.transform.TransformPoint(skeletonPosition);
        }
    }
}