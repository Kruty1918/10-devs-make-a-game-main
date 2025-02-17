using Bonjoura.Services;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Player
{
    public sealed class PlayerHealth : Health
    {
        void Awake()
        {
            OnDieEvent += PlayerDeath;
        }


        public void PlayerDeath(string reason)
        {
            _losePanel.Open(true);
            _quickTipText.text = PosthumousTipGenerator.Instance.GenerateQuickTip(reason);
            SM.Instance<InputManager>().ChangeCursorState(true);
            SM.Instance<PlayerController>().FPSCamera.enabled = !SM.Instance<PlayerController>().FPSCamera.enabled;
            Time.timeScale = 0;
        }
    }
}