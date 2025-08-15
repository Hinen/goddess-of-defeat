using UnityEngine;

namespace Core {
    [RequireComponent(typeof(LineRenderer))]
    public class CircleRenderer : MonoBehaviour {
        public LineRenderer lineRenderer;
        public int segments = 32;
        public float radius = 5f;

        private void Start() {
            Reset();
        }

        public void Reset() {
            lineRenderer.useWorldSpace = true;
            lineRenderer.loop = true;
            lineRenderer.positionCount = segments;
        }
        
        public void UpdateCircle(Vector3 center) {
            var points = new Vector3[segments];

            for (var i = 0; i < segments; i++) {
                var angle = 2 * Mathf.PI * i / segments;
                var x = Mathf.Cos(angle) * radius;
                var y = Mathf.Sin(angle) * radius;
                points[i] = new Vector3(x, y, -1f) + center;
            }

            lineRenderer.SetPositions(points);
        }
    }
}
