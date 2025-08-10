using System.Linq;
using UnityEngine;

namespace Game.Bones {
    public class BoneBase : MonoBehaviour {
        public BoneBase[] children;

        protected virtual void ApplySpringForcePosition(Vector3 positionDiff) {
            if (children == null)
                return;
            
            foreach (var child in children)
                child.ApplySpringForcePosition(positionDiff);
        }
    }
}