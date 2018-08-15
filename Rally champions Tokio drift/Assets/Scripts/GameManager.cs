using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<Car> cars;
    float verticalParts = 1;
    float horizontalParts = 1;

    void Start ()
    {
        GetScreenParts();
        if (cars.Count != 1)
        {
            SplitScreen();
        }
	}
	
	void Update () {
		
	}

    void GetScreenParts()
    {
        while(cars.Count > verticalParts * horizontalParts)
        {
            if (Screen.width / verticalParts > Screen.height / horizontalParts) {
                if (horizontalParts - verticalParts > 1) horizontalParts -= 1;
                verticalParts += 1;
            }
            else
            {
                if (verticalParts - horizontalParts > 1) verticalParts -= 1;
                horizontalParts += 1;
            }
        }
    }

    void SplitScreen()
    {
        float v = 0;
        float h = 0;
        foreach(Car car in cars)
        {
            Camera[] carCameras = car.GetComponentsInChildren<Camera>();
            foreach (Camera camera in carCameras)
            {
                camera.rect = new Rect(
                    v / verticalParts , 1 - ((1 + h) / horizontalParts), 
                    1 / verticalParts, 1 / horizontalParts 
                    );
            }
            if(v == verticalParts - 1)
            {
                v = 0;
                h += 1;
            }
            else
                v += 1;
        }
    }
}
