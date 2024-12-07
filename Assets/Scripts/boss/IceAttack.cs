using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class IceAttack : MonoBehaviour
{
   [HideInInspector] public float distance;
    Image Warn;
    //1 speed for each 1.7 distance;
    private float stoppingDistance = 6;
    public float _RealDistance;
    float _speed;
    float pathLength = 0;
    public float elapsed = 0.0f;
    [SerializeField] private float _turnSpeed = 2.75f;
  [HideInInspector] public NavMeshAgent agent;
    NavMeshPath path;
    bool CanRepeat;

    public static IceAttack instance;
    private void Awake()
    {
        instance = this;
        Warn = GameObject.Find("Canvas").transform.Find("IceWarn").GetComponent<Image>();
        agent = gameObject.GetComponent<NavMeshAgent>();
         path = new NavMeshPath();
        CanRepeat = true;

    }
    private void OnEnable()
    {
        Warn.gameObject.SetActive(true);
        PlayerIceLocation();
     DistanceFloat();
       agent.speed = DistanceFloat() / 2.2f;


    }
    private void OnDisable()
    {
        Destroy(BossController.instance.IceLocationMother);
    }
    void Start()
    {

      
       
        

    }


    void Update()
    {
     
     
        _RealDistance = Vector3.Distance(transform.position, BossController.instance.IceLocationMother.transform.position);


        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        agent.SetDestination(BossController.instance.IceLocationMother.transform.position);
        
        if (_RealDistance < 2)
        {
           

         
            transform.localPosition = new Vector3(0, -1.9f, 2.5f);
            agent.speed = 0;
            VFx.instance.gameObject.SetActive(false);
        }
        else { }

        if (_RealDistance < stoppingDistance)
        {
            Warn.gameObject.SetActive(false);

            gameObject.SetActive(false);
            VFx.instance.CanInstintiate = false;
            elapsed = 0;
            gameObject.SetActive(false);
            VFx.instance.gameObject.SetActive(false);
        }

        else
        {

            VFx.instance.CanInstintiate = true;
            elapsed += Time.deltaTime;
        }



        if (distance < 60)
        {
            LevelManager.instance.CanIceAttackOn = true;
        }
        else
        {
            LevelManager.instance.CanIceAttackOn = false;
        }
        #region comment
        //if (elapsed > 1.0f)
        //{
        //    elapsed -= 1.0f;
        //    NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        //}
        //for (int i = 0; i < path.corners.Length - 1; i++)
        //    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);

        //Vector3 direction = (LevelManager.instance.player.position - transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);

        //Debug.Log(distance);

        //Vector3 point;
        //if (RandomPoint(transform.position, out point))
        //{
        //    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
        //}
        //NavMesh.CalculatePath(transform.position, LevelManager.instance.IceLocationMother.transform.position, NavMesh.AllAreas, path);

        //Debug.Log(GetRemainingDistance());-
        #endregion

    }

    public void PlayerIceLocation()
    {
        BossController.instance.IceLocationMother = Instantiate(BossController.instance.IceLocation,
            LevelManager.instance.player.transform.position, Quaternion.identity, null);

    }
    public float DistanceFloat()
    {
       
        distance = Vector3.Distance(transform.position, BossController.instance.IceLocationMother.transform.position);
        return distance;
       
    }
 


}
