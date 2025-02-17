using Bonjoura.Enemy;
using Bonjoura.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bonjoura.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float _attackDistance = 3.5f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private LayerMask _enemyLayer;

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Attack();
            }
        }

        private void Attack()
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, _attackDistance);

            foreach (Collider enemy in enemies)
            {
                if (enemy.GetComponent<EnemyHealth>() != null)
                {
                    enemy.GetComponent<Health>().Damage(_damage, "player");
                }
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 boxCenter = transform.position + transform.forward * _attackDistance;
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
            Gizmos.DrawSphere(Vector3.zero, _attackDistance);
        }
#endif
    }
}