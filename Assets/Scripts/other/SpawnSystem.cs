using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpawnSystem : MonoBehaviour
{
  
    public static SpawnSystem instance;
    //before the ciel
    private float EnemyMax;
    //after the ciel
     float EnemyMaxReal;
    public bool SpawnerOn = true;
    private float EnemyMultiplie=1.5f;
   //the boss prefab
    public GameObject Boss;
    //the boss inside the game
    public GameObject RealBoss;
    public GameObject BossHome;
    public List<GameObject> spawners = new List<GameObject>();
    //enemies list prefabs
    public List<GameObject> Enemies = new List<GameObject>();
    //EnemyCountIntheWholeRound
    public int EnemyCount;
    //EnemyMaxCountAtTheSameTime
    private int EnemyMaxInRound=8;
    public int RoundCount;
    public Text RoundCountText;
 
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {


        InvokeRepeating("Spawner", 1, 1f);
        EnemyMax = 2;
        RoundCount = 1;

    }

    // Update is called once per frame
    void Update()
    {
        RoundSystem();
        print(EnemyMax);

        if (RoundCount % 2 == 0 && RealBoss == null&&LevelManager.instance.RealEnemyCount==0)
        {
            BossSpawn();
        }
        else { return; }

    }
    public void Spawner()
    {

        int SpawnRandomization = Random.Range(0, 10);
        int EnemyRandomization = Random.Range(0, 5);
        if (LevelManager.instance.enemies.Count < EnemyMaxInRound && EnemyCount < EnemyMax && SpawnerOn == true)
        {


            EnemyCount += 1;
            //Instantiate(Enemies[EnemyRandomization], spawners[SpawnRandomization].transform.position,
            //    Quaternion.identity, spawners[SpawnRandomization].transform);
            GameObject enemy = Instantiate(Enemies[EnemyRandomization], spawners[SpawnRandomization].transform.position,
                Quaternion.identity, spawners[SpawnRandomization].transform);
            LevelManager.instance.enemies.Add(enemy);
        }

    }
    public void BossSpawn()
    {


        RealBoss = Instantiate(Boss, BossHome.transform.position, Quaternion.identity);
    }
    public void RoundSystem()
    {
        RoundCountText.text = RoundCount.ToString();

        if (EnemyCount == EnemyMax && LevelManager.instance.RealEnemyCount == 0&& RealBoss==null)
        {
            EnemyCount = 0;
            RoundCount += 1;
            EnemyMaxReal = Mathf.Ceil(EnemyMax);
            EnemyMax = EnemyMaxReal * EnemyMultiplie;
            EnemyMax = Mathf.Ceil(EnemyMax); // or Mathf.Ceil(result);
            Debug.Log(EnemyMax);
        }


    }
}
