using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bossattack1 : MonoBehaviour
{
   public BossController bossController;
     Image Warn;
  
   
    // Start is called before the first frame update
    private void Awake()
    {
        Warn = GameObject.Find("Canvas").transform.Find("FireWarn").GetComponent<Image>();
       
    }
    void Start()
    {
        Warn.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void BossAttack1Trigger()
    {
        
        //Warn.gameObject.SetActive(false);
        //Destroy(Danger);
        //
    }
    //IEnumerator AttackTrigger()
    //{
    //    trigger.enabled = true;
    //    yield return new WaitForSeconds(1f);
    //    trigger.enabled = false;

    //}
}
