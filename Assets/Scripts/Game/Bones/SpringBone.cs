using UnityEngine;

namespace Game.Bones {
    public class SpringBone : BoneBase {
        [SerializeField]
        private float stiffness = 2f;
    
        [SerializeField]
        private float damping = 1f;
        
        private Vector3 _setupPosition;
        private Vector3 _oldPosition;
        
        private Vector3 _velocity;

        private void Start() {
            _setupPosition = transform.localPosition;
        }
        
        protected override void ApplySpringForcePosition(Vector3 positionDiff) {
            _oldPosition = transform.localPosition;
            
            var position= _oldPosition + positionDiff;
            var displacement = position - _setupPosition;
            var springForce = -stiffness * displacement;
            var dampingForce = damping * _velocity;
            var force = springForce - dampingForce;
            
            _velocity += force * Time.fixedDeltaTime;
            position += _velocity * Time.fixedDeltaTime;
            transform.localPosition = position;
            
            var diff = position - _oldPosition;
            base.ApplySpringForcePosition(diff);
        }
    }
}
