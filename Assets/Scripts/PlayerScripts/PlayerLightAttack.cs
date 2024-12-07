using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttack : MonoBehaviour
{
    public GameObject lightattack;
    
    public SphereCollider trigger;
  
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2f);
        lightattack.SetActive(false);
        trigger.enabled = false;
        Invoke("PlayerLightTrigger", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PlayerLightTrigger()
    {
        lightattack.SetActive(true);

      
        StartCoroutine(AttackTrigger2());
    }
    IEnumerator AttackTrigger2()
    {
        trigger.enabled = true;
        yield return new WaitForSeconds(0.2f);
        trigger.enabled = false;

    }
 
}
