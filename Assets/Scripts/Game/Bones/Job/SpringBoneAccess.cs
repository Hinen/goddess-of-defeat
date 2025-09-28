using UnityEngine;

namespace Game.Bones.Job {
    public struct MainSpringBoneAccess {
        public SpringBone.SpringBoneData Data;
        
        public Vector3 SetupParentSkeletonPositionDistance;
        public Vector3 ParentSkeletonPosition;
        
        public int SubSpringBoneStartIndex;
        public int SubSpringBoneCount;
        
        public Vector3 SkeletonPosition;
        
        // OutPut
        public Vector3 Velocity;
        public Vector3 Result;
    }
    
    public struct SubSpringBoneAccess {
        public SpringBone.SpringBoneData Data;
        
        public Vector3 SetupParentSkeletonPositionDistance;
        public Vector3 ParentSkeletonPosition;
    }
}