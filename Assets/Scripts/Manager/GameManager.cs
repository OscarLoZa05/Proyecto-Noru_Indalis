using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public bool haveKenon = false;

    //[SerializeField] private GameObject _optionCanvas;
    //public Slider _sliderSensibility;
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
        //float _sliderValue = _sliderSensibility.value;
        //Debug.Log(_sliderValue);
        //PlayerData.Instance.cameraSensitivity = _sliderSensibility.value;
    }

    public void Pause()
    {
        if(_isPaused == false)
        {
            AudioListener.pause = true;
            _isPaused = !_isPaused;
            Time.timeScale = 0;
            //canvasPause.SetActive(true);
        }
        else
        {
            AudioListener.pause = false;
            _isPaused = !_isPaused;
            Time.timeScale = 1;
            //canvasPause.SetActive(false);
        }
    }
    public void UpdateSensiblity()
    {
        
        PlayerData.Instance.cameraSensitivity = 5;
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
