using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using SGS29.Utilities;

namespace Bonjoura.Player
{
    public class BobyMovement : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Animator _mobAnimator;
        [SerializeField] private float _detectionRadius = 10f;
        [SerializeField] private float _patrolRadius = 15f;
        [SerializeField] private float _waitTime = 2f;
        [SerializeField] private float _maxTimeToReachTarget = 5f;
        [SerializeField] private float _distanceToDetonate = 2f;

        [Header("SFX")]
        [SerializeField] private SFXPoolManager _sfxPoolManager;
        [SerializeField] private SfxSO _explosionSFX;
        [SerializeField] private SfxSO _noticedThePlayer;

        private NavMeshAgent _agent;
        private bool _isWaiting;
        private bool _isChasing;
        private bool _isBlowingUp;
        private Vector3 _currentTarget;
        private float _speed;
        private float _timeSinceLastPathUpdate;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _speed = _agent.speed;
            _player = SM.Instance<PlayerController>().GetComponentInChildren<CharacterController>().transform;
            _mobAnimator = GetComponentInChildren<Animator>();
            _sfxPoolManager = GetComponentInChildren<SFXPoolManager>();
            StartCoroutine(PatrolRoutine());
        }

        private void Update()
        {

            if (_isBlowingUp)
            {
                return;
            }
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
                _mobAnimator.SetBool("isRunning", false);
            }

        }

        private void EnemyMovement()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (distanceToPlayer <= _detectionRadius)
            {
                if (!_isChasing)
                    _sfxPoolManager.GiveSFXSourceToTheObject(_noticedThePlayer, gameObject);

                _isChasing = true;
                _isWaiting = false;
                _currentTarget = _player.position;
                _agent.speed = _speed;
                _agent.SetDestination(_currentTarget);
                _mobAnimator.SetBool("isRunning", true);
                Vector3 direction = (_player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                if (distanceToPlayer <= _distanceToDetonate)
                {
                    _isBlowingUp = true;
                    _mobAnimator.SetBool("BlowingUp", true);
                    _sfxPoolManager.GiveSFXSourceToTheObject(_explosionSFX, gameObject); //SoundEffect
                    StopAllCoroutines();
                    _agent.isStopped = true;
                }
            }
            else
            {
                if (_isChasing)
                {
                    _isChasing = false;
                    StartCoroutine(PatrolRoutine());
                }
                else if (!_isWaiting && !_agent.pathPending && _agent.remainingDistance < 0.5f)
                {
                    StartCoroutine(PatrolRoutine());
                }
            }
        }

        private IEnumerator PatrolRoutine()
        {
            _isWaiting = true;
            yield return new WaitForSeconds(Random.Range(1f, _waitTime));

            _currentTarget = GetRandomNavMeshPoint();
            _agent.speed = _speed / 2;
            _agent.SetDestination(_currentTarget);

            _isWaiting = false;

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