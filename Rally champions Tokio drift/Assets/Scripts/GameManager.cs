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
    Dictionary<Car, int> places = new Dictionary<Car, int>();
    int place = 1;
    public Text EndResults;
    public Camera mainCamera;
    int timeLeft = 3;

    bool gameStarted;
    bool gameFinished;

    void Start ()
    {
        carsLeft = new List<Car>(cars);
        GetScreenParts();
        SplitScreen();
        StartCounting();
    }
	
	void Update () {
        CountDown();
        if (gameStarted)
        {
            CheckCarsProgress();
        }
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
     void StartCounting()
    {
        StartCoroutine("CountDownOneSecond");
    }

    void CountDown()
    {
        foreach(Car car in cars)
        {
            if (timeLeft > 0)
                car.Counter.text = timeLeft.ToString();
            else if (timeLeft == 0)
            {
                car.Counter.text = "GO!";
                gameStarted = true;
                gameFinished = false;
                car.StartRace();
            }
            else if (timeLeft == -2)
            {
                car.Counter.enabled = false;
                StopCoroutine("CountDownOneSecond");
            }
        }
    }

    IEnumerator CountDownOneSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
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
                places.Add(car, place);
                car.EndRace(place);
            }
        }
        foreach(Car car in finishedCars)
        {
            place++;
            carsLeft.Remove(car);
        }
        if (carsLeft.Count == 0) EndRace();
    }

    void EndRace()
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine();
        foreach(KeyValuePair<Car,int> place in places)
        {
            text.Append(place.Value.ToString());
            text.Append(". ");
            text.Append(place.Key.name);
            text.AppendLine();
            place.Key.enabled = false;
        }
        mainCamera.rect = new Rect(0, 0, 1, 1);
        EndResults.transform.parent.gameObject.SetActive(true);
        EndResults.enabled = true;
        EndResults.text += text.ToString();
        gameStarted = false;
        gameFinished = true;
    }
}
