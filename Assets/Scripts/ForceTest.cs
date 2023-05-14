using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{
    public GameObject go;    // Start is called before the first frame update
    public float force;
    public bool thrown = false;

    void Start()
    {
        go = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.N) && thrown == false){
Debug.Log("Push");
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.useGravity = true;
                rb.AddForce(Vector3.forward * force);

                        thrown = true;

        }

        
    }
}
