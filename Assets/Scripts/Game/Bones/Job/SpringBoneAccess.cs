using UnityEngine;

namespace Game.Bones.Job {
    public struct MainSpringBoneAccess {
        public SpringBone.SpringBoneData Data;
        
        public Vector3 SetupParentDistance;
        public Vector3 ParentSkeletonPosition;
        
        public int SubSpringBoneStartIndex;
        public int SubSpringBoneCount;
        
        // OutPut
        public Vector3 Velocity;
        public Vector3 SkeletonPosition;
    }
    
    public struct SubSpringBoneAccess {
        public SpringBone.SpringBoneData Data;
        
        public Vector3 SetupParentDistance;
        public Vector3 ParentSkeletonPosition;
    }
}