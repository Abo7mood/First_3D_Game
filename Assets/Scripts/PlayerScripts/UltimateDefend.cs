using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateDefend : MonoBehaviour
{
    public GameObject particlemom;
    public GameObject PlayerS;
    private Vector3 Randomize = new Vector3(1f, 1f, 0);
    public GameObject Particle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("C")|| other.CompareTag("C1"))
        {
    
          
            HitParticle();
        }
        
    }
    private void HitParticle()
    {
      
        particlemom = Instantiate(Particle, PlayerS.transform.position, Quaternion.identity, PlayerS.transform);
        Destroy(particlemom, 1f);
        particlemom.transform.parent = null;
       



    }
}
