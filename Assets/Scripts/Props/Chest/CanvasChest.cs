using UnityEngine;

public class CanvasChest : MonoBehaviour
{
    private Transform camTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + camTransform.rotation * Vector3.forward, camTransform.rotation * Vector3.up);
    }
}
