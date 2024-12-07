using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_LookAt : MonoBehaviour
{  
   public new Camera camera;
    void Awake() => camera = Camera.main;
    void LateUpdate()=>  transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);   
}
