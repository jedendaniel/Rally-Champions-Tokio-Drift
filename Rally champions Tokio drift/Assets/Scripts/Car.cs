using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour {
    
    float verticalInput;
    float horizontalInput;
    Rigidbody rb;
    public InputSettings inputSettings;
    public Text lapText;
    public Text endText;
    int lap;
    [HideInInspector]
    public int laps;
    Camera[] cameras;
    public string name;
    public Canvas uiCanvas;

    public int Lap
    {
        get
        {
            return lap;
        }
    }

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        cameras = GetComponentsInChildren<Camera>();
    }
	
	void Update () {
        verticalInput = Input.GetAxis(inputSettings.verticalAxis);
        horizontalInput = Input.GetAxis(inputSettings.horizontalAxis);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lap+")
        {
            if (transform.position.z < other.transform.position.z)
                lap++;
            else
                lap--;
            lapText.text = "Lap: " + Lap.ToString() + "/" + laps.ToString();
        }
    }

    public void EndRace(int place)
    {
        endText.gameObject.SetActive(true);
        StringBuilder message = new StringBuilder();
        message.Append(place);
        message.Append(GetNumberSuffix(place));
        message.Append(" place!");
        message.AppendLine();
        message.Append("Good job!");
        endText.text = message.ToString();
        MeshRenderer rend;
        rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
    }

    public static string GetNumberSuffix(int number)
    {
        if (number == 1) return "st";
        if (number == 2) return "nd";
        if (number == 3) return "rd";
        return "th";
    }

    private void OnDisable()
    {
        uiCanvas.enabled = false;
        foreach(Camera cam in cameras)
        {
            cam.enabled = false;
        }
    }
}
