using Bonjoura.Player;
using SGS29.Utilities;
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
            SM.Instance<InputManager>().ChangeCursorState(_isPanelOpen);
        }
    }
}