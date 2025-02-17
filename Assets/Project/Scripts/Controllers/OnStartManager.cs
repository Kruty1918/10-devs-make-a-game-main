using UnityEngine;

namespace Bonjoura.Managers
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
