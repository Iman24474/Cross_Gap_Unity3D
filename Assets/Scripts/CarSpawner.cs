using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{

    public Color[] colors;
    public float[] gaps;
    public GameObject[] carPool;
    public int trialCount;
    public float carCreationDelay;
    float customTime;
    float timeReset;
    GameObject head;
    [HideInInspector] public float delay;
    [HideInInspector] public bool firstCar = true;
    [HideInInspector] public bool pause;
    [HideInInspector] public bool isPink;
    [HideInInspector] public bool isYellow;
    [HideInInspector] public float headZ;
    [HideInInspector] public float waitTime;
    [HideInInspector] public float elapsedTime;
    [HideInInspector] public bool start = false;
    [HideInInspector] public GameObject pinkCar;
    [HideInInspector] public Dictionary<float, List<List<float>>> CarInfo = new Dictionary<float, List<List<float>>>();
    


    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.Find("Main Camera");
        delay = carCreationDelay;

    }

    // Update is called once per frame
    void Update()
    {
        headZ = head.transform.position.z;
        customTime += Time.deltaTime;
        delay -= Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // Check for 'S' key press or new trial state
        if((Input.GetKeyDown(KeyCode.S) || start) && !pause)
        {
            start = true;
            // Create a car if the delay has elapsed and the head is in the correct position
            if(delay <= 0 && headZ <= 0)
            {
                CarCreation();
            }          

        }

    }

    


    void CarCreation()
    {
        // Randomly select a prefab from the pool
        GameObject selectedPrefab = carPool[Random.Range(0, carPool.Length)];

        if(firstCar)
        {
            // Instantiate the first car
            pinkCar = Instantiate(selectedPrefab, new Vector3(-125f, 0, 0), Quaternion.Euler(-90, 90, 0));
            // Set the material color to magenta based on the car tag
            if(selectedPrefab.tag == "VW")
            {
                pinkCar.GetComponent<MeshRenderer>().materials[1].color = Color.magenta;
            }

            else
            {
                pinkCar.GetComponent<MeshRenderer>().materials[0].color = Color.magenta;
            }            

            // Resest the customtimer
            timeReset = customTime;
            customTime -= timeReset;

            // Set the wait time for the next car
            waitTime = gaps[Random.Range(0, gaps.Length)];

            // Update the trial state
            firstCar = false;

            // Increment the car entity ID
            selectedPrefab.GetComponent<CarEntity>().entityID++;
            //Set the intantiated car's tag to "ClonedCar"
            pinkCar.tag = "ClonedCar";

        }

        else if(customTime >= waitTime)
        {
            GameObject obj = Instantiate(selectedPrefab, new Vector3(-125f, 0, 0), Quaternion.Euler(-90, 90, 0));

            int index = Random.Range(0, colors.Length);
            if(selectedPrefab.tag == "VW")
            {
                obj.GetComponent<MeshRenderer>().materials[1].color = colors[index];
                if(index == 5)
                {
                    obj.GetComponent<MeshRenderer>().materials[1].color = Color.yellow;
                }
            }

            else
            {
                obj.GetComponent<MeshRenderer>().materials[0].color = colors[index];
                if(index == 5)
                {
                    obj.GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
                }
            }  

            // Resest the customTimer
            timeReset = customTime;
            customTime -= timeReset;

            // if color of the car is yellow
            if(index == 5)
            {
                // Select a gap between 3.5 to 5.0 seconds
                waitTime = gaps[Random.Range(4, gaps.Length)];
            }

            else
            {
                // Select the next gap size between 1.5 to 5.0 seconds
                waitTime = gaps[Random.Range(0, gaps.Length)];
            }

            // Increment the car entity ID
            selectedPrefab.GetComponent<CarEntity>().entityID++;  
            //Set the intantiated car's tag to "ClonedCar" 
            obj.tag = "ClonedCar";    
        } 
       
    }




}


