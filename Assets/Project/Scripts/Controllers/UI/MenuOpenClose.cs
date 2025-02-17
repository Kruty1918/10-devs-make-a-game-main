using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bonjoura.UI
{
    public class MenuOpenClose : MonoBehaviour
    {
        [SerializeField] private GameObject _menuToOpen;
        [SerializeField] private InputAction _openCloseAction;

        public bool IsOpened { get; private set; }

        private void Awake()
        {
            IsOpened = _menuToOpen.activeSelf;
        }

        private void OnEnable()
        {
            _openCloseAction.Enable();
            _openCloseAction.performed += Toggle;
        }

        private void OnDisable()
        {
            _openCloseAction.Disable();
            _openCloseAction.performed -= Toggle;
        }

        private void Toggle(InputAction.CallbackContext callbackContext)
        {
            // Debug.Log($"MenuOpenClose toggle {SM.Instance<InputManager>().CursorShowed}");
            if (SM.Instance<InputManager>().CursorShowed && IsOpened == false)
            {
                foreach (MenuOpenClose thisopener in FindObjectsOfType<MenuOpenClose>())
                {
                    thisopener.Close();
                }
            }


            IsOpened = !IsOpened;
            _menuToOpen.SetActive(IsOpened);
            SM.Instance<PlayerController>().FPSCamera.enabled = !IsOpened;
            SM.Instance<InputManager>().ChangeCursorState(IsOpened);
        }

        public void Close()
        {
            if (IsOpened)
            {
                IsOpened = false;
                _menuToOpen.SetActive(IsOpened);
            }
        }
    }
}