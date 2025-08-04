using UnityEngine;

public class SpringLine : MonoBehaviour {
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject dot;

    [SerializeField]
    private GameObject springTest;

    private void LateUpdate() {
        lineRenderer.SetPosition(0, springTest.transform.position);
        lineRenderer.SetPosition(1, dot.transform.position);
    }
}
