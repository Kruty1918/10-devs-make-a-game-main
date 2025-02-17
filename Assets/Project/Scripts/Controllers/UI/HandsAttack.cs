using UnityEngine;
using SGS29.Utilities;
using Bonjoura.Player;

namespace Bonjoura.UI.Inventory
{
    public class HandsAttack : MonoBehaviour
    {
        [SerializeField] private GameObject armObj;
        [SerializeField] private Animator animator;
        [SerializeField] private string animationName = "Attack";

        void Start()
        {
            SM.Instance<PlayerAttack>().OnAttack += AttackPlay;
        }

        void OnDisable()
        {
            if (SM.HasSingleton<PlayerAttack>())
                SM.Instance<PlayerAttack>().OnAttack -= AttackPlay;
        }

        private void AttackPlay()
        {
            animator.Play(animationName);
        }
    }
}
