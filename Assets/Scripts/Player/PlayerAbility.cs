using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerAbility : MonoBehaviour
{

    //Inputs
    private InputAction _ability1;
    private InputAction _ability2;
    private InputAction _ability3;
    private InputAction _ability4;
    private InputAction _kenonAbility;
    /*
    //Canvas
    [Header("Canvas")]
    public Image manaBarImage;
    public Image imageAbility1;
    public Image imageAbility2;
    public Image imageAbility3;
    public Image imageAbility4;
    */

    //WaterAbilities
    [Header("WAttack")]
    public float cooldownAbility1 = 15;
    public float currentCooldown1 = 15;
    public bool ability1Used = false;
    private float maxDistance = 10;
    private float playerForceImpulse = 20;
    public int manaWasted1 = 15;

    [Header("WState")]
    public float cooldownAbility2 = 10;
    public float currentCooldown2 = 10;
    public bool ability2Used = false;
    public int manaWasted2 = 25;

    //FireAbilities
    [Header("FState")]
    public float cooldownAbility3 = 10;
    public float currentCooldown3 = 10;
    public int chargeAbility;
    public bool ability3Used = false;
    public int manaWasted3 = 50;

    [Header("FAttack")]
    public float cooldownAbility4 = 10;
    public float currentCooldown4 = 10;
    public bool ability4Used = false;
    [SerializeField] private Vector3 hitBoxLocalOffset = new Vector3(0f, 1f, 3f);
    [SerializeField] private Vector3 hitBoxSize = new Vector3(5f, 4f, 10f); 
    public int manaWasted4 = 50;

    [Header("Kenon")]
    public bool canKenonAttack = false;
    [SerializeField] private Vector3 hitBoxKenonLocalOffset;
    [SerializeField] private Vector3 hitBoxKenonSize;

    PlayerController _playerController;
    PlayerResources _playerResource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerResource = GetComponent<PlayerResources>();

        _ability1 = InputSystem.actions["WaterAttack"];
        _ability2 = InputSystem.actions["WaterState"];
        _ability3 = InputSystem.actions["FireState"];
        _ability4 = InputSystem.actions["FireAttack"];
    }

    // Update is called once per frame
    void Update()
    {

        //fireAttackRange  = transform.rotation * rangeAttack;

        if(_ability1.WasPressedThisFrame() && ability1Used == false && _playerResource.currentMana >= manaWasted1)
        {
            WAttack();
            ManaUsed(manaWasted1);
            ability1Used = true;
            currentCooldown1 = 0;
            //imageAbility1.fillAmount = 0;
        }
        if(_ability2.WasPressedThisFrame() && ability2Used == false && _playerResource.currentMana >= manaWasted2)
        {
            StartCoroutine(WState());
            ManaUsed(manaWasted2);
            ability2Used = true;
            currentCooldown2 = 0;
            //imageAbility2.fillAmount = 0;
        }
        if(_ability3.WasPressedThisFrame() && ability3Used == false && _playerResource.currentMana >= manaWasted3)
        {
            StartCoroutine(FState());
            ManaUsed(manaWasted3);
            ability3Used = true;
            currentCooldown3 = 0;
            //imageAbility3.fillAmount = 0;
        }
        if(_ability4.WasPressedThisFrame() && ability4Used == false && _playerResource.currentMana >= manaWasted4)
        {
            FAttack();
            ManaUsed(manaWasted4);
            ability4Used = true;
            currentCooldown4 = 0;
            //imageAbility4.fillAmount = 0;
        }

        if(ability1Used)
        {
            currentCooldown1 += Time.deltaTime;
            float progressAbility1 = currentCooldown1 / cooldownAbility1;
            //imageAbility1.fillAmount = progressAbility1;

            if(currentCooldown1 >= cooldownAbility1)
            {
                ability1Used = false;
            }
        }
        if(ability2Used)
        {
            currentCooldown2 += Time.deltaTime;
            float progressAbility2 = currentCooldown2 / cooldownAbility2;
            //imageAbility2.fillAmount = progressAbility2;

            if(currentCooldown2 >= cooldownAbility2)
            {
                ability2Used = false;
            }
        }
        if(ability3Used)
        {
            currentCooldown3 += Time.deltaTime;
            float progressAbility3 = currentCooldown3 / cooldownAbility3;
            //imageAbility3.fillAmount = progressAbility3;

            if(currentCooldown3 >= cooldownAbility3)
            {
                ability3Used = false;
            }
        }
        if(ability4Used)
        {
            currentCooldown4 += Time.deltaTime;
            float progressAbility4 = currentCooldown4 / cooldownAbility4;
            //imageAbility4.fillAmount = progressAbility4;

            if(currentCooldown4 >= cooldownAbility4)
            {
                ability4Used = false;
            }
        }
    }

    void WAttack()
    {
        Debug.Log("Habilidad 1 Usada");
        Collider[] enemies = Physics.OverlapSphere(transform.position, maxDistance);
            foreach (Collider enemy in enemies)
            {
                if(enemy.transform.gameObject.layer == 7)
                {
                    Rigidbody _enemyRigidBody = enemy.GetComponent<Rigidbody>();

                    Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    float forceMultiplier = Mathf.Clamp01(1 - (distanceToEnemy / maxDistance));

                    directionToEnemy.y = 0;

                    Vector3 force = (directionToEnemy * forceMultiplier);

                    _enemyRigidBody.AddForce(0, 2, 0, ForceMode.Impulse);
                    _enemyRigidBody.AddForce(force * playerForceImpulse, ForceMode.Impulse);
                    

                    Debug.Log(force);
                }
            }
    }

    IEnumerator WState()
    {
        Debug.Log("Habilidad 2 Usada");
        _playerController._playerSpeed = 20;
        yield return new WaitForSeconds(5);
        _playerController._playerSpeed = 5;
    }

    void FAttack()
    {
        Debug.Log("Habilidad 3 Usada");

        Vector3 hitboxWorldCenter = transform.TransformPoint(hitBoxLocalOffset);

        /*Collider[] enemies = Physics.OverlapBox(hitBoxWorldCenter, hitBoxSize * 0.5f, transform.rotation);
            foreach (Collider enemy in enemies)
            {

            }*/
    }

    IEnumerator FState()
    {
        Debug.Log("Habilidad 4 Usada");

        for (chargeAbility = 0; chargeAbility <= 4; chargeAbility++)
        {
            _playerResource.currentHealth += 10;
            _playerResource.currentHealth = Mathf.Clamp(_playerResource.currentHealth, 0, _playerResource.maxHealth);
            yield return new WaitForSeconds(2);
        }
    }

    void ManaUsed(int ManaWasted)
    {
        _playerResource.currentMana -= ManaWasted;
        
        _playerResource.UpdateManaBar();
    }
    



    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(hitBoxLocalOffset, hitBoxSize);

    }
}