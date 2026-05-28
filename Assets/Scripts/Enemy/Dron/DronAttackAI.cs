using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.VFX;

public class DronAttackAI : MonoBehaviour, IEnemy
{
    private NavMeshAgent _enemyAgent;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _originPlayer;
    private AudioSource _as;
    public AudioClip _attackSFX;
    public AudioClip _deadSFX; 
    public AudioClip takedamage;

    [SerializeField] private ParticleSystem[] _explosionVFX = new ParticleSystem[4];
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
    [SerializeField] private int _currentLife;
    [SerializeField] private int _maxLife = 20;
    [SerializeField] private bool _isDead = false;

    //Player
    private Transform _player;

    public EnemyState currentState;
    public VisualEffect miEfectoVisual;
    
    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
        _as = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentState = EnemyState.Chasing;
        _attackTimer = _attackDelay;
        _currentLife = _maxLife;
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
        if(_isDead) return;
        if(_currentLife <= 0)
        {
            StartCoroutine(Dead());
        }
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
        if(_isDead) return;
        if(_currentLife <= 0)
        {
            StartCoroutine(Dead());
        }
        _enemyAgent.isStopped = true;
        
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= _attackDelay)
        {
            Attack();
        }        
    }

    void Attack()
    {
        _as.PlayOneShot(_attackSFX);
        miEfectoVisual.Play();
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

    public void TakeDamage(int damage)
    {
        _currentLife -= damage;
    }

    IEnumerator Dead()
    {
        _isDead = true;
        foreach (var item in _explosionVFX)
        {
            item.Play();
        }
        _as.PlayOneShot(_deadSFX);
        yield return new WaitForSeconds(1.8f);
        
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("Me has hecho dañi");
            TakeDamage(10);
            _as.PlayOneShot(takedamage);
            
            //collider.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
