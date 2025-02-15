using Bonjoura.Inventory;
using Bonjoura.Services;
using Bonjoura.Triggers;
using UnityEngine;

namespace Bonjoura.Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        private static PlayerController _playerController;

        public static PlayerController Instance => _playerController;
        
        [Header("Data")]
        [SerializeField] private PlayerData playerData;
        
        [Header("References")] 
        [SerializeField] private PlayerMoving playerMoving;
        [SerializeField] private PlayerJump playerJump;
        [SerializeField] private FPSCamera fpsCamera;
        [SerializeField] private ItemInventory itemInventory;
        [SerializeField] private InteractRaycast interactRaycast;
        [SerializeField] private PlayerHealth health;
        [SerializeField] private PlayerHungerSystem playerHungerSystem;
        [SerializeField] private PlayerTemperature playerTemperatureSystem;

        [SerializeField] private Experience XPScript;
        [SerializeField] private ParticleSystem XPparticle;
        
        public PlayerData PlayerData => playerData;
        public FPSCamera FPSCamera => fpsCamera;
        
        public PlayerHungerSystem PlayerHungerSystem => playerHungerSystem;
        public PlayerTemperature PlayerTemperatureSystem => playerTemperatureSystem;
        
        public PlayerMoving PlayerMoving => playerMoving;
        public PlayerJump PlayerJump => playerJump;
        public ItemInventory ItemInventory => itemInventory;
        public InteractRaycast InteractRaycast => interactRaycast;
        public PlayerHealth Health => health;


        public ParticleSystem GetXPParticle() => XPparticle;
        public Experience GetExperienceScript() => XPScript;
        private void Awake()
        {
            if (_playerController)
            {
                Destroy(this);
                return;
            }
            _playerController = this;
        }
    }
}