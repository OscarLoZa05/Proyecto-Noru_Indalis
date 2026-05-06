using UnityEngine;

public class CoreManager : MonoBehaviour
{
    void Start()
    {
        //Core SetUp for the game
        //Load everything like AudioManagers, Save System...
        SceneController.Instance
            .NewTransition()
            .Load(SceneDataBase.Slots.Level2, SceneDataBase.Scenes.Level2)
            .WithOverlay()
            .Perform();
    }


}
