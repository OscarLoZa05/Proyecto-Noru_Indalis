using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public static PlayerData Instance; 

    [Header("Mana")]
    public float currentMana = 100;
    public float maxMana = 100;

    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth = 100;

    [Header("Kenon")]
    public int maxNoru = 100;
    public int currentNoru = 0;

    [Header("Potions")]
    public int manaPotions = 0;
    public int healthPotions = 0; 

    [Header("Money")]
    public int money = 0;
    [Header("Sensiblity")]
    
    public float cameraSensitivity = 10;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
