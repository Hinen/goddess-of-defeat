using UnityEngine;

namespace Game.Bones {
    public class SpringBone : BoneBase {
        [SerializeField]
        private float mass = 1f;
        
        [SerializeField]
        private float stiffness = 2f;
    
        [SerializeField]
        private float damping = 1f;
        
        private Vector3 _setupPosition;
        private Vector3 _velocity;

        private void Start() {
            _setupPosition = transform.localPosition;
        }
        
        protected override void ApplySpringForcePosition(Vector3 positionDiff) {
            var oldPosition = transform.localPosition;
            
            var position = oldPosition + positionDiff;
            var displacement = position - _setupPosition;
            var springForce = -stiffness * displacement;
            var dampingForce = damping * _velocity;
            var force = springForce - dampingForce;
            var acceleration = mass != 0f ? force / mass : force;

            _velocity += acceleration * Time.fixedDeltaTime;
            position += _velocity * Time.fixedDeltaTime;
            transform.localPosition = position;
            
            var diff = position - oldPosition;
            base.ApplySpringForcePosition(diff);
        }
    }
}
