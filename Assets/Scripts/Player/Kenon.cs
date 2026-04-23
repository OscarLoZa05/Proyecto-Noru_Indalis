using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;


public class Kenon : MonoBehaviour
{
    private InputAction _kenonAbility;
    public Image kenonImage;
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
        }
    }

    public IEnumerator Habilidad()
    {
        yield return null;
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