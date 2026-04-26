using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.isChangingScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            .Unload(SceneDataBase.Slots.Mivo)
            .Load(SceneDataBase.Slots.Prado, SceneDataBase.Scenes.Prado)
            .WithOverlay()
            .Perform();   
    }
}
