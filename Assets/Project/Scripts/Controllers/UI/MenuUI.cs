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
            SM.Instance<PlayerController>().FPSCamera.enabled = !SM.Instance<PlayerController>().FPSCamera.enabled;
            _isPanelOpen = !_isPanelOpen;
            SM.Instance<InputManager>().ChangeCursorState(_isPanelOpen);
        }
    }
}