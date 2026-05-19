using UnityEngine;

public class Level2Manager : MonoBehaviour
{

    public GameObject[] misEnemigos; // Arrastra aquí tus 3 enemigos
    private bool eventoLanzado = false;

    public GameObject WendigoFinal;
    public Kenon _kenon;
    public Level2To3 _level2to3;
    public GameObject kenonCanvas;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //_kenon = GameObject.FindWithTag("Player").GetComponent<Kenon>();
    }
    
    void Start()
    {
        GameManager.Instance.isChangingScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventoLanzado) return;
        if (Tutorial())
        {
            GameManager.Instance.haveKenon = true;
            kenonCanvas.SetActive(true);
            //Debug.Log("¡Los 3 enemigos han muerto!");
            eventoLanzado = true;
        }
        if(WendigoFinal = null)
        {
            _level2to3.canPass = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            NextLevel();  
        }
    }

    void NextLevel()
    {
        GameManager.Instance.isChangingScene = true;
        SceneController.Instance
            .NewTransition()
            .Load(SceneDataBase.Slots.Level2, SceneDataBase.Scenes.Level2)
            .WithOverlay()
            .Perform();   
    }
    bool Tutorial()
    {
        foreach (GameObject enemigo in misEnemigos)
        {
            if(enemigo != null)
            {
                return false;
            }
        }
        return true;
    }
}
