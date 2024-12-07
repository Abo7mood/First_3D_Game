
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFade : MonoBehaviour
{
   
    //#region powers;
    //public GameObject LightPrefab;
    //public GameObject HeartPrefab;
    //public GameObject ShieldPrefab;
    //public GameObject SwordPrefab;
    //#endregion

    #region GameObjects
    GameObject enemycanvas;

    public GameObject floatingText;
    public GameObject floatingText1;
    public GameObject[] UI;
    #endregion
    #region Construct
    private Animator anim;
    public Image barImage;
    public Image damagedBarImage;
    public Image BackGround;

    Ragdoll ragdoll;
    private HealthSystem healthSystem;
    private Color DamagedColor;
    public Dissolver dissolver;
    EnemyController enemyController;
    Timer timer;
    #endregion

    #region Float&int
    int killScore = 200;
    public int MaxHealth = 100;

    float LifeTime1 = 1.25f;
    float LifeTime2 = 5f;
    float alpha = 0;
    [SerializeField] private float alphaSpeed = .15f;

    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;
    private float damagedHealthFadeTimer;
    private float damagedHealthShrinkTimer;
    #endregion

    #region Boolean
    private bool CanFloat;
    private bool CanFloat1;

    public bool isdead = false;
    #endregion

    
    private void Awake()
    {
        timer = GameObject.Find("Canvas").transform.Find("Statics").GetComponent<Timer>();
        enemycanvas = transform.Find("EnemyCanvas").transform.Find("LockOnArrow").gameObject;
        ragdoll = GetComponent<Ragdoll>();
        anim = GetComponentInChildren<Animator>();
        DamagedColor = damagedBarImage.color;
        DamagedColor.a = 0f;
        damagedBarImage.color = DamagedColor;
        enemyController = GetComponent<EnemyController>();
        healthSystem = GetComponent<HealthSystem>();

    }


    private void Start()
    {
      
        enemycanvas.SetActive(true);
        if (floatingText)
        {
            CanFloat = true;
        }
        else { CanFloat = false; }

        if (floatingText1)
        {
            CanFloat1 = true;
        }
        else { CanFloat1 = false; }

        BackGround.color = new Color(1, 1, 1, 0);
        barImage.color = new Color(1, 0, 0, 0);
        damagedBarImage.color = new Color(1, 1, 1, 0);
        transform.Find("EnemyCanvas").GetComponent<RectTransform>().localScale = new Vector3(MaxHealth * .00001f, 0.0015f, 0.1f);
        healthSystem.SetUp(MaxHealth);
        SetHealth(healthSystem.GetHealthNormalized());
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void Update()
    {
        HealBar();
        
    }
    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        SetHealth(healthSystem.GetHealthNormalized());

    }
    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        if (DamagedColor.a <= 0)
            damagedBarImage.fillAmount = barImage.fillAmount;
        DamagedColor.a = 1f;
        damagedBarImage.color = DamagedColor;
        damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        SetHealth(healthSystem.GetHealthNormalized());
    }

    private void SetHealth(float healthNormalized)
    {
        barImage.fillAmount = healthNormalized;
        if (this.healthSystem.healthAmount <= 0)
        {
            if (dissolver != null) { dissolver.Life = false; }

            if (this.gameObject.name == "Boss(Clone)")
            {
               
                Destroy(this.gameObject, LifeTime2);
          
            }
            else
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                Destroy(this.gameObject, LifeTime1);
                
                
            }
         
            LevelManager.instance.score += killScore;
            enemyController.GetComponent<Light>().enabled = false;
            foreach (GameObject gg in UI)
            {
                GameObject.Destroy(gg);
            }
            if (!isdead)
            {
                timer.EnemyKill += 1;
                enemycanvas.SetActive(false);
                //StartCoroutine(enemyController.AddForceUp(5f));
                StartCoroutine(enemyController.KnockBackFreeze3());


                    Invoke("instintiatePrefabs", 0.1f);
               
                Invoke("DeadEnemy", .11f);
                anim.SetTrigger("EnemyDie");
                isdead = true;
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
    public void EnemyHeal(float Heal)
    {
        healthSystem.Heal(Heal);
        if (CanFloat == true)
        {
            var go = Instantiate(floatingText1, transform.position, Quaternion.identity, transform);
            go.GetComponent<TextMesh>().text = Heal.ToString();
        }

    }

    private void DeadEnemy()
    {
        ragdoll.SetColliders(true);
        ragdoll.SetRigidbody(false);

    }
    private void instintiatePrefabs()
    {
        int RandomPrefabs = Random.Range(0, 4);
        var go = Instantiate(LevelManager.instance.PowerPrefabs[RandomPrefabs], transform.position, Quaternion.identity, transform);
        go.transform.parent = null;
    }
  private void HealBar()
    { 
        if (barImage != null)
        {
            BackGround.color = new Color(1, 1, 1, alpha += Time.deltaTime * alphaSpeed);
            barImage.color = new Color(1, 0, 0, alpha += Time.deltaTime * alphaSpeed);
        }
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
            }
        }
    }
    public void IsBig()
    {if (LevelManager.instance.isbig == true)
        {
            MaxHealth += 100;
        }
      
    }
}

