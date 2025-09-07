using UnityEngine;

namespace Game.Bones {
    public class BoneParentConnector : MonoBehaviour {
        public SkeletonBone[] mainParent;
        public SkeletonBone[] subParent;
        
        public bool IsEmpty => mainParent.Length == 0 && subParent.Length == 0;
        
        public SkeletonBone GetParent(int idx) {
            if (idx < mainParent.Length)
                return mainParent[idx];
            
            idx -= mainParent.Length;
            if (idx < subParent.Length)
                return subParent[idx];
            
            return null;
        }
        
        public bool IsMainParent(int idx) => idx < mainParent.Length;
    }
}