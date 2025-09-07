using UnityEngine;

namespace Game.Bones.Job {
    public struct SpringBoneAccess {
        public SpringBone.SpringBoneData Data;
        public Vector3 SetupParentDistance;
        public Vector3 ParentSkeletonPosition;
        public Vector3 SkeletonPosition;
        
        // OutPut
        public Vector3 Velocity;
    }
}