using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Game.Bones {
    public class SpringBoneChain : MonoBehaviour {
        [SerializeField]
        private SpriteSkin skin;

        private void Awake() {
            foreach (var boneTransform in skin.boneTransforms) {
                if (boneTransform == skin.rootBone)
                    boneTransform.AddComponent<AnchorBone>();
                else
                    boneTransform.AddComponent<SpringBone>();
            }
        }
    }
}
