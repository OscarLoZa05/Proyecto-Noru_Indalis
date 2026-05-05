using UnityEngine;
using UnityEngine.UI;

public class CanvasChest : MonoBehaviour
{
    private Transform _player; 
    private float maxDistance = 50;
    private Transform camTransform;
    public Material materialCanva;
    private Image _image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _image = GetComponent<Image>();
    }

    void Start()
    {
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + camTransform.rotation * Vector3.forward, camTransform.rotation * Vector3.up);

        DegradadoDeCanvas();
    }

    public void DegradadoDeCanvas()
    {
        float distance = Vector3.Distance(_player.transform.position, transform.position);
        float distanceByTheWay = Mathf.Clamp01(1 - (distance / maxDistance));
        distanceByTheWay *= 255;
        Debug.Log("Estas a esta distancia" + distanceByTheWay);
        Color tempColor = _image.color;
        tempColor.a = distanceByTheWay;
        _image.color = tempColor;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
