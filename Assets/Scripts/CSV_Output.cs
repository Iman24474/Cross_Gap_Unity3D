// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using UnityEngine;

// public class CSV_Output : MonoBehaviour
// {
//     string myFilePath;
//     string path;
// 	string ParticipantId;
//     int age;
//     string gender;
//     string date;
//     int counter;
//     int duplicate =1;

//     Headset headSet;
//     Txt_Output txt_Output;
//     CarSpawner carEntitySpawner;
// 	StreamWriter fileWriter;


//     void Awake()
//     {
//         headSet = GameObject.Find("XR Origin (XR Rig)").GetComponent<Headset>();
//         carEntitySpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
//         txt_Output = GetComponent<Txt_Output>();
//     }

// 	private void Start()
// 	{
        
//         ParticipantId = txt_Output.ParticipantId;
//         age = txt_Output.age;
//         gender = txt_Output.gender;
//         date = DateTime.Now.ToString();


// 		//internal storage/android/data/yourApp/files/testFile.txt
//         myFilePath = Application.dataPath + "/RawData/CSV_Format/" + ParticipantId + "_GJ.csv";

// 		while(File.Exists(myFilePath))
//         {
//             myFilePath = Application.dataPath + "/RawData/CSV_Format/" + ParticipantId + "_GJ_" + duplicate + ".csv";
//             duplicate++;
//         }  


//         WriteToFile("Date" + "," + "Participant ID" + "," + "Age" + "," + "Gender" + "," + "Time" + "," + "Head_X_Pos" + "," + "Head_Y_Pos" + "," 
//         + "Head_Z_Pos" + "," + "Head_X_Rot" + "," + "Head_Y_Rot" + "," + "Head_Z_Rot" + "," + "CarCreationTime" + "," +
//         "CarId" + "," + "CarSize" + "," + "Car_X_Pos" + "," + "Car_Y_Pos" + "," + "Car_Z_Pos" + "," + "GapSize\n");
// 	}

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     private void OnApplicationQuit()
//     {
//         counter = headSet.HeadXPos.Count;
//         StringBuilder stringbuilder = new StringBuilder();
//         for (int i = 0; i < counter; i++)
//         {
//             stringbuilder.Append(date + "," + ParticipantId + "," + age + "," + gender + "," +  headSet.TotalTime[i].ToString("F4") 
//                 + "," + headSet.HeadXPos[i].ToString("F4") + "," + headSet.HeadYPos[i].ToString("F4") + "," + headSet.HeadZPos[i].ToString("F4") 
//                 + "," + headSet.HeadXRot[i].ToString("F4") + "," + headSet.HeadYRot[i].ToString("F4") + "," + headSet.HeadZRot[i].ToString("F4") 
//                 + "," + carEntitySpawner.carStatus[i].ToString("F4") + "," + carEntitySpawner.carEntityId[i].ToString("F4") 
//                 + "," + carEntitySpawner.carSize[i].ToString("F4") + "," + carEntitySpawner.carPosition[i].x.ToString("F4")
//                 + "," + carEntitySpawner.carPosition[i].y.ToString("F4") + "," + carEntitySpawner.carPosition[i].z.ToString("F4")
//                 + "," + carEntitySpawner.gapTime[i].ToString("F4") + "\n");
//         }
//         path = myFilePath;
//         File.AppendAllText(path, stringbuilder.ToString());
//     }

//     public void WriteToFile(string message)
//     {
//         try
//         {
//             fileWriter = new StreamWriter(myFilePath, true);
//             fileWriter.Write(message);
//             fileWriter.Close();
//         }
//         catch(System.Exception e)
//         {
//             Debug.LogError("Cannot write into the file");
//         }
//     }

// }
