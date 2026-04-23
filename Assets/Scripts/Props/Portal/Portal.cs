using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    [SerializeField] private Renderer portalRenderer;
    [SerializeField] private bool nilloIsInside = false;
    [SerializeField] private float maxPortal = 10;
    [SerializeField] private float minPortal = 0.6f;
    [SerializeField] private float currentPortal;

    public int sceneCharging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPortal = maxPortal;
    }

    void Awake()
    {
        portalRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(nilloIsInside && currentPortal >= minPortal)
        {
            currentPortal -= Time.deltaTime * 4;
            PortalGraph();
        }
        if(!nilloIsInside && currentPortal <= maxPortal)
        {
            currentPortal += Time.deltaTime * 4;
            PortalGraph();
        }
    }

    void PortalGraph()
    {
        Material mat = portalRenderer.material;
        mat.SetFloat("_RadialDistortion", currentPortal);
    }

    /*void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            SceneManager.Instance.SceneChange(sceneCharging);
        }
    }*/
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            nilloIsInside = true;
        }
    }
    void OnTriggerExit(Collider colldier)
    {
        if(colldier.gameObject.CompareTag("Player"))
        {
            nilloIsInside = false;
        }
    }

    /*void NextLevel()
    {
        GameManager.Instance.isChangingScene = true;
        SceneController.Instance
            .NewTransition()
            .Load(SceneDataBase.Slots.Mivo, SceneDataBase.Scenes.Mivo)
            .WithOverlay()
            .Perform();   
    }*/
}
