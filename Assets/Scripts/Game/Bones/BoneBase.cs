using Core;
using UnityEngine;

namespace Game.Bones {
    public class BoneBase : MonoBehaviour {
        public SpringBone[] mainChildren;
        public SpringBone[] subChildren;
        
        private CircleRenderer _boneCircleRenderer;

        private void Awake() {
            InitCircleRenderer();
            InitChildBoneLineRenderer();
        }
        
        private void InitCircleRenderer() {
            var lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = Resources.Load<Material>("Materials/Sprites-Default");
            lineRenderer.startColor = GetCircleRendererColor();
            lineRenderer.endColor = GetCircleRendererColor();
            lineRenderer.widthMultiplier = 10f;
            lineRenderer.sortingLayerName = "Gizmo";
            
            _boneCircleRenderer = gameObject.AddComponent<CircleRenderer>();
            _boneCircleRenderer.lineRenderer = lineRenderer;
            _boneCircleRenderer.segments = 32;
            _boneCircleRenderer.radius = 25f;
        }

        private void InitChildBoneLineRenderer() {
            foreach (var child in mainChildren)
                CreateLine(child, Color.green);
            
            foreach (var child in subChildren)
                CreateLine(child, Color.yellow);
            
            return;
            void CreateLine(BoneBase child, Color color) {
                var boneLineRenderer = Resources.Load<BoneLineRenderer>("Prefabs/BoneLineRenderer");
                var line = Instantiate(boneLineRenderer, transform);
                line.Init(this, child, color);
            }
        }

        protected virtual Color GetCircleRendererColor() {
            return Color.red;
        }

        protected virtual void LateUpdate() {
            _boneCircleRenderer.UpdateCircle(transform.position);
        }

        protected void ApplySpringForcePositionToChildren(Vector3 positionDiff) {
            foreach (var child in mainChildren)
                child.ApplySpringForcePosition(true, positionDiff);
            
            foreach (var child in subChildren)
                child.ApplySpringForcePosition(false, positionDiff);
        }
    }
}