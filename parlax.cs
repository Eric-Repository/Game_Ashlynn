using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parlax : MonoBehaviour
{
    public float speed;

    //public float Yend;
    //public float Ystart;
   
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        //if (transform.position.y > Yend)
        //{
        //    Vector2 pos = new Vector2(transform.position.x, Ystart);
        //    transform.position = pos;
        //}
    }
}
