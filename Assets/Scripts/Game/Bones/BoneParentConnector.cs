using UnityEngine;

namespace Game.Bones {
    public class BoneParentConnector : MonoBehaviour {
        public SkeletonBone[] mainParent;
        public SkeletonBone[] subParent;
        
        public bool IsEmpty => mainParent.Length == 0 && subParent.Length == 0;
    }
}