using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{//this is the enemy to anim victory
    #region allVarible

    #region enemyfloats

    [HideInInspector] public float FreezePercent;
    [HideInInspector] public float EnemyDamage1 = 12;
    [HideInInspector] public float EnemyDamage2 = 24;
    [HideInInspector] public float EnemyDamage3 = 25;
    [HideInInspector] public float EnemyDamage4 = 70;
    [HideInInspector] public float EnemyDamage5 = 50;

    [HideInInspector] public float EnemyDamage1Sword;
    [HideInInspector] public float EnemyDamage2Sword;

    [HideInInspector] public float SpeedPercentegeEnemy = 20;

    [HideInInspector] public int EnemyYellowMaxHealth = 100;
    [HideInInspector] public float EnemyYellowPlusDamage6;
    [HideInInspector] public float EnemyYellowPlusDamage5;
    #endregion

    #region PlayerFloats
    [HideInInspector] public float PlayerDamage1 = 10;
    [HideInInspector] public float PlayerDamage2 = 20;
    [HideInInspector] public float PlayerDamage3 = 5;
    [HideInInspector] public float PlayerDamage4 = 10;
    [HideInInspector] public float PlayerDamage5 = 5;
    [HideInInspector] public float PlayerDamage6 = 10;
    [HideInInspector] public float PlayerDamage7 = 55;
    [HideInInspector] public float PlayerDamage8 = 10;
    [HideInInspector] public float PlayerDamage9 = 5;
    [HideInInspector] public float PlayerDamage10 = 10;
    [HideInInspector] public float PlayerDamage11= 40;
    [HideInInspector] public float PlayerDamage12 = 25;
    [HideInInspector] public float PlayerDamage13 = 20;
    [HideInInspector] public float PlayerDamage14 = 25;
    

    [HideInInspector] public float YellowAbilityDamage = 20;
    [HideInInspector] public float EnemyBigDamage = 40;
    [HideInInspector] public float EnemySmallDamage =3;
    [HideInInspector] public float SpeedPercentegePlayer = 10;
    [HideInInspector] public float FreezeSpeedPercentegePlayer = 50;
    [HideInInspector] public float _SpeedCooldown = 10f;
    [HideInInspector] public float _FreezeSpeedCooldown =1;
    [HideInInspector] public float _ShieldCooldown = 10f;
    [HideInInspector] public float _ShieldSpeed = 100;

    [HideInInspector] public int PlayerMaxHealth = 0;

    [HideInInspector] public int SwordAbilityDamage = 0;
    [HideInInspector] public float SwordAbilityTime = 10f;

    [HideInInspector] public int IceAbilityDamage = 2;
    [HideInInspector] public float IceAbilityTime = 1;
    [HideInInspector] public float IceAbilityTimeRepeat = .2f;

    [HideInInspector] public int FireAbilityDamage = 3;
    [HideInInspector] public float FireAbilityTime = 3f;
    [HideInInspector] public float FireAbilityTimeRepeat = .5f;


    [HideInInspector] public float PlayerSpeed = 2;



    #endregion

    #endregion

    #region powers
    public List<GameObject> PowerPrefabs;
    public List<GameObject> BossAbilites;
    #region bool
    [HideInInspector] public bool isHeal=false;
    [HideInInspector] public bool isbig = false;
    [HideInInspector] public bool isLight = false;
    [HideInInspector] public bool CanSword = true;
    [HideInInspector] public bool isSword = false;

    [HideInInspector] public bool CanIceAttackOn = true;
    #endregion
    #region float
    [HideInInspector] public float HealAmount = 100;
    #endregion
    //hart Speed shield sword Light

    #endregion
    #region GameObjects;
    public List<GameObject> enemies = new List<GameObject>();
    public Transform knight;
    public Transform player;
    public Transform MainCanvas;
    public GameObject Camera;
    //0=speed,1=shield,2=sword
    public List<GameObject> Icons = new List<GameObject>();

    #endregion

    #region scripts
    public static LevelManager instance;
    public static PlayerMovement playerMovement;
    public Postprossingfather postprossingfather;
    #endregion

    #region otherfloat&int
    public float enemyNumbers;
    public int score;
    public int levelItems;
    public float RealEnemyCount;
    public int IconsCount;
    #endregion

    public GameObject Nuke;
    public GameObject NukeBomb;
    private void Awake()
    {

        instance = this;
        HealAmount = 100f;
        CanIceAttackOn = true;
        
    }
    private void Start()
    {
        //Invoke("SetActiveVoid", 2.95f);
        //Invoke("SetActiveVoid2", 3.3f);
        SetValues();
           EnemyYellowPlusDamage5 = 3f + PlayerDamage5;
        EnemyYellowPlusDamage6 = 3f + PlayerDamage6;
       
        EnemyDamage1Sword = EnemyDamage1 + 10;
        EnemyDamage2Sword = EnemyDamage2 + 10;
       
        isbig = false;
        isHeal = false;
        PlayerMaxHealth = HealthBarShrink.instance.MaxHealth;
        CanSword = true;
        _SpeedCooldown = 10f;
        _ShieldCooldown = 10f;
        SwordAbilityTime = 10f;
    }
    private void Update()
    {
     
        EnemyCountVoid();
      
    }
  
    public IEnumerator SpeedIcon()
    {
        Icons[0].SetActive(true);
        yield return new WaitForSeconds(_SpeedCooldown);
        Icons[0].SetActive(false);
    
    }
    public IEnumerator ShieldIcon()
    {
        Icons[1].SetActive(true); 
        yield return new WaitForSeconds(_ShieldCooldown);
        Icons[1].SetActive(false);
 
    }
    public IEnumerator SwordIcon()
    {

        Icons[2].SetActive(true);     
        yield return new WaitForSeconds(SwordAbilityTime);
        Icons[2].SetActive(false);     
    }
    public void EnemyCountVoid()
    {
        RealEnemyCount = enemies.Count;
        for (int i = 0; i < enemies.Count; i++)
        {

            if (enemies[i] == null && RealEnemyCount >= 1)
            {
                enemies.RemoveAt(i);
                i--;

            }
            else { return; }
        }
    }

    private void SetValues()
    {
     
   EnemyDamage1 = 12;
 EnemyDamage2 = 24;
  EnemyDamage3 = 25;
   EnemyDamage4 = 70;
   EnemyDamage5 = 50;



 SpeedPercentegeEnemy = 20;
 EnemyYellowMaxHealth = 100;
 

    PlayerDamage1 = 10;
    PlayerDamage2 = 20;
    PlayerDamage3 = 5;
    PlayerDamage4 = 10;
    PlayerDamage5 = 5;
    PlayerDamage6 = 10;
    PlayerDamage7 = 55;
    PlayerDamage8 = 10;
    PlayerDamage9 = 5;
    PlayerDamage10 = 10;
    PlayerDamage11 = 40;
    PlayerDamage12 = 25;
    PlayerDamage13 = 20;
    PlayerDamage14 = 25;


    YellowAbilityDamage = 20;
    EnemyBigDamage = 40;
    EnemySmallDamage = 3;
    SpeedPercentegePlayer = 10;
    FreezeSpeedPercentegePlayer = 50;
    _SpeedCooldown = 10f;
    _FreezeSpeedCooldown = 1;
    _ShieldCooldown = 10f;
    _ShieldSpeed = 100;

 PlayerMaxHealth = 0;
SwordAbilityDamage = 0;
    SwordAbilityTime = 10f;

     IceAbilityDamage = 2;
    IceAbilityTime = 1;
    IceAbilityTimeRepeat = .2f;

    FireAbilityDamage = 3;
    FireAbilityTime = 3f;
    FireAbilityTimeRepeat = .5f;


    PlayerSpeed = 2;


        
   

 
   isHeal = false;
   isbig = false;
   isLight = false;
   CanSword = true;
   isSword = false;

   CanIceAttackOn = true;
 
    HealAmount = 100;
  
}
    private void SetActiveVoid() { Nuke.SetActive(true); SoundManager.instance._source[2].enabled = true; }
    private void SetActiveVoid2()=>NukeBomb.SetActive(false);
}
