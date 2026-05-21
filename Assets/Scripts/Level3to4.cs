using UnityEngine;

public class Level3To4 : MonoBehaviour
{
    public bool canPass = false;
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
        if(collider.gameObject.CompareTag("Player") && canPass)
        {
            NextLevel();  
        }
    }

    void NextLevel()
    {
        GameManager.Instance.isChangingScene = true;
        SceneController.Instance
            .NewTransition()
            .Unload(SceneDataBase.Slots.Level3)
            .Load(SceneDataBase.Slots.Level4, SceneDataBase.Scenes.Level4)
            .WithOverlay()
            .Perform();   
    }
}
