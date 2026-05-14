using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;


public class Kenon : MonoBehaviour
{
    private InputAction _kenonAbility;
    public Image kenonImage;
    public GameObject kenonAbiltyVideo;
    //public bool canKenonAttack = false;
    [Header("Kenon")]
    public Vector3 attackZone = new Vector3 (25,25,25);
    public bool haveKenon = false;

    void Awake()
    {
        _kenonAbility = InputSystem.actions["KenonAttack"];
    }

    void Start()
    {
        PlayerData.Instance.currentNoru = PlayerData.Instance.maxNoru;
    }

    void Update()
    {
        if(_kenonAbility.WasPressedThisFrame() && PlayerData.Instance.currentNoru == PlayerData.Instance.maxNoru && haveKenon)
        {
            
            StartCoroutine(Habilidad());

            //StartCoroutine(Habilidad());
        }
    }

    public void AtaqueKenon()
    {
        Collider[] enemies = Physics.OverlapBox(transform.position, attackZone);
            foreach (Collider enemy in enemies)
            {
                if(enemy.transform.gameObject.layer == 7)
                {
                    IEnemy enemy1 = enemy.GetComponent<IEnemy>();
                    if(enemy1 != null)
                    {
                        
                        //Debug.Log(enemy.transform.name);
                        enemy1.TakeDamage(75); 
                    }
                }
            }
    }
    

    public IEnumerator Habilidad()
    {
        
        PlayerData.Instance.currentNoru = 0;
        UpdateKenonBar();
        Time.timeScale = 0;
        kenonAbiltyVideo.SetActive(true);
        yield return new WaitForSecondsRealtime(5.4f);
        Debug.LogWarning("CHAVBL ESTO FALLA");
        kenonAbiltyVideo.SetActive(false);
        Time.timeScale = 1;
        AtaqueKenon();
        Debug.Log("TUPUTAMDARE");
    }

    public void ChargingNoru(int quantity)
    {
        PlayerData.Instance.currentNoru += quantity;
        UpdateKenonBar();
    }

    void UpdateKenonBar()
    {
        float noruBar = PlayerData.Instance.currentNoru / PlayerData.Instance.maxNoru;
        kenonImage.fillAmount = noruBar;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, attackZone);
    }
}