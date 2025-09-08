using System.Collections.Generic;
using Game.Bones.Job;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game.Bones {
    public class SpringBoneChain : MonoBehaviour {
        private readonly List<SpringBone> _springBones = new();
        private NativeArray<MainSpringBoneAccess> _mainSpringBoneAccesses;
        private NativeList<SubSpringBoneAccess> _subSpringBoneAccesses;
        
        private JobHandle _jobHandle;

        private void Start() {
            _springBones.AddRange(GetComponentsInChildren<SpringBone>());
            InitSpringBoneAccesses();
        }
        
        private void InitSpringBoneAccesses() {
            _mainSpringBoneAccesses = new NativeArray<MainSpringBoneAccess>(_springBones.Count, Allocator.Persistent);
            _subSpringBoneAccesses = new NativeList<SubSpringBoneAccess>(Allocator.Persistent);
            
            RefreshSpringBoneAccesses();
        }

        private void OnDestroy() {
            _jobHandle.Complete();
            _mainSpringBoneAccesses.Dispose();
            _subSpringBoneAccesses.Dispose();
        }

        public void FixedUpdate() {
            _jobHandle.Complete();

            for (var i = 0; i < _mainSpringBoneAccesses.Length; i++)
                _springBones[i].ApplyJobResult(_mainSpringBoneAccesses[i].Velocity, _mainSpringBoneAccesses[i].SkeletonPosition);

            RefreshSpringBoneAccesses();
            RegisterJob();
        }
        
        private void RefreshSpringBoneAccesses() {
            var subSpringBoneIndex = 0;
            
            for (var i = 0; i < _mainSpringBoneAccesses.Length; i++) {
                var mainAccess = _springBones[i].GetMainSpringBoneAccess();
                mainAccess.SubSpringBoneStartIndex = subSpringBoneIndex;
                subSpringBoneIndex += mainAccess.SubSpringBoneCount;
                _mainSpringBoneAccesses[i] = mainAccess;

                for (var j = 0; j < _springBones[i].SubParentCount; j++)
                    _subSpringBoneAccesses.Add(_springBones[i].GetSubSpringBoneAccess(j));
            }
        }
        
        private void RegisterJob() {
            var job = new SpringChainJob {
                DeltaTime = Time.deltaTime,
                MainSpringBoneAccesses = _mainSpringBoneAccesses,
                SubSpringBoneAccesses = _subSpringBoneAccesses.AsArray()
            };
            
            _jobHandle = job.Schedule(_mainSpringBoneAccesses.Length, 8);
        }
    }
}
