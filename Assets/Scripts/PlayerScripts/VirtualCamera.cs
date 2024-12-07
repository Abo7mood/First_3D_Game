using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class VirtualCamera : MonoBehaviour
{
    CinemachineVirtualCamera _virtualCamera;
    PlayerMovement playerMovement;
    FindEnemy findEnemy;
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        findEnemy = GameObject.Find("Player").transform.Find("Track").GetComponent<FindEnemy>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (findEnemy.isLockin == true && findEnemy.closestEnemyInDirection != null && findEnemy.LockAt == false)
        {
            Invoke("lookF", 0.01f);
            lookF();
        }
    }
    void lookF() => _virtualCamera.m_LookAt = findEnemy.closestEnemyInDirection;

}
