using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Video;

public class Level2Manager : MonoBehaviour
{

    public GameObject[] misEnemigos; // Arrastra aquí tus 3 enemigos
    private bool eventoLanzado = false;

    public GameObject WendigoFinal;
    public Kenon _kenon;
    public Level2To3 _level2to3;
    public CanvasGroup kenonCanvas;
    public float multiplaied = 2;
    public float alphaCount = 0;  
    public bool combatTutorial = false;  

    [Header("Referencias UI")]
    [SerializeField] private GameObject canvasObjeto;
    [SerializeField] private RawImage rawImageComponente;

    [Header("Componentes de Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip primerVideo;  // El video que se reproduce primero
    [SerializeField] private VideoClip segundoVideo;

    public GameObject[] objetosParaDesactivar;

    public Transform wendigoSpawn;

    [SerializeField] private GameObject objetoConShader;
    private Renderer targetRenderer;

    public bool camaraActivada = false;
    public float nuevoValor = 0;
    public DialogueManager _dialogueManager;
    public DialogueData _dialogueData;
    public GameObject _canvasDialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //_kenon = GameObject.FindWithTag("Player").GetComponent<Kenon>();
    }
    
    void Start()
    {
        GameManager.Instance.DesactivarControlesYCamara();
        _canvasDialogue.SetActive(true);
        _dialogueManager.IniciarDialogo(_dialogueData);
        GameManager.Instance.isChangingScene = false;
        targetRenderer = objetoConShader.GetComponent<Renderer>();
        targetRenderer.material.SetFloat("_Opacity", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(combatTutorial && alphaCount < 1)
        {
            FadeInKenon();
            Debug.Log("FadeInKenon");
        }

        if(TutorialCombate() && !GameManager.Instance.haveKenon)
        {
            GameManager.Instance.haveKenon = true;
            combatTutorial = true;
            PlayerData.Instance.currentNoru = 100;
            StartCoroutine(SecuenciaVideoCoroutine());
        }
        if(camaraActivada == true && nuevoValor <= 1)
        {
            nuevoValor += Time.deltaTime * 0.05f;
            CambiarValorShader();
        }
        
    }

    void FadeInKenon()
    {
        alphaCount += Time.deltaTime * multiplaied;
        kenonCanvas.alpha = alphaCount;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            NextLevel();  
        }
    }
    private IEnumerator SecuenciaVideoCoroutine()
    {
        AudioListener.pause = true;
        
        // ----------------------------------------
        // 1. REPRODUCIR EL PRIMER VIDEO
        // ----------------------------------------
        videoPlayer.clip = primerVideo;
        canvasObjeto.SetActive(true);
        videoPlayer.Play();

        // Esperamos a que el primer video esté preparado para saber su duración real
        while (!videoPlayer.isPrepared)
        {
            yield return null; 
        }

        // Esperamos automáticamente los segundos que dura el primer video
        yield return new WaitForSeconds((float)videoPlayer.length);

        // Detenemos el primer video para hacer la transición limpia
        videoPlayer.Stop();


        // ----------------------------------------
        // 2. REPRODUCIR EL SEGUNDO VIDEO
        // ----------------------------------------
        videoPlayer.clip = segundoVideo;
        videoPlayer.Play();

        // Esperamos a que el segundo video esté preparado
        while (!videoPlayer.isPrepared)
        {
            yield return null; 
        }

        // Esperamos automáticamente los segundos que dura el segundo video
        yield return new WaitForSeconds((float)videoPlayer.length);

        foreach (var items in objetosParaDesactivar)
        {
            items.SetActive(false);
        }

        // 3. FIN DE LA SECUENCIA
        videoPlayer.Stop();
        canvasObjeto.SetActive(false);
        
        // Opcional: Desactivar el canvas al terminar el segundo video si ya no quieres mostrar nada más
        // canvasObjeto.SetActive(false);
        //Wendigo(); 
        //camaraActivada = true;
        _dialogueManager.RetomarDialogo(_dialogueData);
        AudioListener.pause = false;
        Cursor.visible = true;
        
    }

    public void CambiarValorShader()
    {
        if (targetRenderer != null)
        {
            // Reemplaza "_TuVariableReference" por el texto que copiaste de Shader Graph
            targetRenderer.material.SetFloat("_Opacity", nuevoValor);
            
            // Si fuera un Color:
            // targetRenderer.material.SetColor("_TuColorReference", Color.red);
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

    public void Wendigo()
    {
        GameObject Wendigo = PoolManager.Instance.GetPooledObject("Wendigo", wendigoSpawn.position, wendigoSpawn.rotation);
        Wendigo.SetActive(true);
    }   
    bool TutorialCombate()
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
