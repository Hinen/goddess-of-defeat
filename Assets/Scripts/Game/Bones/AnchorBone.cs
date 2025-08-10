using UnityEngine;

namespace Game.Bones {
    public class AnchorBone : BoneBase {
        private Vector3 _oldLocalPosition;

        public void Update() {
            _oldLocalPosition = transform.localPosition;
        }
        
        protected override void LateUpdate() {
            var diff = transform.localPosition - _oldLocalPosition;
            ApplySpringForcePosition(diff);
            
            base.LateUpdate();
        }
    }
}
