using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WendigoAI : MonoBehaviour
{

    private AudioSource _audioSource;
    private NavMeshAgent _enemyAgent;
    private Animator _animator;
    [SerializeField] AudioSource _audioFoots;
    [SerializeField] AudioClip _footSFX;
    [SerializeField] AudioClip _deadSFX;
    
    public enum EnemyState
    {
        Chasing,
        Charging,
        Attacking,
        Dead,
    }

    public EnemyState currentState;

    //Chasing
    [SerializeField] private float _detectionRange = 7f;

    //Attack
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackTimer;
    [SerializeField] private float _attackDelay = 2;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private int _attackRadius = 5;

    //Charging
    [SerializeField] private float _chargingTimer;
    [SerializeField] private float _chargingDelay = 5;

    //Player
    private Transform _player;

    //Life
    [SerializeField] private int _currentLife;
    [SerializeField] private int _maxLife = 500;
    [SerializeField] private bool _isDead = false;

    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<Animator>();

        _player = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        _currentLife = _maxLife;
        currentState = EnemyState.Chasing;
        _animator.SetBool("IsWalking", true);
        _attackTimer = _attackDelay;
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
            case EnemyState.Dead:
                Dead();
            break;
            default:
                Chasing();
            break;
        }
    }

    void Chasing()
    {
        if(_isDead) return;
        if(_currentLife <= 0)
        {
            Dead();
        }
    
        _enemyAgent.SetDestination(_player.position);
        _enemyAgent.isStopped = false;
        if(OnRange(_attackRange))
        {
            currentState = EnemyState.Attacking;
        }
    }

    void Charging()
    {
        if(_isDead) return;
        if(_currentLife <= 0)
        {
            Dead();
        }
        _enemyAgent.isStopped = true;
        _enemyAgent.ResetPath();

        _chargingTimer += Time.deltaTime;

        if(_chargingTimer >= _chargingDelay)
        {
            currentState = EnemyState.Chasing;
            _chargingTimer = 0;
        }
    }

    void Attacking()
    {
        if(_isDead) return;
        if(_currentLife <= 0)
        {
            Dead();
        }
        if(OnRange(_attackRange))
        {
            
            _enemyAgent.isStopped = true;

            _attackTimer += Time.deltaTime;

            if(_attackTimer >= _attackDelay)
            {  
                Attack1();  
                _animator.SetTrigger("IsAttacking");
                _attackTimer = 0;
                currentState = EnemyState.Charging;
                _animator.SetBool("IsCooldawn", true);
                _animator.SetBool("IsWalking", false);
            }
        }
        if(!OnRange(_attackRange))
        {
            _animator.SetBool("IsCooldawn", false);
            _animator.SetBool("IsWalking", true);
            currentState = EnemyState.Chasing;
        }
    }
    


    
    public void Attack1()
    {
        Collider[] players = Physics.OverlapSphere(_attackPosition.position, _attackRadius);
            foreach (Collider item in players)
            {
                if(item.gameObject.CompareTag("Player"))
                {
                    PlayerResources _playerResources = item.GetComponent<PlayerResources>();
                    
                    if(_playerResources != null)
                    {
                        _playerResources.TakeDamage(75);
                        currentState = EnemyState.Charging;
                    }
                }
            }
        
            
    }

    public void SoundFoot()
    {
        _audioFoots.PlayOneShot(_footSFX);
    }

    void Dead()
    {
        _animator.SetTrigger("IsDead");
        _isDead = true;
        return;
  
    }
    
    
    

    void TurnToCharging()
    {
        currentState = EnemyState.Charging;
    }

    void TakeDamage(int damage)
    {
        _currentLife -= damage;
    }

    

    bool OnRange(float distance)
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

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Arrow"))
        {
            TakeDamage(20);
            collider.gameObject.SetActive(false);
        }
        if(collider.gameObject.CompareTag("Fire"))
        {
            collider.gameObject.SetActive(false);
            TakeDamage(50);
        }
    }

    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(_attackPosition.position, _attackRadius);
    }
}

