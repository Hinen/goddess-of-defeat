using System.Linq;
using UnityEngine;

namespace Game.Bones {
    public class BoneBase : MonoBehaviour {
        public BoneBase child;
        
        private void Start() {
            child = GetComponentsInChildren<BoneBase>().FirstOrDefault(x => x != this);
        }

        public void Foo() {
            if (child != null)
                child.Foo();
        }
    }
}