using Bonjoura.Services;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Player
{
    public sealed class PlayerHealth : Health
    {
        public void PlayerDeth(GameObject LosePanle, TMPro.TextMeshProUGUI quickTipText, string reason)
        {
            LosePanle.SetActive(true);
            quickTipText.text = PosthumousTipGenerator.Instance.GenerateQuickTip(reason);
            SM.Instance<InputManager>().ChangeCursorState(true);
            PlayerController.Instance.FPSCamera.enabled = !PlayerController.Instance.FPSCamera.enabled;
            Time.timeScale = 0;
        }
    }
}