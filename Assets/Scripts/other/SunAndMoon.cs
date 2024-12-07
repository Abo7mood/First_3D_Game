using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAndMoon : MonoBehaviour
{
  
    [SerializeField]
    float eulerAngX;
    [SerializeField]
    float eulerAngY;
    [SerializeField]
    float eulerAngZ;

    public Transform b;

    GameObject _Particle;
    GameObject _Particle1;
    GameObject _Particle2;
    public float x;

    // Start is called before the first frame update



    private void Awake()
    {
        _Particle = GameObject.Find("Rain");
        _Particle1 = GameObject.Find("Cloud");
        _Particle2 = GameObject.Find("Cloud1");
       

    }

    // Update is called once per frame
    void Update()
    {

        eulerAngX = transform.localEulerAngles.x;
        eulerAngY = transform.localEulerAngles.y;
        eulerAngZ = transform.localEulerAngles.z;

       
        transform.RotateAround(Vector3.zero, Vector3.right, x * Time.deltaTime);

        if (eulerAngX > 200)
        {
         
            _Particle.GetComponent<ParticleSystem>().Play();
            _Particle1.GetComponent<ParticleSystem>().Play();
            _Particle2.GetComponent<ParticleSystem>().Stop();


        }
        
        else
        {
            _Particle.GetComponent<ParticleSystem>().Stop();
            _Particle1.GetComponent<ParticleSystem>().Stop();
            _Particle2.GetComponent<ParticleSystem>().Play();

        }

    }
}
