using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void SceneChange(int SceneNumber)
    {
        //SceneManager.LoadScene(SceneNumber);
    }

    public void Options()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
