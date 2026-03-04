using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    //Inputs
    private InputAction _attackInput;

    //Componente
    private Animator _animator;
    private Transform _mainCamera;
    private PlayerController _pCotroller;

    //Shoot
    [SerializeField] private Transform _bulletSpawn;

    void Awake()
    {
        _attackInput = InputSystem.actions["Attack"];

        _animator = GetComponent<Animator>();
        _pCotroller = GetComponent<PlayerController>();
        _mainCamera = Camera.main.transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_attackInput.WasPressedThisFrame())
        {
            Attack();
            _animator.SetTrigger("IsAttacking");
        }
    }

    void Attack()
    {
        if(_pCotroller.isAiming == false)
        {
            GameObject bullet = PoolManager.Instance.GetPooledObject("Bullet", _bulletSpawn.position, _bulletSpawn.rotation);
            bullet.SetActive(true);
        }
        else
        {
            GameObject bullet = PoolManager.Instance.GetPooledObject("Bullet", _bulletSpawn.position, _mainCamera.rotation);
            bullet.SetActive(true);
        }
        
    }
}
