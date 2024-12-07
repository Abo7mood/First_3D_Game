using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    public bool Life;
  
    SkinnedMeshRenderer skinnedMeshRenderer;
    public Material materialdissolve;
    public Material materialEnemy;

    public bool HideObject;
    public float DissolveSpeed = 1f;
  
    // Start is called before the first frame update
    private void Awake() { Life = true; skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>(); } 
    void Start()
    {
      
        Invoke("Changemat", 0.01f);
        Invoke("Changemat2", 0.02f);
        Invoke("Changemat", 1.5f);
        materialdissolve.SetFloat("_NoiseStrength", 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Life==true)
        {
            materialdissolve.SetFloat("_NoiseStrength", Mathf.MoveTowards(materialdissolve.GetFloat("_NoiseStrength"), -0.25f, DissolveSpeed * Time.deltaTime));
        }
        else
        {
            skinnedMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            Invoke("Changemat2", 0f);
            materialdissolve.SetFloat("_NoiseStrength", Mathf.MoveTowards(materialdissolve.GetFloat("_NoiseStrength"), 1.25f, DissolveSpeed*Time.deltaTime));
        }
      
    } 
    void Changemat()
    {
        Material[] mats = new Material[] { materialEnemy };
        skinnedMeshRenderer.materials = mats;

    }
    void Changemat2()
    {
        Material[] mats = new Material[] { materialdissolve };
        skinnedMeshRenderer.materials = mats;

    }
}