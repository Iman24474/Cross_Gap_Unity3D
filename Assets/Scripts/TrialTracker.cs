using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialTracker : MonoBehaviour
{
    [HideInInspector] public float trialNum;
    CarSpawner carSpawner;
    HeadMotionAnalysis headMotionAnalysis;
    // Start is called before the first frame update
    void Start()
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
        headMotionAnalysis = GameObject.Find("XR Origin (XR Rig)").GetComponent<HeadMotionAnalysis>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Trial Num: " + trialNum);
    }

    private void OnTriggerEnter(Collider other) {

        if(other.tag == "Head" && gameObject.tag == "Untagged" && carSpawner.start)
        {
            gameObject.tag = "TrialStarted";
            carSpawner.pause = true;
        }

        else if(other.tag == "Head" && gameObject.tag == "TrialStarted")
        {
            gameObject.tag = "Untagged";
            trialNum++;
            headMotionAnalysis.tagged = false;
            headMotionAnalysis.roadEntry = false;
            headMotionAnalysis.roadExit = false;
            carSpawner.pause = false;
            

            // Update the trial state and allow creating a new pink car
            carSpawner.delay = carSpawner.carCreationDelay;
            carSpawner.firstCar = true;
            
            // Check if the current trial number is equal to the total number of trials
            if(trialNum == carSpawner.trialCount)
            {
                carSpawner.start = false;
            }
            else
            {
                carSpawner.start = true; // start a new trial
            }
            
        }
    }
}
