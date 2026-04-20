using UnityEngine;
using UnityEngine.UI;
public class ShopAP : MonoBehaviour, IInteractable
{

    //Controler
    [Header("Controller")]
    public GameObject canvasAP;
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
        healthCount --;
        HealthText();
        PriceNow();
    }

    void ManaText()
    {
        manaText.text = ":" + manaCount.ToString();
    }
    void HealthText()
    {
        healthText.text = ":" + healthCount.ToString();
    }

    public void PriceNow()
    {
        manaPriceMultiplied = priceMana * manaCount;
        healthPriceMultiplied = priceHealth * healthCount;
        totalPurchase = manaPriceMultiplied + healthPriceMultiplied;

        price.text = totalPurchase.ToString();
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
            ManaText();
            HealthText();
            PriceNow();
            Debug.Log("TU PUTA MADREEEEEEE");
        }
    }
}