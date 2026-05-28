using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private InputAction _stopAction;

    [Header("Conditions")]
    public bool _isPaused = false;
    public bool _isDead = false;
    public bool _shopOpen = false;
    public bool isChangingScene = false;
    public bool haveKenon = false;

    public PlayerInput _playerInput;

    [Header("Pause")]
    public GameObject PauseCanvas; 

    [Header("Audio Registers")]
    public AudioSource _BGM;
    public AudioSource _playerSounds;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _stopAction = InputSystem.actions["Stop"];
    }

    void Update()
    {
        if (_isDead || _shopOpen || isChangingScene) return;

        if (_stopAction.triggered)
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (_isPaused) return;

        _isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        
        if (PauseCanvas != null) 
        {
            PauseCanvas.SetActive(true);
            AsignarBotonResumePorCodigo();
        }
        
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        _isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        
        if (_BGM != null) _BGM.UnPause();
        if (_playerSounds != null) _playerSounds.UnPause();

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        _stopAction.Reset();

        if (PauseCanvas != null) 
        {
            PauseCanvas.SetActive(false);
        }
    }

    private void AsignarBotonResumePorCodigo()
    {
        if (PauseCanvas == null) return;

        Button resumeButton = PauseCanvas.GetComponentInChildren<Button>(true);

        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(() => {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ResumeGame();
                }
            });
        }
    }

    // --- MÉTODOS DE CONTROL DE INPUTS ---
    public void DesactivarControlesYCamara()
    {
        if (_playerInput != null && _playerInput.actions.enabled)
        {
            _playerInput.actions.Disable();
            Debug.Log("Controles y cámara BLOQUEADOS para el diálogo.");
        }
    }

    public void ActivarControlesYCamara()
    {
        if (_playerInput != null && !_playerInput.actions.enabled)
        {
            _playerInput.actions.Enable();
            Debug.Log("Controles y cámara DESBLOQUEADOS automáticamente tras el 3º diálogo.");
        }
    }

    public void BloqueoDeInputs()
    {
        if (_playerInput.actions.enabled)
        {
            _playerInput.actions.Disable();
        }
        else
        {
            _playerInput.actions.Enable();
        }
    }

    // --- MÉTODOS DE REGISTRO ---
    public void RegisterPauseCanvas(GameObject canvas) 
    { 
        PauseCanvas = canvas; 
        AsignarBotonResumePorCodigo();
    }
    public void RegisterBGM(AudioSource bgmSource) { _BGM = bgmSource; }
    public void RegisterPlayerSounds(AudioSource playerSource) { _playerSounds = playerSource; }
    public void UpdateSensiblity() { PlayerData.Instance.cameraSensitivity = 5; }
    public void RegisterPlayerInput(PlayerInput playerInputComponent)
{
    _playerInput = playerInputComponent;
    Debug.Log("PlayerInput registrado correctamente en el GameManager.");
}
    public void QuitGame() { Application.Quit(); }
}