//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using Tempest.Player;
using UnityEngine;

namespace Tempest.Movement
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public PlayerGroundedState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Update()
        {
            if (!_player.Sensors.IsGrounded)
            {
                _stateMachine.ChangeState(_player.AirState);
                return;
            }

            float inputX = _player.InputFrame.Move.x;
            _player.Motor.FaceInput(inputX);

            if (_player.InputFrame.JumpPressed)
            {
                _player.Motor.Jump(_player.Config.JumpForce);
                _stateMachine.ChangeState(_player.AirState);
            }
        }

        public override void FixedUpdate()
        {
            float inputX = _player.InputFrame.Move.x;
            float maxSpeed = _player.InputFrame.RunHeld
                ? _player.Config.RunSpeed
                : _player.Config.WalkSpeed;

            _player.Motor.SetHorizontalMovement(
                inputX,
                maxSpeed,
                _player.Config.GroundAcceleration,
                _player.Config.GroundDeceleration
                );
        }
    }
}