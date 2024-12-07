using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{  
   public Collider[] colliders;
    Vector3 test;
    // Start is called before the first frame update
    void Awake()
    {         
        SetRigidbody(true);
        SetColliders(false);
    }
    
    public void SetRigidbody(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody  in rigidbodies)
        {    
            rigidbody.isKinematic = state;
            if (state == true)
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
            else
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

        }
       
    }
    public void SetColliders(bool state)
    {
        foreach (Collider collider in colliders) { collider.enabled = state;}         
    }
}
