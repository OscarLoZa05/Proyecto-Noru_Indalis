using UnityEngine;

public class Level3Manager : MonoBehaviour
{

    private GameObject _player;
    public Transform _spawnPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }
    
    void Start()
    {
        GameManager.Instance.isChangingScene = false;
        _player.transform.position = _spawnPosition.position;

    }

    // Update is called once per frame
    void Update()
    {
        //_player.transform.position = _spawnPosition.position;
    }

    /*void OnTriggerEnter(Collider collider)
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
            .Unload(SceneDataBase.Slots.Mivo)
            .Load(SceneDataBase.Slots.Prado, SceneDataBase.Scenes.Prado)
            .WithOverlay()
            .Perform();   
    }*/
}
