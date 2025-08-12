using UnityEngine;

namespace Game.Bones {
    public class BoneLineRenderer : MonoBehaviour {
        private LineRenderer _lineRenderer;
        
        private BoneBase _bone;
        private BoneBase _nextBone;
        
        private void Awake() {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start() {
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.sortingLayerName = "Gizmo";
            _lineRenderer.positionCount = 2;
            
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
        }

        public void Init(BoneBase bone, BoneBase nextBone) {
            _bone = bone;
            _nextBone = nextBone;
        }

        private void LateUpdate() {
            if (_nextBone == null)
                return;
            
            _lineRenderer.SetPosition(0, _bone.transform.position);
            _lineRenderer.SetPosition(1, _nextBone.transform.position);
        }
    }
}