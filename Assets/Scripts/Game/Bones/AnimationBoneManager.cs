using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Game.Bones {
    public class AnimationBoneManager : MonoBehaviour {
        private SkeletonBone[] _originSkeletonBones;
        private GameObject _copied;
        
        private readonly Dictionary<GameObject, AnimationBone> _mapping = new();
        public IReadOnlyDictionary<GameObject, AnimationBone> Mapping => _mapping;
        
        public void Start() {
            CacheOriginComponents();
            Copy();
            InjectionAnimationBone();
            Matching();
        }

        private void CacheOriginComponents() {
            _originSkeletonBones = GetComponentsInChildren<SkeletonBone>(true);
        }

        private void Copy() {
            _copied = Instantiate(gameObject, transform);
            _copied.transform.localPosition = Vector3.zero;
            
            var copiedSkeleton = _copied.GetComponent<Skeleton.Skeleton>();
            if (copiedSkeleton != null)
                Destroy(copiedSkeleton);
            
            var copiedAnimationBoneManager = _copied.GetComponent<AnimationBoneManager>();
            if (copiedAnimationBoneManager != null)
                Destroy(copiedAnimationBoneManager);
            
            var copiedSpringBoneChains = _copied.GetComponentsInChildren<SpringBoneChain>(true);
            foreach (var copiedSpringBoneChain in copiedSpringBoneChains)
                Destroy(copiedSpringBoneChain);
            
            var copiedSpriteRenderers = _copied.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var copiedSpriteRenderer in copiedSpriteRenderers)
                copiedSpriteRenderer.enabled = false;
        }

        private void InjectionAnimationBone() {
            var spriteSkins = _copied.GetComponentsInChildren<SpriteSkin>(true);
            foreach (var spriteSkin in spriteSkins) {
                foreach (var child in spriteSkin.boneTransforms)
                    child.gameObject.AddComponent<AnimationBone>();
            }
        }
        
        private void Matching() {
            var copiedSkeletonBones  = _copied.GetComponentsInChildren<SkeletonBone>(true);
            var count = _originSkeletonBones.Length;
            for (var i = 0; i < count; i++) {
                var copiedSkeletonBone = copiedSkeletonBones[i];
                var copiedAnimationBone = copiedSkeletonBone.GetComponent<AnimationBone>();
                if (copiedAnimationBone == null) {
                    Debug.Log("???");
                    continue;
                }
                
                var copiedSpringBone = copiedSkeletonBone.GetComponent<SpringBone>();
                if (copiedSpringBone != null)
                    Destroy(copiedSpringBone);
                
                Destroy(copiedSkeletonBone);
                _mapping.Add(_originSkeletonBones[i].gameObject, copiedAnimationBone);
            }
        }
    }
}