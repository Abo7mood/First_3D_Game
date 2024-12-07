using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Cinemachineshake : MonoBehaviour
{ 
    public static Cinemachineshake instance { get; private set; }
    public static CinemachineFreeLook freeLook { get; private set; }
 
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startinginstatntiy;

    private void Awake()
    {               
            instance = this;
        freeLook = GetComponent<CinemachineFreeLook>();
    
    }
    

    private void Start()
    {
        CinemachineBasicMultiChannelPerlin[] cfree =
            freeLook.GetComponentsInChildren<CinemachineBasicMultiChannelPerlin>();
        foreach (CinemachineBasicMultiChannelPerlin joint in cfree)
        {
            joint.m_AmplitudeGain =0;
          
        }
       
    }

    public void Shaker(float instantity,float time)
    {                
        CinemachineBasicMultiChannelPerlin[] cfree =
           freeLook.GetComponentsInChildren<CinemachineBasicMultiChannelPerlin>();
            foreach (CinemachineBasicMultiChannelPerlin joint in cfree)
            {
                joint.m_AmplitudeGain = instantity;
                startinginstatntiy = instantity;
                shakeTimer = time;
            shakeTimerTotal = time;
            }
    
    }

    private void Update()
    {
       
        CinemachineBasicMultiChannelPerlin[] c =
      freeLook.GetComponentsInChildren<CinemachineBasicMultiChannelPerlin>();
        foreach (CinemachineBasicMultiChannelPerlin joint in c)
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;

                joint.m_AmplitudeGain = Mathf.Lerp(startinginstatntiy, 0f, 1 - (shakeTimer / shakeTimerTotal));

            }

            if (joint.m_AmplitudeGain > 0 && joint.m_AmplitudeGain < 1.7f)
                joint.m_AmplitudeGain = 0;
            if (joint.m_AmplitudeGain > 0 && shakeTimer < 0)
            {
                joint.m_AmplitudeGain = 0;
            }
        }
    }
   

}
