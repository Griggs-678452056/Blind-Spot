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

        private Transform _player;
        private bool _canSeePlayer;

        [field: SerializeField] public Transform ViewPoint { get; private set; }
        [field: SerializeField] public float PlayerViewAngle { get; private set; } = 60f;
        [field: SerializeField] public float PlayerViewRange { get; private set; } = 10f;

        private void Awake()
        {
            _initialRotation = transform.localRotation;
        }

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                _player = player.transform;
            }
        }

        private void Update()
        {
            RotateCamera();

            CheckForPlayer();
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

        private void CheckForPlayer()
        {
            _canSeePlayer = false;

            if (_player == null || ViewPoint == null)
            {
                return;
            }

            Vector3 origin = ViewPoint.position;
            Vector3 target = _player.position + Vector3.up;

            Vector3 directionToPlayer = target - origin;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer > PlayerViewRange)
            {
                return;
            }

            Vector3 direction = directionToPlayer.normalized;

            float angle = Vector3.Angle(ViewPoint.forward, direction);

            if (angle > PlayerViewAngle / 2)
            {
                return;
            }

            if (Physics.Raycast(origin, direction, out RaycastHit hit, distanceToPlayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _canSeePlayer = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (ViewPoint == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;

            Vector3 origin = ViewPoint.position;

            Vector3 leftDirection = Quaternion.Euler(0, -PlayerViewAngle / 2, 0) * ViewPoint.forward;
            Vector3 rightDirection = Quaternion.Euler(0, PlayerViewAngle / 2, 0) * ViewPoint.forward;

            Gizmos.DrawRay(origin, leftDirection * PlayerViewRange);
            Gizmos.DrawRay(origin, rightDirection * PlayerViewRange);

            Gizmos.color = _canSeePlayer ? Color.red : Color.green;

            Gizmos.DrawRay(origin, ViewPoint.forward * PlayerViewRange);
        }
    }
}
