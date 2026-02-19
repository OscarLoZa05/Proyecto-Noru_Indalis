using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //Componentes
    private CharacterController _controller;
    private Animator _animator;
    private Transform _mainCamera;

    //Inputs
    public Vector2 _moveValue;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _interactAction;
    private InputAction _dashAction;
    
    //Movimiento
    [Header("Movement")]
    public float _playerSpeed = 5;
    private float _playerForce = 2;
    private float _pushForce = 10;
    private float _smoothTime = 0.2f;
    private float _turnSmoothVelocity;
    public float _speed;
    public float _speedChangeRate = 10;
    public float targetAngle;

    //Suelo
    [Header("Ground")]
    public Transform _sensorPosition;
    public LayerMask _groundLayer;
    private float _sensorRadius = 0.7f;

    //Gravedad
    [Header("Gravity")]
    public Vector3 _playerGravity;
    private float _gravity = -9.81f;

    //Interact
    [Header("Interact")]
    public Transform _interactionPosition;
    public Vector3 _interactionRadius;

    //Jump
    [Header("Jump")]
    public float jumpTimeOut = 0.5f;
    public float fallTimeOut = 0.15f; 
    public float _jumpHeight = 2f;
    
    float _jumpTimeOutDelta;
    float _fallTimeOutDelta;
    
    [Header("Dash Cooldown")]
    [SerializeField] private float dashCooldown = 1.25f;
    private bool isDashOnCooldown = false;
    private Coroutine dashCooldownRoutine;

    [Header("Dash")]
    [SerializeField] private float _dashSpeed = 20;
    [SerializeField] private float _dashTime = 0.25f;
    private Vector3 _lastMoveDirection;
    private bool isDashing = false;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        //_animator = GetComponentInChildren<Animator>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _interactAction = InputSystem.actions["Interact"];
        _dashAction = InputSystem.actions["Dash"];

        _mainCamera = Camera.main.transform;
    }
    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();

        //Acciones
        if (_jumpAction.WasPerformedThisFrame() && IsGrounded())
        {
            Jump();
        }
        if (_interactAction.WasPressedThisFrame())
        {
            Interact();
        }
        if(_dashAction.WasPressedThisFrame() && _moveValue != Vector2.zero && !isDashing && !isDashOnCooldown)
        {
            StartCoroutine(Dash());
        }        

        Movement();

        Gravity();
    }

    private float _smoothSpeed = 0f;
    void Movement()
    {
        if(isDashing) return;
        
        Vector3 direction = new Vector3(_moveValue.x, 0, _moveValue.y);

        float targetSpeed = _playerSpeed;
        
        if(direction == Vector3.zero)
        {
            targetSpeed = 0;
        }

        _speed = Mathf.SmoothDamp(_speed, targetSpeed * direction.magnitude, ref _smoothSpeed, 0.1f);

        if (direction != Vector3.zero)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            _lastMoveDirection = (Quaternion.Euler(0, targetAngle, 0) * Vector3.forward).normalized;
        }

        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        _controller.Move(_speed * Time.deltaTime * moveDirection.normalized  + _playerGravity * Time.deltaTime);
    }

        void Jump()
    {
        if(_jumpTimeOutDelta <= 0)
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }
    }

    void Gravity()
    {
        //_animator.SetBool("Grounded", IsGrounded());

        if(IsGrounded())
        {
            _fallTimeOutDelta = fallTimeOut;
            
            //_animator.SetBool("Jump", false);
            //_animator.SetBool("Fall", false);
            if(_playerGravity.y < 0)
            {
                _playerGravity.y = -2;
            }

            if(_jumpTimeOutDelta >= 0)
            {
                _jumpTimeOutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeOutDelta = jumpTimeOut;

            if(_fallTimeOutDelta >= 0)
            {
                _fallTimeOutDelta -= Time.deltaTime;
            }
            else
            {
                //_animator.SetBool("Fall", true);
            }
            
            _playerGravity.y += _gravity * Time.deltaTime;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float timer = 0;

        while(timer < _dashTime)
        {
            _controller.Move(_lastMoveDirection.normalized * _dashSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }
        
        isDashing = false;
        dashCooldownRoutine = StartCoroutine(DashCoolDown());
    }

    IEnumerator DashCoolDown()
    {
        isDashOnCooldown = true;
        yield return new WaitForSecondsRealtime(dashCooldown);
        isDashOnCooldown = false;
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }

    void Interact()
    {
        Collider[] objectsToGrab = Physics.OverlapBox(_interactionPosition.position, _interactionRadius);
            foreach (Collider item in objectsToGrab)
            {
                if(item.gameObject.layer == 6)
                {
                    IInteractable interactableObject = item.GetComponent<IInteractable>();
                    if(interactableObject != null)
                    {
                        interactableObject.Interact(); 
                    }
                }
            }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_interactionPosition.position, _interactionRadius);
    }
}
