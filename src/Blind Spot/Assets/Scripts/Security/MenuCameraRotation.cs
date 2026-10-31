using UnityEngine;

namespace BlindSpot
{
    public class MenuCameraRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationAngle = 45f;
        [SerializeField] private float _rotationSpeed = 20f;

        private float _initialAngle;

        private void Awake()
        {
            _initialAngle = transform.localEulerAngles.y;
        }

        private void Update()
        {
            float angle = Mathf.PingPong(
                Time.time * _rotationSpeed,
                _rotationAngle * 2)
                - _rotationAngle;

            transform.localRotation = Quaternion.Euler(0, _initialAngle + angle, 0);
        }
    }
}
