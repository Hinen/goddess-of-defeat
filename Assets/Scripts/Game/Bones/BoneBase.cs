using Core;
using UnityEngine;

namespace Game.Bones {
    public class BoneBase : MonoBehaviour {
        public BoneBase parent;
        public BoneBase[] children;

        private CircleRenderer _boneCircleRenderer;

        private void Awake() {
            _boneCircleRenderer = GetComponentInChildren<CircleRenderer>();    
        }

        protected virtual void LateUpdate() {
            _boneCircleRenderer.UpdateCircle(transform.position);
        }

        protected virtual void ApplySpringForcePosition(Vector3 positionDiff) {
            if (children == null)
                return;
            
            foreach (var child in children)
                child.ApplySpringForcePosition(positionDiff);
        }
    }
}