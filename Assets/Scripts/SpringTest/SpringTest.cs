using UnityEngine;

public class SpringTest : MonoBehaviour {
    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float mass = 1.0f;
    
    private float _velocityY;
    
    private void FixedUpdate() {
        var forceY = gravity * mass;
        _velocityY += forceY * Time.fixedDeltaTime;
        gameObject.transform.position += new Vector3(0, _velocityY * Time.fixedDeltaTime, 0);
    }
}
