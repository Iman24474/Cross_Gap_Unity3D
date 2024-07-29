using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMotionAnalysis : MonoBehaviour
{
    
    float timeOfEntry;
    float timeToSpare;
    float enterRoad;
    float exitRoad;
    CarSpawner carSpawner;
    TrialTracker trialTracker;
    [HideInInspector] public bool tagged;
    [HideInInspector] public bool roadEntry;
    [HideInInspector] public bool roadExit; 
    [HideInInspector] public List<float> gapsGeneratedRounded = new List<float>();   
    [HideInInspector] public List<float> gapsGeneratedActual = new List<float>();   
    [HideInInspector] public List<float> leadCarColor = new List<float>();   
    [HideInInspector] public Dictionary<float, List<float>> HeadMotionCarStream = new Dictionary<float,List<float>>();
    [HideInInspector] public Dictionary<float, List<float>> GapsSeenRounded = new Dictionary<float,List<float>>();
    [HideInInspector] public Dictionary<float, List<float>> GapsSeenActual = new Dictionary<float,List<float>>();
    [HideInInspector] public Dictionary<float, List<float>> YellowLeadCar = new Dictionary<float,List<float>>();
    // Start is called before the first frame update
    void Start()
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
        trialTracker = GameObject.Find("TriggerZone").GetComponent<TrialTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        float headZ = Camera.main.transform.position.z;
        float headX = Camera.main.transform.position.x;

        TagCars();

        GameObject leadCar = GameObject.FindWithTag("LeadCar");
        GameObject tailCar = GameObject.FindWithTag("TailCar");

        if(tagged && !roadEntry)
        {
            float leadCarX;
            float tailCarX;
            float leadCarSize;
            float tailCarSize;

            timeOfEntry = TimeOfEntryCalculation();
            enterRoad = carSpawner.elapsedTime;
            
            if(leadCar != null)
            {
                leadCarX = leadCar.transform.position.x;
                tailCarX = tailCar.transform.position.x;
                leadCarSize = leadCar.GetComponent<CarEntity>().carSize;
                tailCarSize = tailCar.GetComponent<CarEntity>().carSize;
            }

            else
            {
                leadCarX = -5000f; // To show error
                leadCarSize = -9999; // To show error
                tailCarX = tailCar.transform.position.x;
                tailCarSize = tailCar.GetComponent<CarEntity>().carSize;
            }

            HeadMotionCarStream.Add(trialTracker.trialNum, new List<float>()
            {
                enterRoad,
                headX,
                headZ,
                leadCarX,
                leadCarSize,
                tailCarX,
                tailCarSize,
                timeOfEntry
            });

            GapsSeenRounded.Add(trialTracker.trialNum, gapsGeneratedRounded);

            ActualGapsSeenCalculator();
            GapsSeenActual.Add(trialTracker.trialNum, gapsGeneratedActual);

            YellowLeadCar.Add(trialTracker.trialNum, leadCarColor);
            
            gapsGeneratedRounded = new List<float>();
            gapsGeneratedActual = new List<float>();
            leadCarColor = new List<float>();

        }

        if(tagged && headZ >= 1.5 && !roadExit)
        {
            float leadCarX;
            float tailCarX;
            float leadCarSize;
            float tailCarSize;

            timeToSpare = TimeToSpareCalculation();
            exitRoad = carSpawner.elapsedTime;

            float crossingTime = exitRoad - enterRoad;

            if(leadCar != null)
            {
                leadCarX = leadCar.transform.position.x;
                tailCarX = tailCar.transform.position.x;
                leadCarSize = leadCar.GetComponent<CarEntity>().carSize;
                tailCarSize = tailCar.GetComponent<CarEntity>().carSize;
            }

            else
            {
                leadCarX = -5000f; // To show error
                leadCarSize = -9999; // To show error
                tailCarX = tailCar.transform.position.x;
                tailCarSize = tailCar.GetComponent<CarEntity>().carSize;
            }

            HeadMotionCarStream[trialTracker.trialNum].Add(exitRoad);
            HeadMotionCarStream[trialTracker.trialNum].Add(headX);
            HeadMotionCarStream[trialTracker.trialNum].Add(headZ);
            HeadMotionCarStream[trialTracker.trialNum].Add(leadCarX);
            HeadMotionCarStream[trialTracker.trialNum].Add(leadCarSize);
            HeadMotionCarStream[trialTracker.trialNum].Add(tailCarX);
            HeadMotionCarStream[trialTracker.trialNum].Add(tailCarSize);
            HeadMotionCarStream[trialTracker.trialNum].Add(timeToSpare);
            HeadMotionCarStream[trialTracker.trialNum].Add(crossingTime);

        }
    
    
    }

    void TagCars()
    {
        float headZ = Camera.main.transform.position.z;
        float headX = Camera.main.transform.position.x;
        // Find all game objects with the tag "ClonedCar"
        GameObject[] clonedCars = GameObject.FindGameObjectsWithTag("ClonedCar");
        // Initial distance for comparison, set to a high value
        float tailCarDist = 2000; // All cars are instantiated at a distance of 125 
        float leadCarDist = -2000;
        string trialStatus = GameObject.Find("TriggerZone").tag;

        if(!tagged && headZ >= -1.5f && trialStatus.Equals("TrialStarted"))
        {
            foreach(GameObject car in clonedCars)
            {
                float headAndCarDist = headX - car.transform.position.x;
                // Find the tail car distance from subject at the time of entering the road
                if(headAndCarDist > 0 && headAndCarDist <= tailCarDist)
                {
                    tailCarDist = headAndCarDist;
                }

                // Find the lead car distance from subject at the time of entering the road
                if(headAndCarDist <= 0 && headAndCarDist >= leadCarDist)
                {
                    leadCarDist = headAndCarDist;
                }
            }

            // Give tags to lead and tail cars
            foreach(GameObject car in clonedCars)
            {
                float headAndCarDist = headX - car.transform.position.x;
                if(headAndCarDist == tailCarDist)
                {
                    car.tag = "TailCar";
                }
                if(headAndCarDist == leadCarDist)
                {
                    car.tag = "LeadCar";
                }
            }

            // Give a tage to cars that are behind the closest tail car
            foreach(GameObject car in clonedCars)
            {
                float dist = headX - car.transform.position.x;
                if(dist > tailCarDist)
                {
                    car.tag = "DestroyedCar" ;
                }
            }

            tagged = true;
        }
    }

    float TimeOfEntryCalculation()
    {
        float distTime = -9999f; // Means subject entered the road before the pink car passes
        const float carSpeed = 11.176f;
        float headX = Camera.main.transform.position.x;
        GameObject leadCar = GameObject.FindWithTag("LeadCar");
        if(leadCar != null)
        {
            float carSize = leadCar.GetComponent<CarEntity>().carSize;
            float leadCarRearBumper = leadCar.transform.position.x - carSize / 2;
            float headToLeadDist = leadCarRearBumper - headX;
            distTime = headToLeadDist / carSpeed;                
        } 
        roadEntry = true;
        return distTime;         
    }

    float TimeToSpareCalculation()
    {
        float headX = Camera.main.transform.position.x;
        const float carSpeed = 11.176f;
        GameObject tailCar = GameObject.FindWithTag("TailCar");

        float carSize = tailCar.GetComponent<CarEntity>().carSize;
        float tailCarFrontBumper = tailCar.transform.position.x + carSize / 2;
        float headToTailDist = headX - tailCarFrontBumper;
        float distTime = headToTailDist / carSpeed;
        roadExit = true;

        return distTime;
    }

    void ActualGapsSeenCalculator()
    {
        for(int i=1; i < gapsGeneratedActual.Count; i++)
        {
            gapsGeneratedActual[i-1] = gapsGeneratedActual[i] - gapsGeneratedActual[i-1];
        }
        gapsGeneratedActual.RemoveRange(gapsGeneratedRounded.Count, gapsGeneratedActual.Count - gapsGeneratedRounded.Count);
    }


}
