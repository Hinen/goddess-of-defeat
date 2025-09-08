using Core;
using Game.DebugTools;
using UnityEngine;

namespace Game.Bones {
    public abstract class BoneBase : MonoBehaviour {
        protected abstract Constants.BoneType BoneType { get; }
        
        private Skeleton _skeleton;
        private CircleRenderer _boneCircleRenderer;
        
        protected BoneParentConnector BoneParentConnector;
        protected bool IsAnchorBone => BoneParentConnector == null || BoneParentConnector.IsEmpty;
        public int SubParentCount => BoneParentConnector != null ? BoneParentConnector.subParent.Length : 0;
        
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
        
        protected virtual void InitParentBoneLineRenderer() {
            if (!IsAnchorBone)
                CreateLine(BoneParentConnector.mainParent, CircleColor);
        }
        
        protected void CreateLine(ParentBone parent, Color color) {
            var boneLineRenderer = Resources.Load<BoneLineRenderer>("Prefabs/BoneLineRenderer");
            var line = Instantiate(boneLineRenderer, transform);
            line.Init(this, parent, BoneType, color);
        }

        protected virtual void LateUpdate() {
            _boneCircleRenderer.lineRenderer.enabled = IsCircleRendererActive();
            _boneCircleRenderer.UpdateCircle(WorldPosition);
        }
        
        protected virtual bool IsCircleRendererActive() {
            return BoneVisualizeToggle.BoneTypeToVisibility[BoneType];
        }

        protected Vector3 ToSkeletonSpace(Vector3 worldPosition) {
            return _skeleton.transform.InverseTransformPoint(worldPosition);
        }

        protected Vector3 ToWorldSpace(Vector3 skeletonPosition) {
            return _skeleton.transform.TransformPoint(skeletonPosition);
        }
    }
}