using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class Analysis_Output : MonoBehaviour
{
    string myFilePath;
	string ParticipantId;
    int age;
    string gender;
    string date;
    int duplicate=1;

    HeadMotionAnalysis headMotionAnalysis;
	StreamWriter fileWriter;
    CarSpawner carSpawner;
    Txt_Output txt_Output;

    void Awake()
    {
        headMotionAnalysis = GameObject.Find("XR Origin (XR Rig)").GetComponent<HeadMotionAnalysis>();
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
        txt_Output = GetComponent<Txt_Output>();

    }
    // Start is called before the first frame update
    void Start()
    {
        ParticipantId = txt_Output.ParticipantId;
        age = txt_Output.age;
        gender = txt_Output.gender;
        date = DateTime.Now.ToString();


        //internal storage/android/data/yourApp/files/testFile.txt
        myFilePath = Application.dataPath + "/RawData/TXT_Format/" + ParticipantId + "_GC_Analysis.txt";

		while(File.Exists(myFilePath))
        {
            myFilePath = Application.dataPath + "/RawData/TXT_Format/" + ParticipantId + "_GC_Analysis_" + duplicate + ".txt";
            duplicate++;
        }              
                

        WriteToFile("Gap Cross Project\n" + "\nDate: "
        + date + "\nParticipant ID: " + ParticipantId + "\nAge: " + age 
        + "\nGender: " + gender + "\n\n\n");
        
    }

    private void OnApplicationQuit()
    {

        StringBuilder stringBuilder = new StringBuilder();

        foreach(float key in headMotionAnalysis.HeadMotionCarStream.Keys)
        {
            stringBuilder.Append(
                "==================== Trial " + key + " ====================" + "\n" +
                "Enter Road (s): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][0].ToString("F4") + "\n" +
                "Ped X (m): " + "\t\t\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][1].ToString("F4") + "\n" +
                "Ped Z (m): " + "\t\t\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][2].ToString("F4") + "\n" +
                "Lead Car X (m): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][3].ToString("F4") + "\n" +
                "Lead Car Size (m): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][4].ToString("F4") + "\n" +
                "Tail Car X (m): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][5].ToString("F4") + "\n" +
                "Tail Car Size (m): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][6].ToString("F4") + "\n\n" +
                "Exit Road (s): " + "\t\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][8].ToString("F4") + "\n" +
                "Ped X (m): " + "\t\t\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][9].ToString("F4") + "\n" +
                "Ped Z (m): " + "\t\t\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][10].ToString("F4") + "\n" +
                "Lead Car X (m): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][11].ToString("F4") + "\n" +
                "Lead Car Size (m): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][12].ToString("F4") + "\n" +
                "Tail Car X (m): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][13].ToString("F4") + "\n" +
                "Tail Car Size (m): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][14].ToString("F4") + "\n\n" +
                "Crossing Time (s): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][16].ToString("F4") + "\n" +
                "Time of Entry (s): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][7].ToString("F4") + "\n" +
                "Time to Spare (s): " + "\t\t\t\t\t\t" + headMotionAnalysis.HeadMotionCarStream[key][15].ToString("F4") + "\n\n" +
                "Gaps Seen (Actual): " + "\t");


                for(int i = 0; i < headMotionAnalysis.GapsSeenActual[key].Count; i++)
                {
                    if(headMotionAnalysis.YellowLeadCar[key][i] == 1)
                    {
                       stringBuilder.Append(
                       headMotionAnalysis.GapsSeenActual[key][i].ToString("F4") + " *| "
                       ); 
                    }
                    else
                    {
                        stringBuilder.Append(
                        headMotionAnalysis.GapsSeenActual[key][i].ToString("F4") + " | "
                        );

                    }
                }
                stringBuilder.Append("\n" +
                "Gaps Seen (Rounded): " + "\t");


                for(int i = 0; i < headMotionAnalysis.GapsSeenRounded[key].Count; i++)
                {
                    if(headMotionAnalysis.YellowLeadCar[key][i] == 1)
                    {
                       stringBuilder.Append(
                       headMotionAnalysis.GapsSeenRounded[key][i].ToString("F1") + " *| "
                       ); 
                    }
                    else
                    {
                        stringBuilder.Append(
                        headMotionAnalysis.GapsSeenRounded[key][i].ToString("F1") + " | "
                        );

                    }
                }
                stringBuilder.Append("\n\n");

        }

        File.AppendAllText(myFilePath, stringBuilder.ToString());


    }

    void WriteToFile(string message)
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
