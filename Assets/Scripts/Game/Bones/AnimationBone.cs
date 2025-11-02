using System.Linq;
using UnityEngine;

namespace Game.Bones {
    public class AnimationBone : MonoBehaviour, ISpringBoneParent {
        [SerializeField]
        private bool isDebug;
        
        private Skeleton.Skeleton _skeleton;
        
        public Vector3 Position => transform.position;
        public Vector3 SkeletonPosition {
            get {
                if (isDebug)
                    Debug.Log(ToSkeletonPosition(Position));
                
                return ToSkeletonPosition(Position);
            }
        }

        private Vector3 ToSkeletonPosition(Vector3 worldPosition) => _skeleton.transform.InverseTransformPoint(worldPosition);

        private void Awake() {
            _skeleton = GetComponentsInParent<Skeleton.Skeleton>().Last();
        }
    }
}