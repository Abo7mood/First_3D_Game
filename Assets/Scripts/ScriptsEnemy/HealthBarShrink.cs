using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarShrink : MonoBehaviour
{

    #region Construct
    private Timer _timer;
    public static HealthBarShrink instance;
    SpawnSystem spawnSystem;
    PlayerMovement playerMovement;

    public Color highHealthColor = new Color(1f, 0.2853684f, 2783019f);
    public Color mediumHealthColor = new Color(1, 1f, 1);
    public Color lowHealthColor = new Color(1f, 0.259434f, 0.259434f);
    public RawImage HealthCool;
    public Image barImage;
    public Image damagedBarImage;
    HealthPlayerAnimation _HealthAnimation;
    private HealthSystem healthSystem;
    Animator anim;

    public GameObject[] Enemies;
    #endregion
    #region Float&int
    int enemyNumber = -1;

      private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 1f;
   public int MaxHealth=500;

    private float damagedHealthShrinkTimer;
    #endregion

   
   
  
   
    
    private void Awake()
    {
        instance = this;
        _timer = GameObject.Find("Canvas").transform.Find("Statics").GetComponent<Timer>();   
        spawnSystem = GameObject.Find("GameManager").GetComponent<SpawnSystem>();
        playerMovement = GetComponent<PlayerMovement>();
          healthSystem = GetComponent<HealthSystem>();
        anim = GetComponentInChildren<Animator>();
    }
   
    private void Start() {
        healthSystem.SetUp(MaxHealth);
     
        SetHealth(healthSystem.GetHealthNormalized());
        damagedBarImage.fillAmount = barImage.fillAmount;
              
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;          
    }
    private void Update() {


        Rect uvRect = HealthCool.uvRect;
        uvRect.x += 0.2f * Time.deltaTime;
        HealthCool.uvRect = uvRect;
        if (Input.GetKeyDown(KeyCode.L))
        {
            healthSystem.Heal(100);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            healthSystem.Damage(100);
        }

        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0)
        {
         
            if (barImage.fillAmount < damagedBarImage.fillAmount)
            {
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
          
            }
        }
        if (this.healthSystem.healthAmount == MaxHealth)
        {
            LevelManager.instance.isHeal = true;
        }
        else { LevelManager.instance.isHeal = false; }


    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        SetHealth(healthSystem.GetHealthNormalized());
        damagedBarImage.fillAmount = barImage.fillAmount;
       
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
        SetHealth(healthSystem.GetHealthNormalized());
             
    }

    private void SetHealth(float healthNormalized) {
       barImage.fillAmount = healthNormalized;
       
        if (this.healthSystem.healthAmount <= 0)
        {
            _timer.StopTimer();
            spawnSystem.SpawnerOn = false;
            playerMovement._isInput = false;
            CancelInvoke("Ultimate2HitPlayer");
            anim.SetTrigger("Die");
            A1();
        }      
    }
    public void VEnemyC(float EnemyC)
    {
       
        healthSystem.Damage(EnemyC);
    }
    public void VEnemyH(float EnemyH)
    {

        healthSystem.Heal(EnemyH);
    }
    public void A1()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gameObject in Enemies)
        {

            enemyNumber++;
            if (Enemies[enemyNumber].GetComponent<EnemyController>() != null)
            {
                
                Enemies[enemyNumber].GetComponent<EnemyController>().A();
            }
               
            else if(Enemies[enemyNumber].GetComponent<EnemyController>() == null)
            {

                Enemies[enemyNumber].GetComponent<BossController>().A();
            }
            else
            {
                return;
            }
           
        }
    }
}
