using System;
using Bonjoura.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bonjoura.Services
{
    public sealed class InteractRaycast : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask allowedLayers;

        private Transform _raycastPoint;
        private GameObject _currentDetectObject;
        public GameObject CurrentDetectObject => _currentDetectObject;

        public event Action OnRaycastEvent;

        private void Awake()
        {
            _raycastPoint = PlayerController.Instance.FPSCamera.transform;
        }

        private void Update()
        {
            if (!CursorOnUI())
            {
                Raycast();
            }
        }

        private void Raycast()
        {
            if (!Physics.Raycast(_raycastPoint.position, _raycastPoint.forward, out var hit, rayDistance, allowedLayers))
                _currentDetectObject = null;
            else
                _currentDetectObject = hit.transform.gameObject;

            OnRaycastEvent?.Invoke();
        }

        private bool CursorOnUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
