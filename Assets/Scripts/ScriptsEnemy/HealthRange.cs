using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRange : MonoBehaviour
{
    public ParticleSystem[] particleaSystem;
    public GameObject MyRealEnemy;
    public GameObject HealFather;
    public List<Transform> enemies = new List<Transform>(); //The enemies player detection collider is touching
    public GameObject HealthParticle;
    public GameObject HealthParticle1;
    public ParticleSystem[] ParticleSystem1;
    public ParticleSystem[] ParticleSystem3;
    private List<ParticleSystem> ParticleSystem2;
    private void Awake()
    {
       // HealthParticle = transform.Find("HealParticle").gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
          

                enemies.Add(other.transform);

        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            for (int i = 0; i < enemies.Count; i++)
            {
               
                    enemies.RemoveAt(i);
               
            }

        }
       
    }


    public void HealEnemies()
    {
          for (int i = 0; i < enemies.Count; i++)
            {
              
                if (enemies[i] != null)
                    enemies[i].GetComponent<HealthBarFade>().EnemyHeal(100);
                
            }
        MyRealEnemy.GetComponent<HealthBarFade>().EnemyHeal(100);



    }
    public void InstintiateHealParticle()
    {
       
        if (HealthParticle != null)
        {
            Debug.Log("work");
            var HealingSystem = Instantiate(HealthParticle, transform.position,Quaternion.identity,null);
            if (HealingSystem != null)
            {

                foreach (Transform child in HealingSystem.transform)
                {
                    child.GetComponent<ParticleSystem>().Play();

                }
            }
           // HealingSystem.transform.parent = null;
           //var ParticleHeal= HealingSystem.GetComponentInChildren<ParticleSystem>();
           // ParticleHeal.Play();
        
            Destroy(HealingSystem, 3f);
        }
    
      

    }
}
