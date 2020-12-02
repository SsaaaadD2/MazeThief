using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFired : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        //Destroy bullet 4 seconds after creating
        //In case it hit nothing and is in random space
        Invoke("DestroyObject", 4f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
