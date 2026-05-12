using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{

    public CanvasGroup _canvasGroup;
    public bool _alphaIsActived = false;
    public float _alpha = 1;
    public float minAlpha = 0;
    private Animator _animator;


    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(_alphaIsActived && _alpha >= minAlpha)
        {
            _alpha -= Time.deltaTime;
            _canvasGroup.alpha = _alpha;
        }
    }

    public void NewGame()
    {
        Debug.Log("Hola");
        _animator.SetTrigger("ClickOptions");
        

        SceneController.Instance
            .NewTransition()
            .Unload(SceneDataBase.Slots.MainMenu)
            .Load(SceneDataBase.Slots.Level2, SceneDataBase.Scenes.Level2)
            .WithOverlay()
            .Perform(); 
    }
    public void Alpha()
    {
        _alphaIsActived = true;
    }
}
