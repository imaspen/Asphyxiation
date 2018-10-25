using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol1 : MonoBehaviour
{
    public float speed = 1.19f;
    public float a;
    public float b;

    //public float x;
    public float y;
    Vector3 pointA;
    Vector3 pointB;

    void Start()
    {
        pointA = new Vector3(a, y, 0);
        pointB = new Vector3(b, y, 0);
    }

    void Update()
    {
        //PingPong between 0 and 1
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(pointA, pointB, time);
    }
}
