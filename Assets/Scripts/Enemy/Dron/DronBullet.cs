using UnityEngine;

public class DronBullet : MonoBehaviour
{
    public float speed = 50f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    // Opcional: Desactivar si choca con algo
    void OnTriggerEnter(Collider collider)
    {
        gameObject.SetActive(false);
        if(collider.gameObject.CompareTag("Player"))
        {
            PlayerResources _pr = collider.gameObject.GetComponent<PlayerResources>();
            _pr.TakeDamage(10);
        }
    }
}