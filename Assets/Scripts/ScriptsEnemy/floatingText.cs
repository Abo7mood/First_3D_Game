using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class floatingText : MonoBehaviour
{
    public float DestroyTime=2f;
    public Vector3 Randomize = new Vector3(.8f, 0, 0);
    public new Camera camera;  
    void Awake() => camera = Camera.main;
    void LateUpdate()=>   transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);  
    void Start()
        {
        transform.localPosition += new Vector3(Random.Range(-Randomize.x, Randomize.x),
            Random.Range(-Randomize.y, Randomize.y),
            Random.Range(-Randomize.z, Randomize.z));
Destroy(gameObject, DestroyTime);
        TextMesh text = GetComponent<TextMesh>();
        if(gameObject.name== "Floating Text(Clone)")
        {
            text.color = new Color(Random.Range(0.8f, 1), Random.Range(0, .2f), Random.Range(0, .2f), 1);
        }
        else { text.color = new Color(Random.Range(0.2f, 0f), Random.Range(0.8f, 1), Random.Range(0.4f, 0f), 1); }
     
       
        }
}
 