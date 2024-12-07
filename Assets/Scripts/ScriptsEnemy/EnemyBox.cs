using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour
{
    //float min = 2, max=5;
   public CapsuleCollider capsuleCollider;
    // Start is called before the first frame update
    void Start() => capsuleCollider = GetComponent<CapsuleCollider>();
    public void Collider() => capsuleCollider.enabled = true;
}
