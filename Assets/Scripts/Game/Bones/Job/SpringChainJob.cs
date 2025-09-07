using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Game.Bones.Job {
    [BurstCompile]
    public struct SpringChainJob : IJobParallelFor {
        public NativeArray<SpringBoneAccess> SpringBoneAccesses;
        
        public void Execute(int index) {
            var a = SpringBoneAccesses[index];
        }
    }
}