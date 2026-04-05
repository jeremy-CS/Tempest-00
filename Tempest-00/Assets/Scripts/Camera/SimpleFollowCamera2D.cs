//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tempest.Camera
{
    public class SimpleFollowCamera2D : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform _target;

        [Header("Follow Settings")]
        [SerializeField] private float _smoothTime = 0.15f;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, -10f);

        private Vector3 _currentVelocity;

        private void LateUpdate()
        {
            if (_target == null) return;

            Vector3 targetPosition = _target.position + _offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);
        }
    }
}