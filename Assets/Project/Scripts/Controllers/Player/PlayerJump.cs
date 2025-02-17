using SGS29.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bonjoura.Player
{
    public sealed class PlayerJump : MonoBehaviour
    {
        private float jumpForce = 1;
        private void Jumping(InputAction.CallbackContext ob)
        {
            if (PlayerController.Instance.PlayerMoving.IsGrounded)
                PlayerController.Instance.PlayerMoving.AddVelocityY(Mathf.Sqrt(PlayerController.Instance.PlayerData.JumpForce * jumpForce * -2f * PlayerController.Instance.PlayerData.GravityForce));
        }

        private void OnEnable()
        {
            SM.Instance<InputManager>().Player.Jump.started += Jumping;
        }

        private void OnDisable()
        {
            if (SM.HasSingleton<InputManager>())
                SM.Instance<InputManager>().Player.Jump.started -= Jumping;
        }
        public void UpdateMovingJump(float _jump)
        {
            jumpForce = _jump;
        }
    }
}

