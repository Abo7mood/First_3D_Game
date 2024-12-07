using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateobj1 : MonoBehaviour
{
    #region rotate
    public Transform traansform;
   
   public float y=2;
    private Vector3 center;
    
  
   
    public Rigidbody rb;
   
    #endregion

    #region scalevarible
   
    public Vector3 maxscale;
    public Vector3 minScale;
    public Vector3 oldminscale;
    public bool reatlable;
    public float speed;
    public float duration;
    
    public float appeartime;
    #endregion

    #region boolean
 
    #endregion
    private void Awake()
    {
        center = new Vector3(traansform.position.x, traansform.position.y, traansform.position.z);
        
        transform.localScale = minScale;
        rb = GetComponent<Rigidbody>();
    }
   
   
    // Update is called once per frame
    void Update() => rb.transform.RotateAround(center, Vector3.up * Time.deltaTime, y);

    
    IEnumerator Start()
    {
        minScale = transform.localScale;
        while (reatlable)
        {
            yield return RepeatLerp(minScale, maxscale, duration);
            yield return new WaitForSeconds(appeartime);
            yield return RepeatLerp(maxscale, minScale, duration);
            Destroy(this.gameObject);
        }
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
