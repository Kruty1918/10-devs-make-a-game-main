using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bonjoura.UI
{
    /// <summary>
    /// Controls the opening and closing of the menu, along with game state changes when toggling the menu.
    /// </summary>
    public class MenuOpenClose : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private GameObject _menuToOpen;
        [SerializeField] private InputAction _openCloseAction;

        [Space()]
        [Header("States")]
        [SerializeField] private GameState openGameState = GameState.Paused; // State when the menu is opened
        [SerializeField] private GameState closeGameState = GameState.Played; // State when the menu is closed

        public bool IsOpened { get; private set; }

        private void Awake()
        {
            // Initialize the IsOpened state based on the menu's current visibility
            IsOpened = _menuToOpen.activeSelf;
        }

        private void OnEnable()
        {
            // Enable the input action and subscribe to its event
            _openCloseAction.Enable();
            _openCloseAction.performed += Toggle;
        }

        private void OnDisable()
        {
            // Disable the input action and unsubscribe from its event
            _openCloseAction.Disable();
            _openCloseAction.performed -= Toggle;
        }

        /// <summary>
        /// Toggles the menu's visibility and changes the game state accordingly.
        /// </summary>
        private void Toggle(InputAction.CallbackContext callbackContext)
        {
            // Toggle the menu visibility
            IsOpened = !IsOpened;
            _menuToOpen.SetActive(IsOpened);

            // Enable or disable FPS camera based on the menu state
            SM.Instance<PlayerController>().FPSCamera.enabled = !IsOpened;

            // Change the cursor state based on whether the menu is opened or closed
            SM.Instance<InputManager>().ChangeCursorState(IsOpened);

            // Set the game state based on whether the menu is opened or closed
            GameStates.SetState(IsOpened ? openGameState : closeGameState);
        }

        /// <summary>
        /// Closes the current menu if it's opened.
        /// </summary>
        public void Close()
        {
            if (IsOpened)
            {
                IsOpened = false;
                _menuToOpen.SetActive(false);

                // Set the game state to the "close" state when the menu is closed
                GameStates.SetState(closeGameState);
            }
        }
    }
}