using UnityEngine;

namespace Game.Bones.Job {
    public struct MainSpringBoneAccess {
        public SpringBone.SpringBoneData Data;
        
        public Vector3 SetupParentDistance;
        public Vector3 ParentPosition;
        
        public int SubSpringBoneStartIndex;
        public int SubSpringBoneCount;
        
        // OutPut
        public Vector3 Velocity;
        public Vector3 Position;
    }
    
    public struct SubSpringBoneAccess {
        public SpringBone.SpringBoneData Data;
        
        public Vector3 SetupParentDistance;
        public Vector3 ParentPosition;
    }
}