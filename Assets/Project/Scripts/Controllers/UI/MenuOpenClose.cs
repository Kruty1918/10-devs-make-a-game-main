using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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

        // Static list to track active panels
        private static List<MenuOpenClose> activePanels = new List<MenuOpenClose>();

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
            // If the menu is being opened, close all other active panels first
            if (!IsOpened)
            {
                CloseAllOtherPanels();
            }

            // Toggle the menu visibility
            IsOpened = !IsOpened;
            _menuToOpen.SetActive(IsOpened);

            // Enable or disable FPS camera based on the menu state
            SM.Instance<PlayerController>().FPSCamera.enabled = !IsOpened;

            // Change the cursor state based on whether the menu is opened or closed
            SM.Instance<InputManager>().ChangeCursorState(IsOpened);

            // Set the game state based on whether the menu is opened or closed
            GameStates.SetState(IsOpened ? openGameState : closeGameState);

            // If the menu is opened, add this panel to the active list
            if (IsOpened)
            {
                activePanels.Add(this);
            }
            else
            {
                // If the menu is closed, remove this panel from the active list
                activePanels.Remove(this);
            }
        }

        /// <summary>
        /// Closes all active panels except for the current one.
        /// </summary>
        private void CloseAllOtherPanels()
        {
            // Create a copy of the active panels list to iterate over
            var panelsToClose = new List<MenuOpenClose>(activePanels);

            // Loop through the copied list and close each panel
            foreach (var panel in panelsToClose)
            {
                if (panel != this)
                {
                    panel.Close();
                }
            }
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

                // Remove this panel from the active list
                activePanels.Remove(this);
            }
        }
    }
}
