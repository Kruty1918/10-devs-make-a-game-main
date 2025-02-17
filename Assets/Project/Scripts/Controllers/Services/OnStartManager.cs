using Bonjoura.Player;
using UnityEngine;

namespace Bonjoura.Services
{
    public sealed class OnStartManager : MonoBehaviour
    {
        [Header("Cursor")]
        [SerializeField] private bool hideCursorByStart = true;

        private void Awake()
        {
            if (hideCursorByStart) InputManager.Instance.HideCursor();
            else InputManager.Instance.ShowCursor();
        }
    }
}
