using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class Txt_Output : MonoBehaviour
{
    string myFilePath;
	public string ParticipantId;
    public int age;
    public string gender;
    string date;
    int counter;
    int duplicate=1;

    Headset headSet;
    CarSpawner carSpawner;
	StreamWriter fileWriter;


    void Awake()
    {
        headSet = GameObject.Find("XR Origin (XR Rig)").GetComponent<Headset>();
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
    
    }

	private void Start()
	{

        date = DateTime.Now.ToString();


        //internal storage/android/data/yourApp/files/testFile.txt
        myFilePath = Application.dataPath + "/RawData/TXT_Format/" + ParticipantId + "_GC.txt";

		while(File.Exists(myFilePath))
        {
            myFilePath = Application.dataPath + "/RawData/TXT_Format/" + ParticipantId + "_GC_" + duplicate + ".txt";
            duplicate++;
        }              
                

        WriteToFile("Gap Cross Project\n" + "\nDate: "
        + date + "\nParticipant ID: " + ParticipantId + "\nAge: " + age 
        + "\nGender: " + gender + "\n\n\n");
	}

    
    private void OnApplicationQuit()
    {

        StringBuilder stringBuilder = new StringBuilder();

        foreach(float key in headSet.HeadRecord.Keys)
        {
            stringBuilder.Append(key.ToString("F4") + "\t"
            + headSet.HeadRecord[key][0].ToString("F0") + "\t"
            + headSet.HeadRecord[key][1].ToString("F0") + "\t"
            + headSet.HeadRecord[key][2].ToString("F3") + "\t"
            + headSet.HeadRecord[key][3].ToString("F3") + "\t"
            + headSet.HeadRecord[key][4].ToString("F3") + "\t"
            + headSet.HeadRecord[key][5].ToString("F3") + "\t"
            + headSet.HeadRecord[key][6].ToString("F3") + "\t"
            + headSet.HeadRecord[key][7].ToString("F3") + "\t\t\t"
            + "HeadRecord: Time | Status | Trial | X-Pos | Y-Pos | Z-Pos | X-Rot | Y-Rot | Z-Rot\n"
            );

            if(carSpawner.CarInfo.ContainsKey(key))
            {
                foreach(List<float> value in carSpawner.CarInfo[key])
                {
                    stringBuilder.Append(key.ToString("F4") + "\t"
                    + value[0].ToString("F0") + "\t"
                    + value[1].ToString("F0") + "\t"
                    + value[2].ToString("F4") + "\t"
                    + value[3].ToString("F4") + "\t\t"
                    + value[4].ToString("F1") + "\t\t"
                    + value[5].ToString("F0") + "\t"
                    + value[6].ToString("F0") + "\t\t\t\t"
                    + "CarRecord: Time | Status | Id | Size | X-Pos | Gap | IsPink | IsYellow\n"
                    );
                }
            }
        }

        File.AppendAllText(myFilePath, stringBuilder.ToString());


    }
    
    
    
    
    
    public void WriteToFile(string message)
    {
        try
        {
            fileWriter = new StreamWriter(myFilePath, true);
            fileWriter.Write(message);
            fileWriter.Close();
        }
        catch(System.Exception e)
        {
            Debug.LogError("Cannot write into the file");
        }
    }

}
