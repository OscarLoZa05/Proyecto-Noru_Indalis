using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PlayerResources : MonoBehaviour
{

    //Inputs
    private InputAction healthPotionInput;
    private InputAction manaPotionInput;
    
    //ManaBar
    [Header("Mana")]
    public float currentMana = 100;
    public float maxMana = 100;
    public float mana;

    //ManaHealth
    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth = 100;

    [Header("TextsPotions")]
    public Text manaText;
    public Text healthText;

    //Potions
    [Header("Potions")]
    public int manaPotions = 0;
    public int healthPotions = 0;
    public int healthPotionAmount = 25;
    public int manaPotionAmount = 25;

    private PlayerAbility _playerAbility;

    void Awake()
    {
        _playerAbility = GetComponent<PlayerAbility>();

        healthPotionInput = InputSystem.actions["PotionsHealth"];
        manaPotionInput = InputSystem.actions["PotionsMana"];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateManaBar()
    {
        float mana = currentMana / maxMana;
        //manaBarImage.fillAmount = currentMana;
    }
}
