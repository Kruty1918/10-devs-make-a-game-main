using UnityEngine;

namespace Bonjoura.Player
{
    public sealed class PlayerMoving : MonoBehaviour
    {
        [Header("Ground")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;

        [Header("Physics")]
        [SerializeField] private float friction = 8f;

        private bool _isBlockMovement;

        private float _yRotation;

        private CharacterController _characterController;
        private bool _isGround;

        private Vector3 _velocityInput;
        private Vector3 _forwardMove;

        private Vector3 _forwardMoveWithoutVelocity;

        private Vector3 _velocity;
        private Vector3 _currentForce = Vector3.zero;
        private Vector3 _moveVelocity;

        private float _originalHeight;

        public float OriginalHeight => _originalHeight;

        public CharacterController CharacterController => _characterController;
        public bool IsGrounded => _isGround;
        public Vector3 ForwardMove => _forwardMove;
        public Vector3 ForwardMoveWithoutVelocity => _forwardMoveWithoutVelocity;

        public Vector3 Velocity => _velocity;

        private float speed = 1;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _originalHeight = _characterController.height;
        }

        private void Update()
        {
            CheckGround();

            VelocitySetter();

            Movement();
            Gravity();

            _characterController.Move(_velocity * Time.deltaTime);

            ApplyExternalForces();
        }

        private void ApplyExternalForces()
        {
            if (_currentForce.magnitude > 0.1f)
            {
                _characterController.Move(_currentForce * Time.deltaTime);
                _currentForce = Vector3.Lerp(_currentForce, Vector3.zero, friction * Time.deltaTime);
            }
        }

        private void CheckGround()
        {
            _isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (_isGround && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }


        private void VelocitySetter()
        {
            if (!_isGround)
            {
                if (InputManager.Instance.MoveAxis.x != 0)
                    _velocityInput.x = Mathf.MoveTowards(_velocityInput.x, InputManager.Instance.MoveAxis.x, PlayerController.Instance.PlayerData.FlyDelta * Time.deltaTime);
                if (InputManager.Instance.MoveAxis.y != 0)
                    _velocityInput.y = Mathf.MoveTowards(_velocityInput.y, InputManager.Instance.MoveAxis.y, PlayerController.Instance.PlayerData.FlyDelta * Time.deltaTime);
            }
            else
            {
                _velocityInput = Vector3.MoveTowards(_velocityInput, InputManager.Instance.MoveAxis, PlayerController.Instance.PlayerData.DeltaMove * Time.deltaTime);
            }
        }

        private void Movement()
        {
            Vector3 right = PlayerController.Instance.FPSCamera.transform.right;
            Vector3 forward = PlayerController.Instance.FPSCamera.transform.forward;

            if (Mathf.Approximately(forward.y, 1)) forward = -PlayerController.Instance.FPSCamera.transform.up;
            else if (Mathf.Approximately(forward.y, -1)) forward = PlayerController.Instance.FPSCamera.transform.up;

            forward = new Vector3(forward.x, 0, forward.z).normalized;

            _forwardMove = right * _velocityInput.x + forward * _velocityInput.y;
            _forwardMoveWithoutVelocity = right * InputManager.Instance.MoveAxis.x + forward * InputManager.Instance.MoveAxis.y;

            if (_isBlockMovement) return;
            _moveVelocity.x = _forwardMove.x * PlayerController.Instance.PlayerData.SpeedMove * speed;
            _moveVelocity.z = _forwardMove.z * PlayerController.Instance.PlayerData.SpeedMove * speed;
            _characterController.Move(_moveVelocity * Time.deltaTime);
        }

        private void Gravity()
        {
            _velocity.y += PlayerController.Instance.PlayerData.GravityForce * Time.deltaTime;
        }

        public void BlockMovement()
        {
            _isBlockMovement = true;
        }

        public void UnBlockMovement()
        {
            _isBlockMovement = false;
        }

        public void SetVelocity(Vector3 velocity) => _velocity = velocity;

        public void AddVelocity(Vector3 velocity) => _velocity += velocity;

        public void AddVelocityX(float xVelocity) => _velocity.x += xVelocity;
        public void AddVelocityY(float yVelocity) => _velocity.y += yVelocity;
        public void AddVelocityZ(float zVelocity) => _velocity.z += zVelocity;

        public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            switch (mode)
            {
                case ForceMode.Force:
                    _currentForce += force * Time.deltaTime;
                    break;
                case ForceMode.Impulse:
                    _currentForce += force;
                    break;
            }
        }
        public void UpdateMovingSpeed(float _speed)
        {
            speed = _speed;
        }
    }
}
