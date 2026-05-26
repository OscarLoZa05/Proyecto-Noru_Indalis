using UnityEngine;

public class AttackWendigo : MonoBehaviour
{
    private WendigoAI _parentAI;

    [Header("Attack Area")]
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private float _attackRadius = 5f;
    [SerializeField] private int _damage = 50;

    void Awake()
    {
        _parentAI = GetComponentInParent<WendigoAI>();
    }

    // Pon este evento en el FRAME 0 de tu animación de ataque
    public void StartAttackAnimation()
    {
        if (_parentAI != null) _parentAI.ForzarFreno(true);
    }

    // Pon este evento en el FRAME EXACTO del impacto visual
    public void Attack()
    {
        if (_attackPosition == null) return;

        Collider[] hitColliders = Physics.OverlapSphere(_attackPosition.position, _attackRadius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                // Busca el script de vida de tu jugador (Ajusta 'PlayerResources' al nombre real de tu script)
                PlayerResources playerLife = hit.GetComponent<PlayerResources>();
                if (playerLife != null)
                {
                    playerLife.TakeDamage(_damage); // Ajusta 'TakeDamage' al nombre de tu función
                    Debug.Log("¡El jugador ha recibido daño del Wendigo!");
                }
            }
        }
    }

    // Pon este evento en el ÚLTIMO FRAME de tu animación de ataque
    public void EndAttackAnimation()
    {
        if (_parentAI != null) 
        {
            _parentAI.TerminarAtaqueYEntrarEnCooldown(); 
        }
    }

    public void SoundFoot()
    {
        if (_parentAI != null) _parentAI.SoundFoot();
    }

    void OnDrawGizmos()
    {
        if (_attackPosition != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_attackPosition.position, _attackRadius);
        }
    }
}