using UnityEngine;

public class CameraAnimator : MonoBehaviour
{
    private Animator _animator;

    public CanvasGroup _canvasGroup;
    public bool _alphaIsActived = false;
    public float _alpha = 1;
    public float minAlpha = 0;
    public float velocityAlpha = 2;

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
            _alpha -= Time.deltaTime * velocityAlpha;
            _canvasGroup.alpha = _alpha;
        }
        
    }
    public void AnimatorCamera()
    {
        _animator.SetTrigger("Click");
    }
    public void Alpha()
    {
        _alphaIsActived = true;
    }
    public void ChangeLevel()
    {
        SceneController.Instance
            .NewTransition()
            .Unload(SceneDataBase.Slots.MainMenu)
            .Load(SceneDataBase.Slots.Level2, SceneDataBase.Scenes.Level2)
            .WithOverlay()
            .Perform(); 
    }
    
}
