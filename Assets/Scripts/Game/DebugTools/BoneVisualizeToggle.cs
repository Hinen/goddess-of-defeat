using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Game.DebugTools {
    public class BoneVisualizeToggle : MonoBehaviour {
        public static readonly Dictionary<Constants.BoneType, bool> BoneTypeToVisibility = new() {
            { Constants.BoneType.Skeleton, false },
            { Constants.BoneType.Animation, false },
            { Constants.BoneType.Spring, false }
        };
        
        [SerializeField]
        private Constants.BoneType boneType;

        public void OnValueChanged(bool value) {
            BoneTypeToVisibility[boneType] = value;
        }
    }
}