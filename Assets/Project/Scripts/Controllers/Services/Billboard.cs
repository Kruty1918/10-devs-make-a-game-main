using Bonjoura.Player;
using UnityEngine;

namespace Bonjoura.Services
{
    public sealed class Billboard : MonoBehaviour
    {
        [SerializeField] private float scaleMultiplier;
        private Transform _cameraTransform;

        private void Start()
        {
            _cameraTransform = PlayerController.Instance.FPSCamera.transform;
        }

        private void LateUpdate()
        {
            if (!_cameraTransform) return;
            transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward,
                _cameraTransform.rotation * Vector3.up);

            if (scaleMultiplier == 0) return; 
            float distance = Vector3.Distance(transform.position, _cameraTransform.transform.position);
            transform.localScale = Vector3.one * (distance * scaleMultiplier);
        }
    }
}
