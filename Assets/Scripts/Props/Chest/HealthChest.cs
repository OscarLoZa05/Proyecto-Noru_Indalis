using Unity.Profiling;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthChest : MonoBehaviour, IInteractable
{
    //Booleanas
    private bool isOpen = false;

    //Components
    private Animator _animator;
    private PlayerResources _playerResources; 
    [SerializeField] private ParticleSystem _chestParticles;

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
            _playerResources.healthPotions++;
            _playerResources.HealthText();

            _playerResources.Money();
        }
        return;
    }
}