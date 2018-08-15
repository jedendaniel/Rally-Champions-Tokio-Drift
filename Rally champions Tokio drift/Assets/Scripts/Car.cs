using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    
    float verticalInput;
    float horizontalInput;
    Rigidbody rb;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
    }
	
	void Update () {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        MoveForward();
        RotateCar();
    }

    void MoveForward()
    {
        rb.AddForce(rb.transform.forward * 20 * verticalInput);
    }

    void RotateCar()
    {
        if (Mathf.Abs(rb.velocity.x) > 1f || Mathf.Abs(rb.velocity.z) > 1f)
        {
            rb.transform.Rotate(new Vector3(0, horizontalInput, 0));
        }
    }
}
