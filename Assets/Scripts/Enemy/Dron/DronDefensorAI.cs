using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class DronDefensorAI : MonoBehaviour
{
    public NavMeshAgent _enemyAgent;
    public enum EnemyState
    {
        Protecting,
        Nothing,
    }


    //

    //Player
    public Transform target;

    public EnemyState currentState;
    
    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = EnemyState.Nothing;
    }

    void Update()
    {
        switch(currentState)
        {
            case EnemyState.Protecting:
                Protect(target);
            break;
            case EnemyState.Nothing:
                Nothing();
            break;
            default:
                Nothing();
            break;

        }
    }

    public void Prepare(Transform target)
    {

        currentState = EnemyState.Protecting;
    }

    void Protect(Transform target)
    {
        _enemyAgent.SetDestination(target.position);
        Debug.Log("Putamadre");
    }

    void Nothing()
    {
        return;
    }



    /*public bool OnRange(float distance)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        
        if(distanceToPlayer <= distance)
        {
         return true;    
        }
        else
        {
            return false;
        }  
    }*/
}
