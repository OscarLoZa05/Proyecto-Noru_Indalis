using UnityEngine;

public class PauseCanvasRegistrar : MonoBehaviour
{
    public GameObject credits;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {
        
    }
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPauseCanvas(this.gameObject);
            // Opcional: Asegúrate de que empiece desactivado
            gameObject.SetActive(false); 
        }
    }
    public void OpenCredits()
    {
        credits.SetActive(true);
    }
    public void CloseCredits()
    {
        credits.SetActive(false);
    }

}
