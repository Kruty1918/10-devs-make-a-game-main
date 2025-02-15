using Bonjoura.Player;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Bonjoura.Services
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer = false;

        [Header("Health")]
        [SerializeField] private float maximumHealth;
        [SerializeField] private float currentHealth;

        [SerializeField] private float cooldownHeal;
        [SerializeField] private float cooldownDamage;

        [SerializeField] private Slider _healthBar;
        [SerializeField] private GameObject _losePanle;
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
            _healthBar.maxValue = MaximumHealth;
            Debug.Log("max value set to MaxHP on object: " + gameObject);
            _healthBar.value = currentHealth;
        }

        public bool Heal(int value)
        {
            if (currentHealth >= maximumHealth) return false;
            if (!_isCanHeal) return false;
            if (!Timer.SimpleTimer(_cooldownHealDelay, cooldownHeal)) return false;
            _cooldownHealDelay = Time.time;
            currentHealth += value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maximumHealth);
            OnHealEvent?.Invoke();
            OnValueChange?.Invoke();
            _isCanHeal = false;
            _healthBar.value = currentHealth;

            return true;
        }


        public bool Damage(int value, string reason)
        {
            if (!_isCanDamage) return false;
            if (!Timer.SimpleTimer(_cooldownDamageDelay, cooldownDamage)) return false;
            _cooldownDamageDelay = Time.time;
            currentHealth -= value;
            _healthBar.value = currentHealth;
            Canvas.ForceUpdateCanvases();  // trying to force update canvas                     //I have so much problems whith HP Slided update ((
            Debug.Log("HealthBar Value is set to currentHealth on object: " + gameObject);      //Idk wtf is going on
            Debug.Log("HealthBar Value Updated to: " + _healthBar.value + " On slider: " + _healthBar);
            currentHealth = Mathf.Clamp(currentHealth, 0, maximumHealth);                       //                                 9-th dev
            OnDamageEvent?.Invoke();
            OnValueChange?.Invoke();
            if (currentHealth > 0) return true;
            OnDieEvent?.Invoke();
            _isCanDamage = false;

            if (_isPlayer)
            {
                if (currentHealth <= 0)
                {
                    PlayerHealth playerHealth = gameObject.GetComponent<PlayerHealth>();
                    playerHealth.PlayerDeth(_losePanle, _quickTipText, reason);
                }
            }

            return true;
        }

        public void SetMaximumHealth(int value)
        {
            maximumHealth = value;
            currentHealth = value;
        }
        private void Update()
        {
            if (_isPlayer)
            {
                if (PlayerController.Instance.PlayerHungerSystem.CurrentHunger >= 80 && (PlayerController.Instance.PlayerTemperatureSystem.Temperature > 30f && PlayerController.Instance.PlayerTemperatureSystem.Temperature < 44f))
                {
                    if (Timer.SimpleTimer(_cooldownHealDelay, cooldownHeal)) _isCanHeal = true;
                    Heal(14); // 7 times for full health;
                }
            }
        }
    }
}