using UnityEngine;
using UnityEngine.InputSystem;

namespace BlindSpot
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _gravity = -9.81f;

        [SerializeField] private Animator _animator;

        private WinLoseController _winLoseController;

        [SerializeField] private Transform _model;
        [SerializeField] private float _turnSpeed = 12f;
        private float _verticalVelocity;

        private void OnEnable()
        {
            _moveAction.action.Enable();
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _winLoseController = FindAnyObjectByType<WinLoseController>();
        }

        private void Update()
        {
            Move();
        }

        private void OnDisable()
        {
            _moveAction.action.Disable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                _winLoseController.ShowWinPanel();
            }
        }

        private void Move()
        {
            Vector2 moveInput = _moveAction.action.ReadValue<Vector2>();

            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            
            if (_controller.isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity = -2f; // Небольшое отрицательное значение для удержания игрока на земле
            }

            _verticalVelocity += _gravity * Time.deltaTime;

            Vector3 velocity = moveDirection * _moveSpeed;
            velocity.y = _verticalVelocity;

            _controller.Move(velocity * Time.deltaTime);
            _animator.SetFloat("Speed", moveInput.magnitude);

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion lookDirection = Quaternion.LookRotation(moveDirection);

                _model.rotation = Quaternion.Slerp(
                    _model.rotation,
                    lookDirection,
                    _turnSpeed * Time.deltaTime);
            }


        }
    }
}
