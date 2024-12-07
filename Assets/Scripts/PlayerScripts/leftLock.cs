using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class leftLock : MonoBehaviour
{
    
    public float NextLockLeft = 0;
    private float LockLeftRate = 5f;
    CinemachineVirtualCamera _virtualCamera;
    FindEnemy _findEnemy;
    [HideInInspector] public Vector3 Clos;
    public List<Transform> enemies = new List<Transform>(); //The enemies player detection collider is touching
    public Transform closestEnemyInDirectionleft; // The closest enemy in the direction the player is facing
    public Transform body;
    BoxCollider boxCollider;

    private void Awake()
    {
        _findEnemy = GameObject.Find("Player").transform.Find("Track").GetComponent<FindEnemy>();
        _virtualCamera = GameObject.Find("Cam").transform.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        transform.LookAt(LevelManager.instance.player);
        if (Time.time >= NextLockLeft)
        {
            if (Input.GetButtonDown("LockOnL") && _findEnemy.LockAt == true && !_findEnemy.ePressed)
            {
                NextLockLeft = Time.time + 1f / LockLeftRate;
                _findEnemy.qPressed = true;
                _findEnemy.timeForQ = 0;
                StartCoroutine(SetLeft(0f));
                StartCoroutine(LockinED());
               
                Invoke("lookL", 0.05f);
            }
        }
        foreach (Transform go in enemies)
        {
            if (closestEnemyInDirectionleft != null&&go!=null)
            {
                Vector3 diff = go.transform.position - transform.position;
                Vector3 position = body.transform.position;
                Vector3 enemy = go.transform.position;
                Debug.DrawLine(position, enemy, Color.red);
                Clos = closestEnemyInDirectionleft.transform.position;
                Debug.DrawLine(position, Clos, Color.green);
            }
            else { }
        }
    }
    private void FindClosestGameObject()
    {
        Transform closest = null;
        float distance = 1000f;
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
            closestEnemyInDirectionleft = closest;
            }
          
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
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
        if (other.tag == "Enemy")
        {

            for (int i = 0; i < enemies.Count; i++)
            {
                if (other.transform == enemies[i])
                {
                    if (enemies[i] == closestEnemyInDirectionleft)
                    {

                        Invoke("DeleteClosestEnemy", 0.05f);
                    }
                    enemies.RemoveAt(i);
                }
            }

        }
    }

    private void DeleteClosestEnemy() => closestEnemyInDirectionleft = null;
    private void OnDrawGizmos()
    {
        if (closestEnemyInDirectionleft != null && _findEnemy.isLock == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(closestEnemyInDirectionleft.transform.position, .3f);
        }
        else if (closestEnemyInDirectionleft != null && _findEnemy.isLock == false)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(closestEnemyInDirectionleft.transform.position, .3f);
        }
    }
    void lookL()
    {
        if (closestEnemyInDirectionleft != null)
            _virtualCamera.m_LookAt = _findEnemy.closestEnemyInDirection;
    }
    IEnumerator LockinED()
    {
        yield return new WaitForSeconds(0.02f);
        _findEnemy.isLockin = false;
    }
    public IEnumerator SetLeft(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        if (closestEnemyInDirectionleft != null)
            _findEnemy.closestEnemyInDirection = closestEnemyInDirectionleft;
    }
}
