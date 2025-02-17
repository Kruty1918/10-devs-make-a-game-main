using UnityEngine;
using UnityEngine.AI;
using Bonjoura.Player;
using Bonjoura.Services;

namespace Bonjoura.AI
{
    public class MimicMovement : MonoBehaviour
    {
        // The same logic as MobMovement but without Peaceful branch (+few exception)

        [SerializeField] private CharacterController _characterController;

        [SerializeField] private Transform _player;
        [SerializeField] private Animator _mobAnimator;
        [SerializeField] private float _detectionRadius = 10f;
        [SerializeField] private float _attackRadius = 5f;
        [SerializeField] private int _attackDamage = 10;
        [SerializeField] private float _patrolRadius = 15f;
        [SerializeField] private float _waitTime = 2f;
        [SerializeField] private float _maxTimeToReachTarget = 5f;

        [SerializeField] private GameObject _particleSystemPrefab;



        private NavMeshAgent _agent;
        //private bool _isWaiting; never used
        //private bool _isChasing; never used
        private bool _isAttacking;
        private Vector3 _currentTarget;
        private float _speed;
        private float _timeSinceLastPathUpdate;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _speed = _agent.speed;
            _player = PlayerController.Instance.GetComponentInChildren<CharacterController>().transform;
            _characterController = _player.GetComponent<CharacterController>();
            _mobAnimator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            EnemyMovement();


            if (_agent.hasPath && _agent.remainingDistance > 0.1f)
            {
                _timeSinceLastPathUpdate += Time.deltaTime;

                if (_timeSinceLastPathUpdate > _maxTimeToReachTarget)
                {
                    _timeSinceLastPathUpdate = 0f;
                    _currentTarget = GetRandomNavMeshPoint();

                    _agent.SetDestination(_currentTarget);
                }
            }
            else
            {
                _timeSinceLastPathUpdate = 0f;
            }
        }

        private void EnemyMovement()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (distanceToPlayer <= _detectionRadius && !_isAttacking)
            {
                if (distanceToPlayer <= _attackRadius)
                {
                    Attack();
                }
                else
                {
                    _mobAnimator.SetBool("Attack", false);
                    //_isChasing = true;
                    //_isWaiting = false;
                    _currentTarget = _player.position;
                    _agent.speed = _speed;
                    _agent.SetDestination(_currentTarget);
                    _mobAnimator.SetBool("isMoving", true);
                    Vector3 direction = (_player.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }
            }
            else if (distanceToPlayer > _detectionRadius)
            {
                _isAttacking = false;
                //_isChasing = false;
                _mobAnimator.SetBool("isMoving", false);
            }
            else if (_isAttacking)
            {
                // Rotate mimic when attack
                Vector3 direction = (_player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        private void Attack()
        {
            if (_isAttacking) return;
            _isAttacking = true;
            _mobAnimator.SetBool("Attack", true);
            _mobAnimator.SetBool("isMoving", false);
        }

        private Vector3 GetRandomNavMeshPoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _patrolRadius + transform.position;

            return NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _patrolRadius, NavMesh.AllAreas)
                ? hit.position
                : transform.position;
        }
    }
}