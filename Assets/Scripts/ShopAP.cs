using UnityEngine;
using UnityEngine.UI;
public class ShopAP : MonoBehaviour, IInteractable
{

    //Controler
    [Header("Controller")]
    public GameObject canvasAP;
    public GameObject canvasPlayer;
    public GameObject canvasKenon;
    [SerializeField] private bool _isOpenShop = false;

    //Shop
    [Header("Count")]
    [SerializeField] private int manaCount = 0;
    [SerializeField] private int healthCount = 0;
    public Text manaText;
    public Text healthText;

    [SerializeField] private int manaPriceMultiplied;
    [SerializeField] private int healthPriceMultiplied;

    [SerializeField] private int priceMana = 156;
    [SerializeField] private int priceHealth = 289;
    public Text price;
    public int totalPurchase;

    //Type
    [Header("Type")]
    [SerializeField] private bool manaPotionActived = false;
    [SerializeField] private GameObject manaCanva;
    [SerializeField] private bool healthPotionActived = true;
    [SerializeField] private GameObject healthCanva;

    //Componentes
    private PlayerResources _playerResources;

    void Awake()
    {
        _playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(totalPurchase);
    }

    public void Interact()
    {
        if(_isOpenShop == false)
        {
            //Canvas
            canvasPlayer.SetActive(false);
            canvasKenon.SetActive(false);
            _isOpenShop = true;
            canvasAP.SetActive(true);

            //Cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameManager.Instance._shopOpen = true;
            Time.timeScale = 0;

            return;
        }

        if(_isOpenShop == true)
        {
            CloseShop();
        }
    }

    public void CloseShop()
    {
        //Canvas
        canvasPlayer.SetActive(true);
        canvasKenon.SetActive(true);
        _isOpenShop = false;
        canvasAP.SetActive(false);

        //Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance._shopOpen = false;
        Time.timeScale = 1;

        manaCount = 0;
        healthCount = 0;
        ManaText();
        HealthText();

        return;
    }

    public void PlusMana()
    {
        manaCount ++;
        ManaText();
        PriceNow();
    }
    public void PlusHealth()
    {
        healthCount ++;
        HealthText();
        PriceNow();
    }
    public void MenosMana()
    {
        if(manaCount > 0)
        {
            manaCount --;
            ManaText();
            PriceNow();
        } 
    }
    public void MenosHealth()
    {
        if(healthCount > 0)
        {
           healthCount --;
           HealthText();
           PriceNow(); 
        }
    }

    void ManaText()
    {
        manaText.text = ": " + manaCount.ToString();
    }
    void HealthText()
    {
        healthText.text = ": " + healthCount.ToString();
    }

    public void PriceNow()
    {
        manaPriceMultiplied = priceMana * manaCount;
        healthPriceMultiplied = priceHealth * healthCount;
        totalPurchase = manaPriceMultiplied + healthPriceMultiplied;

        //price.text = totalPurchase.ToString();
    }

    public void Purchase()
    {
        if(_playerResources.money >= totalPurchase)
        {
            _playerResources.manaPotions += manaCount;
            _playerResources.ManaText();

            _playerResources.healthPotions += healthCount;
            _playerResources.HealthText();

            _playerResources.money -= totalPurchase;

            Debug.Log("Prueba");
            manaCount = 0;
            healthCount = 0;
            
            PriceNow();
            Debug.Log("TU PUTA MADREEEEEEE");

            if(healthPotionActived)
            {
                HealthText();
                return;
            }
            if(manaPotionActived)
            {
                ManaText();
                return;
            }

        }
    }

    public void ManaButton()
    {
        healthCount = 0;
        HealthText();
        manaPotionActived = true;
        healthPotionActived = false;
        healthCanva.SetActive(false);
        manaCanva.SetActive(true);

    }
    public void HealthButton()
    {
        manaCount = 0;
        ManaText();
        manaPotionActived = false;
        healthPotionActived = true;
        healthCanva.SetActive(true);
        manaCanva.SetActive(false);
        
    }
}