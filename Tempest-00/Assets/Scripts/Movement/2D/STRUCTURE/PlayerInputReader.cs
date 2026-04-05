//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tempest.Movement
{
    public struct PlayerInputFrame
    {
        //Movement Direction
        public Vector2 Move;

        //Input State
        public bool JumpPressed;
        public bool JumpHeld;
        public bool RunHeld;
    }

    public class PlayerInputReader : MonoBehaviour
    {
        public PlayerInputFrame ReadInput()
        {
            PlayerInputFrame input = new PlayerInputFrame
            {
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),

                JumpPressed = Input.GetButtonDown("Jump"),
                JumpHeld = Input.GetButton("Jump"),
                RunHeld = Input.GetKey(KeyCode.LeftShift)
            };

            return input;
        }
    }
}