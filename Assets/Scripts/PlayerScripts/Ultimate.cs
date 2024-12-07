using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Ultimate : MonoBehaviour
{
    ManaBar mana;
    public GameObject[] Particle;
    public GameObject FreelookCam;
    public GameObject _virtualCamera;
    //Bomb bomb;
    FindEnemy _findEnemy;
    private Animator anim;
    Ultimate U;
    public GameObject[] enemy;
    private void Awake()
    {
        mana = LevelManager.instance.player.GetComponent<ManaBar>();
        StartCoroutine(VirtualcameraONOFF());
           _findEnemy = GameObject.Find("Player").transform.Find("Track").GetComponent<FindEnemy>();
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        Particle = GameObject.FindGameObjectsWithTag("Ultimate");
        U = GetComponent<Ultimate>();
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
    public void DoUltiamte()
    {
        StartCoroutine(LevelManager.instance.player.GetComponent<PlayerMovement>().Damage(1.8f));
        mana.SpendMana(25);
        StartCoroutine(Lockfalse());
        if (FreelookCam.activeSelf == true)
        {
            Cinemachineshake.instance.Shaker(50f, 5f);
        }
        else { }

        //Invoke("Bomb", 0.02f);
        UltimatePlay();
        GetComponent<Collider>().enabled = true;
        StartCoroutine(HideUltimate());
    }

    IEnumerator HideUltimate()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider>().enabled = false;
    }
    private void UltimatePlay()
    {
        Particle[0].GetComponent<ParticleSystem>().Play();
        Particle[1].GetComponent<ParticleSystem>().Play();
        Particle[2].GetComponent<ParticleSystem>().Play();
        Particle[3].GetComponent<ParticleSystem>().Play();
        Particle[4].GetComponent<ParticleSystem>().Play();
        Particle[5].GetComponent<ParticleSystem>().Play();
        Particle[6].GetComponent<ParticleSystem>().Play();
        Particle[7].GetComponent<ParticleSystem>().Play();
        Particle[8].GetComponent<ParticleSystem>().Play();
        Particle[9].GetComponent<ParticleSystem>().Play();
        Particle[10].GetComponent<ParticleSystem>().Play();

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