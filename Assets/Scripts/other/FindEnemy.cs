using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemy : MonoBehaviour
{
    #region Float
    public float timeForQ = 0;
    public float timeForE = 0;
    #endregion
    #region Bool
    public bool LockF = true;
    public bool isLock = false;
    public bool isLockin = false;
    public bool LockAt = false;
    public bool qPressed = false;
    public bool ePressed = false;
    #endregion



    private string[] Locks = { "rightLock", "leftLock" };
  
   
  
    #region Constructer
    public PlayerMovement playerMovement;//playerscript
    public List<Transform> enemies = new List<Transform>(); //The enemies player detection collider is touching
    public Transform closestEnemyInDirection; // The closest enemy in the direction the player is facing
    public Transform body; //The body the player using to find as the player
    public Transform Track;

    BoxCollider triggercollider;
    [HideInInspector] public Vector3 Clos;
    [HideInInspector] public Quaternion Clos1;
    #endregion

    private void Awake()=> triggercollider = GetComponent<BoxCollider>(); 

    private void Update()
    {
        CheckInput(); CheckLocks();
        
   
    }

    private void FindClosestGameObject()
    {
        Transform closest = null;
        float distance = 10000f;
        Vector3 position = body.transform.position;

        foreach (Transform go in enemies)
        {
            if (go != null)
            {
                Vector3 diff = go.transform.position - transform.position;
                float curDistance = diff.sqrMagnitude;

                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
                closestEnemyInDirection = closest;

            }
         
                

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (other.transform == enemies[i])
                {

                    return;
                }
            }
            enemies.Add(other.transform);
            FindClosestGameObject();
        }

        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            for (int i = 0; i < enemies.Count; i++)
            {
                if (other.transform == enemies[i])
                {
                    if (enemies[i] == closestEnemyInDirection)
                    {
                        

                        Invoke("DeleteClosestEnemy", 0.01f);
                    }
                    enemies.RemoveAt(i);
                }
            }
        }

    }

    private void OnDrawGizmos()
    {

        if (closestEnemyInDirection != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(closestEnemyInDirection.transform.position, .3f);
        }
    }

    private void DeleteClosestEnemy() => closestEnemyInDirection = null;

    private void SetTrackBody()
    {
        Quaternion bodyR = body.rotation;
        Track.rotation = bodyR;

        Vector3 position = body.transform.position;
        Track.localPosition = new Vector3(0, 0, 2.5f);
    }
    private void SetTrackClos()
    {
        Clos = closestEnemyInDirection.transform.position;
        Track.position = Clos;

        Clos1 = closestEnemyInDirection.rotation;
        Track.rotation = Clos1;

    }
    private void SetTrackColliderClos()
    {
        triggercollider.center = new Vector3(0, 0, 0);
    }
    private void SetTrackColliderBody()
    {
        triggercollider.center = new Vector3(-20, 0, 290);
    }

    private void ColliderEnable() => triggercollider.enabled = true;
    private void ColliderDisable() => triggercollider.enabled = false;

    private void DisableRight() => closestEnemyInDirection.transform.Find(Locks[0]).GetComponent<rightLock>().closestEnemyInDirectionright = null;
    private void DisableLeft() => closestEnemyInDirection.transform.Find(Locks[1]).GetComponent<leftLock>().closestEnemyInDirectionleft = null;
    private void CheckInput()
    {
        if (qPressed)
        {
            timeForQ += Time.deltaTime;
            if (timeForQ > .2f)
             qPressed = false;
               
            
        }
        if (ePressed)
        {
            timeForE += Time.deltaTime;
            if (timeForE > .2f)
             ePressed = false;
               
            
        }


        if (Input.GetButtonDown("LockOnL") && LockAt == true)
        {
            qPressed = true;
            timeForQ = 0;
        }
        if (Input.GetButtonDown("LockOnR") && LockAt == true)
        {
            ePressed = true;
            timeForE = 0;
        }
        if (Input.GetButtonDown("LockOn"))
        {
            isLock = !isLock;
            LockF = !LockF;
        }

    }
    private void CheckLocks()
    {
        if (isLock == false)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i].Equals(true))
                enemies[i].transform.Find("EnemyCanvas").transform.Find("LockOnArrow").gameObject.SetActive(false);
            }
        }


        if (isLock == false && closestEnemyInDirection != null)
        {
           
            closestEnemyInDirection.transform.Find(Locks[0]).GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
            closestEnemyInDirection.transform.Find(Locks[1]).GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
            closestEnemyInDirection.transform.Find(Locks[0]).GetComponent<BoxCollider>().center = new Vector3(-2f, 0, 0);
            closestEnemyInDirection.transform.Find(Locks[1]).GetComponent<BoxCollider>().center = new Vector3(.2f, 0, 0);

            isLockin = true;
        }

        if (isLock == true && closestEnemyInDirection != null)
        {
           
           
            Invoke("SetTrackColliderClos", 0f);
            Invoke("SetTrackClos", 0f);
            closestEnemyInDirection.transform.Find(Locks[0]).GetComponent<BoxCollider>().size = new Vector3(60, 20, 200);
            closestEnemyInDirection.transform.Find(Locks[1]).GetComponent<BoxCollider>().size = new Vector3(60, 20, 200);
            closestEnemyInDirection.transform.Find(Locks[0]).GetComponent<BoxCollider>().center = new Vector3(-31.5f, 0, 0);
            closestEnemyInDirection.transform.Find(Locks[1]).GetComponent<BoxCollider>().center = new Vector3(31.5f, 0, 0);
            closestEnemyInDirection.transform.Find(Locks[0]).GetComponent<BoxCollider>().enabled = true;
            closestEnemyInDirection.transform.Find(Locks[1]).GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(LockAtin());

            isLockin = true;

        }
        else
        {
           
            LockAt = false;
            Invoke("SetTrackBody", 0f);
            Invoke("SetTrackColliderBody", 0f);
            isLockin = false;
        }
        if (closestEnemyInDirection == null)
            isLock = false;

        foreach (Transform go in enemies)
        {
            if (go != null)
            {
                Vector3 diff = go.transform.position - transform.position;
                float curDistance = diff.sqrMagnitude;
                if (go != closestEnemyInDirection)
                {
                    go.transform.Find(Locks[0]).GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
                    go.transform.Find(Locks[1]).GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
                    go.transform.Find(Locks[0]).GetComponent<BoxCollider>().center = new Vector3(2f, 0, 0);
                    go.transform.Find(Locks[1]).GetComponent<BoxCollider>().center = new Vector3(2f, 0, 0);
                }
                else if (go == closestEnemyInDirection)
                {
                    go.transform.Find(Locks[0]).GetComponent<BoxCollider>().size = new Vector3(60, 20, 200);
                    go.transform.Find(Locks[1]).GetComponent<BoxCollider>().size = new Vector3(60, 20, 200);
                    go.transform.Find(Locks[0]).GetComponent<BoxCollider>().center = new Vector3(-31.5f, 0, 0);
                    go.transform.Find(Locks[1]).GetComponent<BoxCollider>().center = new Vector3(31.5f, 0, 0);

                }

                if (closestEnemyInDirection != null)
                {

                    Vector3 position = body.transform.position;
                    Vector3 enemy = go.transform.position;
                    Debug.DrawLine(position, enemy, Color.red);
                    Clos = closestEnemyInDirection.transform.position;
                    Debug.DrawLine(position, Clos, Color.green);
                }
                else { }

            }
        }
    }

    IEnumerator LockAtin()
    {
        yield return new WaitForSeconds(0.01f);
        LockAt = true;
    }

}

