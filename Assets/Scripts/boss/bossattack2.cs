using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bossattack2 : MonoBehaviour
{
   public GameObject lightattack;
     Image Warn;
   public SphereCollider trigger;
    public GameObject Danger;
    // Start is called before the first frame update
    private void Awake()
    {
        lightattack.SetActive(false);
        trigger.enabled = false;
         
        Warn = GameObject.Find("Canvas").transform.Find("LightWarn").GetComponent<Image>();
     
    }
    void Start()
    {
      
        Warn.gameObject.SetActive(true);
        Invoke("BossAttack2Trigger",1f);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void BossAttack2Trigger()
    {
        lightattack.SetActive(true);
        Warn.gameObject.SetActive(false);
        Destroy(Danger);
        StartCoroutine(AttackTrigger());
    }
    IEnumerator AttackTrigger()
    {
        trigger.enabled = true;
        yield return new WaitForSeconds(0.2f);
        trigger.enabled = false;

    }
}
