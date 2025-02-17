using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Bonjoura.Player;
using SGS29.Utilities;

namespace Bonjoura.AI
{
    public class MobMovement : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Animator _mobAnimator;
        [SerializeField] private bool _isEnemyMob = false;
        [SerializeField] private float _detectionRadius = 10f;
        [SerializeField] private float _patrolRadius = 15f;
        [SerializeField] private float _waitTime = 2f;
        [SerializeField] private float _escapeDistance = 10f;
        [SerializeField] private float _maxTimeToReachTarget = 5f;

        private NavMeshAgent _agent;
        private bool _isWaiting;
        private bool _isChasing;
        private Vector3 _currentTarget;
        private float _speed;
        private float _timeSinceLastPathUpdate;

        [Header("SFX")]
        [SerializeField] private SFXPoolManager _sfxPoolManager;
        [SerializeField] private SfxSO _mobSpetialSound;
        [SerializeField] private SfxSO _noticedThePlayer;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _speed = _agent.speed;
            _player = SM.Instance<PlayerController>().GetComponentInChildren<CharacterController>().transform;
            _mobAnimator = GetComponent<Animator>();
            _sfxPoolManager = GetComponentInChildren<SFXPoolManager>();
            StartCoroutine(PatrolRoutine());
        }

        private void Update()
        {
            if (_isEnemyMob)
            {
                EnemyMovement();
            }
            else
            {
                PeacefulMobMovement();
            }

            if (_agent.hasPath && _agent.remainingDistance > 0.1f)
            {
                _timeSinceLastPathUpdate += Time.deltaTime;

                if (_timeSinceLastPathUpdate > _maxTimeToReachTarget)
                {
                    _timeSinceLastPathUpdate = 0f;
                    _currentTarget = GetRandomNavMeshPoint();
                    _agent.SetDestination(_currentTarget);
                    _mobAnimator.SetBool("isRunning", true);
                }
            }
            else
            {
                _timeSinceLastPathUpdate = 0f;
                _mobAnimator.SetBool("isRunning", false);
            }
        }

        private void PeacefulMobMovement()
        {
            _player ??= SM.Instance<PlayerController>().transform;
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (distanceToPlayer <= _detectionRadius)
            {
                _isChasing = true;
                _mobAnimator.SetBool("isRunning", true);
                EscapeFromPlayer();
            }
            else
            {
                if (_isChasing)
                {
                    _isChasing = false;
                    _mobAnimator.SetBool("isRunning", false);
                    StartCoroutine(PatrolRoutine());
                }
                else if (!_isWaiting && !_agent.pathPending && _agent.remainingDistance < 0.5f)
                {
                    _mobAnimator.SetBool("isRunning", false);
                    StartCoroutine(PatrolRoutine());
                }
            }
        }

        private void EnemyMovement()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (distanceToPlayer <= _detectionRadius)
            {
                if (!_isChasing) //this need to be here, for only 1 sound effect and not 1 on each frame
                {
                    if (_noticedThePlayer != null) //You can just don`t asign the sound in Refs, so then it will not be played.
                        _sfxPoolManager.GiveSFXSourceToTheObject(_noticedThePlayer, gameObject);//SoundEffect
                    else
                        Debug.Log("Sound, that you try to play is NULL. But its ok, if you don`t want to play sound for this mob :)");
                }
                _isChasing = true; //bcs then _isChasing = true, so condition above will not work each frame.
                _isWaiting = false;
                _currentTarget = _player.position;
                _agent.speed = _speed;
                _agent.SetDestination(_currentTarget);

                Vector3 direction = (_player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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

        private void EscapeFromPlayer()
        {
            Vector3 escapeDirection = (transform.position - _player.position).normalized;
            Vector3 escapePoint = transform.position + escapeDirection * _escapeDistance;

            Vector3 finalEscapePoint = GetRandomNavMeshPointFrom(escapePoint, _escapeDistance);
            _agent.speed = _speed * 2;
            _agent.SetDestination(finalEscapePoint);
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

        private Vector3 GetRandomNavMeshPointFrom(Vector3 point, float radius)
        {
            if (NavMesh.SamplePosition(point, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return transform.position;
        }
    }
}