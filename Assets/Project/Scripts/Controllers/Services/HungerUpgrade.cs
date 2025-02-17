using Bonjoura.Player;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.Services
{
    public class HungerUpgrade : MonoBehaviour
    {
        [SerializeField] private float _hungerMultiplier = 0.5f;

        public void Upgrade()
        {
            SM.Instance<PlayerController>().PlayerHungerSystem.RecountHunger *= _hungerMultiplier;
        }
    }
}