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
            ToggleMenu();
        }

        /// <summary>
        /// Opens or closes the menu.
        /// </summary>
        public void ToggleMenu()
        {
            if (IsOpened)
                Close();
            else
                Open(true);
        }

        /// <summary>
        /// Opens the menu. Optionally closes all other active panels.
        /// </summary>
        /// <param name="closeOthers">If true, closes all other open menus.</param>
        public void Open(bool closeOthers)
        {
            if (IsOpened) return;

            if (closeOthers)
            {
                CloseAllOtherPanels();
            }

            IsOpened = true;
            _menuToOpen.SetActive(true);

            SM.Instance<PlayerController>().FPSCamera.enabled = false;
            SM.Instance<InputManager>().ChangeCursorState(true);

            GameStates.SetState(openGameState);
            activePanels.Add(this);
        }

        /// <summary>
        /// Closes the menu if it's open.
        /// </summary>
        public void Close()
        {
            if (!IsOpened) return;

            IsOpened = false;
            _menuToOpen.SetActive(false);

            SM.Instance<PlayerController>().FPSCamera.enabled = true;
            SM.Instance<InputManager>().ChangeCursorState(false);

            GameStates.SetState(closeGameState);
            activePanels.Remove(this);
        }

        /// <summary>
        /// Closes all active panels except for the current one.
        /// </summary>
        private void CloseAllOtherPanels()
        {
            var panelsToClose = new List<MenuOpenClose>(activePanels);

            foreach (var panel in panelsToClose)
            {
                if (panel != this)
                {
                    panel.Close();
                }
            }
        }
    }
}
