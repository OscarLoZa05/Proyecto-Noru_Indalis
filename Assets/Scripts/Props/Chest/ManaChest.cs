using Unity.Profiling;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaChest : MonoBehaviour, IInteractable
{
    public ActualizaciondeCanvas _actualizacionesdeCanvas;
    //Booleanas
    public bool isOpen = false;

    //Components
    private Animator _animator;
    private PlayerResources _playerResources; 
    [SerializeField] private ParticleSystem _chestParticles;
    [SerializeField] private GameObject canvasButton;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _open;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponent<Animator>();

        _playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(!isOpen)
        {
            _audioSource.PlayOneShot(_open);
            _chestParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            isOpen = true;
            _animator.SetTrigger("IsOpen");
            PlayerData.Instance.manaPotions++;
            _actualizacionesdeCanvas.ManaText();

            _playerResources.Money();
        }
        return;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            canvasButton.SetActive(true);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            canvasButton.SetActive(false);
        }
    }
}