using SGS29.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bonjoura.Player
{
    public sealed class PlayerJump : MonoBehaviour
    {
        private float jumpMultiplier = 1f;

        private void Jumping(InputAction.CallbackContext context)
        {
            var playerController = SM.Instance<PlayerController>();
            var playerMoving = playerController.PlayerMoving;

            if (playerMoving.IsGrounded)
            {
                float jumpForce = Mathf.Sqrt(playerController.PlayerData.JumpForce * jumpMultiplier * -2f * playerController.PlayerData.GravityForce);
                playerMoving.AddVelocityY(jumpForce);
            }
        }

        private void OnEnable()
        {
            SM.Instance<InputManager>().Player.Jump.started += Jumping;
        }

        private void OnDisable()
        {
            if (SM.HasSingleton<InputManager>())
            {
                SM.Instance<InputManager>().Player.Jump.started -= Jumping;
            }
        }

        /// <summary>
        /// Оновлює множник сили стрибка.
        /// </summary>
        public void UpdateJumpMultiplier(float multiplier)
        {
            jumpMultiplier = multiplier;
        }
    }
}