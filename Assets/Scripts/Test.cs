using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3 maxscale;
    public Vector3 minScale;
    public Vector3 oldminscale;
    public bool reatlable;
    public float speed;
    public float duration;

    public float appeartime;

    Vector3 bruh = new Vector3(5, 5, 5);
    public GameObject notbruh;

    

    private void Awake()
    {

        minScale = transform.localScale;
       
    }
    private void Start()
    {
        StartCoroutine(RepeatLerp(minScale, maxscale, duration));
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
       
    }
   
    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;

        }
    }
   
}
