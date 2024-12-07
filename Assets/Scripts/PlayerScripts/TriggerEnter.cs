using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour
{



    #region strings
    string[] E = { "EnemyC", "EnemyC1", "EnemyC2", "EnemyC3", "EnemyC4", "EnemyC5", "EnemyC6", "EnemyC7", "EnemyC8", "EnemyC9", "Punch1", "Punch2" };
    string[] P = { "Speed(Clone)", "", "Shieldd(Clone)", "Hart(Clone)", "Sword(Clone)" };
    string[] A = { "LavaOuterSphere(Clone)", "IceOuterSphere(Clone)", "LightiningOuterSphere(Clone)" };
    #endregion
    //Enemyc,c1=The big Enemy;
    //c is the first damage and weaker c1 is the haver
    //Enemy2c,c3=green;
    //c2 is the first damage and weaker c3 is the haver
    //Enemyc4,c5=yellow;
    //c4 is the first damage and weaker c5 is the haver
    //Enemyc6,c7=blue;
    //c6 is the first damage and weaker c7 is the haver
    //Enemyc8,c9=red;
    //c8 is the first damage and weaker c9 is the haver
    #region Scripts
    PlayerMovement player;
    FindEnemy _findEnemy;
    HealthBarShrink shrink;
    #endregion


    #region GameObjects
    GameObject PlayerTransform;
    public GameObject SphereTransform;
    GameObject Baby;
    public GameObject PlayerHome;
    Rigidbody rb;
    public List<GameObject> PowersPS = new List<GameObject>();

    ParticleSystem HartPS;
    ParticleSystem ShieldPS;
    ParticleSystem SwordPS;
    ParticleSystem SpeedPS;


    #endregion

    #region float and int
    private float FreezeTime = 2f;
    const float Elipson = 0.1f;
    float speed = 1;
    public float duration;


    public float PlayerDamage5Old;
    [HideInInspector] public float PlayerDamage6Old;
    #endregion
    #region Boolean
    bool isbubbling;
    bool Canbubbling;

    #endregion
    #region Vectors 
    Vector3 PlayerScale = new Vector3(5, 5, 5);
    Vector3 minScale = new Vector3(0, 0, 0);
    #endregion
    private void Awake()
    {
        HartPS = PowersPS[0].GetComponent<ParticleSystem>();
        ShieldPS = PowersPS[1].GetComponent<ParticleSystem>();
        SwordPS = PowersPS[2].GetComponent<ParticleSystem>();
        SpeedPS = PowersPS[3].GetComponent<ParticleSystem>();

        Canbubbling = true;
        PlayerTransform = GameObject.Find("Player").transform.Find("PlayerTransform").gameObject;

        shrink = GetComponent<HealthBarShrink>();
        _findEnemy = GetComponentInChildren<FindEnemy>();
        player = GetComponent<PlayerMovement>();
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        
        if (Baby != null)
        {
            rb = Baby.GetComponent<Rigidbody>();
            rb.transform.position = Vector3.Lerp(rb.transform.position, PlayerTransform.transform.position, 1000f * Time.fixedDeltaTime);
        }


        if (isbubbling == true)
        {
            if (Baby != null)
            {
                rb = Baby.GetComponent<Rigidbody>();
                StartCoroutine(RepeatLerp(minScale, PlayerScale, duration));

                if ((rb.transform.position - PlayerTransform.transform.position).magnitude > Elipson)
                    Baby.transform.Translate(0, 0, LevelManager.instance._ShieldSpeed * Time.fixedDeltaTime);
                Vector3.MoveTowards(Baby.transform.localScale, PlayerScale, 1000f);
            }


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == P[0])
        {


            if (PlayerMovement.instance.isSpeedPowerEnabled == false)
            {
                StartCoroutine(LevelManager.instance.SpeedIcon());
                StartCoroutine(PlayerMovement.instance.Speeds());
                Destroy(other.gameObject);
                //sound
                Instantiate(PowersPS[3], other.transform.position, Quaternion.identity);
                SpeedPS.Play();
            }
            else { return; }


        }

        else if (other.name == P[2])
        {
            if (Canbubbling == true)
            {
                StartCoroutine(LevelManager.instance.ShieldIcon());
                Baby = Instantiate(SphereTransform, PlayerTransform.transform.position, Quaternion.identity);
                Destroy(other.gameObject, 0.01f);

                StartCoroutine(SphereBubble());

                isbubbling = true;
                Instantiate(PowersPS[1], other.transform.position, Quaternion.identity);
                ShieldPS.Play();
            }


        }
        else if (other.name == P[3])
        {
            if (LevelManager.instance.isHeal == false)
            {

                shrink.VEnemyH(LevelManager.instance.HealAmount);
                Destroy(other.gameObject, 0.01f);
                Instantiate(PowersPS[0], other.transform.position, Quaternion.identity);
                HartPS.Play();
            }

            else { return; }



        }
        else if (other.name == P[4])
        {

            if (LevelManager.instance.CanSword == true)
            {
                StartCoroutine(LevelManager.instance.SwordIcon());

                Instantiate(PowersPS[2], other.transform.position, Quaternion.identity);
                SwordPS.Play();

                StartCoroutine(PlayerMovement.instance.SwordAbility());
                Destroy(other.gameObject, 0.01f);

            }

        }
        else if (other.name == A[0])
        {          
            //Lava

        }
        else if (other.name == A[1])
        {
            //Ice
         

        }
        else if (other.name == A[2])
        {
            //Light
           

        }
        else if (other.name == "BossAttack(Clone)" && player._IsDamageUltimate == false && player._IsDamage == false)
        {


            LevelManager.instance.player.GetComponent<PlayerMovement>().fallanim();
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage11);

            StartCoroutine(player.Damage(2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHitBoss1());
        }
        else if (other.name == "LightAttack(Clone)" && player._IsDamage == false)
        {



            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage12);
            StartCoroutine(player.Damage(.2f));

            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHitBoss2(1.5f));
        }
        else if (other.name == "UltiamteBossIceAttack(Clone)" && player._IsDamage2 == false)
        {
            Debug.Log("1");
            StartCoroutine(player.IceEffectIenumerator());
            StartCoroutine(KnockBackFreeze(FreezeTime));
            StartCoroutine(FreezehitBoss());
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();
            StartCoroutine(player.Damage(.2f));

            InvokeRepeating("Ultimate2HitPlayer", 0f, .5f);
            StartCoroutine(player.Damage2(5f));
            PlayerMovement.instance.Freeze[0].GetComponent<ParticleSystem>().Play();
            PlayerMovement.instance.Freeze[1].GetComponent<ParticleSystem>().Play();
            PlayerMovement.instance.Freeze[2].GetComponent<ParticleSystem>().Play();
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }


        }


        else { return; }
    }
    private void OnTriggerStay(Collider other)
    {



        //sword1
        if (other.name == E[0] && player._IsDamage == false)
        {
            StartCoroutine(player.GitHit());
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(5f, .35f);

            else { Cinemachineshake.instance.Shaker(7.5f, .75f); }

            StartCoroutine(player.Damage(.2f));
            shrink.VEnemyC(LevelManager.instance.PlayerDamage1);
            player.GitHit();
            PlayerMovement.instance.DPStart();


        }
        //sword2
        else if (other.name == E[1] && player._IsDamage == false)
        {
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();


            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());


        
            
            if (other.gameObject.name == "EnemyC1")
            {
                if (other.GetComponentInParent<EnemyController>().canUltimate == true)
                {
                    other.GetComponentInParent<EnemyController>().canUltimate = false;
                    if (EnemySword.instance.UltimateEnemyIcon!=null)
                    EnemySword.instance.UltimateEnemyIcon.SetActive(false);

                    if (LevelManager.instance.isLight == false)
                    {
                        InvokeRepeating("EnemyHitPlayer", 0f, .3f);
                        StartCoroutine(LightHit());
                        StartCoroutine(EnemyController.Instance.ParticleIEnumerator());
                        shrink.VEnemyC(LevelManager.instance.EnemyBigDamage);
                        StartCoroutine(EnemyController.Instance.LightPlay());
                        StartCoroutine(player.GitHitBoss2(1f));
                    }

                }
                else { shrink.VEnemyC(LevelManager.instance.PlayerDamage2); }

            }

    
            PlayerMovement.instance.DPStart();
        }
        //sword3
        else if (other.name == E[2] && player._IsDamage == false)
        {
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage3);
            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(2.5f, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());
            PlayerMovement.instance.DPStart();

        }
        //sword4
        else if (other.name == E[3] && player._IsDamage == false)
        {
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage4);
            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());

            if (other.GetComponentInParent<EnemyController>().canUltimate == true)
            {
                other.GetComponentInParent<EnemyController>().canUltimate = false;
                EnemySword.instance.UltimateEnemyIcon.SetActive(false);

                other.gameObject.GetComponent<HealthRange>().HealFather.GetComponent<HealthRange>().HealEnemies();

                other.gameObject.GetComponent<HealthRange>().HealFather.GetComponent<HealthRange>().InstintiateHealParticle();
                StartCoroutine(EnemyController.Instance.ParticleIEnumerator());
            }
            PlayerMovement.instance.DPStart();

        }
        //sword5
        else if (other.name == E[4] && player._IsDamage == false)
        {
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();


            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());
            PlayerMovement.instance.DPStart();
            if (LevelManager.instance.isbig == true)
            {
                shrink.VEnemyC(LevelManager.instance.EnemyYellowPlusDamage5);
            }
            else { shrink.VEnemyC(LevelManager.instance.PlayerDamage5); }

        }
        //sword6
        else if (other.name == E[5] && player._IsDamage == false)
        {
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();


            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());




            PlayerMovement.instance.DPStart();

            if (other.GetComponentInParent<EnemyController>().canUltimate == true)
            {
             


                if (LevelManager.instance.isbig == false)
                {
                    other.GetComponentInParent<EnemyController>().canUltimate = false;
                    if(EnemySword.instance.UltimateEnemyIcon!=null)
                    EnemySword.instance.UltimateEnemyIcon.SetActive(false);
                    StartCoroutine(other.GetComponent<LightAttackFix>().Enemy.GetComponent<EnemyController>().ParticleIEnumerator());
                    shrink.VEnemyC(LevelManager.instance.YellowAbilityDamage);
                    StartCoroutine(other.GetComponent<LightAttackFix>().Enemy.GetComponent<EnemyController>().ScalingSize());
                }




            }


            if (LevelManager.instance.isbig == true)
            {
                shrink.VEnemyC(LevelManager.instance.EnemyYellowPlusDamage6);

            }
            else { shrink.VEnemyC(LevelManager.instance.PlayerDamage6); }
        }
        //sword7
        else if (other.name == E[6] && player._IsDamage == false)
        {
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();


            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());
            PlayerMovement.instance.DPStart();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage7);

            //sword8
        }
        else if (other.name == E[7] && player._IsDamage == false)
        {

            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage8);
            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());

            if (other.GetComponentInParent<EnemyController>().canUltimate==true)
            {
              other.GetComponentInParent<EnemyController>().canUltimate = false;
                EnemySword.instance.UltimateEnemyIcon.SetActive(false);
                StartCoroutine(Freezehit());
                PlayerMovement.instance.Freeze1[0].GetComponent<ParticleSystem>().Play();
                PlayerMovement.instance.Freeze1[1].GetComponent<ParticleSystem>().Play();
                PlayerMovement.instance.Freeze1[2].GetComponent<ParticleSystem>().Play();
                InvokeRepeating("IceAttackPlayer",0, LevelManager.instance.IceAbilityTimeRepeat);
                StartCoroutine(IceFreeze());
                StartCoroutine(EnemyController.Instance.ParticleIEnumerator());
                
            }
            PlayerMovement.instance.DPStart();
        }
        //sword9
        else if (other.name == E[8] && player._IsDamage == false)
        {

            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage9);
            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());
            PlayerMovement.instance.DPStart();
        }
        //sword10
        else if (other.name == E[9] && player._IsDamage == false)
        {
            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage10);
            StartCoroutine(player.Damage(.2f));
            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());
            if (other.GetComponentInParent<EnemyController>().canUltimate == true)
            {
                other.GetComponentInParent<EnemyController>().canUltimate = false;
                EnemySword.instance.UltimateEnemyIcon.SetActive(false);

                StartCoroutine(Firehit());
                player.transform.Find("Test").GetComponent<ParticleSystem>().Play();
                player.transform.Find("Test1").GetComponent<ParticleSystem>().Play();
                InvokeRepeating("FireAttack",0, LevelManager.instance.FireAbilityTimeRepeat);
                StartCoroutine(EnemyController.Instance.ParticleIEnumerator());
            }
            PlayerMovement.instance.DPStart();
        }

        else if (other.name == E[10] && player._IsDamage == false)
        {


            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage13);
            StartCoroutine(player.Damage(.2f));

            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());
        }
        else if (other.name == E[11] && player._IsDamage == false)
        {


            player.cameObject.GetComponent<HealthPlayerAnimation>().PlayHit();

            shrink.VEnemyC(LevelManager.instance.PlayerDamage14);
            StartCoroutine(player.Damage(.2f));

            if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
                Cinemachineshake2.instance.Shaker2(10, .35f);

            else { Cinemachineshake.instance.Shaker(15, .75f); }
            StartCoroutine(player.GitHit());
        }




        else { return; }
    }
    #region Ienumerator
    IEnumerator SphereBubble()
    {
        Canbubbling = false;
        yield return new WaitForSecondsRealtime(LevelManager.instance._ShieldCooldown);
        Canbubbling = true;
        Destroy(Baby);



    }
    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            if (Baby != null)
            {
                i += Time.deltaTime * rate;
                Baby.transform.localScale = Vector3.Lerp(a, b, i);
                yield return null;
            }


        }
    }
    public IEnumerator FreezehitBoss()
    {

        yield return new WaitForSeconds(2.9f);
        CancelInvoke("Ultimate2HitPlayer");

    }
    public IEnumerator Freezehit()
    {

        yield return new WaitForSeconds(LevelManager.instance.IceAbilityTime);
        CancelInvoke("IceAttackPlayer");

        
    }
    public IEnumerator Firehit()
    {

        yield return new WaitForSeconds(LevelManager.instance.FireAbilityTime);
        CancelInvoke("FireAttack");

    }
    public IEnumerator LightHit()
    {

        yield return new WaitForSeconds(1.5f);
        CancelInvoke("EnemyHitPlayer");

    }
    public IEnumerator KnockBackFreeze(float KnockTime)
    {
        PlayerMovement.instance._speed2 = PlayerMovement.instance._speed;




        PlayerMovement.instance.isFreeze = true;
        PlayerMovement.instance._speed = 1;

        yield return new WaitForSeconds(FreezeTime);
        PlayerMovement.instance._speed = LevelManager.instance.PlayerSpeed;

        PlayerMovement.instance.isFreeze = false;

    }
    public IEnumerator IceFreeze()
    {
        player._speed2 = player._speed;
        yield return new WaitForSeconds(0.01f);
        player._speed -= (player._speed / 100) * LevelManager.instance.FreezeSpeedPercentegePlayer;
        player.isSpeedPowerEnabled = true;
       
       
        yield return new WaitForSeconds(LevelManager.instance._FreezeSpeedCooldown);


        player._speed = player._speed2;
        player.isSpeedPowerEnabled = false;
      
       


    }
    #endregion
    #region Voids
    public void Ultimate2HitPlayer() => shrink.VEnemyC(LevelManager.instance.PlayerDamage14);
    public void IceAttackPlayer() => shrink.VEnemyC(LevelManager.instance.IceAbilityDamage);
    public void EnemyHitPlayer()
    {
        if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
            Cinemachineshake2.instance.Shaker2(5, .1f);

        else { Cinemachineshake.instance.Shaker(4, .2f); }
        shrink.VEnemyC(LevelManager.instance.EnemySmallDamage);
    }
    private void FireAttack()
    {
        shrink.VEnemyC(LevelManager.instance.FireAbilityDamage);
        if (_findEnemy.isLock == true && player.VirtualCam.activeSelf == true)
            Cinemachineshake2.instance.Shaker2(3, .1f);

        else { Cinemachineshake.instance.Shaker(2, .2f); }
        shrink.VEnemyC(LevelManager.instance.EnemySmallDamage);
       
    }
    #endregion

}

