using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Ultimate2 : MonoBehaviour
{
    ManaBar mana;
    public PlayerMovement playerMovement;
    public GameObject[] particle2;
    public GameObject FreelookCam;
    public GameObject _virtualCamera;
    //Bomb bomb;
    FindEnemy _findEnemy;
    private Animator anim;
    Ultimate2 U2;
    public GameObject[] enemy;
    
    private void Awake()
    {
        mana = LevelManager.instance.player.GetComponent<ManaBar>();
        _findEnemy = GameObject.Find("Player").transform.Find("Track").GetComponent<FindEnemy>();
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        particle2 = GameObject.FindGameObjectsWithTag("Ultimate2");
        U2 = GetComponent<Ultimate2>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {

        
        if (FreelookCam.activeSelf == false)
        {
            Invoke("FreelookF", 0.01f);
        }
        if (_virtualCamera.activeSelf == false)
        {
            Invoke("Virtual", 0.01f);
        }
    }
    public void DoUltiamte2Damage() => StartCoroutine(ISdamage());
    public void DoUltiamte2()
    {
        StartCoroutine(LevelManager.instance.player.GetComponent<PlayerMovement>().Damage(3.2f));
        mana.SpendMana(25);
        StartCoroutine(ISdamage());
        _findEnemy.isLock = false;
        if (FreelookCam.activeSelf == true)
        {
            Cinemachineshake.instance.Shaker(50f, 5f);
        }
        else { }




        //Invoke("Bomb", 0.02f);
        Ultimate2Play();
        GetComponent<Collider>().enabled = true;
        StartCoroutine(HideUltimate());
    }

    IEnumerator HideUltimate()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider>().enabled = false;
    }
    private void Ultimate2Play()
    {
     
        particle2[0].GetComponent<ParticleSystem>().Play();
        particle2[1].GetComponent<ParticleSystem>().Play();
        particle2[2].GetComponent<ParticleSystem>().Play();
        particle2[3].GetComponent<ParticleSystem>().Play();
        particle2[4].GetComponent<ParticleSystem>().Play();
        particle2[5].GetComponent<ParticleSystem>().Play();
        particle2[6].GetComponent<ParticleSystem>().Play();
        
    }

    IEnumerator Cam(float Wait)
    {
        Cinemachineshake.freeLook.m_Priority = 50;
        yield return new WaitForSeconds(Wait);
        Cinemachineshake.freeLook.m_Priority = 20;

    }
    IEnumerator Lockfalse()
    {
        yield return new WaitForSeconds(.01f);
        _findEnemy.isLock = false;
    }
    IEnumerator VirtualcameraONOFF()
    {
        _virtualCamera.SetActive(true);
        yield return new WaitForSeconds(.001f);
        _virtualCamera.SetActive(false);
    }
    IEnumerator ISdamage()
    {
        playerMovement._IsDamage = true;
        yield return new WaitForSeconds(2.95f);
        playerMovement._IsDamage = false;
    }
    void FreelookF() => Cinemachineshake.instance.Shaker(0, 0);
    void Virtual() => Cinemachineshake2.instance.Shaker2(0, 0);


    //private void Bomb()
    //{
    //    for (int i = 0; i < enemy.Length; i++)
    //    {


    //            enemy[i].GetComponent<EnemyController>().Bombs();

    //    }

    //}
}