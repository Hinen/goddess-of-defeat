using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Bones {
    [Serializable]
    public class ParentBone {
        [SerializeField]
        private BoneBase bone;
        
        private bool _isInitialized;
        private SkeletonBone _skeletonBone;
        private AnimationBone _animationBone;
        private SpringBone _springBone;
        
        public GameObject GameObject => bone != null ? bone.gameObject : null;
        
        public BoneBase GetBone<T>(T sameBone) where T : BoneBase {
            if (!_isInitialized)
                Init();
            
            if (_skeletonBone != null && sameBone is SkeletonBone)
                return _skeletonBone;
            if (_animationBone != null && sameBone is AnimationBone)
                return _animationBone;
            if (_springBone != null && sameBone is SpringBone)
                return _springBone;
            
            if (_skeletonBone != null)
                return _skeletonBone;
            if (_animationBone != null)
                return _animationBone;
            if (_springBone != null)
                return _springBone;

            return null;
        }

        private void Init() {
            _isInitialized = true;
            
            _skeletonBone = bone.GetComponent<SkeletonBone>();
            _animationBone = bone.GetComponent<AnimationBone>();
            _springBone = bone.GetComponent<SpringBone>();
        }
    }
    
    public class BoneParentConnector : MonoBehaviour {
        public ParentBone mainParent;
        public ParentBone[] subParents;

        private readonly Dictionary<ParentBone, Vector3> _setupParentBoneDistances = new();
        public IReadOnlyDictionary<ParentBone, Vector3> SetupParentBoneDistances => _setupParentBoneDistances;
        
        public bool IsEmpty => mainParent == null && subParents.Length == 0;

        public void Init<T>(T sameBone) where T : BoneBase {
            if (mainParent != null) 
                _setupParentBoneDistances[mainParent] = mainParent.GetBone(sameBone).SkeletonPosition - sameBone.SkeletonPosition;

            foreach (var subParent in subParents)
                _setupParentBoneDistances[subParent] = subParent.GetBone(sameBone).SkeletonPosition - sameBone.SkeletonPosition;
        }
        
        public void DivideFromMainParent() {
            if (mainParent != null)
                transform.SetParent(null, true);
        }
        
        private void LateUpdate() {
            if (mainParent != null)
                transform.SetParent(mainParent.GameObject.transform, true);
        }
    }
}