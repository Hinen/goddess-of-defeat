using UnityEngine;

public class SpringTest : MonoBehaviour {
    [SerializeField]
    private GameObject parent;
    public SpringTest childSpring;
    
    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float mass = 5f;

    [SerializeField]
    private float stiffness = 25f;
    
    [SerializeField]
    private float damping = 2f;

    private Vector3 _startPosition;
    private Vector3 _velocity;

    private Vector3 Displacement => transform.position - parent.transform.position;
    private Vector3 SpringForce => -stiffness * Displacement;
    private Vector3 DampingForce => damping * _velocity;

    private void Start() {
        _startPosition = transform.position;
    }

    private void FixedUpdate() {
        var gravityForce = gravity * mass * Vector3.up;
        var force = SpringForce + gravityForce - DampingForce;
        
        if (childSpring != null)
            force += -childSpring.SpringForce + childSpring.DampingForce;
        
        var accelerationY = force / mass;
        
        _velocity += accelerationY * Time.fixedDeltaTime;
        gameObject.transform.position += _velocity * Time.fixedDeltaTime;
    }

    public void OnClickReset() {
        transform.position = _startPosition;
        _velocity = Vector3.zero;
        
        if (childSpring != null)
            childSpring.OnClickReset();
    }
}
