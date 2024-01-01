using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [CreateAssetMenu(fileName = "CameraSetting", menuName = "Settings/CameraSetting")]
    public class CameraSetting : ScriptableObject
    {
        public float TPSDistance;
        public float DistanceSpeedMultiplier;

        public float TPSHeight;
        public float HeightSpeedMultiplier;

        public float TPSRotationDamping;

        public Vector3 ActualCameraRotation;
        public float ACRSpeedMultiplier;
        public float LerpSpeed;
        public bool AntiGravity;
        [ShowIf("AntiGravity")]
        public float antiGravityHeightMultipler;
        public bool isCustomCamera;
        [ShowIf("isCustomCamera")]
        public float customAntiGravityLerpSpeed;

    }
}
