using UnityEngine;
using SGS29.Utilities;

namespace Bonjoura.Player
{
    public class BoatController : MonoBehaviour
    {
        [Header("Boat Settings")]
        [SerializeField] private Transform _seatPosition;
        [SerializeField] private Animator _animator;
        //[SerializeField] private float _moveSpeed = 5f; never used
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _acceleration = 2f;
        [SerializeField] private float _turnSpeed = 50f;
        [SerializeField] private GameObject _player;
        [SerializeField] private float _friction = 0.98f;

        private GameObject _playerParent;
        private Rigidbody _rb;
        private bool _isPlayerInBoat = false;
        private float _currentSpeed = 0f;


        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _playerParent = _player.transform.parent.gameObject;
            _animator = GetComponent<Animator>();

        }

        private void Update()
        {
            if (_isPlayerInBoat)
            {
                HandleBoatControls();


                if (SM.Instance<InputManager>().Player.Interact.WasPressedThisFrame())
                {
                    ExitBoat();
                }
            }
        }

        private void HandleBoatControls()
        {
            Vector2 moveInput = SM.Instance<InputManager>().MoveAxis;

            if (moveInput.y < 0)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, _maxSpeed * moveInput.y, _acceleration * Time.deltaTime);
            }
            else
            {
                _currentSpeed *= _friction;
            }

            float turn = moveInput.x * _turnSpeed;

            _rb.linearVelocity = transform.right * _currentSpeed;
            _rb.angularVelocity = new Vector3(0f, turn * Mathf.Deg2Rad, 0f);

            _animator.SetFloat("SpeedToBlend", Mathf.Abs(_currentSpeed) > 0.1 ? 1 : 0);

        }

        private void EnterBoat()
        {
            _isPlayerInBoat = true;


            _player.GetComponent<CharacterController>().enabled = false;
            _player.GetComponent<PlayerMoving>().enabled = false;

            _player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _player.transform.SetParent(_seatPosition);
            _player.transform.localPosition = Vector3.zero;
            _player.transform.localRotation = Quaternion.identity;
        }

        private void ExitBoat()
        {
            _isPlayerInBoat = false;


            _player.GetComponent<CharacterController>().enabled = true;
            _player.GetComponent<PlayerMoving>().enabled = true;

            _player.transform.SetParent(_playerParent.transform);
            _player.transform.position = transform.position + Vector3.up * 2f;
        }

        public void Sit()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < 2f && SM.Instance<InputManager>().Player.Interact.WasPressedThisFrame())
            {
                EnterBoat();
            }
        }


        private void OnEnable()
        {
            SM.Instance<PlayerController>().InteractRaycast.OnRaycastEvent += Sit;
        }

        private void OnDisable()
        {
            if (SM.HasSingleton<PlayerController>())
                SM.Instance<PlayerController>().InteractRaycast.OnRaycastEvent -= Sit;
        }
    }
}

