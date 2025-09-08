using UnityEngine;

namespace Scene {
    public class SceneBase : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init() {
            Application.targetFrameRate = 144;
            Time.fixedDeltaTime = 0.005f;
        }
    }
}