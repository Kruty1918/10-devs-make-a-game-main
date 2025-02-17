using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Services
{
    public sealed class OnStartManager : MonoBehaviour
    {
        [Header("Cursor")]
        [SerializeField] private bool hideCursorByStart = true;

        private void Awake()
        {
            if (hideCursorByStart) SM.Instance<InputManager>().HideCursor();
            else SM.Instance<InputManager>().ShowCursor();
        }
    }
}
