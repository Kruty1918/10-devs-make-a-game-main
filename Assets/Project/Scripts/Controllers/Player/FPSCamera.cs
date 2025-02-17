using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Player
{
    public sealed class FPSCamera : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float sensitivity = 0.2f;

        public Camera MainCamera => mainCamera;

        private float _xRotation;
        private float _yRotation;

        private void Update()
        {
            CameraRotate();
        }

        private void CameraRotate()
        {
            _xRotation -= SM.Instance<InputManager>().LookAxis.y * sensitivity;
            _yRotation += SM.Instance<InputManager>().LookAxis.x * sensitivity;

            _xRotation = Mathf.Clamp(_xRotation, -90, 90);

            transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        }
    }
}