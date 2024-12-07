using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAttackCollider : MonoBehaviour
{
    BoxCollider box;
    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }
    void Start()
    {
        box.enabled = false;
        StartCoroutine(Box());
    }
    
    IEnumerator Box()
    {
        box.enabled = false;
        yield return new WaitForSeconds(.4f);
        box.enabled = true;
    }
}
