using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySword : MonoBehaviour
{
    
  public static  EnemySword instance;
    public GameObject UltimateEnemyIcon;
    public BoxCollider sword1;
    public BoxCollider sword2;
    EnemySword sword;
    public Animator anim;
 
    private void Awake()
    {

        instance = this;
        sword = GetComponent<EnemySword>();
        anim = GetComponentInChildren<Animator>();
    }
    public void EnemyDoAttack()
    {
        sword1.enabled = true; 
        StartCoroutine(EnemyHideCollider());
    }
    public void EnemyDoAttack1()
    {
     UltimateEnemyIcon.SetActive(false);
        sword2.enabled = true;
        StartCoroutine(EnemyHideCollider1());
    }
  
    IEnumerator EnemyHideCollider()
    {
        sword1.enabled = true;
        yield return new WaitForSeconds(0.15f);
        sword1.enabled = false;
    }
    IEnumerator EnemyHideCollider1()
    {
        sword2.enabled = true;
        yield return new WaitForSeconds(0.15f);
        sword2.enabled = false;
    }
}
