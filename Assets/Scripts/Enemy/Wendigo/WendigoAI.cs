using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WendigoAI : MonoBehaviour
{
    private NavMeshAgent _enemyAgent;
    public enum EnemyState
    {
        Chasing,
        Charging,
        Attacking,
    }

    public EnemyState currentState;

    //Chasing
    [SerializeField] private float _detectionRange = 7f;

    //Attack
    [SerializeField] private float _attackRange = 2f;

    //Charging
    [SerializeField] private float _chargingTimer;
    [SerializeField] private float _chargingDelay = 5;

    //Player
    private Transform _player;

    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        currentState = EnemyState.Charging;
        _chargingTimer = _chargingDelay;
    }

    void Update()
    {
        switch(currentState)
        {
            case EnemyState.Chasing:
                Chasing();
            break;
            case EnemyState.Charging:
                Charging();
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
        _enemyAgent.SetDestination(_player.position);
        if(OnRange(_attackRange))
        {
            currentState = EnemyState.Attacking;
        }
    }

    void Charging()
    {
        _chargingTimer += Time.deltaTime;

        if(_chargingTimer >= _chargingDelay)
        {
            currentState = EnemyState.Attacking;
        }
    }

    void Attacking()
    {
        if(OnRange(_attackRange))
        {
            Attack();
        }
    }

    void Attack()
    {
        Debug.Log("Attack)");
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
}
