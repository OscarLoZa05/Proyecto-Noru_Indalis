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
    [SerializeField] private int _manaReg = 25;

    //ManaHealth
    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth = 100;
    public Image healthBarImage;
    [SerializeField] private int _healthReg = 25;

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
        if(_manaPotionInput.WasPressedThisFrame() && manaPotions > 0)
        {
            Mana();
        }
        if(_healthPotionInput.WasPressedThisFrame() && healthPotions > 0)
        {
            Health();
        }
    }

    void Mana()
    {
        currentMana += _manaReg;
        manaPotions --;
        ManaText();
        UpdateManaBar();
    }
    void Health()
    {
        currentHealth += _manaReg;
        manaPotions --;
        HealthText();
        UpdateHealthBar();
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