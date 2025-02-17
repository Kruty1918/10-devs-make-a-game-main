using Bonjoura.AI;
using Bonjoura.Services;
using UnityEngine;

namespace Bonjoura.Enemy
{
    public class ImpactForce : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private MobMovement mobMovement;
        [SerializeField] private float multiplierX = 15f;
        [SerializeField] private float multiplierY = 5f;

        void Awake()
        {
            health.OnDamageEvent += OnDamage;
        }

        private void OnDamage()
        {
            Vector3 direction = PlayerMathf.DirectionFrom(health.transform.position);
            direction = new Vector3(direction.x, multiplierY, direction.z);
            mobMovement.AddForce(direction * multiplierX, ForceMode.Impulse);
        }
    }
}