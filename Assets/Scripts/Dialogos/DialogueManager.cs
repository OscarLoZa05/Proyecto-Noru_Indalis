using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Text dialogueTxt;
    [SerializeField] Text nameTxt;
    [SerializeField] Image icono;
    [SerializeField] bool Escribiendo;
    [SerializeField] DialogueData Startdialogo;
    public bool DialogueStarted;
    public GameObject DCanvas;
    public int Index;

    public void Start()
    {
        Escribiendo = false;
        dialogueTxt.text = Startdialogo.frases[Index].lafrase;
        nameTxt.text = Startdialogo.frases[Index].nombrePersonaje;
        Debug.Log($"Txt: {dialogueTxt}, Data: {Startdialogo}");
        icono.sprite = Startdialogo.frases[Index].Icono;
    }

    // Llama a esto para empezar el diálogo DESDE EL PRINCIPIO (Frase 1)
    public void IniciarDialogo(DialogueData dialogos)
    {
        Index = 0;
        DialogueStarted = true;
        DCanvas.SetActive(true);

        // Bloqueamos los controles al empezar a hablar (Frase 1)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DesactivarControlesYCamara();
        }

        ActualizarIntefraz(dialogos);
    }

    // Llama a esto desde otro script (un trigger, un botón, etc.) cuando quieras mostrar las frases 4 y 5
    public void RetomarDialogo(DialogueData dialogos)
    {
        DialogueStarted = true;
        DCanvas.SetActive(true);

        // Volvemos a bloquear los controles para que no se mueva mientras lee el final
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DesactivarControlesYCamara();
        }

        // Avanzamos al siguiente índice (que será el 3, es decir, la 4ª frase) antes de actualizar la UI
        Index++; 
        ActualizarIntefraz(dialogos);
    }

    public void ChangeFrase(DialogueData dialogos)
    {
        if(!Escribiendo)
        {
            // --- CORTE EN EL TERCER DIÁLOGO ---
            // Index 2 = Tercera frase. Cuando el jugador pulsa para pasarla:
            if (Index == 2) 
            {
                DCanvas.SetActive(false); // Quitamos el Canvas de diálogos temporalmente
                DialogueStarted = false;

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ActivarControlesYCamara(); // Desbloqueamos controles y cámara
                }

                // Hacemos un return aquí para congelar el Index en 2 hasta que llames a RetomarDialogo
                return; 
            }

            // --- FINAL ABSOLUTO DEL DIÁLOGO (Al terminar la Frase 5) ---
            if (Index >= dialogos.frases.Length - 1)
            {
                DCanvas.SetActive(false);
                Time.timeScale = 1; // Dejar en 1 siempre para evitar que se congele el motor
                DialogueStarted = false;
                Index = 0; // Reseteamos el índice para futuros diálogos por completo

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ActivarControlesYCamara(); // Nos aseguramos de liberar los controles
                }

                // -------------------------------------------------------------
                // ¡AQUÍ COLOCAS LO QUE QUIERES QUE PASE AL ACABAR EL 5º DIÁLOGO!
                // -------------------------------------------------------------
                Debug.Log("El quinto diálogo ha terminado. Ejecutando acción X...");
                
                // Pon aquí tu código. Ejemplos:
                // GameManager.Instance.haveKenon = true;
                // UnityEngine.SceneManagement.SceneManager.LoadScene("SiguienteNivel");
                // tuObjetoOEnemigo.SetActive(true);
                
                // -------------------------------------------------------------

                return;
            }

            Index++;
            ActualizarIntefraz(dialogos);
        }
    }

    public void ActualizarIntefraz(DialogueData dialogos)
    {
        StartCoroutine(TextAnimation(dialogos));
    }

    public IEnumerator TextAnimation(DialogueData dialogos)
    {
        Escribiendo = true;

        dialogueTxt.text = "";
        nameTxt.text = "";

        float duracion = dialogos.Cooldowns[Index];
        
        dialogueTxt.DOText(dialogos.frases[Index].lafrase, duracion).SetEase(Ease.Linear);
        nameTxt.DOText(dialogos.frases[Index].nombrePersonaje, duracion).SetEase(Ease.Linear);
        icono.sprite = dialogos.frases[Index].Icono;

        yield return new WaitForSeconds(duracion);

        Escribiendo = false;
    }
}