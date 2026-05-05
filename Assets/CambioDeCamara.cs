using UnityEngine;
using Unity.Cinemachine; // Si usas una versión antigua, usa: using Cinemachine;

public class CambioDeCamara : MonoBehaviour
{
    [Header("Referencias de Cámaras")]
    // En Cinemachine 3.x, ambos pueden referenciarse como CinemachineCamera
    // pero usamos los tipos específicos si quieres acceder a sus ajustes.
    [SerializeField] private CinemachineCamera freeLookCam; 
    [SerializeField] private CinemachineCamera thirdPersonCam;

    private bool esFreeLookActiva = true;

    void Start()
    {
        // Inicialización: FreeLook manda al principio
        freeLookCam.Priority = 20;
        thirdPersonCam.Priority = 10;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCameras();
        }
    }

    public void ToggleCameras()
    {
        esFreeLookActiva = !esFreeLookActiva;

        if (esFreeLookActiva)
        {
            freeLookCam.Priority = 20;
            thirdPersonCam.Priority = 10;
        }
        else
        {
            // --- SOLUCIÓN AL GIRO LOCO ---
            // Sincronizamos la posición y rotación "en bruto" de la cámara de salida 
            // a la de entrada ANTES de que el Brain haga el cambio.
            thirdPersonCam.ForceCameraPosition(
                freeLookCam.State.RawPosition, 
                freeLookCam.State.RawOrientation
            );

            thirdPersonCam.Priority = 20;
            freeLookCam.Priority = 10;
        }
    }
}