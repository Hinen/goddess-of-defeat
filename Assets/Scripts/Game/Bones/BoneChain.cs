using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Game.Bones {
    public class BoneChain : MonoBehaviour {
        private SpriteSkin _spriteSkin;
        private SpriteRenderer _spriteRenderer;
        
        private readonly List<AnimationBone> _animationBones = new();

        private readonly Dictionary<GameObject, AnimationBone> _foo = new();
        public IReadOnlyDictionary<GameObject, AnimationBone> Foo => _foo;
        
        private void Awake() {
            _spriteSkin = GetComponent<SpriteSkin>();
            CreateAnimationBone();
            CreateRenderingBone(_spriteSkin.rootBone);
        }
        
        private void CreateAnimationBone() {
            foreach (var child in _spriteSkin.boneTransforms)
                _animationBones.Add(child.gameObject.AddComponent<AnimationBone>());
        }

        private void CreateRenderingBone(Transform target) {
            var renderingBone = Instantiate(target, transform);
            renderingBone.name = target.name;
            target.gameObject.SetActive(false);

            _spriteSkin.SetRootBone(renderingBone);
            _spriteSkin.ResetBindPose();
            
            var skeletonBones = target.GetComponentsInChildren<SkeletonBone>();
            foreach (var bone in skeletonBones)
                Destroy(bone);
            
            var springBones = target.GetComponentsInChildren<SpringBone>();
            foreach (var bone in springBones)
                Destroy(bone);
            
            //
            var renderingAnimationBones = renderingBone.GetComponentsInChildren<AnimationBone>();
            foreach (var bone in renderingAnimationBones)
                Destroy(bone);

            var bones = renderingBone.GetComponentsInChildren<SkeletonBone>();
            for (var i = 0; i < bones.Length; i++)
                _foo.Add(bones[i].gameObject, _animationBones[i]);
        }
    }
}