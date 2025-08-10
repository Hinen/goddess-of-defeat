using UnityEngine;

namespace Game.Bones {
    public class SpringBoneChain : MonoBehaviour {
        public BoneBase[] Bones { get; private set; }

        private void Awake() {
            Bones = GetComponentsInChildren<BoneBase>();
        }
    }
}
