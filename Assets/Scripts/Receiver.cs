using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    //public ReceiverHand catchT;
    public Transform catchT;

    // Start is called before the first frame update
    void Start()
    {
        catchT = RecursiveFindChild(transform,"catch");

    }

    void OnTriggerEnter(Collider other){

        if(other.gameObject.tag == "ball")
        {
            Debug.Log("catch ball");

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            other.gameObject.transform.parent = catchT;
        }
    }

    public static Transform RecursiveFindChild(Transform parent, string childName)
    {
        Transform child = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            child = parent.GetChild(i);
            if (child.name == childName)
            {
                break;
            }
            else
            {
                child = RecursiveFindChild(child, childName);
                if (child != null)
                {
                    break;
                }
            }
        }

        return child;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
