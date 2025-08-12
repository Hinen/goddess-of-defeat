using Core;
using UnityEngine;

namespace Game.Bones {
    public class BoneBase : MonoBehaviour {
        [SerializeField]
        private BoneLineRenderer boneLineRendererPrefab;
        
        public SpringBone[] mainChildren;
        public SpringBone[] subChildren;
        
        private CircleRenderer _boneCircleRenderer;

        private void Awake() {
            _boneCircleRenderer = GetComponent<CircleRenderer>();

            foreach (var child in mainChildren)
                CreateLine(child, Color.green);
            
            foreach (var child in subChildren)
                CreateLine(child, Color.yellow);
            
            return;
            void CreateLine(BoneBase child, Color color) {
                var line = Instantiate(boneLineRendererPrefab, transform);
                line.Init(this, child, color);
            }
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