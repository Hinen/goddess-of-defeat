using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game.Bones.Job {
    [BurstCompile]
    public struct SpringChainJob : IJobParallelFor {
        public float DeltaTime;
        public NativeArray<MainSpringBoneAccess> MainSpringBoneAccesses;
        
        [ReadOnly]
        public NativeArray<SubSpringBoneAccess> SubSpringBoneAccesses;
        
        public void Execute(int index) {
            var mainAccess = MainSpringBoneAccesses[index];
            var totalAccel = GetAcceleration(mainAccess.Data,
                mainAccess.SetupParentDistance, 
                mainAccess.ParentSkeletonPosition,
                mainAccess.SkeletonPosition,
                mainAccess.Velocity);
            
            for (var i = mainAccess.SubSpringBoneStartIndex; 
                 i < mainAccess.SubSpringBoneStartIndex + mainAccess.SubSpringBoneCount; 
                 i++) {
                var subAccess = SubSpringBoneAccesses[i];
                totalAccel += GetAcceleration(subAccess.Data, 
                    subAccess.SetupParentDistance, 
                    subAccess.ParentSkeletonPosition,
                    mainAccess.SkeletonPosition,
                    mainAccess.Velocity);
            }
            
            mainAccess.Velocity += totalAccel * DeltaTime;
            mainAccess.SkeletonPosition += mainAccess.Velocity * DeltaTime;
            MainSpringBoneAccesses[index] = mainAccess;
        }

        private static Vector3 GetAcceleration(SpringBone.SpringBoneData data, 
                                                Vector3 setupParentDistance, 
                                                Vector3 parentSkeletonPosition,
                                                Vector3 skeletonPosition,
                                                Vector3 velocity) {
            var currentDistance = parentSkeletonPosition - skeletonPosition;
            var displacement = setupParentDistance - currentDistance;
            var springForce = -data.Stiffness * displacement;
            var dampingForce = data.Damping * velocity;
            var force = springForce - dampingForce;
            
            return data.Mass != 0f ? force / data.Mass : force;
        }
    }
}