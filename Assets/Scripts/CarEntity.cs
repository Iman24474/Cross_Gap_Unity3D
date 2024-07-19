using UnityEngine;
using System.Collections;

/*
 * This object is just a simple car going in a straight line.
 */
public class CarEntity : MonoBehaviour  {
    public float carSize = 0;
    public Vector3 startPos;
	public Vector3 endPos;
	public float velocity = 11.176f;  // m/s = 25 mph
	//public static Dictionary<string, int> entNameToNum;
	public int entityIDCounter = -1;
	public int entityID = 0;
    //[HideInInspector] 
    public bool isActive = false;
    public float travelTime = 0;

}
