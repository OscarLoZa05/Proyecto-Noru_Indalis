using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private InputAction _stopAction;

    //Conditions
    [Header("Conditions")]
    public bool _isPaused = false;
    public bool _isDead = false;
    public bool _shopOpen = false;
    public bool isChangingScene = false;

    //Canvas
    [SerializeField] private GameObject canvasPause;

    void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        _stopAction = InputSystem.actions["Stop"];
    }
    
    void Start()
    {
        
    }


    void Update()
    {
        if(_isDead || _shopOpen) return;
        if(_stopAction.WasPressedThisFrame() && !isChangingScene)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if(_isPaused == false)
        {
            _isPaused = !_isPaused;
            Time.timeScale = 0;
            canvasPause.SetActive(true);
        }
        else
        {
            _isPaused = !_isPaused;
            Time.timeScale = 1;
            canvasPause.SetActive(false);
        }
    }
}
