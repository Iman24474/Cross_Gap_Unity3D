using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Headset : MonoBehaviour
{
    CarSpawner carSpawner;
    TrialTracker trialTracker;
    const float headStatus = 0;
    [HideInInspector] public Dictionary<float, List<float>> HeadRecord = new Dictionary<float, List<float>>();

    void Start() 
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
        trialTracker = GameObject.Find("TriggerZone").GetComponent<TrialTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        // Head data recording
        HeadRecord.Add(carSpawner.elapsedTime, new List<float>()
        {
            headStatus, // 0
            trialTracker.trialNum, // trial number
            Camera.main.transform.position.x, // X-Pos
            Camera.main.transform.position.y, // Y-Pos
            Camera.main.transform.position.z, // Z-Pos
            Camera.main.transform.eulerAngles.x, // X-Rot
            Camera.main.transform.eulerAngles.y, // Y-Rot
            Camera.main.transform.eulerAngles.z // Z-Rot
        });

    }

    
}
