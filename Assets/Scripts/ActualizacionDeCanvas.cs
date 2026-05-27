using UnityEngine;
using UnityEngine.UI;

public class ActualizaciondeCanvas : MonoBehaviour
{

    public Image healthBarImage;
    public Image manaBarImage;
    public Image kenonImage;
    public Text manaText;
    public Text lifeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHealthBar();
        UpdateManaBar();
        ManaText();
        HealthText();
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
    public void UpdateKenonBar()
    {
        float noruBar = PlayerData.Instance.currentNoru / PlayerData.Instance.maxNoru;
        kenonImage.fillAmount = noruBar;
    }
    public void ManaText()
    {
        manaText.text = "x" + PlayerData.Instance.manaPotions.ToString();
    }
    public void HealthText()
    {
        lifeText.text = "x" + PlayerData.Instance.healthPotions.ToString();
    } 
}
