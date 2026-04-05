//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tempest.Movement
{
    [CreateAssetMenu(menuName = "Player/Movement Config")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [Header("Speeds")]
        public float WalkSpeed = 5f;
        public float RunSpeed = 8f;

        [Header("Acceleration")]
        public float GroundAcceleration = 60f;
        public float GroundDeceleration = 80f;
        public float AirAcceleration = 40f;
        public float AirDeceleration = 30f;

        [Header("Jump")]
        public float JumpForce = 12f;

        [Header("Gravity")]
        public float Gravity = -35f;
        public float MaxFallSpeed = -20f;
    }
}