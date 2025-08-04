using UnityEngine;

public class SpringTest : MonoBehaviour {
    [SerializeField]
    private GameObject dot;
    
    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float mass = 1.0f;

    [SerializeField]
    private float stiffness = 7f;
    
    [SerializeField]
    private float damping;

    private Vector3 _velocity;

    private void FixedUpdate() {
        var displacement = transform.position - dot.transform.position;
        var springForce = -stiffness * displacement;
        var gravityForce = gravity * mass * Vector3.up;
        var dampingForce = damping * _velocity;
        
        var forceY = springForce + gravityForce - dampingForce;
        var accelerationY = forceY / mass;
        
        _velocity += accelerationY * Time.fixedDeltaTime;
        gameObject.transform.position += _velocity * Time.fixedDeltaTime;
    }

    public void OnClickReset() {
        transform.position = Vector3.zero;
        _velocity = Vector3.zero;
    }
}
