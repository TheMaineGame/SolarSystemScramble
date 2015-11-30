using UnityEngine;
using System.Collections;

public class Metrics : MonoBehaviour {

	public static string timeStart;
	public static string timeEnd;
	public static int touches;
	public static int buttons;
	public static int wrong;
	public static string win;

	// Use this for initialization
	void Start () {
		timeStart = "";
		timeEnd = "";
		touches = 0;
		buttons = 0;
		wrong = 0;
		win = "False";
	}
	public static void OutputMetrics()
	{
		string line = timeStart + "," + timeEnd + "," + touches + "," + buttons + "," + wrong + "," + win;
		string fileName = "/SSS " + System.DateTime.Now.ToString("MM-dd-yyyy") + ".csv";
		if(!System.IO.File.Exists(Application.persistentDataPath + fileName))
		{
			System.IO.File.Create(Application.persistentDataPath + fileName);
		}
		System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + fileName, true);
		file.WriteLine (line);
		file.Close ();
		//GoogleAnalytics.instance.LogScreen(
	}
}
