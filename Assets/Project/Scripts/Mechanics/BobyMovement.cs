using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Bonjoura.Player;

public class BobyMovement : MonoBehaviour
{
    // The same logic as MobMovement but without Peaceful branch (+ few exception)
    //-Aaayooo ! Its 9-th dev. I added more exceptions for SFX (blowingUp)
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    [SerializeField] private Transform _player;
    [SerializeField] private Animator _mobAnimator;
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private float _patrolRadius = 15f;
    [SerializeField] private float _waitTime = 2f;
    [SerializeField] private float _maxTimeToReachTarget = 5f;
    [SerializeField] private float _distanceToDetonate = 2f;

    [Header("Material")]
    [SerializeField] private Material _materialRedEmission;
    [SerializeField] private float _emissionPower = -5f;

    [SerializeField] private GameObject _particleSystemPrefab;

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
        _player = PlayerController.Instance.GetComponentInChildren<CharacterController>().transform;
        _characterController = _player.GetComponent<CharacterController>();
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
                _sfxPoolManager.GiveSFXSourceToTheObject(_explosionSFX, gameObject);//SoundEffect
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

   

    public void ApplyExplosionForce(CharacterController controller, Vector3 explosionPosition, float explosionForce, float explosionRadius)
    {
        Vector3 direction = controller.transform.position - explosionPosition; 
        float distance = direction.magnitude;

        if (distance < explosionRadius)
        {
            float force = Mathf.Lerp(explosionForce, 0, distance / explosionRadius); 
            Vector3 explosionVelocity = direction.normalized * force;

            StartCoroutine(Knockback(controller, explosionVelocity)); 
        }
    }

    IEnumerator Knockback(CharacterController controller, Vector3 force)
    {
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            controller.Move(force * Time.deltaTime); 
            yield return null;
        }
    }


    void BlowUp()
    {
        this.GetComponentInChildren<Renderer>().material = _materialRedEmission;
        
    }

    void Explode()
    {
        ApplyExplosionForce(_characterController, transform.position, _explosionForce, _explosionRadius);
        Instantiate(_particleSystemPrefab, transform.position + new Vector3 (0,2f,0),Quaternion.identity);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Collider>().enabled = false;

        Destroy(gameObject, 3.5f);
    }
}
