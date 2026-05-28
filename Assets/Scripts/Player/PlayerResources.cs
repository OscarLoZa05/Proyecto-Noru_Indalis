using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
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

    public Transform _spawnPoint;

    //Player
    private PlayerAbility _playerAbility;
    [SerializeField] private string animationID = "life";

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
        DOTween.Restart("mana");
        PlayerData.Instance.currentMana += _manaReg;
        PlayerData.Instance.manaPotions --;
        _actualizacionesdeCanvas.ManaText();
        _actualizacionesdeCanvas.UpdateManaBar();
        PlayerData.Instance.currentMana = Mathf.Clamp(PlayerData.Instance.currentMana, 0, PlayerData.Instance.maxMana);
    }
    void Health()
    {
        DOTween.Restart("life");
        PlayerData.Instance.currentHealth += _healthReg;
        PlayerData.Instance.healthPotions --;
        _actualizacionesdeCanvas.HealthText();
        _actualizacionesdeCanvas.UpdateHealthBar();
        PlayerData.Instance.currentHealth = Mathf.Clamp(PlayerData.Instance.currentHealth, 0, PlayerData.Instance.maxHealth);
    }


    

    public void Money()
    {
        int valueRandom = Random.Range(239, 875);
        PlayerData.Instance.money += valueRandom;
        Debug.Log(valueRandom);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Has recibido Daño");
        if(PlayerData.Instance.currentHealth <= 0) return;
        
        PlayerData.Instance.currentHealth -= damage;
        _actualizacionesdeCanvas.UpdateHealthBar();
        if(PlayerData.Instance.currentHealth <= 0)
        {
            PlayerData.Instance.currentHealth = 100;
            PlayerData.Instance.currentMana = 100;
            _actualizacionesdeCanvas.UpdateHealthBar();
            _actualizacionesdeCanvas.UpdateManaBar();
            transform.position = _spawnPoint.position;
            //GameManager.Instance._isDead = true;
            //_animator.SetTrigger("IsDead");
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