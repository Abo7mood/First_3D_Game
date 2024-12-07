using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class rightLock : MonoBehaviour
{

    public float NextRightLeft = 0;
    private float LockRightRate = 5f;
    CinemachineVirtualCamera _virtualCamera;
    FindEnemy _findEnemy;
    [HideInInspector] public Vector3 Clos;
    public List<Transform> enemies = new List<Transform>(); //The enemies player detection collider is touching
    public Transform closestEnemyInDirectionright; // The closest enemy in the direction the player is facing
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
        if (Time.time >= NextRightLeft)
        {
            if (Input.GetButtonDown("LockOnR") && _findEnemy.LockAt == true && !_findEnemy.qPressed)
            {
                NextRightLeft = Time.time + 1f / LockRightRate;
                _findEnemy.ePressed = true;
                _findEnemy.timeForE = 0;
                StartCoroutine(SetRight(0f));
                StartCoroutine(LockinED());
               
                Invoke("lookR", 0.05f);
            }
        }
        foreach (Transform go in enemies)
        {
            if (closestEnemyInDirectionright != null&&go!=null)
            {
                Vector3 diff = go.transform.position - transform.position;
                Vector3 position = body.transform.position;
                Vector3 enemy = go.transform.position;
                Debug.DrawLine(position, enemy, Color.red);
                Clos = closestEnemyInDirectionright.transform.position;
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
                closestEnemyInDirectionright = closest;
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
                    if (enemies[i] == closestEnemyInDirectionright)
                    {

                        Invoke("DeleteClosestEnemy", 0.05f);
                    }
                    enemies.RemoveAt(i);
                }
            }

        }
    }
    private void DeleteClosestEnemy() => closestEnemyInDirectionright = null;
    private void OnDrawGizmos()
    {
        if (closestEnemyInDirectionright != null && _findEnemy.isLock == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(closestEnemyInDirectionright.transform.position, .3f);
        }
        else if (closestEnemyInDirectionright != null && _findEnemy.isLock == false)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(closestEnemyInDirectionright.transform.position, .3f);
        }
    }
    void lookR()
    {
        if (closestEnemyInDirectionright != null)
            _virtualCamera.m_LookAt = _findEnemy.closestEnemyInDirection;

    }
    IEnumerator LockinED()
    {
        yield return new WaitForSeconds(0f);
        _findEnemy.isLockin = false;
    }
    public IEnumerator SetRight(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        if (closestEnemyInDirectionright != null)
            _findEnemy.closestEnemyInDirection = closestEnemyInDirectionright;
    }
}


