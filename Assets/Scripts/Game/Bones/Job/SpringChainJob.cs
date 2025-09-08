using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Game.Bones.Job {
    [BurstCompile]
    public struct SpringChainJob : IJobParallelFor {
        public float DeltaTime;
        public NativeArray<MainSpringBoneAccess> SpringBoneAccesses;
        
        public void Execute(int index) {
            var access = SpringBoneAccesses[index];
            var springData = access.Data;

            var originDistance = access.SetupParentDistance;
            var currentDistance = access.ParentSkeletonPosition - access.SkeletonPosition;
      
            var displacement = originDistance - currentDistance;
            var springForce = -springData.Stiffness * displacement;
            var dampingForce = springData.Damping * access.Velocity;
            var force = springForce - dampingForce;
            var acceleration = springData.Mass != 0f ? force / springData.Mass : force;
            access.Velocity += acceleration * DeltaTime;
            
            var delta = access.Velocity * DeltaTime;
            access.SkeletonPosition += delta;
            
            SpringBoneAccesses[index] = access;
        }
    }
}