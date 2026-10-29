using System;
using UnityEngine;

namespace BlindSpot
{
    public class SecurityCameraRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationAngle = 60f;
        [SerializeField] private float _rotationSpeed = 30f;

        private Quaternion _initialRotation;
        private float _currentAngle;
        private int _direction = 1;

        private void Awake()
        {
            _initialRotation = transform.localRotation;
        }

        private void Update()
        {
            RotateCamera();
        }

        private void RotateCamera()
        {
            _currentAngle += _direction * _rotationSpeed * Time.deltaTime;

            if (_currentAngle >= _rotationAngle)
            {
                _currentAngle = _rotationAngle;
                _direction = -1;
            }
            else if (_currentAngle <= -_rotationAngle)
            {
                _currentAngle = -_rotationAngle;
                _direction = 1;
            }

            transform.localRotation = _initialRotation * Quaternion.Euler(0, _currentAngle, 0);
        }
    }
}
