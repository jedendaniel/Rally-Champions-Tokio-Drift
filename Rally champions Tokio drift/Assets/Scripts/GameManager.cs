using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public List<Car> cars;
    List<Car> carsLeft;
    float verticalParts = 1;
    float horizontalParts = 1;
    int laps = 1;
    Dictionary<int, Car> places = new Dictionary<int, Car>();
    int place = 1;
    public Text EndResults;
    public Camera mainCamera;

    bool gameFinished;

    void Start ()
    {
        carsLeft = new List<Car>(cars);
        GetScreenParts();
        SplitScreen();
    }
	
	void Update () {
        CheckCarsProgress();
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
            car.laps = laps;
            car.lapText.text = "Lap: 0/" + laps.ToString();
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

    void CheckCarsProgress()
    {
        if (gameFinished) return;
        List<Car> finishedCars = new List<Car>();
        foreach (Car car in carsLeft)
        {
            if (car.Lap == laps)
            {
                finishedCars.Add(car);
                places.Add(place, car);
                car.EndRace(place);
            }
        }
        foreach(Car car in finishedCars)
        {
            place++;
            carsLeft.Remove(car);
        }
        if (carsLeft.Count == 0) EndGame();
    }

    void EndGame()
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine();
        foreach(KeyValuePair<int,Car> place in places)
        {
            text.Append(place.Key.ToString());
            text.Append(". ");
            text.Append(place.Value.name);
            text.AppendLine();
            place.Value.enabled = false;
        }
        mainCamera.rect = new Rect(0, 0, 1, 1);
        EndResults.transform.parent.gameObject.SetActive(true);
        EndResults.enabled = true;
        EndResults.text += text.ToString();
        gameFinished = true;
    }
}
