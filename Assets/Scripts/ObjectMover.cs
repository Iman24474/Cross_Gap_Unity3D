using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    const float speed = 11.176f;
    const float endPoint = 125f;
    const float startPoint = -125f;
    const float centerPoint = 0f;
    float waitTime;
    float carStatus;
    float carXPos;
    bool dataRecording = true;
    bool passedCenterPoint;
    CarSpawner carSpawner;
    CarEntity carEntity;
    HeadMotionAnalysis headMotionAnalysis;
    List<float> CarRecord = new List<float>();
    Color color1;
    Color color2;
    

    // Start is called before the first frame update
    void Start()
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
        headMotionAnalysis = GameObject.Find("XR Origin (XR Rig)").GetComponent<HeadMotionAnalysis>();
        carEntity = GetComponent<CarEntity>();
        color1 = GetComponent<MeshRenderer>().materials[0].color;
        color2 = GetComponent<MeshRenderer>().materials[1].color;
        waitTime = carSpawner.waitTime;
        headMotionAnalysis.gapsGeneratedActual.Add(carSpawner.elapsedTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        carXPos = gameObject.transform.position.x;

        if(!gameObject.tag.Equals("TailCar") && carXPos > centerPoint && !passedCenterPoint)
        {
            headMotionAnalysis.gapsGeneratedRounded.Add(waitTime);
            if (color1 == Color.yellow || color2 == Color.yellow)
            {
                headMotionAnalysis.leadCarColor.Add(1);
                Debug.Log("Yellow Car");
            }
            else
            {
                headMotionAnalysis.leadCarColor.Add(0);
                Debug.Log("Car");
            }
            passedCenterPoint = true;
        }

        // Car data recording
        if(carXPos == startPoint)
        {
            CreatedCar();
        }

        else if(gameObject.tag == "DestroyedCar" && dataRecording)
        {
            DestroyCarByRoadCrossing();
            dataRecording = false;
        }

        else if(carXPos < endPoint)
        {
            MovingCar();
        }

        else if(dataRecording)
        {
            DestroyCar();
            dataRecording = false;
        }

        // Move the gameobject forward along the x-axis
        gameObject.transform.position += Vector3.right * speed * Time.deltaTime;

    }

    
    void CreatedCar()
    {
        carStatus = 1; // Car is created
        float pink = 0;
        float yellow = 0;

        if(color1 == Color.magenta || color2 == Color.magenta) 
        {
            pink = 1;
        }

        if(color1 == Color.yellow || color2 == Color.yellow)
        {
            yellow = 1;
        }

        CarRecord = new List<float>() 
        {
            carStatus, // Car status
            carEntity.entityID, // Car Id
            carEntity.carSize, // Car Size
            startPoint, // Car X-Pos
            waitTime, // Car gap size
            pink, // is the car pink?
            yellow // is the car yellow
        };

        if(!carSpawner.CarInfo.ContainsKey(carSpawner.elapsedTime))
        {
            carSpawner.CarInfo.Add(carSpawner.elapsedTime, new List<List<float>> {CarRecord});
        }
        else
        {
            carSpawner.CarInfo[carSpawner.elapsedTime].Add(CarRecord);
        }
        
    }

    void MovingCar()
    {
        carStatus = 2; // Car is moving
        float pink = 0;
        float yellow = 0;

        if(color1 == Color.magenta || color2 == Color.magenta) 
        {
            pink = 1;
        }

        if(color1 == Color.yellow || color2 == Color.yellow)
        {
            yellow = 1;
        }

        // Car data recording
        CarRecord = new List<float>() 
        {
            carStatus, // Car status
            carEntity.entityID, // Car Id
            carEntity.carSize, // Car Size
            carXPos, // Car X-Pos
            waitTime, // Car gap size
            pink, // is the car pink?
            yellow // is the car yellow
        };

        if(!carSpawner.CarInfo.ContainsKey(carSpawner.elapsedTime))
        {
            carSpawner.CarInfo.Add(carSpawner.elapsedTime, new List<List<float>> {CarRecord});
        }
        else
        {
            carSpawner.CarInfo[carSpawner.elapsedTime].Add(CarRecord);
        }

    }

    void DestroyCarByRoadCrossing()
    {
        carStatus = 3; // Car is destroyed
        float pink = 0;
        float yellow = 0;

        if(color1 == Color.magenta || color2 == Color.magenta) 
        {
            pink = 1;
            carSpawner.isPink = false;
        }

        if(color1 == Color.yellow || color2 == Color.yellow)
        {
            yellow = 1;
            carSpawner.isYellow = false;
        }

        // Car data recording
        CarRecord = new List<float>() 
        {
            carStatus, // Car status
            carEntity.entityID, // Car Id
            carEntity.carSize, // Car Size
            carXPos, // Car X-Pos
            waitTime, // Car gap size
            pink, // is the car pink?
            yellow // is the car yellow
        };

        if(!carSpawner.CarInfo.ContainsKey(carSpawner.elapsedTime))
        {
            carSpawner.CarInfo.Add(carSpawner.elapsedTime, new List<List<float>> {CarRecord});
        }
        else
        {
            carSpawner.CarInfo[carSpawner.elapsedTime].Add(CarRecord);
        }


        Destroy(gameObject);

    }
    void DestroyCar()
    {
        carStatus = 3; // Car is destroyed
        float pink = 0;
        float yellow = 0;

        if(color1 == Color.magenta || color2 == Color.magenta) 
        {
            pink = 1;
            carSpawner.isPink = false;
        }

        if(color1 == Color.yellow || color2 == Color.yellow)
        {
            yellow = 1;
            carSpawner.isYellow = false;
        }
        // Car data recording
        CarRecord = new List<float>() 
        {
            carStatus, // Car status
            carEntity.entityID, // Car Id
            carEntity.carSize, // Car Size
            endPoint, // Car X-Pos
            waitTime, // Car gap size
            pink, // is the car pink?
            yellow // is the car yellow
        };

        carSpawner.CarInfo.Add(carSpawner.elapsedTime, new List<List<float>> {CarRecord});

        // Once the car reaches the endpoint, it will be destroyed
        if(carXPos >= endPoint)
        {
            Destroy(gameObject);
        }
    }

}
