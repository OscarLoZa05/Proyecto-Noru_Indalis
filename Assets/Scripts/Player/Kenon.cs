using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;


public class Kenon : MonoBehaviour
{
    public ActualizaciondeCanvas _actualizacionesdeCanvas;
    private InputAction _kenonAbility;
    //public Image kenonImage;
    public GameObject kenonAbiltyVideo;
    //public bool canKenonAttack = false;
    [Header("Kenon")]
    public float kenonRadius = 10;
    public Transform attackPosition;
    public AudioSource _kenonAudio;
    

    void Awake()
    {
        _kenonAbility = InputSystem.actions["KenonAttack"];
    }

    void Start()
    {
    }

    void Update()
    {
        if(_kenonAbility.WasPressedThisFrame() && PlayerData.Instance.currentNoru >= PlayerData.Instance.maxNoru && GameManager.Instance.haveKenon)
        {
            
            StartCoroutine(Habilidad());

            //StartCoroutine(Habilidad());
        }
    }

    public void AtaqueKenon()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPosition.position, kenonRadius);
            foreach (Collider enemy in enemies)
            {
                if(enemy.transform.gameObject.layer == 7)
                {
                    IEnemy enemy1 = enemy.GetComponent<IEnemy>();
                    if(enemy1 != null)
                    {
                        
                        //Debug.Log(enemy.transform.name);
                        enemy1.TakeDamage(100); 
                    }
                }
            }
    }
    

    public IEnumerator Habilidad()
    {   
        GameManager.Instance.isChangingScene = true;
        _kenonAudio.ignoreListenerPause = true;
        AudioListener.pause = true;
        PlayerData.Instance.currentNoru = 0;
        _actualizacionesdeCanvas.UpdateKenonBar();
        Time.timeScale = 0;
        kenonAbiltyVideo.SetActive(true);
        yield return new WaitForSecondsRealtime(11.417f);
        Debug.LogWarning("CHAVBL ESTO FALLA");
        kenonAbiltyVideo.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
        AtaqueKenon();
        GameManager.Instance.isChangingScene = true;
        Debug.Log("TUPUTAMDARE");
    }

    public void ChargingNoru(int quantity)
    {
        PlayerData.Instance.currentNoru += quantity;
        _actualizacionesdeCanvas.UpdateKenonBar();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attackPosition.position, kenonRadius);
    }
}