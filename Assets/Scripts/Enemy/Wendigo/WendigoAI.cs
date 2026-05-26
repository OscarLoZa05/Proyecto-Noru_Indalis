using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WendigoAI : MonoBehaviour
{
    public enum EnemyState { Chasing, Charging, Attacking, Dead }
    [Header("State Machine")]
    public EnemyState currentState;

    private NavMeshAgent _enemyAgent;
    private Animator _animator;
    private AudioSource _audioSource;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip _footSFX;
    [SerializeField] private AudioClip _deadSFX;
    
    [Header("Ranges & Hysteresis")]
    [SerializeField] private float _attackRange = 2f;    // Distancia para iniciar el ataque
    [SerializeField] private float _escapeRange = 3.5f;  // Colchón de seguridad para cancelar el ataque

    [Header("Attack Settings")]
    [SerializeField] private float _attackDelay = 2f;
    private float _attackTimer;
    private bool _yaAcoquillado = false; // Candado para evitar repeticiones de animación

    [Header("Charging / Cooldown Settings")]
    [SerializeField] private float _chargingDelay = 5f;
    private float _chargingTimer;

    [Header("Life Settings")]
    [SerializeField] private int _currentLife;
    [SerializeField] private int _maxLife = 500;

    private Transform _player;
    private bool _isDead = false;
    private bool _movimientoBloqueado = false;

    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<Animator>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;
    }

    void Start()
    {
        _currentLife = _maxLife;
        _enemyAgent.speed = 3.5f;
        currentState = EnemyState.Chasing;
        
        if (_animator != null)
        {
            _animator.SetBool("IsWalking", true);
            _animator.SetBool("IsCooldown", false);
            _animator.SetBool("IsAttacking", false);
        }
        // Empezamos con el ataque cargado para el primer encuentro
        _attackTimer = _attackDelay; 
    }

    void Update()
    {
        if (_isDead) return;

        // Si la animación de ataque bloquea el movimiento, congelamos al agente inmediatamente
        if (_movimientoBloqueado)
        {
            _enemyAgent.isStopped = true;
            _enemyAgent.velocity = Vector3.zero;
            return;
        }

        float distanceToPlayer = (_player != null) ? Vector3.Distance(transform.position, _player.position) : float.MaxValue;

        switch (currentState)
        {
            case EnemyState.Chasing:
                Chasing(distanceToPlayer);
                break;
            case EnemyState.Charging:
                Charging(distanceToPlayer);
                break;
            case EnemyState.Attacking:
                Attacking(distanceToPlayer);
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    public void ForzarFreno(bool frenar)
    {
        _movimientoBloqueado = frenar;
        if (frenar)
        {
            _enemyAgent.isStopped = true;
            _enemyAgent.velocity = Vector3.zero;
        }
    }

    // Método invocado por el hijo (AttackWendigo) en el ÚLTIMO FRAME de la animación
    public void TerminarAtaqueYEntrarEnCooldown()
    {
        _movimientoBloqueado = false;
        _yaAcoquillado = false; // Abrimos el candado para permitir futuros ataques
        currentState = EnemyState.Charging;
        _chargingTimer = 0;

        if (_animator != null)
        {
            _animator.SetBool("IsAttacking", false); // Apagamos el booleano para salir de la animación
            _animator.SetBool("IsCooldown", true); 
            _animator.SetBool("IsWalking", false);
        }
    }

    void Chasing(float distanceToPlayer)
    {
        _enemyAgent.isStopped = false;
        _enemyAgent.speed = 3.5f;

        if (_player != null) _enemyAgent.SetDestination(_player.position);

        if (distanceToPlayer <= _attackRange)
        {
            currentState = EnemyState.Attacking;
        }
    }

    void Charging(float distanceToPlayer)
    {
        // Forzamos reposo absoluto durante la recarga de energía
        _enemyAgent.isStopped = true;
        _enemyAgent.velocity = Vector3.zero;

        _chargingTimer += Time.deltaTime;

        if (_chargingTimer >= _chargingDelay)
        {
            _chargingTimer = 0;
            
            if (distanceToPlayer <= _attackRange)
            {
                // Si el jugador sigue al lado, atacamos directo sin pasar por caminar
                _attackTimer = _attackDelay; 
                currentState = EnemyState.Attacking;
                if (_animator != null)
                {
                    _animator.SetBool("IsCooldown", false);
                    _animator.SetBool("IsAttacking", true);
                }
            }
            else
            {
                // Si el jugador se ha movido, volvemos a perseguir
                if (_animator != null)
                {
                    _animator.SetBool("IsCooldown", false); 
                    _animator.SetBool("IsWalking", true);
                }
                
                // ATAQUE INSTANTÁNEO: Le damos el temporizador lleno para que
                // en cuanto te toque corriendo, te pegue sin esperar.
                _attackTimer = _attackDelay; 
                
                currentState = EnemyState.Chasing;
            }
        }
    }

    void Attacking(float distanceToPlayer)
    {
        // Cancelamos el ataque solo si te alejas más allá de la zona de escape (Hysteresis)
        if (distanceToPlayer > _escapeRange && !_yaAcoquillado)
        {
            if (_animator != null) _animator.SetBool("IsAttacking", false);
            currentState = EnemyState.Chasing;
            return;
        }

        _enemyAgent.isStopped = true;
        _enemyAgent.velocity = Vector3.zero;

        // Si la animación de ataque ya fue activada, salimos del método para no duplicar llamadas
        if (_yaAcoquillado) return;

        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackDelay)
        {   
            _yaAcoquillado = true; // Cerramos el candado: procesando ataque actual
            if (_animator != null)
            {
                _animator.SetBool("IsWalking", false);
                _animator.SetBool("IsAttacking", true); // Encendemos el booleano
            }
            _attackTimer = 0;
        }
    }

    public void SoundFoot()
    {
        if (_audioSource != null && _footSFX != null) _audioSource.PlayOneShot(_footSFX);
    }

    public void RecibirDanio(int damage)
    {
        if (_isDead) return;
        _currentLife -= damage;
        if (_currentLife <= 0) 
        {
            _currentLife = 0;
            Dead();
        }
    }

    void Dead()
    {
        if (_isDead) return; 
        _isDead = true;

        if (PlayerData.Instance != null) PlayerData.Instance.currentNoru += 40;
        if (_audioSource != null && _deadSFX != null) _audioSource.PlayOneShot(_deadSFX);

        if (_animator != null) _animator.SetTrigger("IsDead");
        
        _enemyAgent.isStopped = true;
        _enemyAgent.enabled = false; 
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Arrow"))
        {
            RecibirDanio(20);
            collider.gameObject.SetActive(false);
        }
        if (collider.gameObject.CompareTag("Fire"))
        {
            collider.gameObject.SetActive(false);
            RecibirDanio(50);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _escapeRange);
    }
}