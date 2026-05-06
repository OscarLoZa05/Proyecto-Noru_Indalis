using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class DronAttackAI : MonoBehaviour
{
    private NavMeshAgent _enemyAgent;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _originPlayer;
    public enum EnemyState
    {
        Chasing,
        Attacking,
    }

    //Chasing
    [SerializeField] private float _detectionRange = 15f;

    //Attack
    [SerializeField] private float _attackRange = 12.5f;
    [SerializeField] private float _attackTimer;
    [SerializeField] private float _attackDelay = 2;

    //Player
    private Transform _player;

    public EnemyState currentState;
    
    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        currentState = EnemyState.Chasing;
        _attackTimer = _attackDelay;
    }

    void Update()
    {
        switch(currentState)
        {
            case EnemyState.Chasing:
                Chasing();
            break;
            case EnemyState.Attacking:
                Attacking();
            break;
            default:
                Chasing();
            break;

        }
    }

    void Chasing()
    {
        if(OnRange(_detectionRange))
        {
            _enemyAgent.isStopped = false;
            _enemyAgent.SetDestination(_player.position);
        }
        if(OnRange(_attackRange))
        {
            currentState = EnemyState.Attacking;
        }
    }

    void Attacking()
    {
        _enemyAgent.isStopped = true;
        
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= _attackDelay)
        {
            Attack();
        }        
    }

    void Attack()
    {
        // 1. Calculas la dirección (Correcto)
        Vector3 direction = (_originPlayer.position - _bulletSpawn.position).normalized;

        // 2. Transformas esa dirección en una rotación real (Corregido)
        // Esto hace que el frente de la bala (eje Z) apunte a la dirección calculada
        Quaternion directionQ = Quaternion.LookRotation(direction);

        // 3. Pides el objeto a la Pool (Correcto)
        GameObject bullet = PoolManager.Instance.GetPooledObject("DronBullet", _bulletSpawn.position, directionQ);

        bullet.SetActive(true);
        currentState = EnemyState.Chasing;
        Debug.Log("Attack");
        _attackTimer = 0;
    }

    public bool OnRange(float distance)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        
        if(distanceToPlayer <= distance)
        {
         return true;    
        }
        else
        {
            return false;
        }  
    }

    void TakeDamage(int damage)
    {
        //_currentLife -= damage;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
