using UnityEngine;
using UnityEngine.UI;

public class MivoCanyon : MonoBehaviour
{

    public CanvasGroup canyon;
    public float currentAlpha = 1;
    public float goalAlpha = 0; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAlpha >= goalAlpha)
        {
            currentAlpha -= Time.deltaTime * 0.25f;
            canyon.alpha = currentAlpha;
        }
    }
}
