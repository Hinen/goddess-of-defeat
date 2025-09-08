using System;
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
        public ParentBone[] subParent;
        
        public bool IsEmpty => mainParent == null && subParent.Length == 0;
        
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