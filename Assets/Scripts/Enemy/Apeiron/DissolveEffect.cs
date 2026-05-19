using UnityEngine;
using System.Collections;

public class DissolveEffect : MonoBehaviour
{
    private AperionAI _apeironAI;

    
    //Muerte
    public SkinnedMeshRenderer skinnedMesh;
    private Material[] skinnedMaterial;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    void Awake()
    {
        _apeironAI = GetComponentInParent<AperionAI>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(skinnedMesh != null)
        {
            skinnedMaterial = skinnedMesh.materials;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_apeironAI._currentLife <= 0)
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        if(skinnedMaterial.Length > 0)
        {
            float counter = 0;
            while(skinnedMaterial[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterial.Length; i++)
                {
                    skinnedMaterial[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
