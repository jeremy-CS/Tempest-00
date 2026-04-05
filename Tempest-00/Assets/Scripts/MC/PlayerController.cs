//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using Tempest.Movement;
using UnityEngine;

namespace Tempest.Player
{
    [RequireComponent(typeof(PlayerInputReader))]
    [RequireComponent(typeof(PlayerSensors2D))]
    [RequireComponent(typeof(PlayerMotor2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementConfig _config;

        private PlayerInputReader _inputReader;

        public PlayerInputFrame InputFrame { get; private set; }
        public PlayerMotor2D Motor { get; private set; }
        public PlayerSensors2D Sensors { get; private set; }
        public PlayerMovementConfig Config => _config;
        public PlayerStateMachine StateMachine { get; private set; }

        public PlayerGroundedState GroundedState { get; private set; }
        public PlayerAirState AirState { get; private set; }


        private void Awake()
        {
            _inputReader = GetComponent<PlayerInputReader>();
            Motor = GetComponent<PlayerMotor2D>();
            Sensors = GetComponent<PlayerSensors2D>();

            StateMachine = new PlayerStateMachine();

            GroundedState = new PlayerGroundedState(this, StateMachine);
            AirState = new PlayerAirState(this, StateMachine);
        }

        private void Start()
        {
            Sensors.Refresh();

            if (Sensors.IsGrounded) StateMachine.Initialize(GroundedState);
            else StateMachine.Initialize(AirState);
        }

        private void Update()
        {
            InputFrame = _inputReader.ReadInput();
            Sensors.Refresh();
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
            Motor.CommitVelocity();
        }
    }
}