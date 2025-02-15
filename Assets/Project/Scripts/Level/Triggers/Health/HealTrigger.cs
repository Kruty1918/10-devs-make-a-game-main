using Bonjoura.Enemy;
using Bonjoura.Player;
using Bonjoura.Services;
using UnityEngine;

namespace Bonjoura.Triggers
{
    public sealed class HealTrigger : MonoBehaviour
    {
        [Header("Health Parametrs")]
        [SerializeField] private HealthType healthType = HealthType.Player;

        [Header("Fields")]
        [SerializeField] private int valueHealth;

        [Header("Trigger Parametrs")]
        [SerializeField] private bool onTriggerEnter = true;
        [SerializeField] private bool onTriggerStay;
        [SerializeField] private bool onTriggerExit;

        private void HealthAction(Health health)
        {
            health.Heal(valueHealth);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!onTriggerEnter) return;
            var playerHealthComponent = collider.GetComponent<PlayerHealth>();
            var enemyHealthComponent = collider.GetComponent<EnemyHealth>();
            
            switch (healthType)
            {
                case HealthType.Player:
                if (playerHealthComponent != null) HealthAction(playerHealthComponent);
                break;

                case HealthType.Enemy:
                if (enemyHealthComponent != null) HealthAction(enemyHealthComponent);
                break;
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            if (!onTriggerStay) return;
            var playerHealthComponent = collider.GetComponent<PlayerHealth>();
            var enemyHealthComponent = collider.GetComponent<EnemyHealth>();
            
            switch (healthType)
            {
                case HealthType.Player:
                if (playerHealthComponent != null) HealthAction(playerHealthComponent);
                break;

                case HealthType.Enemy:
                if (enemyHealthComponent != null) HealthAction(enemyHealthComponent);
                break;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (!onTriggerExit) return;
            var playerHealthComponent = collider.GetComponent<PlayerHealth>();
            var enemyHealthComponent = collider.GetComponent<EnemyHealth>();
            
            switch (healthType)
            {
                case HealthType.Player:
                if (playerHealthComponent != null) HealthAction(playerHealthComponent);
                break;

                case HealthType.Enemy:
                if (enemyHealthComponent != null) HealthAction(enemyHealthComponent);
                break;
            }
        }
    }
}