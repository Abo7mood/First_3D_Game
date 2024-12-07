using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFx : MonoBehaviour
{
    public static VFx instance;
   [HideInInspector] public bool CanInstintiate;
    public float DestroyTime;
    public float MakeTime;
    public GameObject BossAttackPrefab;
    
    ParticleSystem[] IcePS;
     List<ParticleSystem> IceChilds = new List<ParticleSystem>();
    private Vector3 zero = new Vector3(0, 0, 0);
    Transform UltiTransform;
    // Start is called before the first frame update
    void Awake()
    {
       
        instance = this;
        CanInstintiate = true;
        IcePS = BossAttackPrefab.gameObject.GetComponentsInChildren<ParticleSystem>();
      
        Invoke("gfg", 2f);
        
     
    }
    

    private void Start()
    {
        Invoke("Invo", .01f);
       
    }
   
    private void gfg()
    {
     
        BossAttackPrefab.SetActive(true);
        foreach (ParticleSystem ChildPs in IcePS)
        {

            IceChilds.Add(ChildPs);
            ChildPs.GetComponent<ParticleSystem>().Play();
        }




    }
   
    public IEnumerator BossAttackIceInstintiate()
    {
        while  (CanInstintiate==true)
        {
          

            GameObject BossAttackIceDAD= Instantiate(BossAttackPrefab,BossController.instance.IceAttackLocation.transform.position,Quaternion.identity, null);
            BossAttackIceDAD.transform.rotation = BossController.instance.IceAttackRotation.transform.rotation;
            Destroy(BossAttackIceDAD, DestroyTime);
            yield return new WaitForSeconds(MakeTime);
            GameObject BossAttackIceDAD1 = Instantiate(BossAttackPrefab, BossController.instance.IceAttackLocation.transform.position, Quaternion.identity, null);
            BossAttackIceDAD1.transform.rotation = BossController.instance.IceAttackRotation.transform.rotation;
            Destroy(BossAttackIceDAD1, DestroyTime);
       

        }
      
    }
    private void Invo()=> MakeTime = IceAttack.instance.distance /37.5f;
   

}
