using UnityEngine;

public class ArrowBullet : MonoBehaviour
{

    private Rigidbody _rb;

    [SerializeField] private float _arrowVelocity = 5;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _rb.linearVelocity = transform.forward * _arrowVelocity;
    }
}
