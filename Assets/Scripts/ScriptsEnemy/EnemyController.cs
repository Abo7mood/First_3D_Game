using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{  
    [HideInInspector] public int RandomNumber;
    [HideInInspector] public bool canUltimate = false;

    TriggerEnter trigger;
    string[] AnimationNames = { "UltimateHit1", "UltimateHit2", "Falling1", "Falling2" };
    // public Transform start;
    //public OffMeshLink link;


    #region GameObjects
    public GameObject particle;
    public GameObject ShieldP;
    public GameObject Shield;
    GameObject target;
    [SerializeField] GameObject Particle;
    public GameObject ParticleUltimateEnemy;
    public GameObject LightAttackAnimation;
    public GameObject UltimateEnemyIcon;
    private GameObject EnemyTransform;
    #endregion

    #region Constructors
    public ParticleSystem[] test;
    public ParticleSystem[] Freeze;
    NavMeshAgent agent;
    public Animator anim;
    Rigidbody rb;
    //Bomb bomb;
    HealthSystem healthSystem;
    HealthBarFade fade;
    public static EnemyController Instance;
    [HideInInspector] public Vector3 target1;
    private Vector3 Randomize = new Vector3(1f, 1f, 0);
    Transform Cam;

    #endregion


    #region float
    [HideInInspector] public float _Backstep = 1000f;
    [HideInInspector] public float _BackStepTime = 2f;
    public float _attackRaduis = 14f;
    private float _FacingRadius = 5.5f;
    [SerializeField] private float _Speed;
    private float _Speed2 = 0;
    [SerializeField] private float _Acceleration;
    [SerializeField] private float _Stopdistance = 2.9f;
    [SerializeField] private float _Stopdistance2 = 3.1f;
    [SerializeField] private float _CanAttackDistance = 5f;
    [SerializeField] private float _turnSpeed = 2.75f;
    private float _verticalVelocity;
    private float _gravity;
    private float _Attackcooldown = 2f;
    private float _Lightcooldown = 2f;


    #endregion
    #region bool
    private bool _ISBackStep;
    private bool isDead;

    [HideInInspector] public bool _faceTraget = false;

    [HideInInspector] public bool _canCollider = true;
    public bool _isCollider;
    public bool _isCollider2;
    public bool _IsDamage1;
    [HideInInspector] public bool _isagent;
    [HideInInspector] public bool _isagentstop;
    [HideInInspector] public bool _canAttack = true;
    public bool _isUlitimate = true;
    #endregion
    #region Strings
    string[] PL = { "C", "C1", "UltimateAttack", "UltimateAttack2", "LightAttackP(Clone)", "ShieldSphere(Clone)" };
    #endregion
 
    private void Awake()
    {
        Instance = this;



        //start = GameObject.Find("Box1").transform.Find("end");
        //link = GameObject.Find("Link").GetComponent<OffMeshLink>();
        //bomb = GameObject.Find("Ultimate").GetComponent<Bomb>();

        healthSystem = GetComponent<HealthSystem>();
        target = GameObject.Find("Player");
        trigger = target.GetComponent<TriggerEnter>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        fade = GetComponent<HealthBarFade>();
        Cam = GameObject.Find("MiniMapCamera").transform;
        EnemyTransform = transform.Find("EnemyTransform").gameObject;
    }

    void Start()
    {
        _Backstep = 1000;
        _BackStepTime = 2f;
        HealthBarShrink.instance.Enemies = GameObject.FindGameObjectsWithTag("Enemy");
       
        _FacingRadius = 5.5f;
        if (ShieldP != null) { ShieldP.SetActive(false); }

        agent.acceleration = _Acceleration;
        agent.speed = _Speed;
        _ISBackStep = false;
        _canCollider = true;

        _turnSpeed = 0;
        agent.angularSpeed = 275f;

    }

    void Update()
    {
        EnemyTransform.transform.eulerAngles = new Vector3(90, Cam.transform.eulerAngles.y, 0);

        TrueandFalse();
        AnimationFather();
        Agent();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == PL[5] && _IsDamage1 == false)
        {
          

            StartCoroutine(KnockBackFreeze(3f));
            StartCoroutine(AddForce(5));
           
        }
    }
    //Collider 1 Attack
    private void OnTriggerEnter(Collider other)
    {
        Vector3 PushPlayer = other.transform.position - transform.position;
        PushPlayer = PushPlayer.normalized;
        if (other.CompareTag(PL[0]) && _IsDamage1 == false && _isCollider == false)
        {
            StartCoroutine(Damage());
            if (LevelManager.instance.isSword == false) { fade.PlayerAttack1(LevelManager.instance.EnemyDamage1); }

            else { fade.PlayerAttack1(LevelManager.instance.EnemyDamage1Sword); } 
            anim.SetTrigger("TakeDmg");
        }
        if (other.CompareTag(PL[0]) && _IsDamage1 == false && _isCollider == true)
        {
            if (particle == null)
            {
                if (gameObject.name == "Enemy(Clone)")
                {
                    if (Shield != null)
                    {
                        LevelManager.instance.player.GetComponent<PlayerMovement>().Knockback(PushPlayer, 0.2f, 7);
                        HitParticle();
                    }


                }
                else
                {
                    if (Shield != null)
                        HitParticle1();
                }
            }
            Particle.GetComponentInChildren<ParticleSystem>().Play();
        }
        if (other.CompareTag(PL[1]) && _IsDamage1 == false && _isCollider == true)
        {
            if (particle == null)
            {
                if (gameObject.name == "Enemy(Clone)")
                {
                    if (Shield != null)
                    {
                        LevelManager.instance.player.GetComponent<PlayerMovement>().Knockback(PushPlayer, 0.2f, 7);
                        HitParticle();
                    }

                }
                else
                {
                    if (Shield != null)
                        HitParticle1();
                }
            }

            Particle.GetComponentInChildren<ParticleSystem>().Play();
        }
        if (other.CompareTag(PL[1]) && _IsDamage1 == false && _isCollider == false)
        {

            StartCoroutine(Damage());
            if (LevelManager.instance.isSword == false) { fade.PlayerAttack1(LevelManager.instance.EnemyDamage2); }

            else { fade.PlayerAttack1(LevelManager.instance.EnemyDamage2Sword); } 
            anim.SetTrigger("TakeDmg");
        }
        if (other.CompareTag(PL[2]) && _IsDamage1 == false && _isCollider == true)
        {
            StartCoroutine(KnockBackFreeze(1.8f));
            StartCoroutine(AddForce(40));
            if (isDead == false)
            {
                anim.SetTrigger("Falling");
            }
            else { }
            StartCoroutine(Damage());
            fade.PlayerAttack1(LevelManager.instance.EnemyDamage3);
        }
        if (other.CompareTag(PL[2]) && _IsDamage1 == false && _isCollider == false)
        {
            StartCoroutine(KnockBackFreeze(2.1f));
            StartCoroutine(AddForce(80));
            if (isDead == false)
            {
                anim.SetTrigger("UltimateHit");
            }
            else { }
            if (test[0] != null && test[1] != null)
            {
                test[0].GetComponent<ParticleSystem>().Play();
                test[1].GetComponent<ParticleSystem>().Play();
            }
          
            StartCoroutine(Damage());
            fade.PlayerAttack1(LevelManager.instance.EnemyDamage4);
        }
        if (other.CompareTag(PL[3]) && _IsDamage1 == false)
        {
            StartCoroutine(KnockBackFreeze(5f));
            StartCoroutine(FreezeHit(5f));
            StartCoroutine(Damage());
            InvokeRepeating("Ultimate2Hit", 0f, .5f);
            if(Freeze[0] !=null&& Freeze[1] != null&& Freeze[2] != null)
            {
                Freeze[0].GetComponent<ParticleSystem>().Play();
                Freeze[1].GetComponent<ParticleSystem>().Play();
                Freeze[2].GetComponent<ParticleSystem>().Play();
            }
          
        }
        if (other.name == PL[4] && _IsDamage1 == false)
        {
            anim.SetTrigger("TakeDmg");

            StartCoroutine(LightFreeze());
            fade.PlayerAttack1(LevelManager.instance.EnemyDamage5);
            StartCoroutine(Damage());

        }

        else { return; }
    }

    //animation for enemy in Character stats void A1

    #region Ienumerator
    public IEnumerator AddForce(float _Force)
    {
        yield return new WaitForSeconds(0.04f);
        if (isDead == false)
        {
            Vector3 PushDirection = LevelManager.instance.player.transform.position - transform.position;
            PushDirection = -PushDirection.normalized;
            rb.AddForce(PushDirection * _Force * 100);
        }

    }
    public IEnumerator Damage()
    {
        _IsDamage1 = true;
        yield return new WaitForSeconds(0.3f);
        _IsDamage1 = false;
    }
    public IEnumerator DamageShield()
    {
        _IsDamage1 = true;
        yield return new WaitForSeconds(3);
        _IsDamage1 = false;
    }
    public IEnumerator BackStep()
    {
        _ISBackStep = true;
        yield return new WaitForSeconds(_BackStepTime);
        _ISBackStep = false;
    }
    public IEnumerator KnockBackFreeze(float KnockTime)
    {
        yield return new WaitForSeconds(0.02f);
        if (isDead == false)
        {
            _attackRaduis = 0f;
            _isUlitimate = false;
            rb.isKinematic = false;
            _faceTraget = false;
            yield return new WaitForSeconds(KnockTime);
            _isUlitimate = true;
            rb.isKinematic = true;
            _faceTraget = true;
            _attackRaduis = 100f;
        }

    }
    public IEnumerator KnockBackFreeze2(float KnockTime)
    {
        yield return new WaitForSeconds(0.04f);
        if (isDead == false)
        {
            _isUlitimate = false;
            rb.isKinematic = false;
            yield return new WaitForSeconds(KnockTime);
            _isUlitimate = true;
            rb.isKinematic = true;
        }

    }
    public IEnumerator KnockBackFreeze3()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        _isagent = false;
        anim.enabled = false;
        rb.isKinematic = false;
        anim.enabled = false;
        _isUlitimate = false;
        _faceTraget = false;
        ShieldP.SetActive(false);
    }
    public IEnumerator FreezeHit(float time)
    {
        anim.Play("Freeze");
        yield return new WaitForSeconds(time);
        anim.Play("idle tree");
        CancelInvoke("Ultimate2Hit");
    }

    //IEmurator Attack1
    IEnumerator cooldownAttack()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_Attackcooldown);
        _canAttack = true;
    }

    IEnumerator LightFreeze()
    {
        _Speed2 = _Speed;
        yield return new WaitForSeconds(0.01f);
        _Speed -= (_Speed / 100) * LevelManager.instance.SpeedPercentegeEnemy;

        yield return new WaitForSeconds(_Lightcooldown);
        _Speed = _Speed2;
    }

    public IEnumerator ParticleIEnumerator()
    {
        if (gameObject != null&& ParticleUltimateEnemy!=null)
        {
            ParticleUltimateEnemy.SetActive(true);
            yield return new WaitForSeconds(.5f);
            ParticleUltimateEnemy.SetActive(false);
        }
      


    }

    #endregion

    #region void
    public void ColliderEnabled()
    {
        ShieldP.SetActive(true);
        _isCollider = true;
    }
    public void ColliderDisabled()
    {
        ShieldP.SetActive(false);
        _isCollider = false;
    }
    //public void Bombs()
    //{
    //    if (isDead == true)
    //        bomb.Dontate();
    //}
    private void AnimationFather()
    {
        for (int i = 0; i < AnimationNames.Length; i++)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag(AnimationNames[i]))
                StartCoroutine(BackStep());
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Defend"))
            _isCollider = true;
        else
            _isCollider = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            agent.enabled = false;
            _turnSpeed = 0;
            agent.speed = 0.0f;

        }
    }
    private void TrueandFalse()
    {
        if (healthSystem.healthAmount <= 0)
        {
            isDead = true;
        }

        if (_faceTraget == true)
        {
            Vector3 direction = (LevelManager.instance.player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
        }
        else { return; }



        if (_isCollider == true)
            _canCollider = false;

        if (_isagent == true)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            agent.SetDestination(LevelManager.instance.player.position);

        }
        else { gameObject.GetComponent<NavMeshAgent>().enabled = false; }

        if (agent.isActiveAndEnabled == true)
        {
            if (_isagentstop == true)
            {
                _turnSpeed = 2.75f;
                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            }
            else { gameObject.GetComponent<NavMeshAgent>().isStopped = false; _turnSpeed = 0f ; }

        }
    }
    private void Agent()
    {

        //float distanceagent = Vector3.Distance(transform.position, start.transform.position);
        //if (distanceagent<=4f)
        //{

        //    Debug.Log("Jump");
        //    anim.SetTrigger("Jump");
        //}
        //else
        //{
        //    anim.ResetTrigger("Jump");
        //}

        //Move
        anim.SetFloat("Speed", agent.velocity.magnitude);
        float distance = Vector3.Distance(transform.position, LevelManager.instance.player.position);
        //Debug.Log(distance);
        //if (distance <= _FacingRadius)
        //{
        //    _faceTraget = true;
        //}
        //else
        //{
        //    _faceTraget = false;
        //}
        if (distance <= _Stopdistance2 && GetComponentInChildren<EnemyBox>() != null)
        {
            GetComponentInChildren<EnemyBox>().Collider();
        }
        else { }

        if (distance <= _attackRaduis)
        {
            if (_ISBackStep == true)
            {
                agent.enabled = false;
            }
            if (_isUlitimate)
            {

                _isagent = true;
                if (agent != null)
                {
                    _isagentstop = false;
                }

                _faceTraget = true;
                if (distance <= _CanAttackDistance)
                {
                    if (distance <= _Stopdistance)
                    {
                        _isagentstop = true;

                        if (_canAttack)
                        {

                            Attack();
                            StartCoroutine(cooldownAttack());
                        }
                        else { return; }

                    }
                    else if (distance > _Stopdistance)
                    {

                        _isagent = true;
                        if (agent != null)
                        {

                            _isagentstop = false;
                        }

                    }
                }
                else if(distance>_CanAttackDistance)
                { anim.Play("idle tree", 0); }
              

            }
            else { return; }

           
        }
        else { _isagent = false; }

    }
    private void HitParticle()
    {
        particle = Instantiate(Particle, Shield.transform.position, Quaternion.identity, Shield.transform);
        Destroy(particle, 1f);
        particle.transform.parent = null;
        particle.transform.localPosition += new Vector3(Random.Range(-Randomize.x, Randomize.x),
            Random.Range(-Randomize.y, Randomize.y), 0);
    }
    private void HitParticle1()
    {
        particle = Instantiate(Particle, Shield.transform.position, Quaternion.identity, Shield.transform);
        Destroy(particle, 1f);
        particle.transform.parent = null;

    }
    public void EnemyDoAttack2()
    {
    
        RandomNumber = Random.Range(1, 11);
        if (RandomNumber == 1 || RandomNumber == 2 || RandomNumber == 3 || RandomNumber == 4 || RandomNumber == 5 /*||
                RandomNumber == 6 || RandomNumber == 7 ||
                RandomNumber == 8 || RandomNumber == 9 || RandomNumber == 10*/)

        {if(UltimateEnemyIcon!=null)
            UltimateEnemyIcon.SetActive(true);
            canUltimate = true;
        }
        else { return; }
    }

    public void A() { anim.SetTrigger("Victory"); agent.speed = 0; _Speed = 0; }
    public void Hit1() => StartCoroutine(Damage());
    public void Attack() => anim.SetTrigger("Attack3");
    public void Back() => anim.SetTrigger("BackToIdle");
    public void Ultimate2Hit() => fade.PlayerAttack1(LevelManager.instance.EnemyDamage3);
    public void StartLight() => StartCoroutine(LightPlay());

    #endregion



    #region scalevarible

    private Vector3 maxscale = new Vector3(1.5f, 1.5f, 1.5f);
    private Vector3 minScale = new Vector3(1, 1, 1);

    private bool reatlable = true;
    private float speed = 1.5f;
    private float duration = 1;

    private float appeartime = 5;

    #endregion


    public IEnumerator ScalingSize()
    {
        
            minScale = transform.localScale;
            reatlable = true;


            while (reatlable)
            {
                LevelManager.instance.isbig = true;


                fade.MaxHealth += LevelManager.instance.EnemyYellowMaxHealth;

           
                yield return RepeatLerp(minScale, maxscale, duration);
                yield return new WaitForSeconds(appeartime);
                yield return RepeatLerp(maxscale, minScale, duration);
            
                

                fade.MaxHealth -= LevelManager.instance.EnemyYellowMaxHealth;

                yield return new WaitForSeconds(.75f);
                LevelManager.instance.isbig = false;

                reatlable = false;




            }
        
            
        

    }
    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        if (this != null)
        {

            float i = 0.0f;
            float rate = (1.0f / time) * speed;
            while (i < 1.0f)
            {


                i += Time.deltaTime * rate;
                transform.localScale = Vector3.Lerp(a, b, i);
                yield return null;



            }
        }
        else {  }
       
           
    }
    
    public IEnumerator LightPlay()
    {
        if (LightAttackAnimation != null)
        {
            LightAttackAnimation.SetActive(true);
            yield return new WaitForSeconds(.2f);
            LightAttackAnimation.SetActive(false);
        }
       
    }
   
}
