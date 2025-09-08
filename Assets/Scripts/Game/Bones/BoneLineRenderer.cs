using Core;
using Game.DebugTools;
using UnityEngine;

namespace Game.Bones {
    public class BoneLineRenderer : MonoBehaviour {
        private LineRenderer _lineRenderer;
        
        private BoneBase _bone;
        private BoneBase _nextBone;
        private Constants.BoneType _boneType;
        
        private void Awake() {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start() {
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.sortingLayerName = "Gizmo";
            _lineRenderer.positionCount = 2;
        }

        public void Init(BoneBase bone, ParentBone nextBone, Constants.BoneType boneType, Color color) {
            _bone = bone;
            _nextBone = nextBone.GetBone(bone);
            _boneType = boneType;
            
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }

        private void LateUpdate() {
            _lineRenderer.enabled = BoneVisualizeToggle.BoneTypeToVisibility[_boneType];
            if (_nextBone == null)
                return;
            
            _lineRenderer.SetPosition(0, _bone.WorldPosition);
            _lineRenderer.SetPosition(1, _nextBone.WorldPosition);
        }
    }
}