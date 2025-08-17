using Game.Bones;
using UnityEngine;

namespace Core {
    public static class Utility {
        public static Vector3 ToSkeletonSpace(this Vector3 worldPosition, Skeleton skeleton) {
            return skeleton.transform.InverseTransformPoint(worldPosition);
        }
        
        public static Vector3 ToWorldSpace(this Vector3 skeletonPosition, Skeleton skeleton) {
            return skeleton.transform.TransformPoint(skeletonPosition);
        }
    }
}