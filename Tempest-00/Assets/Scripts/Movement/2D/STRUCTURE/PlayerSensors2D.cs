//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tempest.Movement
{
    public class PlayerSensors2D : MonoBehaviour
    {
        [Header("Ground Check")]
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.5f, 0.1f);
        [SerializeField] private LayerMask _groundLayerMask;

        public bool IsGrounded { get; private set; }

        public void Refresh()
        {
            //Sanity check
            if (_groundCheckPoint == null)
            {
                Debug.LogWarning($"{nameof(PlayerSensors2D)} on {name} is missing a ground check point reference.");
                IsGrounded = false;
                return;
            }

            Collider2D hit = Physics2D.OverlapBox(
                point: _groundCheckPoint.position,
                size: _groundCheckSize,
                angle: 0f,
                layerMask: _groundLayerMask
                ); 

            IsGrounded = hit != null;
        }

        private void OnDrawGizmosSelected()
        {
            //Sanity check
            if (_groundCheckPoint == null) return;

            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        }
    }
}