using UnityEngine;
using System.Collections;

public class SuperSecretQuit : MonoBehaviour {
	public GameObject[] planets;
	public bool showNumpad;
	public bool secretButton;
	public Material spacey;
	public Material spaceyBox;
	private string passcode;

	public GUIStyle clearStyle;
	//GUI Matrix
	private Vector3 scale;
	private float originalHeight = 1080.0f;
	private float originalWidth = 1920.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI()
	{
		scale = new Vector3(Screen.width/originalWidth,Screen.height/originalHeight,1);
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);
		if(GUI.Button(new Rect(0,0,300,300),"",clearStyle))
		{
			StartCoroutine("ButtonTimeOut");
			secretButton = true;
		}
		if(secretButton && GUI.Button(new Rect(1620,780,300,300),"",clearStyle))
		{
			showNumpad = true;
		}
		if(showNumpad)
			NumpadGUI();
		GUI.matrix = svMat;
	}
	void NumpadGUI()
	{
		if(GUI.Button(new Rect(10,325,50,50),"1"))
		{
			passcode += "1";
		}
		if(GUI.Button(new Rect(60,325,50,50),"2"))
		{
			passcode += "2";
		}
		if(GUI.Button(new Rect(110,325,50,50),"3"))
		{
			passcode += "3";
		}
		if(GUI.Button(new Rect(10,375,50,50),"4"))
		{
			passcode += "4";
		}
		if(GUI.Button(new Rect(60,375,50,50),"5"))
		{
			passcode += "5";
		}
		if(GUI.Button(new Rect(110,375,50,50),"6"))
		{
			passcode += "6";
		}
		if(GUI.Button(new Rect(10,425,50,50),"7"))
		{
			passcode += "7";
		}
		if(GUI.Button(new Rect(60,425,50,50),"8"))
		{
			passcode += "8";
		}
		if(GUI.Button(new Rect(110,425,50,50),"9"))
		{
			passcode += "9";
		}
		if(GUI.Button(new Rect(60,475,50,50),"0"))
		{
			passcode += "0";
		}
		if(GUI.Button(new Rect(10,475,50,50),"Clear"))
		{
			showNumpad = false;
			passcode = "";
		}
		if(GUI.Button(new Rect(110,475,50,50),"Enter"))
		{
			if(passcode == "5111149")
			{
				RenderSettings.skybox = spaceyBox;
				//System.Diagnostics.Process.Start("explorer.exe");
				foreach(GameObject p in planets)
				{
					Debug.Log("WORKING");
					p.transform.GetComponent<Renderer>().material = spacey;
				}
			}
			else
			{
				showNumpad = false;
				passcode = "";
			}
		}
	}
	IEnumerator ButtonTimeOut()
	{
		yield return new WaitForSeconds (1.0f);
		secretButton = false;
		StopCoroutine ("ButtonTimeOut");
	}

}
