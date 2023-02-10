using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRotatingObject : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.forward * 0.2f); // * Time.deltaTime); //* rotateSpeedFactor

        transform.position += new Vector3(-7f * Time.deltaTime, 0f, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
