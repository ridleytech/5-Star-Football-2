using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter : MonoBehaviour
{

    public KeyCode keyCode = KeyCode.Mouse0;
        public KeyCode keyCode2 = KeyCode.Mouse1;

		public GameObject ball;
		public Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);
		public Vector3 force = new Vector3(0f, 0f, 7f);
		public float mass = 3f;

		void Update () {
			if (Input.GetKeyDown(keyCode)) {




				GameObject b = (GameObject)GameObject.Instantiate(ball, transform.position + transform.rotation * spawnOffset, transform.rotation);
				var r = b.GetComponent<Rigidbody>();
				SphereCollider sc = b.GetComponent<SphereCollider>();

				// GameObject net = GameObject.Find("net-w-holes");
				// Cloth netCloth = net.GetComponent<Cloth>();


				//  var ClothColliders = new ClothSphereColliderPair[1];
				// 	ClothColliders[0] = new ClothSphereColliderPair(sc);

				// 	netCloth.sphereColliders = ClothColliders;



				
			

				



				if (r != null) {
					r.mass = mass;

					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					r.AddForce(Quaternion.LookRotation(ray.direction) * force, ForceMode.VelocityChange);
				}
			}

            if (Input.GetKeyDown(keyCode2)) {
                Debug.Log("start route");

                GameObject player = GameObject.Find("WR-UCC");
                WRRouteAgent ra = player.GetComponent<WRRouteAgent>();
                ra.StartRoute();
            }
		}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
