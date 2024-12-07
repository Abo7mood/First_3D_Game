using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSys : MonoBehaviour
{
    ParticleSystem p;
    // Start is called before the first frame update
    private void Awake() => p = GetComponent<ParticleSystem>();
   

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startP()
    {
        p.Play(); p.enableEmission = true;
    }
    public void stopP()
    {
        p.Stop(); p.enableEmission = false;
    }
}
