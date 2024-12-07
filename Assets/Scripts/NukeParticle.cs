using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeParticle : MonoBehaviour
{
    public GameObject Expolsion;
    public float Power = 100;
    public float Radius = 100;
    public float Force = 100;
    void Start()
    {
        Invoke("Dontate", .4f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    
  
    public void Dontate()
    {
        Vector3 ExpolsionPosition = Expolsion.transform.position;
        Collider[] colliders = Physics.OverlapSphere(ExpolsionPosition, Radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(Power, ExpolsionPosition, Radius, Force, ForceMode.Impulse);
            }
        }
        
    }
 
}
