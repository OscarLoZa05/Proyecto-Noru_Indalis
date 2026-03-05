using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PlayerResources : MonoBehaviour
{

    //Inputs
    private InputAction _healthPotionInput;
    private InputAction _manaPotionInput;
    
    //ManaBar
    [Header("Mana")]
    public float currentMana = 100;
    public float maxMana = 100;
    public float mana;
    public Image manaBarImage;

    //ManaHealth
    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth = 100;
    public Image healthBarImage;

    [Header("TextsPotions")]
    public Text manaText;
    public Text healthText;

    //Potions
    [Header("Potions")]
    public int manaPotions = 0;
    public int healthPotions = 0;

    private PlayerAbility _playerAbility;

    void Awake()
    {
        _playerAbility = GetComponent<PlayerAbility>();

        _healthPotionInput = InputSystem.actions["PotionsHealth"];
        _manaPotionInput = InputSystem.actions["PotionsMana"];
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
    public void UpdateManaBar()
    {
        float mana = currentMana / maxMana;
        manaBarImage.fillAmount = mana;
    }
    public void ManaText()
    {
        manaText.text = "x" + manaPotions.ToString();
    } 
    
    public void UpdateHealthBar()
    {
        float life = currentHealth / maxHealth;
        healthBarImage.fillAmount = life;
    }

    public void HealthText()
    {
        healthText.text = "x" + healthPotions.ToString();
    } 
}