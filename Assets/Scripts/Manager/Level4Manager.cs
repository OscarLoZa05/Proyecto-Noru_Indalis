using UnityEngine;

public class Level4Manager : MonoBehaviour
{
    private BoxCollider _bC;
    public GameObject final;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _bC = GetComponent<BoxCollider>();
    }
    void Start()
    {
        GameManager.Instance.isChangingScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            final.SetActive(true);
            GameManager.Instance.PauseGame();
        }
    }
}
