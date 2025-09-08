using System.Collections.Generic;
using Game.Bones.Job;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game.Bones {
    public class SpringBoneChain : MonoBehaviour {
        private readonly List<SpringBone> _springBones = new();
        private NativeArray<MainSpringBoneAccess> _mainSpringBoneAccesses;
        
        private JobHandle _jobHandle;

        private void Start() {
            _springBones.AddRange(GetComponentsInChildren<SpringBone>());
            InitSpringBoneAccesses();
        }
        
        private void InitSpringBoneAccesses() {
            _mainSpringBoneAccesses = new NativeArray<MainSpringBoneAccess>(_springBones.Count, Allocator.Persistent);
            
            RefreshSpringBoneAccesses();
        }

        private void OnDestroy() {
            _jobHandle.Complete();
            _mainSpringBoneAccesses.Dispose();
        }

        public void FixedUpdate() {
            _jobHandle.Complete();

            for (var i = 0; i < _mainSpringBoneAccesses.Length; i++)
                _springBones[i].ApplyJobResult(_mainSpringBoneAccesses[i].Velocity, _mainSpringBoneAccesses[i].SkeletonPosition);

            RefreshSpringBoneAccesses();
            RegisterJob();
        }
        
        private void RefreshSpringBoneAccesses() {
            for (var i = 0; i < _mainSpringBoneAccesses.Length; i++)
                _mainSpringBoneAccesses[i] = _springBones[i].GetMainSpringBoneAccess();
        }
        
        private void RegisterJob() {
            var job = new SpringChainJob {
                DeltaTime = Time.deltaTime,
                SpringBoneAccesses = _mainSpringBoneAccesses
            };
            
            _jobHandle = job.Schedule(_mainSpringBoneAccesses.Length, 8);
        }
    }
}
