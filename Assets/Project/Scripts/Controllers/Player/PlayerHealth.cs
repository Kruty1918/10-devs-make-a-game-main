using Bonjoura.Services;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Player
{
    public sealed class PlayerHealth : Health
    {
        public void PlayerDeath(GameObject LosePanel, TMPro.TextMeshProUGUI quickTipText, string reason)
        {
            LosePanel.SetActive(true);
            quickTipText.text = PosthumousTipGenerator.Instance.GenerateQuickTip(reason);
            SM.Instance<InputManager>().ChangeCursorState(true);
            PlayerController.Instance.FPSCamera.enabled = !PlayerController.Instance.FPSCamera.enabled;
            Time.timeScale = 0;
        }
    }
}