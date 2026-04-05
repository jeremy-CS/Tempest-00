//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using Tempest.Player;
using UnityEngine;

namespace Tempest.Movement
{
    public abstract class PlayerMovementState
    {
        protected readonly PlayerController _player;
        protected readonly PlayerStateMachine _stateMachine;

        protected PlayerMovementState(PlayerController player, PlayerStateMachine stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}