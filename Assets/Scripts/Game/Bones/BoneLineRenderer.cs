using Core;
using UnityEngine;

namespace Game.Bones {
    public class BoneLineRenderer : MonoBehaviour {
        private BoneBase _bone;
        private LineRenderer _lineRenderer;

        private void Awake() {
            _bone = GetComponent<BoneBase>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start() {
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.sortingLayerName = "Gizmo";
            _lineRenderer.positionCount = 2;
            
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
        }

        private void LateUpdate() {
            if (_bone.parent == null)
                return;
            
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _bone.parent.transform.position);
        }
    }
}