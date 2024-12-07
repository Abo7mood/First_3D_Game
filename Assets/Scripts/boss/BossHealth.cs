
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    #region GameObjects
    public RawImage BossFace;
    public Image barImage;
    public Image damagedBarImage;
    public Image barImage1;
    public Image damagedBarImage1;
    public Image BackGround;
     GameObject RealCanvas;
    public GameObject enemycanvas;
    public GameObject floatingText;
    public GameObject[] UI;

    public CapsuleCollider caollider;

    #endregion

    #region Construct
    Timer timer;
    Postprossingfather postprossingfather;
    public BossDissolver dissolver;

    BossController bossController;

    Ragdoll ragdoll;
    private Animator anim;

    private HealthSystem healthSystem;
    private Color DamagedColor;
    #endregion
    
    #region float&int
    private int HealthCheck = 0;
    int killScore = 200;
    public int MaxHealth = 100;

    float LifeTime1 = 1.25f;
    float LifeTime2 = 2.6f;
    float alpha = 0;
    [SerializeField] private float alphaSpeed = .15f;
    [SerializeField] private float alphaSpeed2 = 6f;

    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;
    private float HealthPrecetege;
    private float damagedHealthFadeTimer;
    private float damagedHealthShrinkTimer;
    #endregion

    #region Boolean
    private bool isDissolve;
    private bool CanFloat;
    public bool isdead = false;

    #endregion

    

    private void Awake()
    {
        RealCanvas = GameObject.Find("Canvas").transform.Find("BossHealth").gameObject;
        timer = GameObject.Find("Canvas").transform.Find("Statics").GetComponent<Timer>();
        isDissolve = false;
        postprossingfather = GameObject.Find("PostManager").GetComponent<Postprossingfather>();
        BossFace = GameObject.Find("Canvas").transform.Find("BossHealth").Find("BossFace").GetComponent<RawImage>();
        barImage = GameObject.Find("Canvas").transform.Find("BossHealth").Find("HealthBar").GetComponent<Image>();
        damagedBarImage= GameObject.Find("Canvas").transform.Find("BossHealth").Find("HealthBarDamage").GetComponent<Image>();
        barImage1= GameObject.Find("Canvas").transform.Find("BossHealth").Find("HealthBar1").GetComponent<Image>();
        damagedBarImage1 = GameObject.Find("Canvas").transform.Find("BossHealth").Find("HealthBarDamage1").GetComponent<Image>();
         BackGround= GameObject.Find("Canvas").transform.Find("BossHealth").Find("HealthGround").GetComponent<Image>();
        this.gameObject.Equals(true);
        ragdoll = GetComponent<Ragdoll>();
        anim = GetComponentInChildren<Animator>();
        DamagedColor = damagedBarImage.color;
        DamagedColor.a = 0f;
        damagedBarImage.color = DamagedColor;

        damagedBarImage1.color = damagedBarImage.color;

        bossController = GetComponent<BossController>();
        healthSystem = GetComponent<HealthSystem>();

    }


    private void Start()
    {
        HealthBarShrink.instance.Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemycanvas.SetActive(true);
        if (floatingText)
        {
            CanFloat = true;
        }
        else { CanFloat = false; }

        BackGround.color = new Color(.5f, .5f, .5f, 0);
        barImage.color = new Color(1, 0, 0, 0);
        damagedBarImage.color = new Color(1, 1, 1, 0);
        barImage1.color = new Color(1, 0, 0, 0);
        damagedBarImage1.color = new Color(1, 1, 1, 0);
    
        healthSystem.SetUp(MaxHealth);
        SetHealth(healthSystem.GetHealthNormalized());
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void Update()
    {
        Alpha();
        Switcher();
          HealthPrecetege = healthSystem.healthAmount * 100 / MaxHealth;        
        HealthChecker();
        //Debug.Log("HealthCheck"+HealthCheck);
       

    }
    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        SetHealth(healthSystem.GetHealthNormalized());

    }
    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        if (DamagedColor.a <= 0)
        {
            damagedBarImage.fillAmount = barImage.fillAmount;
            damagedBarImage1.fillAmount = barImage.fillAmount;
        }
        if (HealthCheck == 2)
        {
            bossController.percentege = 1;
            bossController.percentege1 = 1;
        }
            
        DamagedColor.a = 1f;
        damagedBarImage.color = DamagedColor;
        damagedBarImage1.color = damagedBarImage.color;
        damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void SetHealth(float healthNormalized)
    {
        
       
            barImage.fillAmount = healthNormalized;
        barImage1.fillAmount = healthNormalized;
        if (this.healthSystem.healthAmount <= 0)
        {
            if (dissolver != null) { dissolver.Life = false; }

            if (gameObject.name == "Enemy(Clone)")
            {
                Destroy(this.gameObject, LifeTime1);
            }
            else
            {

                Invoke("TurnOnDissolve", .5f);
                Destroy(this.gameObject, LifeTime2);
            }

            LevelManager.instance.score += killScore;
            bossController.GetComponent<Light>().enabled = false;
            foreach (GameObject gg in UI)
            {
                GameObject.Destroy(gg);
            }
            if (!isdead)
            {
                timer.EnemyKill += 1;
             
                caollider.enabled = false;
               
             
                //StartCoroutine(enemyController.AddForceUp(5f));
                postprossingfather.SetLight();
                StartCoroutine(bossController.KnockBackFreeze3());
                Invoke("DeadEnemy", .11f);
                anim.SetTrigger("EnemyDie");
                isdead = true;
               
               
                Invoke("instintiatePrefabs", 0.1f);
            }


        }


    }
    public void PlayerAttack1(float Damage)
    {
        healthSystem.Damage(Damage);
        if (CanFloat == true)
        {
            var go = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
            go.GetComponent<TextMesh>().text = Damage.ToString();
        }

    }

    private void DeadEnemy()
    {
        ragdoll.SetColliders(true);
        ragdoll.SetRigidbody(false);

    }
    private void HealthChecker()
    {
        if (HealthPrecetege >= 100&& HealthPrecetege >= 66)
        {
            HealthCheck = 1;

        }else if(HealthPrecetege <= 66 &&HealthPrecetege >= 33)
        {
            HealthCheck = 2;
        }
        else if (HealthPrecetege <= 33 && HealthPrecetege >= 0)
        {
            HealthCheck = 3;
        }
    }
    private void instintiatePrefabs()
    {
        int RandomGenerate = Random.Range(0, 3);
        var go = Instantiate(LevelManager.instance.BossAbilites[RandomGenerate], transform.position, Quaternion.identity, transform);
        go.transform.parent = null;
    }
    private void TurnOnDissolve() => isDissolve = true;
   
    private void Switcher()
    {
        switch (HealthCheck)
        {
            default:
            case 1:

                bossController.stage = BossController.state.Stage1;
                break;
            case 2:

                bossController.stage = BossController.state.Stage2;
                break;
            case 3:

                bossController.stage = BossController.state.Stage3;
                break;

        }
    }
    private void Alpha()
    {
        if (barImage != null && barImage1 != null && isDissolve == false)
        {
            BossFace.color = new Color(1, 1, 1, alpha += Time.deltaTime * alphaSpeed);
            BackGround.color = new Color(.5f, .5f, .5f, alpha += Time.deltaTime * alphaSpeed);
            barImage.color = new Color(1, 0, 0, alpha += Time.deltaTime * alphaSpeed);
            barImage1.color = new Color(1, 0, 0, alpha += Time.deltaTime * alphaSpeed);
        }
        else if (barImage != null && barImage1 != null && isDissolve == true)
        {

            BossFace.color = new Color(1, 1, 1, alpha -= Time.deltaTime * alphaSpeed2);
            BackGround.color = new Color(.5f, .5f, .5f, alpha -= Time.deltaTime * alphaSpeed2);
            barImage.color = new Color(1, 0, 0, alpha -= Time.deltaTime * alphaSpeed2);
            barImage1.color = new Color(1, 0, 0, alpha -= Time.deltaTime * alphaSpeed2);
        }
        else { return; }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            healthSystem.Damage(99);
        }
        if (DamagedColor.a > 0 && damagedBarImage != null)
        {
            damagedHealthFadeTimer -= Time.deltaTime;
            if (damagedHealthFadeTimer < 0)
            {
                float fadeAmount = 5f;
                DamagedColor.a -= fadeAmount * Time.deltaTime;
                damagedBarImage.color = DamagedColor;
                damagedBarImage1.color = damagedBarImage.color;
            }
        }
    }
}