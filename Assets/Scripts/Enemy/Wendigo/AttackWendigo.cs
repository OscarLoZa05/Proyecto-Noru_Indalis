using JetBrains.Annotations;
using UnityEngine;

public class AttackWendigo : MonoBehaviour
{

    private WendigoAI _wendigoAI;
    public Transform _attackPosition;
    public float _attackRadius = 50; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _wendigoAI = GetComponentInParent<WendigoAI>();
    }

    public void Attack1()
    {
        Collider[] players = Physics.OverlapSphere(_attackPosition.position, _attackRadius);
            foreach (Collider item in players)
            {
                if(item.gameObject.CompareTag("Player"))
                {
                    PlayerResources _playerResources = item.GetComponent<PlayerResources>();
                    
                    if(_playerResources != null)
                    {
                        _playerResources.TakeDamage(75);
                        _wendigoAI.currentState = WendigoAI.EnemyState.Charging;
                    }
                }
            }
        
            
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_attackPosition.position, _attackRadius);
    }
}
