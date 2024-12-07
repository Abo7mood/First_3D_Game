using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    PlayerSword sword;
 
    private void Awake()=> sword = GetComponent<PlayerSword>();
      
    public void DoAttack1()
    {
        transform.Find("C").GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(HideCollider2());

    }
    public void DoAttackU1()
    {
        transform.Find("C1").GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(HideCollider3());
    }
    
    IEnumerator HideCollider2()
    {
        yield return new WaitForSeconds(0.1f);
        transform.Find("C").GetComponent<BoxCollider>().enabled = false;
    }
    IEnumerator HideCollider3()
    {
        yield return new WaitForSeconds(.4f);
        transform.Find("C1").GetComponent<BoxCollider>().enabled = false;
    }
    
}
