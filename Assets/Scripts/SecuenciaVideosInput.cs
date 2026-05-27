using UnityEngine;
using UnityEngine.Video;

public class SecuenciaVideosInput : MonoBehaviour
{
    [Header("Referencias")]
    public VideoPlayer videoPlayer;

    [Header("Lista de Videos")]
    [Tooltip("Asegúrate de poner los 5 videos en el orden correcto (0 al 4)")]
    public VideoClip[] listaDeVideos;

    [Header("Configuración Evento Final")]
    [Tooltip("¿Cuántos segundos antes de que acabe el ÚLTIMO video quieres activar el evento?")]
    public float segundosAntesDeTerminar = 1.5f; 

    private int indiceActual = 0;
    private bool eventoEjecutado = false; // Evita que la función se llame mil veces en el Update

    void Start()
    {
        if (videoPlayer == null || listaDeVideos.Length < 5)
        {
            Debug.LogError("Por favor, asigna el VideoPlayer y asegúrate de tener al menos 5 videos en la lista.");
            return;
        }

        videoPlayer.loopPointReached += AlTerminarVideoInstancia;
        ReproducirPaso(0);
    }

    void Update()
    {
        if (videoPlayer == null) return;

        // --- 0. ATAJO PARA SALTAR TODO (Bloq Mayús / CapsLock) ---
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            Debug.Log("Secuencia saltada con Bloq Mayús.");
            videoPlayer.Stop(); // Detiene el video actual
            NextLevel();
            return; // Salimos del Update para evitar interferencias
        }

        // --- 1. CONTROL DE INPUTS ---
        switch (indiceActual)
        {
            case 1:
                if (Input.GetKeyDown(KeyCode.LeftShift)) AvanzarAPaso(2);
                break;

            case 3:
                if (Input.GetKeyDown(KeyCode.Space)) AvanzarAPaso(4);
                break;
        }

        // --- 2. DETECTOR PARA EL EVENTO ANTICIPADO (SOLO EN EL ÚLTIMO VIDEO) ---
        if (indiceActual == 4 && !eventoEjecutado && videoPlayer.isPlaying)
        {
            // Calculamos cuánto tiempo le queda al video
            double tiempoRestante = videoPlayer.length - videoPlayer.time;

            if (tiempoRestante <= segundosAntesDeTerminar)
            {
                eventoEjecutado = true; // Nos aseguramos de que solo se ejecute UNA vez
                MiFuncionEspecial();
            }
        }
    }

    // Pon aquí adentro lo que quieres que pase un poco antes de terminar
    void MiFuncionEspecial()
    {
        NextLevel();
        Debug.Log("¡EVENTO ANTICIPADO! Faltan " + segundosAntesDeTerminar + " segundos para que termine el video final.");
        
        Debug.Log("PUTAMADRE");
    }

    void AlTerminarVideoInstancia(VideoPlayer vp)
    {
        if (indiceActual == 0) AvanzarAPaso(1);
        else if (indiceActual == 2) AvanzarAPaso(3);
        else if (indiceActual == 4)
        {
            Debug.Log("El último video ha terminado por completo.");
        }
    }

    void AvanzarAPaso(int nuevoIndice)
    {
        indiceActual = nuevoIndice;
        ReproducirPaso(indiceActual);
    }

    void ReproducirPaso(int indice)
    {
        if (indice >= listaDeVideos.Length) return;

        videoPlayer.isLooping = (indice == 1 || indice == 3);
        videoPlayer.clip = listaDeVideos[indice];
        videoPlayer.Play();
        
        // Si volvemos a empezar o reiniciamos de algún modo, reseteamos el trigger del evento
        if (indice == 4) eventoEjecutado = false; 

        Debug.Log("Reproduciendo Video " + indice + " (Modo Loop: " + videoPlayer.isLooping + ")");
    }

    void NextLevel()
    {
        // Evitamos que se intente cambiar de nivel más de una vez si se presiona rápido
        if (GameManager.Instance.isChangingScene) return;

        GameManager.Instance.isChangingScene = true;
        SceneController.Instance
            .NewTransition()
            .Unload(SceneDataBase.Slots.Onboard)
            .Load(SceneDataBase.Slots.Level2, SceneDataBase.Scenes.Level2)
            .WithOverlay()
            .Perform();   
    }
}