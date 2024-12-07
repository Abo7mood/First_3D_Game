using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    //[SerializeField]
    //Transform rotationCenter;

    //[SerializeField]
    //float rotationRadius = 2f, angularSpeed = 2f;

    //float posX, posY, posZ, angle = 0f;
    public float x_speed = -5f;
    public float y_speed = -3f;
    public float z_speed = -3f;
    //public float x_scale = 1f;
    //public float y_scale = 1f;
    //public float z_scale = 1f;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //float y = transform.localPosition.y;
        //posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;

        //posZ = rotationCenter.position.z + Mathf.Sin(angle) * rotationRadius;
        //transform.position = new Vector3(posX, y,posZ);
        //angle = angle + Time.deltaTime * angularSpeed;

        //if (angle >= 360f)
        //    angle = 0f;







        transform.Rotate(x_speed, y_speed, z_speed);

    }






}


