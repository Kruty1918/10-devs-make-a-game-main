using Bonjoura.Player;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SGS29.Utilities;

namespace Bonjoura.Services
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer = false;

        [Header("Health Settings")]
        [SerializeField] private float maximumHealth;
        [SerializeField] private float currentHealth;

        [SerializeField] private float cooldownHeal;
        [SerializeField] private float cooldownDamage;

        [Header("UI Elements")]
        [SerializeField] private Slider _healthBar;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private TextMeshProUGUI _quickTipText;

        private bool _isCanHeal = true;
        private bool _isCanDamage = true;

        private float _cooldownHealDelay;
        private float _cooldownDamageDelay;

        public float CurrentHealth => currentHealth;
        public float MaximumHealth => maximumHealth;

        public event Action OnHealEvent;
        public event Action OnDamageEvent;
        public event Action OnDieEvent;
        public event Action OnValueChange;

        private void Start()
        {
            InitializeHealth();
        }

        private void InitializeHealth()
        {
            currentHealth = maximumHealth;
            UpdateHealthBar();
        }

        public bool Heal(int value)
        {
            if (!CanHeal(value)) return false;
            ApplyHealing(value);
            return true;
        }

        private bool CanHeal(int value)
        {
            return currentHealth < maximumHealth && _isCanHeal && Timer.SimpleTimer(_cooldownHealDelay, cooldownHeal);
        }

        private void ApplyHealing(int value)
        {
            _cooldownHealDelay = Time.time;
            currentHealth = Mathf.Clamp(currentHealth + value, 0, maximumHealth);
            _isCanHeal = false;

            OnHealEvent?.Invoke();
            OnValueChange?.Invoke();
            UpdateHealthBar();
        }

        public bool Damage(int value, string reason)
        {
            if (!CanTakeDamage()) return false;
            ApplyDamage(value, reason);
            return true;
        }

        private bool CanTakeDamage()
        {
            return _isCanDamage && Timer.SimpleTimer(_cooldownDamageDelay, cooldownDamage);
        }

        private void ApplyDamage(int value, string reason)
        {
            _cooldownDamageDelay = Time.time;
            currentHealth = Mathf.Clamp(currentHealth - value, 0, maximumHealth);

            OnDamageEvent?.Invoke();
            OnValueChange?.Invoke();
            UpdateHealthBar();

            if (currentHealth <= 0)
            {
                HandleDeath(reason);
            }
        }

        private void HandleDeath(string reason)
        {
            OnDieEvent?.Invoke();
            _isCanDamage = false;

            if (_isPlayer)
            {
                PlayerHealth playerHealth = GetComponent<PlayerHealth>();
                playerHealth.PlayerDeath(_losePanel, _quickTipText, reason);
            }
        }

        public void SetMaximumHealth(int value)
        {
            maximumHealth = value;
            currentHealth = value;
            UpdateHealthBar();
        }

        private void Update()
        {
            if (_isPlayer && ShouldAutoHeal())
            {
                Heal(14);
            }
        }

        private bool ShouldAutoHeal()
        {
            var player = SM.Instance<PlayerController>();
            return player.PlayerHungerSystem.CurrentHunger >= 80 &&
                   player.PlayerTemperatureSystem.Temperature > 30f &&
                   player.PlayerTemperatureSystem.Temperature < 44f &&
                   Timer.SimpleTimer(_cooldownHealDelay, cooldownHeal);
        }

        private void UpdateHealthBar()
        {
            if (_healthBar != null)
            {
                _healthBar.maxValue = maximumHealth;
                _healthBar.value = currentHealth;
            }
        }
    }
}
