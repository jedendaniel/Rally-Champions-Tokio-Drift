using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    
    float verticalInput;
    float horizontalInput;
    Rigidbody rb;
    Camera mainCamera;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
	
	void Update () {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        MoveForward();
        RotateCar();
        AlignCameraAngle();
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
            FollowCarWithCamera();
        }
    }

    void FollowCarWithCamera()
    {
        mainCamera.transform.position += rb.velocity / 50;
    }

    void AlignCameraAngle()
    {
        float angleDiff = rb.rotation.eulerAngles.y - mainCamera.transform.rotation.eulerAngles.y;
        mainCamera.transform.RotateAround(transform.position, Vector3.up, angleDiff);
    }
}
