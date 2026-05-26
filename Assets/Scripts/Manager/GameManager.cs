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

    [Header("Pause")]
    public GameObject PauseCanvas { get; private set; }

    // Cambiados a públicos estándar para que los otros scripts puedan asignarlos sin problemas
    [Header("Audio Registers")]
    public AudioSource _BGM;
    public AudioSource _playerSounds;

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
            _isPaused = true;
            Time.timeScale = 0;
            
            if(PauseCanvas != null) PauseCanvas.SetActive(true);
            
            // Pausamos los audios si están asignados
            if(_BGM != null && _BGM.isPlaying) _BGM.Pause();
            if(_playerSounds != null && _playerSounds.isPlaying) _playerSounds.Pause();
        }
        else
        {
            _isPaused = false;
            Time.timeScale = 1;
            
            if(PauseCanvas != null) PauseCanvas.SetActive(false);
            
            // Reanudamos los audios
            if(_BGM != null) _BGM.UnPause();
            if(_playerSounds != null) _playerSounds.UnPause();
        }
    }

    // --- MÉTODOS DE REGISTRO ---

    public void RegisterPauseCanvas(GameObject canvas)
    {
        PauseCanvas = canvas;
    }

    public void RegisterBGM(AudioSource bgmSource)
    {
        _BGM = bgmSource;
    }

    public void RegisterPlayerSounds(AudioSource playerSource)
    {
        _playerSounds = playerSource;
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