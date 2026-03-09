using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private bool isOpen = false;

    private Animator _animator;
    private PlayerResources _playerResources;

    [SerializeField] private ParticleSystem _chestParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(!isOpen)
        {
            isOpen = true;
            _animator.SetTrigger("IsOpen");
            _playerResources.manaPotions++;
            _playerResources.healthPotions++;
            _playerResources.ManaText();
            _playerResources.HealthText();
            Destroy(_chestParticles);
        }
        return;
        
    }
}
