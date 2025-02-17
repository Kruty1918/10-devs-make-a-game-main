using Bonjoura.Managers;
using Bonjoura.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bonjoura.UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        protected bool _isPanelOpen;

        protected void OpenMenu(InputAction.CallbackContext obj)
        {
            panel.SetActive(!panel.activeSelf);
            PlayerController.Instance.FPSCamera.enabled = !PlayerController.Instance.FPSCamera.enabled;
            _isPanelOpen = !_isPanelOpen;
            InputManager.Instance.ChangeCursorState(_isPanelOpen);
        }
    }
}