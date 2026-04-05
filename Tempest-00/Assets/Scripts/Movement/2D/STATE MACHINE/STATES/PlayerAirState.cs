//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using Tempest.Player;
using UnityEngine;

namespace Tempest.Movement
{
    public class PlayerAirState : PlayerMovementState
    {
        public PlayerAirState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Update()
        {
            float inputX = _player.InputFrame.Move.x;
            _player.Motor.FaceInput(inputX);

            if (_player.Sensors.IsGrounded && _player.Motor.VerticalVelocity <= 0f)
            {
                _stateMachine.ChangeState(_player.GroundedState);
            }
        }

        public override void FixedUpdate()
        {
            float inputX = _player.InputFrame.Move.x;

            _player.Motor.SetHorizontalMovement(
                inputX,
                _player.Config.WalkSpeed,
                _player.Config.AirAcceleration,
                _player.Config.AirDeceleration
                );

            _player.Motor.ApplyGravity(
                _player.Config.Gravity,
                _player.Config.MaxFallSpeed
                );
        }
    }
}