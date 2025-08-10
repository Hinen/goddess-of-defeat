using UnityEngine;

namespace Game.Bones {
    public class AnchorBone : BoneBase {
        private Vector3 _oldLocalPosition;

        public void Update() {
            _oldLocalPosition = transform.localPosition;
        }
        
        public void LateUpdate() {
            var diff = transform.localPosition - _oldLocalPosition;
            Debug.Log("Anchor Diff : " + diff);
            
            ApplySpringForcePosition(diff);
        }
    }
}
