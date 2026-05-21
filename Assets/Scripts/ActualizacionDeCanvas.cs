using UnityEngine;
using UnityEngine.UI;

public class ActualizaciondeCanvas : MonoBehaviour
{

    public Image healthBarImage;
    public Image manaBarImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar()
    {
        Debug.Log("Update");
        float life = (float)PlayerData.Instance.currentHealth / PlayerData.Instance.maxHealth;
        healthBarImage.fillAmount = life;
    }
    public void UpdateManaBar()
    {
        Debug.Log("Update");
        float mana = PlayerData.Instance.currentMana / PlayerData.Instance.maxMana;
        manaBarImage.fillAmount = mana;
    }
}
