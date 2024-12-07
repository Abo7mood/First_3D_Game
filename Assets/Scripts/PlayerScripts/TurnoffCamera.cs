using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnoffCamera : MonoBehaviour
{
    FindEnemy _findEnemy;
    public GameObject Virtualcamera;
    public GameObject Freelook;

    void Awake() => _findEnemy = GameObject.Find("Player").transform.Find("Track").GetComponent<FindEnemy>();

    void Update()
    {
        if (_findEnemy.isLock == true)
        {
            Virtualcamera.SetActive(true);
            Freelook.SetActive(false);
        }
        else
        {
            Virtualcamera.SetActive(false);
            Freelook.SetActive(true);
        }
    }
}
