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

    [Header("Barra")]
    public int currentNoru = 0;
    public int maxNoru = 100;

    void Awake()
    {
        _kenonAbility = InputSystem.actions["KenonAttack"];
    }

    void Start()
    {
        currentNoru = maxNoru;
    }

    void Update()
    {
        if(_kenonAbility.WasPressedThisFrame() && currentNoru == maxNoru)
        {
            StartCoroutine(Habilidad());

            //StartCoroutine(Habilidad());
        }
    }

    public IEnumerator Habilidad()
    {
        currentNoru = 0;
        UpdateKenonBar();
        Time.timeScale = 0;
        kenonAbiltyVideo.SetActive(true);
        yield return new WaitForSecondsRealtime(5.4f);
        Debug.LogWarning("CHAVBL ESTO FALLA");
        kenonAbiltyVideo.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChargingNoru(int quantity)
    {
        currentNoru += quantity;
        UpdateKenonBar();
    }

    void UpdateKenonBar()
    {
        float noruBar = currentNoru / maxNoru;
        kenonImage.fillAmount = noruBar;
    }
}