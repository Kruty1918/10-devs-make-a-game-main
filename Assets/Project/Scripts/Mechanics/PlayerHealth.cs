using Bonjoura.Managers;
using Bonjoura.Services;
using UnityEngine;

namespace Bonjoura.Player
{
    public sealed class PlayerHealth : Health
    {
        public void PlayerDeth(GameObject LosePanle, TMPro.TextMeshProUGUI quickTipText, string reason)
        {
            LosePanle.SetActive(true);
            quickTipText.text = PosthumousTipGenerator.Instance.GenerateQuickTip(reason);
            InputManager.Instance.ChangeCursorState(true);
            PlayerController.Instance.FPSCamera.enabled = !PlayerController.Instance.FPSCamera.enabled;
            Time.timeScale = 0;
        }
    }
}