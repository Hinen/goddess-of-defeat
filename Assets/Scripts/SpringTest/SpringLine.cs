using UnityEngine;

public class SpringLine : MonoBehaviour {
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject dot;

    [SerializeField]
    private SpringTest springTest;

    private void LateUpdate() {
        lineRenderer.SetPosition(0, dot.transform.position);

        var spring = springTest;
        for (var i = 1;; i++) {
            lineRenderer.SetPosition(i, spring.transform.position);

            spring = spring.childSpring;
            if (spring == null)
                break;
        }
    }
}
