using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class Postprossingfather : MonoBehaviour
{

    public Volume volume;

   
    ColorAdjustments colorAdjustments;
    public PostProcessData processData;
    void Start()
    {

       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
      
    }
    public void SetDark()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.active = true;
        }
    }
    public void SetLight()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.active = false;
        }
    }

}
