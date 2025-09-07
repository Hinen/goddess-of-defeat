using System.Collections.Generic;
using System.Linq;
using Game.Bones.Job;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game.Bones {
    public class SpringBoneChain : MonoBehaviour {
        private readonly List<SpringBone> _springBones = new();
        private NativeArray<SpringBoneAccess> _springBoneAccesses;
        
        private JobHandle _jobHandle;

        private void Start() {
            _springBones.AddRange(GetComponentsInChildren<SpringBone>());
            InitSpringBoneAccesses();
        }
        
        private void InitSpringBoneAccesses() {
            _springBoneAccesses = new NativeArray<SpringBoneAccess>(_springBones.Sum(x => x.ParentCount), Allocator.Persistent);
            RefreshSpringBoneAccesses();
        }
        
        private void RefreshSpringBoneAccesses() {
            for (var i = 0; i < _springBones.Count; i++) {
                for (var j = 0; j < _springBones[i].ParentCount; j++)
                    _springBoneAccesses[i + j] = _springBones[i].GetAccess(j);
            }
        }

        private void OnDestroy() {
            _springBoneAccesses.Dispose();
        }

        public void FixedUpdate() {
            _jobHandle.Complete();
            
            RefreshSpringBoneAccesses();
            RegisterJob();
        }
        
        private void RegisterJob() {
            var job = new SpringChainJob {
                SpringBoneAccesses = _springBoneAccesses
            };
            
            _jobHandle = job.Schedule(_springBoneAccesses.Length, 64);
        }
    }
}
