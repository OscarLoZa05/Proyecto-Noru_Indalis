using UnityEngine;

public class CanvasPersistente : MonoBehaviour
{
    public static CanvasPersistente Instance;

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
    }
}