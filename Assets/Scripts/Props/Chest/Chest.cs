using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private bool isOpen = false;

    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(!isOpen)
        {
            _animator.SetTrigger("IsOpen");
            
        }
        return;
        
    }
}
