using UnityEngine;

public class FireSlash : MonoBehaviour
{
    private Rigidbody _rigidBody;

    [SerializeField] private float _fireSlashVelocity = 5;

    [SerializeField] private int Hola = 3;
    
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _rigidBody.linearVelocity = transform.forward * _fireSlashVelocity;
    }
}
