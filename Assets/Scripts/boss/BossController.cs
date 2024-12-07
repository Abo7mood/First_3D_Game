using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    Transform Cam;
    SphereCollider trigger;
    Postprossingfather postprossingfather;
    public event EventHandler BossStage;
    public event EventHandler BossStage2;

    public event EventHandler AgentA;
    string[] AnimationNames = { "UltimateHit1", "UltimateHit2", "Falling1", "Falling2" };
    public enum state
    {
        Stage1, Stage2, Stage3
    }
    #region Int
    [HideInInspector] public int percentege = 1;
    [HideInInspector] public int percentege1 = 1;

    [HideInInspector] public int percentege2 = 1;
    [HideInInspector] public int percentege3 = 1;
    #endregion

    #region floats
    private float _FacingRadius = 5.5f;
    [SerializeField] private float _Speed;
    [SerializeField] private float _Acceleration;
    [SerializeField] private float _Stopdistance = 5f;
    [SerializeField] private float _Stopdistance2 = 3.1f;
  
    [SerializeField] private float _turnSpeed = 2.75f;
    private float _verticalVelocity;
    private float _gravity;
    private float _Attackcooldown1 = 10f;
    private float _Attackcooldown2 = 10f;
    private float _Attackcooldown3 = 5f;

    private float _percentegecooldown1 = 15;
    private float _percentegecooldown2 = 20;
    private float _percentegecooldown3 = 15;
    private float _percentegecooldown4 = 25;

    [HideInInspector] public float _Backstep = 1000f;
    [HideInInspector] public float _BackStepTime = 2f;
    public float _attackRaduis = 1000f;

    public float IceAttackRate;

    [HideInInspector] public float distance;
    public float MaxDistanceAttack;
    public float MinDistanceAttack;

    private float FreezeTime = 5;
    private float RealDistance;

    #endregion

    #region Booleans
    private bool _ISBackStep;
    private bool isDead;
    private bool IsAgent;
    private bool Stage1;
    private bool Stage2;

    [HideInInspector] public bool _faceTraget = false;
    [HideInInspector] public bool _canCollider = true;
    public bool _isCollider;
    public bool _isCollider2;
    public bool _IsDamage1;
    [HideInInspector] public bool _isagent;
    [HideInInspector] public bool _isagentstop;
    [HideInInspector] public bool _canAttack = true;
    [HideInInspector] public bool _canAttack2 = true;

    public bool _Forma = true;

    public bool _isUlitimate = true;

    private bool justicefix = false;
  
    private bool isFreeze = false;
    bool called = false;
    bool isStage;
    bool fixattackwheniceenable;
    #endregion

    #region GameObjects;
    public GameObject zzz;
    public GameObject BossIceAttackMother;
  
    public GameObject Shield;
    public GameObject Attack1Mother;
    public GameObject Attack2Mother;
    public GameObject Attack3Mother;
    GameObject target;
    [SerializeField] GameObject Attack1C;
    [SerializeField] GameObject Attack2C;
    [SerializeField] GameObject Attack3C;

    public GameObject IceAttackRotation;
    public GameObject IceAttackLocation;

    private Transform PlayerHome;

    public GameObject IceLocation;
    public GameObject IceLocationMother;
    public GameObject Vfx;
    Image warn;

   private GameObject BossTransform;
 
    #endregion


    #region Construct
    public ParticleSystem[] test;
    public ParticleSystem[] Freeze;
    List<ParticleSystem> IcePS = new List<ParticleSystem>();

    NavMeshAgent agent;
    public Animator anim;
    Rigidbody rb;

    public static BossController instance;
    #endregion

    #region Scripts
    HealthSystem healthSystem;
    BossHealth BossHealth;
    #endregion
    public state stage;

    #region Vectors
    [HideInInspector] public Vector3 target1;
    private Vector3 Randomize = new Vector3(1f, 1f, 0);
    #endregion

 
    private void Awake()
    {
        instance = this;
        Cam = GameObject.Find("MiniMapCamera").transform;
        IceAttackRotation = transform.Find("IceAttack").gameObject;
        IceAttackLocation = transform.Find("IceAttack").transform.Find("IceAttackLocation").gameObject;
        postprossingfather = GameObject.Find("PostManager").GetComponent<Postprossingfather>();
        warn = GameObject.Find("Canvas").transform.Find("FireWarn").GetComponent<Image>();
        AgentA += AgentC1;
        BossStage += BossUnlock;
        BossStage2 += BossUnlock2;
        Shield = GameObject.Find("BossHome").transform.Find("Shield").gameObject;
        PlayerHome = GameObject.Find("Player").transform.Find("PlayerHome");
        //bomb = GameObject.Find("Ultimate").GetComponent<Bomb>();
        healthSystem = GetComponent<HealthSystem>();
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        BossHealth = GetComponent<BossHealth>();
        BossTransform = transform.Find("BossTransform").gameObject;
    }

    void Start()
    {
       
       
        _Backstep = 1000;
        _BackStepTime = 2f;

        _FacingRadius = 5.5f;
        _turnSpeed = 0;
        Stage1 = true;
        Stage2 = false;
        IsAgent = false;
        zzz.SetActive(false);

        Shield.SetActive(false);
        InvokeRepeating("Plus", 0, 1);
        InvokeRepeating("Plus1", 0, 1);
        InvokeRepeating("Plus2", 0, 1);
        InvokeRepeating("Plus3", 0, 1);

        _Forma = true;
        agent.acceleration = _Acceleration;
        agent.speed = 0;
        _ISBackStep = false;
        _canCollider = true;
        agent.angularSpeed = 275f;
        _attackRaduis = 300;
        _Speed = 0;
    }

    private void OnDrawGizmos()
    {

        if (_Forma == false)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        BossTransform.transform.eulerAngles = new Vector3(90, Cam.transform.eulerAngles.y,0);



        //transform.localRotation= new Quaternion(50,
        //     PlayerMovement.instance.transform.rotation.y, PlayerMovement.instance.transform.rotation.z
        //     , PlayerMovement.instance.transform.rotation.w);
        if (stage == state.Stage2 && _Forma == true)
        {
          
            isStage = true;
        }
        else { isStage = false; }
                if (isFreeze == true)
        {
            if (Attack1Mother != null)
            {
                Destroy(Attack1Mother);
            }
            else if (Attack2Mother != null)
            {
                Destroy(Attack2Mother);
            }
            else if (Attack3Mother != null)
            {
                Destroy(Attack3Mother);
            }
        }
        //Debug.Log("A"+percentege);
        //Debug.Log("B"+percentege1);
        //Debug.Log("C" + percentege2);
        //Debug.Log("S" + percentege3);
        if (_Forma == false)
        {
            zzz.SetActive(true);
        }
        else
        {
            zzz.SetActive(false);
        }

        Stages();
        TrueandFalse();
        AnimationFather();

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("C") && _IsDamage1 == false && _isCollider == false)
        {

            StartCoroutine(Damage());
            BossHealth.PlayerAttack1(12);
            anim.SetTrigger("TakeDmg");
        }

        if (other.CompareTag("C1") && _IsDamage1 == false && _isCollider == false)
        {

            StartCoroutine(Damage());
            BossHealth.PlayerAttack1(24);
            anim.SetTrigger("TakeDmg");
        }

        if (other.CompareTag("UltimateAttack") && _IsDamage1 == false && _isCollider == false&&isStage==false)
        {

            if (isDead == false)
            {
                anim.SetTrigger("TakeDmg");
            }
            else { }
            test[0].GetComponent<ParticleSystem>().Play();
            test[1].GetComponent<ParticleSystem>().Play();
            StartCoroutine(Damage());
            BossHealth.PlayerAttack1(70);
        }
        if (other.CompareTag("UltimateAttack2") && _IsDamage1 == false&&isStage==false) 
        {
           
                  
                StartCoroutine(Freezehit());
            StartCoroutine(Damage());
            InvokeRepeating("Ultimate2Hit", 0f, .5f);
            Freeze[0].GetComponent<ParticleSystem>().Play();
            Freeze[1].GetComponent<ParticleSystem>().Play();
            Freeze[2].GetComponent<ParticleSystem>().Play();
            StartCoroutine(KnockBackFreeze(FreezeTime));
        }
    }


    #region Voids
    #region Animation Voids
    public void A() { anim.SetTrigger("Victory"); _Speed = 0; agent.speed = 0; }
    public void Hit1() => StartCoroutine(Damage());
    public void Attack() { anim.SetTrigger("Attack3"); } 
    public void Attack1() => anim.SetTrigger("Attack1");
    public void Attack2() => anim.SetTrigger("Attack2");
    public void Ultimate2Hit() => BossHealth.PlayerAttack1(10);
    #endregion
    #region AttacksVoids
    public void Attack1Event()
    {
        Attack1Mother = Instantiate(Attack1C, PlayerHome.transform.position, Quaternion.identity, PlayerHome.transform);
        Destroy(Attack1Mother, 8f);
        Attack1Mother.transform.parent = null;
    }
    public void Attack1EventW()
    {

        GameObject Danger = GameObject.Find("BossAttack(Clone)").transform.Find("ColliderIgnore").transform.Find("Danger").gameObject;
        Destroy(Danger);
        StartCoroutine(AttackTrigger());
        warn.gameObject.SetActive(false);

    }
    public void Attack2Event()
    {


        Attack2Mother = Instantiate(Attack2C, PlayerHome.transform.position, Quaternion.identity, PlayerHome.transform);
        Destroy(Attack2Mother, 3f);
        Attack2Mother.transform.parent = null;


    }
    public void Attack3Event()
    {

        postprossingfather.SetDark();
        anim.ResetTrigger("Magic");
        Destroy(Attack3Mother, 0f);



    }
    public void Attack4Event()
    {
        Attack1Mother = Instantiate(Attack1C, PlayerHome.transform.position, Quaternion.identity, PlayerHome.transform);
        Destroy(Attack1Mother, 8f);
        Attack1Mother.transform.parent = null;
    }
    public void Attack5Event()
    {
        Attack1Mother = Instantiate(Attack1C, PlayerHome.transform.position, Quaternion.identity, PlayerHome.transform);
        Destroy(Attack1Mother, 8f);
        Attack1Mother.transform.parent = null;
    }
    #endregion
    #region PlusVoids
    void Plus() => percentege += 1;
    void Plus1() => percentege1 += 1;
    void Plus2() => percentege2 += 1;
    void Plus3() => percentege3 += 1;

    #endregion
    #region Agents
    private void Agent3()
    {

        distance = Vector3.Distance(transform.position, LevelManager.instance.player.position);


        if (distance > MinDistanceAttack && distance < MaxDistanceAttack && !called && isFreeze == false)
        {

            InvokeRepeating("IceAttackVoid", 1, 10);
            called = true;

        }
        //Debug.Log(distance);
        anim.SetFloat("Speed", agent.velocity.magnitude);

        if (distance <= _Stopdistance2 && GetComponentInChildren<EnemyBox>() != null)
        {
            GetComponentInChildren<EnemyBox>().Collider();
        }
        else { }

        if (distance <= _attackRaduis)
        {

            _isagent = true;
            if (_ISBackStep == true)
            {
                agent.enabled = false;
            }
            if (_isUlitimate)
            {

                if (agent != null)
                {
                    _isagentstop = false;
                }
            }
            _faceTraget = true;

            if (distance <= _Stopdistance)
            {
                _isagentstop = true;

                if (_canAttack && isFreeze == false)
                {

                    Attack();

                }
                else { }

            }
            else if (distance > _Stopdistance)
            {

                _isagent = true;
                if (agent != null)
                {

                    _isagentstop = false;
                }

            }


            else { }

            if (distance < _FacingRadius)
            {
                _turnSpeed = 2.75f;
            }
            else if (distance > _FacingRadius)
            {
                _turnSpeed = 0f;
            }
            else { return; }
        }
        else { _isagent = false; }

    }
    private void Agent2()
    {

        anim.SetFloat("Speed", 0);
        float distance = Vector3.Distance(transform.position, LevelManager.instance.player.position);

        if (_ISBackStep == true)
        {
            agent.enabled = false;
        }

        _isagent = true;
        _faceTraget = true;

        _isagentstop = true;

        if (_canAttack && _canAttack2 && isFreeze == false)
        {

            Attack2();
            StartCoroutine(cooldownAttack2());
        }
        else { }


    }
    private void Agent1()
    {


        anim.SetFloat("Speed", 0);
        float distance = Vector3.Distance(transform.position, LevelManager.instance.player.position);

        if (_ISBackStep == true)
        {
            agent.enabled = false;
        }

        _isagent = true;
        _faceTraget = true;

        _isagentstop = true;
       
            _isagentstop = true;
            if (_canAttack && isFreeze == false)
            {


                Attack1();
                StartCoroutine(cooldownAttack1());


            }
        


        else { }


    }
    #endregion
    #region Stages Voids
    private void BossUnlock(object sender, EventArgs e)
    {

        BossStage -= BossUnlock;
        Stage1 = false;
        Stage2 = true;
        _Forma = true;
        percentege = 1;
        percentege1 = 1;
        percentege2 = 1;
        percentege3 = 1;
    }
    private void BossUnlock2(object sender, EventArgs e)
    {

        BossStage2 -= BossUnlock2;
        _Speed = 8.5f;
        agent.speed = _Speed;
    }

    private void Stages()
    {
        if (Stage1 == true)
        {
            if (percentege % _percentegecooldown1 == 0)
            {
                _Forma = false;
            }
            if (percentege1 % _percentegecooldown2 == 0)
            {

                Invoke("formax", 0f);
            }
            else
            { }
        }
        else if (Stage2 == true)
        {
            if (percentege2 % _percentegecooldown3 == 0)
            {
                _Forma = false;
            }
            if (percentege3 % _percentegecooldown4 == 0)
            {
                Invoke("formax", 0f);
            }
            else
            { }
        }
        else { return; }



        switch (stage)
        {

            default:
            case state.Stage1:

                if (_Forma == true)
                    Agent1();

                _turnSpeed = 2.75f;
                break;

            case state.Stage2:
                BossStage?.Invoke(this, EventArgs.Empty);

                if (_Forma == true)
                {
                    _canAttack2 = true;
                    Agent2();
                    Shield.SetActive(true);
                }

                else { Shield.SetActive(false); _canAttack2 = false; }
                _turnSpeed = 2.75f;
                break;

            case state.Stage3:
                BossStage2?.Invoke(this, EventArgs.Empty);
                if(stage==state.Stage1)

                anim.SetTrigger("Idle");
                _Forma = true;

                Shield.SetActive(false);
                if (IsAgent == true)
                {
                    Agent3();
                }
                else
                {
                    anim.SetTrigger("Magic");

                }


                _turnSpeed = 2.75f;
                StartCoroutine(AgentC());

                break;
        }

    }

    private void AgentC1(object sender, EventArgs e)
    {
        IsAgent = true;
        AgentA -= AgentC1;

    }
    #endregion
    #region OtherVoids
    private void formax()
    {
        percentege = 0;
        percentege1 = 0;
        percentege2 = 0;
        percentege3 = 0;
        _Forma = true;

    }


    private void IceAttackVoid()
    {
        anim.SetTrigger("IceAttack");
        if (isFreeze == false)
            StartCoroutine(IceStop());
    }



    public void IceUltimateAttack()
    {


        IceAttackRotation.SetActive(true);
        Vfx.SetActive(true);
        IceAttackRotation.transform.localPosition = new Vector3(0, -1.9f, 2.5f);
        VFx.instance.CanInstintiate = true;
        StartCoroutine(VFx.instance.BossAttackIceInstintiate());




    }
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
        else { }

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
                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            }
            else { gameObject.GetComponent<NavMeshAgent>().isStopped = false; }

        }


      
    }


#endregion
#endregion

#region Ienumerator
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

    }
    IEnumerator cooldownAttack1()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_Attackcooldown1);
        _canAttack = true;
    }
    IEnumerator cooldownAttack2()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_Attackcooldown2);
        _canAttack = true;
    }
    IEnumerator AgentC()
    {
        yield return new WaitForSeconds(2.4f);
        AgentA?.Invoke(this, EventArgs.Empty);

    }
    IEnumerator AttackTrigger()
    {

       SphereCollider trigger2= GameObject.Find("BossAttack(Clone)").GetComponent<SphereCollider>();
        trigger2.enabled = true;
            yield return new WaitForSeconds(1f);
            trigger2.enabled = false;

        

    }
    public IEnumerator Damage()
    {
        _IsDamage1 = true;
        yield return new WaitForSeconds(0.2f);
        _IsDamage1 = false;
    }
    public IEnumerator BackStep()
    {
        _ISBackStep = true;
        yield return new WaitForSeconds(_BackStepTime);
        _ISBackStep = false;
    }
    public IEnumerator Freezehit()
    {
        anim.Play("Freeze");

        yield return new WaitForSeconds(FreezeTime);
        anim.Play("idle tree");
        CancelInvoke("Ultimate2Hit");

    }
    public IEnumerator IceStop()
    {
     
        _attackRaduis = 0;
        _Speed = 0;
        agent.speed = _Speed;
        yield return new WaitForSeconds(_BackStepTime);
        _attackRaduis = 300;
        _Speed = 8.5f;
        agent.speed = _Speed;
       
    }
    public IEnumerator KnockBackFreeze(float KnockTime)
    {
        yield return new WaitForSeconds(0.02f);
        isFreeze = true;
        _attackRaduis = 0f;
        _isUlitimate = false;
        rb.isKinematic = false;
        _faceTraget = false;
        _Speed = 0;
        agent.speed = _Speed;
        anim.speed = 0;
      
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Debug.Log("1");
        yield return new WaitForSeconds(FreezeTime);
        isFreeze = false;
        _isUlitimate = true;
        rb.isKinematic = true;
        _faceTraget = true;
        _attackRaduis = 100f;
        _Speed = 8.5f;
        agent.speed = _Speed;
        anim.speed = 1;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    #endregion
}

