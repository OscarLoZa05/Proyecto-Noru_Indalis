using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{


    private Animator _animator;
    private Camera _mainCamera;
    private CameraAnimator _cameraAnimator;
    [SerializeField] private GameObject canvasOption;


    void Awake()
    {
        _mainCamera = Camera.main;
        _cameraAnimator = _mainCamera.GetComponent<CameraAnimator>();
        _animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        Debug.Log("Hola");
    }
    public void AnimatorCamera()
    {
        _cameraAnimator.AnimatorCamera(); 
    }
    public void Options()
    {
        canvasOption.SetActive(true);
    }
    public void CloseOptions()
    {
        canvasOption.SetActive(false);
        //PlayerData.Instance.cameraSensitivity = GameManager.Instance.sliderSensibility.value;
    }
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
