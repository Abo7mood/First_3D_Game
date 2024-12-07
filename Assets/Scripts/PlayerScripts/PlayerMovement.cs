using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Transform mapCam;
    public static PlayerMovement instance;
    #region Scripts
    TriggerEnter trigger;
    HealthBarShrink shrink;
    HealthPlayerAnimation _HealthAnimation;
    ManaBar _manabar;
    FindEnemy _findEnemy;
    private HealthSystem health;
    CharacterController Character;
    SpawnSystem spawnSystem;
    ParticleSystem DP;
    #endregion
    #region GameObjects;
    private GameObject[] enemies;
    public GameObject Attack2Mother;
    [SerializeField] GameObject Attack2C;

    public Animator anim;
    Transform cam;
    Transform Cam;
    public GameObject FreelookCam;
    public GameObject VirtualCam;
    public GameObject[] leftLocks;
    public GameObject[] rightLocks;
    public GameObject cameObject;

    public GameObject[] Trails;
    public GameObject LightGlow;
    public GameObject IceEffect;

    private GameObject MyPlayerTransform;

    [SerializeField] GameObject[] TestObject;
 
    #endregion
    #region vector
    private Vector3 dashMove;
    private Vector3 dir;
    private Vector3 move;
    private Vector3 CamVector;
    private SkinnedMeshRenderer _renderer;
    GameObject particle;
    public ParticleSystem[] TrailsPS;

    public ParticleSystem[] Freeze;
    public ParticleSystem[] Freeze1;

    Transform CM;
    Vector3 cmvector;
    #endregion

    #region FloatandInt
    private int Xbox_One_Controller = 0;
    private int PS4_Controller = 0;

    private float horizontal;
    private float vertical;
    private float horizontal1;
    private float vertical1;

    private float _TargetDistanceup;
    private float _TargetDistancedown;
    private float _TargetDistanceforward;
    private float _TargetDistanceback;
    private float _TargetDistanceleft;
    private float _TargetDistanceright;

    public float _FreezeTime;
    public float _dashLength = 0.15f;
    public float _dashSpeed = 23f;
    public float _dashResetTime = 1f;
    private float _LockHorizontal;
    private float _LockVertical;

    [SerializeField]
    private float _verticalVelocity;

    private float _Acceleration = 2f;
    private float _Decceleration = 2.75f;
    private float maximiumWalkVelocity = 0.5f;
    private float maximiumRunVelocity = 1f;
    private float _theUltimateTimer = 0f;
    private float _theUltimateTimer2 = 0f;
    private float _theUltimateTimer3 = 0f;
    private float _dashing = 0f;
    private float _dashingTime = 0f;

    private float _nextAttackTime2 = 0f;
    private float _nextAttackTime3 = 0f;
    private float _nextAttackTime4 = 0f;


   
    [SerializeField] private float _sprintMana = .1f;

    private float KnockBackConter;

    public float sprint;
    public float sprint1;
    public float Mysprint;
    public float Myanimsprint;
    //Attack cooldown
    public float _attackRate = 1f;
    public float _attackRate2 = 1f;
    public float _attackRate3 = 1f;
    public float _gravity;
    public float _speed;
   [HideInInspector] public float _speed2;

    private float _SpeedCooldown;
    public int Testingbro = -1;

    private float Lens;
    private float step = .2f;
    private float step2 = .02f;
    #endregion

    #region Bool
    public bool _IsDamage = false;
    public bool _IsDamage2 = false;
    public bool _IsDamageUltimate = false;
    private bool _CanDash2 = true;
    private bool _canDash = true;
    private bool _dashingNow = false;
    private bool _dashReset = true;

    private bool _isHide = true;
    private bool _canFreeze;
    private bool _isGrounded;

    private bool IsSprint2;

    [HideInInspector] public bool isSpeedPowerEnabled = false;
    [HideInInspector] public bool isSprint;
    [HideInInspector] public bool _isInput = true;
    [HideInInspector] public bool isSprint1;

    [HideInInspector] public bool isFreeze;

    private bool Zoomout = false;
    private bool Zoomin = false;
    private bool isIceEffectEnable;
    #endregion

   
    private void Awake()
    {
           instance = this;
        Reference();
    }
    private void Start()
    {
        
        particle.SetActive(false);
        Lens = LevelManager.instance.Camera.transform.Find("CM FreeLook1").GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView;
        isSpeedPowerEnabled = false;
        _renderer.material.enableInstancing = false;

        //_renderer.material.SetColor("_BaseColor", _c);
        //_renderer.material.SetColor("_SpecColor", _c);
       
        Invoke("EnableInput", .5f);
        _isInput = false;
        _speed2 = LevelManager.instance.PlayerSpeed;

    }

    private void FixedUpdate()
    {
        if (Zoomout == true)
        {
            LevelManager.instance.Camera.transform.Find("CM FreeLook1")
           .GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = Mathf.Lerp(40, 60, step += Time.deltaTime);
        } if (Zoomin == true)
        {
            LevelManager.instance.Camera.transform.Find("CM FreeLook1")
          .GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = Mathf.Lerp(60, 40, step2 += Time.deltaTime);
        }
     
       
    }

    void Update()
    {

        TransformFix();
     
        if (_isInput==true)
        {

            Raycasting();
           

            Hide();
            if (KnockBackConter <= 0)
            {
                playerMove();
            }
            else
            {
                KnockBackConter -= Time.deltaTime;
            }

            if (isFreeze == false)
            {
                
                AttackFather();
                AnimationFather();
                Speed();
                PlayerDodge();

            }
        }
     

        InputCheck();
     

        leftLocks = GameObject.FindGameObjectsWithTag("L");
        rightLocks = GameObject.FindGameObjectsWithTag("R");

        
    }
    #region Voids
    private void ChangeVelocity(bool isbackpressed, bool isforwardpressed, bool isleftpressed, bool isrightpressed, bool isSprint, float CurrentMaxVelocity)
    {
        //if Player presses foward 
        if (isforwardpressed && _LockVertical < CurrentMaxVelocity)
        {
            _LockVertical += Time.deltaTime * _Acceleration;
        }
        //if Player presses Backward 
        if (isbackpressed && _LockVertical > -CurrentMaxVelocity)
        {
            _LockVertical -= Time.deltaTime * _Acceleration;
        }
        //increase Velocity in the left direction
        if (isleftpressed && _LockHorizontal > -CurrentMaxVelocity)
        {
            _LockHorizontal -= Time.deltaTime * _Acceleration;
        }
        //increase Velocity in the right direction
        if (isrightpressed && _LockHorizontal < CurrentMaxVelocity)
        {
            _LockHorizontal += Time.deltaTime * _Acceleration;
        }
        //decrease VelocityZ
        if (!isforwardpressed && _LockVertical > 0.0f)
        {
            _LockVertical -= Time.deltaTime * _Decceleration;
        }
        //decrease VelocityZ
        if (!isbackpressed && _LockVertical < 0.0f)
        {
            _LockVertical += Time.deltaTime * _Decceleration;
        }


        //increase velocityX if left is not pressed and VelocityX < 0
        if (!isleftpressed && _LockHorizontal < 0.0f)
        {
            _LockHorizontal += Time.deltaTime * _Decceleration;
        }
        //decrease velocityX if right is not pressed and VelocityX > 0
        if (!isrightpressed && _LockHorizontal > 0.0f)
        {
            _LockHorizontal -= Time.deltaTime * _Decceleration;
        }

        if (isforwardpressed && _LockVertical > CurrentMaxVelocity)
        {
            _LockVertical -= Time.deltaTime * _Decceleration;
            //round to the currentmaxVelocity if within offeset
            if (_LockVertical > CurrentMaxVelocity && _LockVertical < (CurrentMaxVelocity + 0.05f))
            {
                _LockVertical = CurrentMaxVelocity;
            }
        }

        if (isbackpressed && _LockVertical < -CurrentMaxVelocity)
        {
            _LockVertical += Time.deltaTime * _Decceleration;
            //round to the currentmaxVelocity if within offeset
            if (_LockVertical < -CurrentMaxVelocity && _LockVertical > (-CurrentMaxVelocity - 0.05f))
            {
                _LockVertical = -CurrentMaxVelocity;
            }
        }
        if (isleftpressed && _LockHorizontal < -CurrentMaxVelocity)
        {
            _LockHorizontal += Time.deltaTime * _Decceleration;
            //round to the currentmaxVelocity if within offeset
            if (_LockHorizontal < -CurrentMaxVelocity && _LockHorizontal > (-CurrentMaxVelocity - 0.05f))
            {
                _LockHorizontal = -CurrentMaxVelocity;
            }
        }
    }
    private void LockOrResetVelocity(bool isbackpressed, bool isforwardpressed, bool isleftpressed, bool isrightpressed, bool isSprint, float CurrentMaxVelocity)
    {
        //reset Velocity X
        if (!isrightpressed && !isleftpressed && _LockHorizontal != 0.0f && (_LockHorizontal > -.05f && _LockHorizontal < 0.05f))
        {
            _LockHorizontal = 0f;
        }
        //lock Forward
        if (isforwardpressed && isSprint && _LockVertical > CurrentMaxVelocity)
        {
            _LockVertical = CurrentMaxVelocity;
        }
        //decelerate to the maximum walk velocity

        //round to the currentMaxvelocity if within offeset
        else if (isforwardpressed && _LockVertical < CurrentMaxVelocity && _LockVertical > (CurrentMaxVelocity - 0.05f))
        {
            _LockVertical = CurrentMaxVelocity;
        }

        //lock Forward
        if (isbackpressed && isSprint && _LockVertical < -CurrentMaxVelocity)
        {
            _LockVertical = -CurrentMaxVelocity;
        }
        //decelerate to the maximum walk velocity

        //round to the currentMaxvelocity if within offeset
        else if (isbackpressed && _LockVertical > -CurrentMaxVelocity && _LockVertical < (-CurrentMaxVelocity + 0.05f))
        {
            _LockVertical = -CurrentMaxVelocity;
        }



        //lock Left
        if (isleftpressed && isSprint && _LockHorizontal < -CurrentMaxVelocity)
        {
            _LockHorizontal = -CurrentMaxVelocity;
        }
        //decelerate to the maximum walk velocity

        //round to the currentMaxvelocity if within offeset
        else if (isleftpressed && _LockHorizontal > -CurrentMaxVelocity && _LockHorizontal < (-CurrentMaxVelocity + 0.05f))
        {
            _LockHorizontal = -CurrentMaxVelocity;
        }
    }
    private void Lockonanimation()
    {                  //input
        bool isforwardpressed = Input.GetKey(KeyCode.W);
        bool isbackpressed = Input.GetKey(KeyCode.S);
        bool isleftpressed = Input.GetKey(KeyCode.A);
        bool isrightpressed = Input.GetKey(KeyCode.D);
        isSprint = Input.GetButton("Sprint") && (vertical != 0 || horizontal != 0) && _manabar.CanRun == true && _isInput;
        //set current maxvelocity
        float CurrentMaxVelocity = isSprint ? maximiumRunVelocity : maximiumWalkVelocity;

        ChangeVelocity(isbackpressed, isforwardpressed, isleftpressed, isrightpressed, isSprint, CurrentMaxVelocity);
        LockOrResetVelocity(isbackpressed, isforwardpressed, isleftpressed, isrightpressed, isSprint, CurrentMaxVelocity);

    }
    private void AttackFather()
    {

        //Attack1
        if (Input.GetButtonDown("Attack1") && anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchForward").Equals(false) &&
                  anim.GetCurrentAnimatorStateInfo(0).IsTag("AttackU1").Equals(false) &&
       anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack1").Equals(false))

        {
            _manabar.SpendMana(2);
            anim.SetTrigger("Attack1");

        }

        else { anim.ResetTrigger("Attack1"); }
        //AttackU1
        if (Input.GetButtonDown("Attack2") && anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchForward").Equals(false) &&
          anim.GetCurrentAnimatorStateInfo(0).IsTag("AttackU1").Equals(false) &&
          anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack1").Equals(false) && _isInput)

        {
            _manabar.SpendMana(4);
            anim.SetTrigger("AttackU1");
        }

        else { anim.ResetTrigger("AttackU1"); }
        //Ultimate1
        if (Input.GetButton("Attack4") && _isInput && Input.GetButton("Attack3").Equals(false) /*&& Time.time >= _nextAttackTime2*/)

        {

            _theUltimateTimer += Time.deltaTime;

            if (_theUltimateTimer >= 1f)
            {
                StartCoroutine(Actives());
                anim.SetTrigger("Attack3");
                _nextAttackTime2 = Time.time + 30f / _attackRate2;

                _theUltimateTimer = 0f;
            }

        }
        else { anim.ResetTrigger("Attack3"); }
        //Utimate2
        if (Input.GetButton("Attack3") && _isInput && Input.GetButton("Attack4").Equals(false) /*&& Time.time >= _nextAttackTime2*/)

        {

            _theUltimateTimer2 += Time.deltaTime;

            if (_theUltimateTimer2 >= 1f)
            {
                anim.SetTrigger("Attack4");
                _nextAttackTime3 = Time.time + 30f / _attackRate2;

                _theUltimateTimer2 = 0f;
            }

        }
        else { anim.ResetTrigger("Attack4"); }
        //ultimate3
        if (Input.GetButton("Attack3") && Input.GetButton("Attack4") && _isInput &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("AttackU1").Equals(false) &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack1").Equals(false) &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack3").Equals(false) &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack4").Equals(false) &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Fall").Equals(false))

        {


            anim.SetLayerWeight(2, 1f);
            _nextAttackTime4 = Time.time + 30f / _attackRate3;
            anim.SetTrigger("MagicAttack");


        }
        else { anim.ResetTrigger("MagicAttack"); }
    }
    private void Raycasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            _TargetDistanceforward = hit.distance;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit))
        {
            _TargetDistanceleft = hit.distance;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit))
        {
            _TargetDistanceright = hit.distance;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit))
        {
            _TargetDistanceback = hit.distance;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit))
        {
            _TargetDistancedown = hit.distance;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit))
        {
            _TargetDistanceup = hit.distance;
        }

    }
    private void Hide()
    {

        if (Input.GetKeyDown(KeyCode.J) && _isHide == true)
        {
            _isHide = false;
        }
        else if (Input.GetKeyDown(KeyCode.J) && _isHide == false)
        {
            _isHide = true;
        }


        if (_isHide == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        else if (_isHide == true)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void Speed()
    {
     
        //هذا حق اللوك اون ماله دخل بالسبرنت العادي
        if (isSprint == true && Myanimsprint < .5f)
        {
          
            Myanimsprint += Time.deltaTime * _Acceleration;
        }
        else if (isSprint == true && Myanimsprint > .5f)
        {
         
            Myanimsprint = .5f;
        }
        else if (isSprint == false && Myanimsprint > 0f)
        {
        
            Myanimsprint -= Time.deltaTime * _Decceleration;
        }
        //هنا عاد السبرنت الحقيقي اللي تحت كله
        if (isSprint == true && _manabar.barImage.fillAmount >= 0 && _manabar.Tire == false && _manabar.CanRun == true &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("AttackU1").Equals(false) &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack1").Equals(false) &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack3").Equals(false)
            && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack4").Equals(false)
            && anim.GetCurrentAnimatorStateInfo(0).IsName("Fall").Equals(false))
           
        {
            sprint = isSprint ? 3f : 1;
            isSprint = true;
            _manabar.SpendMana(_sprintMana);
         
        }
        else if (isSprint == true && _manabar.Tire == true && _manabar.CanRun == false)
        {
          

            isSprint = false;

            sprint = isSprint ? 1f : 1;

        }
        else if (_manabar.Tire == true && _canDash == false && _manabar.CanRun == false)
        {  
            isSprint = false;

            sprint = isSprint ? 1f : 1;
        }
        else if (_manabar.barImage.fillAmount < .01f)
        {
       
            isSprint = false;
        }
        else
        {

        }
    }
    private void PlayerDodge()
    {
        if (Character.isGrounded == false&&isIceEffectEnable==false)
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
        }
        

        horizontal1 = Input.GetAxisRaw("Horizontal");
        vertical1 = Input.GetAxisRaw("Vertical");

        if (Character.isGrounded)
        {
            if (Input.GetButton("Crouch") == true && _dashing < _dashLength && _dashingTime < _dashResetTime
                && _dashReset == true && _canDash == true && _CanDash2 == true &&
                anim.GetCurrentAnimatorStateInfo(0).IsTag("AttackU2").Equals(false)
                && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack2").Equals(false) &&
                anim.GetCurrentAnimatorStateInfo(0).IsTag("AttackU1").Equals(false) &&
                anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack1").Equals(false)
                && anim.GetCurrentAnimatorStateInfo(0).IsTag("Hit").Equals(false)
                && _manabar.barImage.fillAmount >= .3 && _manabar.Tire == false && _isInput&&isFreeze==false)
            {
                if (horizontal != 0 || vertical != 0)
                {
                    Crouch();
                    StartCoroutine(Dodge());
                    //لما اللاعب يسوي لوك اون
                    if (_findEnemy.isLockin == true)
                    {

                        if (_LockHorizontal > 0)
                        {
                            anim.SetTrigger("CrouchRight");
                        }
                        else if (_LockHorizontal < 0)
                        {
                            anim.SetTrigger("CrouchLeft");
                        }
                        else if (_LockHorizontal == 0 && _LockVertical > 0)
                        {
                            anim.SetTrigger("CrouchForward");
                        }
                        else if (_LockHorizontal == 0 && _LockVertical < 0)
                        {
                            anim.SetTrigger("CrouchBackward");
                        }
                    }
                    //لما اللاعب مو مسوي لوك اون
                    else
                    {
                        anim.SetTrigger("CrouchForward");
                    }



                    Vector3 move2 = new Vector3(horizontal1, 0, vertical1);
                    move2 = cam.TransformDirection(move2);

                    move2 = new Vector3(move2.x, _verticalVelocity,
                        move2.z);
                    Character.Move(move2 * Time.deltaTime);

                    if (move2.magnitude > 0.01f)
                    {
                        float angle = Mathf.Atan2(move2.x, move2.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        transform.rotation = Quaternion.Euler(0, angle, 0);
                    }
                    dashMove = move2;
                    _canDash = false;
                    _dashReset = false;
                    _dashingNow = true;

                }

            }
            if (_dashingNow == true && _dashing < _dashLength)
            {
                StartCoroutine(Damage2(.2f));
                Character.Move(dashMove * _dashSpeed * Time.deltaTime);
                _dashing += Time.deltaTime;
            }

            if (_dashing >= _dashLength)
            {
                _dashingNow = false;
            }

            if (_dashingNow == false)
            {
                Character.Move(move * _speed * Time.deltaTime);
            }

            if (_dashReset == false)
            {
                _dashingTime += Time.deltaTime;
            }

            if (Character.isGrounded && _canDash == false && _dashing >= _dashLength)
            {
                _canDash = true;
                _dashing = 0f;
            }

            if (_dashingTime >= _dashResetTime && _dashReset == false)
            {
                _dashReset = true;
                _dashingTime = 0f;
            }
        }

    }
    private void AnimationFather()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack3"))
        {
            StartCoroutine(Damage2(2f));
        }
        //coldownif 
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("AttackU1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack3") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack4") ||
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Fall") || _canFreeze)
        {
            horizontal = 0;
            vertical = 0;
        }


    }
    private void InputCheck()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {

            if (names[x].Length == 19)
            {
                //print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
            }
            if (names[x].Length == 33)
            {
               // print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;
                Xbox_One_Controller = 1;

            }
            if (names[x].Length == 19)
            {
                _LockHorizontal = Input.GetAxis("Horizontal");
                _LockVertical = Input.GetAxis("Vertical");
            }
            else { }


        }
    }
    public void CanFreeze() => StartCoroutine(FreezeP());
    public void Crouch() => _manabar.SpendMana(20);
    public void EnableInput() => _isInput = true;
    private void B() => anim.SetTrigger("B");
    private void A() => anim.SetTrigger("A");
    public void fallanim() => anim.SetTrigger("Fall");
    public void Knockback(Vector3 direction, float KnockBackTimer, float KnockBackForce)
    {

        KnockBackConter = KnockBackTimer;
        move = direction * KnockBackForce;
    }
    #endregion
    private void playerMove()
    {
        move = new Vector3(horizontal, 0, vertical);
        if (isFreeze == false)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
      
        isSprint = Input.GetButton("Sprint") && (vertical != 0 || horizontal != 0) && _manabar.CanRun == true && _isInput;

        bool isLockingZ = Input.GetButton("Sprint") && (horizontal > 0) && _manabar.CanRun == true && _isInput;
        bool isNLockingZ = Input.GetButton("Sprint") && (horizontal < 0) && _manabar.CanRun == true && _isInput;
        bool isLockingV = Input.GetButton("Sprint") && (vertical > 0) && _manabar.CanRun == true && _isInput;
        bool isNLockingV = Input.GetButton("Sprint") && (vertical < 0) && _manabar.CanRun == true && _isInput;
        sprint = isSprint ? 3f : 1;


        if (_findEnemy.isLockin == true)
        {


            Lockonanimation();

            Invoke("A", 0.01f);
            anim.SetFloat("velz", Mathf.Clamp(_LockHorizontal, -.5f, 0.5f) + (isLockingZ ? 0.50f : 0) + (isNLockingZ ? -0.50f : 0));
            anim.SetFloat("vely", Mathf.Clamp(_LockVertical, -.5f, 0.5f) + (isLockingV ? 0.50f : 0) + (isNLockingV ? -0.50f : 0));
            dir = new Vector3(_findEnemy.closestEnemyInDirection.transform.position.x
                , transform.position.y,
                _findEnemy.closestEnemyInDirection.transform.position.z);

            transform.LookAt(dir);

            dir.x = 0;

        }
        else
        {



            Invoke("B", 0.01f);
            anim.SetFloat("Speed", Mathf.Clamp(move.magnitude, 0f, 0.5f) + Myanimsprint);
            if (move.magnitude > 0.1f)
            {
                float angle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
        if (isFreeze == false)
            move = cam.TransformDirection(move);

        if (isFreeze == false)
        {
            move = new Vector3(move.x * _speed * sprint, _verticalVelocity,
        move.z * _speed * sprint);
            Character.Move(move * Time.deltaTime);
        }
        
      

        if (move.magnitude > 1)       
            move = move.normalized;
        
    }

    public void Attack2Event()
    {

        Attack2Mother = Instantiate(Attack2C, spawnSystem.Boss.transform.Find("LightPosition").transform.position, Quaternion.identity);

        foreach (GameObject go in LevelManager.instance.enemies)
        {
            if (go != null)
            {
                Attack2Mother = Instantiate(Attack2C, go.transform.Find("LightPosition").transform.position,
            Quaternion.identity);
            }
            //foreach (GameObject go in LevelManager.instance.enemies)
            //{
            //    Testingbro++;
            //    Attack2Mother = Instantiate(Attack2C, LevelManager.instance.enemies[asf[Testingbro]].transform.position,
            //   Quaternion.identity, LevelManager.instance.enemies[asf[Testingbro]].transform);
            //}
            Destroy(Attack2Mother, 3f);
            Attack2Mother.transform.parent = null;
            StartCoroutine(LightAttackSwitcher());
        }
               
        
         

    }
    public void Attack3Event()=> StartCoroutine(LightAttackSwitcher());
    
    public void DPStart() => DP.Play();
    private void TransformFix()
    {
        MyPlayerTransform.transform.eulerAngles = new Vector3(90, Cam.transform.eulerAngles.y, 0);
        cmvector = new Vector3(90, 0, LevelManager.instance.Camera.transform.Find("CM FreeLook1")
          .GetComponent<CinemachineFreeLook>().m_XAxis.Value * -1);
        CamVector = new Vector3(MyPlayerTransform. transform.position.x, -43.8f, MyPlayerTransform.transform.position.z);
        mapCam.transform.position = Vector3.Lerp(mapCam.transform.position, CamVector, 30f * Time.deltaTime);
        mapCam.eulerAngles = cmvector;
    }
   
    private void Reference()
    {
        trigger = GetComponent<TriggerEnter>();
        _findEnemy = GameObject.Find("Player").transform.Find("Track").GetComponent<FindEnemy>();
        _manabar = GetComponent<ManaBar>();
        _HealthAnimation = gameObject.GetComponent<HealthPlayerAnimation>();
        shrink = GetComponent<HealthBarShrink>();
        health = GetComponent<HealthSystem>();
        Character = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        spawnSystem = GameObject.Find("GameManager").GetComponent<SpawnSystem>();
        cam = Camera.main.transform;
        particle = GameObject.Find("Player").transform.Find("knight").transform.Find("Particle").gameObject;

        Character = gameObject.GetComponent<CharacterController>();
        _renderer = GameObject.Find("Player").transform.Find("knight").transform.Find("Group").GetComponent<SkinnedMeshRenderer>();
        DP = gameObject.transform.Find("DP").GetComponent<ParticleSystem>();
        mapCam = GameObject.Find("MiniMapCamera").transform;
        CM = GameObject.Find("Cam").transform.Find("CM FreeLook1").transform;
        Cam = GameObject.Find("MiniMapCamera").transform;
        MyPlayerTransform = transform.Find("PlayerTransform").gameObject;
    }
    #region IEnumerator
    public IEnumerator Damage(float DamageTime)
    {
        _IsDamage = true;
        yield return new WaitForSeconds(DamageTime);
        _IsDamage = false;
    }
    public IEnumerator Damage2(float DamageTime)
    {
        _IsDamage2 = true;
        yield return new WaitForSeconds(DamageTime);
        _IsDamage2 = false;
    }
    
    IEnumerator CanDash3()
    {
        _CanDash2 = false;
        yield return new WaitForSeconds(0.25f);
        _CanDash2 = true;
    }
    IEnumerator FreezeP()
    {
        _canFreeze = true;
        yield return new WaitForSeconds(_FreezeTime);
        _canFreeze = false;
    }
    IEnumerator Dodge()
    {
        Character.center = new Vector3(-0.09f, -0.6f, 0.05f);
        _gravity = 24;
        Character.radius = 0.3f;
        Character.height = 1.5f;
        yield return new WaitForSeconds(_dashLength);
        Character.center = new Vector3(-0.09f, 0, 0.05f);
        Character.radius = 0.76f;
        Character.height = 2.55f;
        _gravity = 16;
    }
    public IEnumerator GitHit()
    {
       
        anim.SetLayerWeight(1, 1);
        anim.SetTrigger("Hit");
        StartCoroutine(CanDash3());
        yield return new WaitForSeconds(0.67f);
        anim.SetLayerWeight(1, 0);

    }
    public IEnumerator GitHitBoss1()
    {

        StartCoroutine(CanDash3());
        yield return new WaitForSeconds(0.67f);


    }
    public IEnumerator GitHitBoss2(float Time)
    {
        _speed = .5f;
        StartCoroutine(CanDash3());
        yield return new WaitForSeconds(Time);
        _speed = 2f;

    }
    public IEnumerator Speeds()
    {
        _speed2 = _speed;
        yield return new WaitForSeconds(0.01f);
        _speed += (_speed / 100) * LevelManager.instance.SpeedPercentegePlayer;
        isSpeedPowerEnabled = true;
        Zoomout = true;
        Zoomin = false;
        particle.SetActive(true);
        anim.SetLayerWeight(3, 1f);
        anim.SetTrigger("SpeedEffect");
        yield return new WaitForSeconds(LevelManager.instance._SpeedCooldown);
      
        particle.SetActive(false);
        _speed = _speed2;
        isSpeedPowerEnabled = false;
        Zoomout = false;
        Zoomin = true;
        anim.SetTrigger("SpeedEffect2");
        anim.SetLayerWeight(3, 0);
       
        
    }
    public IEnumerator SwordAbility()
    {
       
        LevelManager.instance.isSword = true;
        LevelManager.instance.CanSword = false;
        Trails[0].SetActive(true);
        Trails[1].SetActive(true);
        TrailsPS[0].Play(true);
        TrailsPS[1].Play(true);
        //ParticleSystemSlashEffectEnable
        yield return new WaitForSeconds(LevelManager.instance.SwordAbilityTime);
        Trails[0].SetActive(false);
        Trails[1].SetActive(false);
        TrailsPS[0].Play(false);
        TrailsPS[1].Play(false);
        LevelManager.instance.CanSword = true;
        LevelManager.instance.isSword = false;
    }
    private IEnumerator LightAttackSwitcher()
    {
        LightGlow.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        LightGlow.SetActive(false);
    }
    public IEnumerator IceEffectIenumerator()
    {    
        anim.SetTrigger("Up");
        yield return new WaitForSeconds(1f);
        anim.ResetTrigger("Up");
    }
   private IEnumerator Actives()
    {
        TestObject[0].SetActive(false);
        TestObject[1].SetActive(false);
        yield return new WaitForSeconds(2f);
        TestObject[0].SetActive(true);
        TestObject[1].SetActive(true);
    }
    #endregion



}