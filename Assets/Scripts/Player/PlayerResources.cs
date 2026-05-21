using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PlayerResources : MonoBehaviour
{

    public ActualizaciondeCanvas _actualizacionesdeCanvas;
    //Components
    private Animator _animator;

    //Inputs
    private InputAction _healthPotionInput;
    private InputAction _manaPotionInput;
    
    //ManaBar
    [Header("Mana")]

    public Image manaBarImage;
    [SerializeField] private int _manaReg = 25;

    //ManaHealth
    [Header("Health")]
    public Image healthBarImage;
    [SerializeField] private int _healthReg = 25;

    [Header("Texts")]
    public Text manaText;
    public Text healthText;
    public Text moneyText;
    //Money
    [Header("Money")]
    public Text monetText;   

    //Player
    private PlayerAbility _playerAbility;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerAbility = GetComponent<PlayerAbility>();

        _healthPotionInput = InputSystem.actions["PotionsHealth"];
        _manaPotionInput = InputSystem.actions["PotionsMana"];

        //ActualizaciondeCanvas _actualizacionesdeCanvas = GameObject.Find("Actualizacion").GetComponent<ActualizaciondeCanvas>();
    }

    void Start()
    {
        PlayerData.Instance.maxHealth = 100;
        PlayerData.Instance.currentHealth = PlayerData.Instance.maxHealth;
    }


    void Update()
    {
        if(GameManager.Instance._isDead || GameManager.Instance._isPaused || GameManager.Instance._shopOpen) return;

        if(_manaPotionInput.WasPressedThisFrame() && PlayerData.Instance.manaPotions > 0)
        {
            Mana();
        }
        if(_healthPotionInput.WasPressedThisFrame() && PlayerData.Instance.healthPotions > 0)
        {
            Health();
        }
    }

    void Mana()
    {
        PlayerData.Instance.currentMana += _manaReg;
        PlayerData.Instance.manaPotions --;
        ManaText();
        _actualizacionesdeCanvas.UpdateManaBar();
        PlayerData.Instance.currentMana = Mathf.Clamp(PlayerData.Instance.currentMana, 0, PlayerData.Instance.maxMana);
    }
    void Health()
    {
        PlayerData.Instance.currentHealth += _healthReg;
        PlayerData.Instance.healthPotions --;
        HealthText();
        _actualizacionesdeCanvas.UpdateHealthBar();
        PlayerData.Instance.currentHealth = Mathf.Clamp(PlayerData.Instance.currentHealth, 0, PlayerData.Instance.maxHealth);
    }


    public void ManaText()
    {
        manaText.text = "x" + PlayerData.Instance.manaPotions.ToString();
    } 
    


    public void HealthText()
    {
        healthText.text = "x" + PlayerData.Instance.healthPotions.ToString();
    } 

    public void Money()
    {
        int valueRandom = Random.Range(239, 875);
        PlayerData.Instance.money += valueRandom;
        Debug.Log(valueRandom);
    }

    public void TakeDamage(float damage)
    {
        if(PlayerData.Instance.currentHealth <= 0) return;
        
        PlayerData.Instance.currentHealth -= damage;
        _actualizacionesdeCanvas.UpdateHealthBar();
        if(PlayerData.Instance.currentHealth <= 0)
        {
            GameManager.Instance._isDead = true;
            _animator.SetTrigger("IsDead");
            Debug.Log("Muerto");
            //Destroy(gameObject);
        }
    }

    public void ManaShoot()
    {
        PlayerData.Instance.currentMana += 5;
        _actualizacionesdeCanvas.UpdateManaBar();
    }
}