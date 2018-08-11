using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    bool up = false;
    Rigidbody rb;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            up = true;
        }
        else
        {
            up = false;
        }
	}

    private void FixedUpdate()
    {
        if (up && rb.velocity.x < 5 && rb.velocity.y < 5)
        {
            rb.AddForce(new Vector3(20, 0, 0));
        }
    }
}
