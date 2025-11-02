using Core;
using Game.Bones;
using Game.DebugTools;
using UnityEngine;

namespace Game.OldBones {
    public abstract class BoneBase : MonoBehaviour {
        protected abstract Constants.BoneType BoneType { get; }
        
        private Skeleton.Skeleton _skeleton;
        private CircleRenderer _boneCircleRenderer;
        
        public BoneParentConnector BoneParentConnector;
        public bool IsAnchorBone => BoneParentConnector == null || BoneParentConnector.IsEmpty;
        public int SubParentCount => BoneParentConnector != null ? BoneParentConnector.subParents.Length : 0;
        
        protected virtual Color CircleColor => Color.red;
        public virtual Vector3 Position { get; protected set; }
        public Vector3 SkeletonPosition => ToSkeletonPosition(Position);
        
        protected virtual void Awake() {
            Position = transform.position;
            
            _skeleton = GetComponentInParent<Skeleton.Skeleton>();
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
            _boneCircleRenderer.UpdateCircle(Position);
        }
        
        protected virtual bool IsCircleRendererActive() {
            return BoneVisualizeToggle.BoneTypeToVisibility[BoneType];
        }
        
        public Vector3 ToSkeletonPosition(Vector3 worldPosition) {
            return _skeleton.transform.InverseTransformPoint(worldPosition);
        }
        
        public Vector3 ToWorldPosition(Vector3 skeletonPosition) {
            return _skeleton.transform.TransformPoint(skeletonPosition);
        }
    }
}