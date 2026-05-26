using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using System.Collections;

public class AperionAI : MonoBehaviour, IEnemy
{
    public enum EnemyState 
    { 
        Patrolling, 
        Chasing, 
        Attacking, 
        Dead 
    }
    
    [Header("State Machine")]
    public EnemyState currentState;

    private NavMeshAgent _enemyAgent;
    private Transform _player;
    private Animator _animator;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _patrolPoints;
    
    [Header("Ranges")]
    [SerializeField] private float _detectionRange = 7f;
    [SerializeField] private float _attackRange = 2f;

    [Header("Attack Settings")]
    [SerializeField] private float _attackDelay = 5f;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private float _attackRadius = 5f;
    [SerializeField] private int _damage = 25;
    private float _attackTimer;
    private bool _isAttackingAnimation = false; // Controla si la animación está en curso

    [Header("Life Settings")]
    public int _currentLife;
    [SerializeField] private int _maxLife = 150;
    private bool _isDead = false;

    [Header("Drone Settings")]
    [SerializeField] private bool _dronUsed = false;
    [SerializeField] private int _distanceToDron = 25;

    [Header("VFX")]
    public VisualEffect VFXGraph;

    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;
    }

    void Start()
    {
        _currentLife = _maxLife;
        currentState = EnemyState.Patrolling;
        _enemyAgent.speed = 4;
        _attackTimer = _attackDelay;

        if (_patrolPoints != null && _patrolPoints.Length > 0)
        {
            PatrollingPoints();
        }
    }

    void Update()
    {
        if (_isDead) return;

        if (_currentLife <= 0)
        {
            ChangeState(EnemyState.Dead);
            return;
        }

        // Si está en medio de la animación de golpe, forzamos velocidad 0 absoluta y cortamos el Update
        if (_isAttackingAnimation)
        {
            _enemyAgent.isStopped = true;
            _enemyAgent.velocity = Vector3.zero;
            return; 
        }

        float distanceToPlayer = (_player != null) ? Vector3.Distance(transform.position, _player.position) : float.MaxValue;

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrolling(distanceToPlayer);
                break;
            case EnemyState.Chasing:
                Chasing(distanceToPlayer);
                break;
            case EnemyState.Attacking:
                Attacking(distanceToPlayer);
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;
        if (newState == EnemyState.Dead)
        {
            Dead();
        }
    }

    void Patrolling(float distanceToPlayer)
    {
        if (distanceToPlayer <= _detectionRange)
        {
            currentState = EnemyState.Chasing;
            _enemyAgent.speed = 9;
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsCharging", false);
            return;
        }

        if (!_enemyAgent.pathPending && _enemyAgent.remainingDistance < 0.5f)
        {
            PatrollingPoints();
        }
    }

    void PatrollingPoints()
    {
        if (_patrolPoints == null || _patrolPoints.Length == 0) return;
        _enemyAgent.SetDestination(_patrolPoints[Random.Range(0, _patrolPoints.Length)].position);
    }

    void Chasing(float distanceToPlayer)
    {
        if (distanceToPlayer > _detectionRange)
        {
            currentState = EnemyState.Patrolling;
            _enemyAgent.speed = 4;
            _animator.SetBool("IsRunning", false);
            return;
        }

        if (distanceToPlayer <= _attackRange)
        {
            currentState = EnemyState.Attacking;
            return;
        }

        if (_player != null) _enemyAgent.SetDestination(_player.position);
    }

    void Attacking(float distanceToPlayer)
    {
        // Si no está atacando físicamente y el jugador se escapó, volvemos a perseguir
        if (distanceToPlayer > _attackRange)
        {
            _enemyAgent.isStopped = false;
            currentState = EnemyState.Chasing;
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsCharging", false);
            _enemyAgent.speed = 9;
            return;
        }

        _enemyAgent.isStopped = true;
        _enemyAgent.velocity = Vector3.zero; // Freno inmediato de la inercia previa
        _animator.SetBool("IsRunning", false);
        _animator.SetBool("IsCharging", true);
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackDelay)
        {
            _animator.SetTrigger("IsAttacking");
            _attackTimer = 0; 
        }
    }

    // =================================================================
    // EVENTOS DE ANIMACIÓN (Configúralos en la línea de tiempo del clip)
    // =================================================================

    // 1. Pon este evento en el FRAME 0 (Inicio de la animación de golpe)
    public void StartAttackAnimation()
    {
        _isAttackingAnimation = true;
        _enemyAgent.isStopped = true;
        _enemyAgent.velocity = Vector3.zero;
    }

    // 2. Este es tu evento actual (Frame del impacto visual)
    public void Attack()
    {
        if (_isDead) return;

        _detectionRange = 20;
        if (_attackPosition == null) return;

        Collider[] players = Physics.OverlapSphere(_attackPosition.position, _attackRadius);
        foreach (Collider item in players)
        {
            if (item.gameObject.CompareTag("Player"))
            {
                PlayerResources playerResources = item.GetComponent<PlayerResources>();
                if (playerResources != null)
                {
                    playerResources.TakeDamage(_damage);
                    Help();
                }
            }
        }
    }

    // 3. Pon este evento en el ÚLTIMO FRAME (Fin de la animación de golpe)
    public void EndAttackAnimation()
    {
        _isAttackingAnimation = false;
        _enemyAgent.isStopped = false;
    }

    // =================================================================

    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        _detectionRange = 100;
        _currentLife -= damage;
    }

    void Help()
    {   
        if (_dronUsed) return;
        
        Collider[] drones = Physics.OverlapSphere(transform.position, _distanceToDron);
        Transform mostNear = null;
        float minDistance = float.MaxValue;
            
        foreach (Collider defenser in drones)
        {
            if (defenser.gameObject.CompareTag("Defensor"))
            {      
                float distance = Vector3.Distance(transform.position, defenser.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    mostNear = defenser.transform;
                }
            }
        }

        if (mostNear != null)
        {
            DronDefensorAI dronDefensorAI = mostNear.GetComponent<DronDefensorAI>();
            if (dronDefensorAI != null)
            {
                dronDefensorAI.target = transform;
                dronDefensorAI.Prepare(mostNear);
                _dronUsed = true;
            }
        }
    }

    void Dead()
    {
        if (_isDead) return;
        _isDead = true;

        if (PlayerData.Instance != null) PlayerData.Instance.currentNoru += 15;
        if (VFXGraph != null) VFXGraph.Play();

        _animator.SetTrigger("IsDead");
        
        _enemyAgent.isStopped = true;
        _enemyAgent.enabled = false; 

        StartCoroutine(Destruccion());
    }

    IEnumerator Destruccion()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Arrow"))
        {
            collider.gameObject.SetActive(false);
            TakeDamage(20);   
        }
        if (collider.gameObject.CompareTag("Fire"))
        {
            TakeDamage(100);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        if (_attackPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_attackPosition.position, _attackRadius);
        }

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _distanceToDron);
    
        if (_patrolPoints != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform point in _patrolPoints)
            {
                if (point != null) Gizmos.DrawWireSphere(point.position, 1f);
            }
        }
    }
}