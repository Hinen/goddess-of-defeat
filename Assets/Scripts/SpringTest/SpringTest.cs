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
   
    private float _velocityY;

    private void FixedUpdate() {
        var displacement = transform.position.y - dot.transform.position.y;
        var springForceY = -stiffness * displacement;
        var forceY = springForceY + gravity * mass;
        var accelerationY = forceY / mass;
        
        _velocityY += accelerationY * Time.fixedDeltaTime;
        gameObject.transform.position += new Vector3(0, _velocityY * Time.fixedDeltaTime, 0);
    }
}
