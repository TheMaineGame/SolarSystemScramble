using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class GameController : MonoBehaviour {
	
	// These bools take away the option to pick a planet as a possible planet.
	private bool mercuryPlaced, venusPlaced, earthPlaced, marsPlaced, jupiterPlaced, saturnPlaced, uranusPlaced, neptunePlaced;
	private bool mercuryWrong, venusWrong, earthWrong, marsWrong, jupiterWrong, saturnWrong, uranusWrong, neptuneWrong;
	
	public int zoomNum;
	public int sndNum;
	public int counter = 0;
	public int discriptionNum;
	
	public float cameraSpeed;
	public float cameraStartDist;
	public static float cameraCurDist;
	public float tipSize;
	public float xRotation;
	public float menuAstroY;//Y position of main menu astronaut
	
	public bool winSound;
	public bool incorrect;
	public static bool correctPopUp;
	public bool zoomIn;
	public bool tip;
	public static bool mainMenu = true;
	public static bool cameraMoving = false;
	public static int planetsPlaced = 0;
	public static bool showInfo = false;
	public static bool clicked = false;
	public static bool planetAlreadyPicked;
	
	public static Vector3 cameraOrigin = new Vector3 (481, -200, 14);
	public Vector3 cameraEndPos;
	public Vector3 cameraEndRot;
	public static Vector3 newCameraPosition;
	
	public List<GameObject> planets = new List<GameObject> ();
	public GameObject menuPlanets;
	public GameObject frames;
	public GameObject winEmit;
	public GameObject astronaut;
	
	public static string planetName;
	public string planetPicked;
	public string[] planetDiscription;
	public string[] planetBigDescriptions;
	public string line;
	
	public Material[] planetMaterial;
	
	public Texture2D[] sndButtons;
	public Texture2D[] buttonTexs;
	public Texture2D tipBox;
	public Texture2D mainMenuTex;
	public Texture2D solarSystemTexture;
	public Texture2D wrongTexture;
	
	public TextAsset descriptionFile;
	public TextAsset bigDescriptionFile;
	
	public AudioSource[] music;
	public AudioSource[] sounds;
	
	public Camera backCam;
	
	//GUI Vars
	public GUIStyle winStyle;
	public GUIStyle zoomStyle;
	public GUIStyle aStyle;
	public GUIStyle magStyle;
	public GUIStyle correctStyle;
	public GUISkin[] skin;
	public Vector2 scrollPosition = Vector2.zero;
	
	//GUI Matrix
	private Vector3 scale;
	private float originalHeight = 1000.0f;
	private float originalWidth = 1000.0f;
	
	public GameObject MercuryMenuPlanet;
	public GameObject VenusMenuPlanet;
	public GameObject EarthMenuPlanet;
	public GameObject MarsMenuPlanet;
	public GameObject JupiterMenuPlanet;
	public GameObject SaturnMenuPlanet;
	public GameObject UranusMenuPlanet;
	public GameObject NeptuneMenuPlanet;
	
	public GameObject ZoomedOutQuad;
	
	
	// Use this for initialization
	void Start () 
	{
		planetsPlaced = 0;
		AssignObjects();
		music[0].Play();
		cameraOrigin = new Vector3 (481, -200, 14);
		showInfo = false;
		clicked = false;
		cameraMoving = false;
//		planetsPlaced = 0;
		xRotation = transform.eulerAngles.x;
		zoomIn = false;
		zoomNum = 0;
		tip = true;
		tipSize = 20;
		mainMenu = true;
		line = descriptionFile.text;
		planetDiscription = line.Split ("," [0]);
		line = bigDescriptionFile.text;
		planetBigDescriptions = line.Split ("-" [0]);
		
		//		System.IO.StreamReader file = new System.IO.StreamReader (Application.dataPath + "/Resources/PlanetDiscriptions.txt",System.Text.Encoding.Default);
		//		while ((line = file.ReadLine()) != null)
		//		{
		//			UnityEngine.Debug.Log(line);
		//			planetDiscription[counter] = line;
		//			counter++;
		//		}
		//		file.Close ();
	}
	
	// Update is called once per frame
	void Update()
	{
		//		Camera.main.fieldOfView = 60 * (16/9f) / ((float)Camera.main.pixelWidth / Camera.main.pixelHeight);
		//REMOVE!!!!!!!!
//		if(Input.GetKeyUp(KeyCode.W))
//		{
//			planetsPlaced = 7;
//		}
//		if(Input.GetKeyUp(KeyCode.D))
//		{
//			Application.LoadLevel(0);
//		}
		//REMOVE!!!!!!!!
		if(!AudioListener.pause)
			sndNum = 0;
		else
			sndNum = 1;
		
		if(tipSize < 200)
		{
			tipSize += (300 - tipSize) * 0.5f * Time.deltaTime;
		}
		
		if(zoomIn && !showInfo)
			Zoom ();

		if(Input.GetMouseButtonDown(0))
		{
			Metrics.touches++;
		}

		if(clicked)
			StartCoroutine("SpawnPlanets");
		cameraSpeed = cameraCurDist;
		if(showInfo)
		{
			ZoomedOutQuad.SetActive(false);
			transform.position = Vector3.MoveTowards(transform.position, newCameraPosition,cameraSpeed * Time.deltaTime);
			astronaut.transform.position = Vector3.MoveTowards(astronaut.transform.position, newCameraPosition,cameraSpeed * Time.deltaTime);
			if(transform.position != newCameraPosition)
			{
				if(!sounds[2].isPlaying)
					sounds[2].Play();
				cameraMoving = true;
			}
			else
				cameraMoving = false;
		}
		else if(planetsPlaced == 8 && !cameraMoving && !correctPopUp)
		{
			zoomIn = false;
			transform.position = Vector3.MoveTowards(transform.position, cameraEndPos,cameraSpeed * Time.deltaTime);
			if(transform.localEulerAngles.x >= 270)
			{
				xRotation += 50 * Time.deltaTime;
			}
			transform.eulerAngles = new Vector3(xRotation,0,0);
		}
		else if(!zoomIn && !mainMenu)
		{
			transform.position = Vector3.MoveTowards(transform.position, cameraOrigin,cameraSpeed * Time.deltaTime);
			if(transform.position != cameraOrigin)
			{
				if(!sounds[2].isPlaying)
					sounds[2].Play(); 
				cameraMoving = true;
			}
			else
				cameraMoving = false;
		}
	}
	
	void AssignObjects()
	{
		if(ZoomedOutQuad == null)
		{
			ZoomedOutQuad = GameObject.FindGameObjectWithTag("ZoomedOutQuad");
			ZoomedOutQuad.SetActive(false);
		}
	}
	
	void OnGUI()
	{
		//camera.ScreenToViewportPoint
		scale = new Vector3(Camera.main.pixelWidth/originalWidth,Camera.main.pixelHeight/originalHeight,1);
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(new Vector3((backCam.pixelWidth-Camera.main.pixelWidth)/2,(backCam.pixelHeight-Camera.main.pixelHeight)/2,0),Quaternion.identity,scale);
		if(GUI.Button(new Rect(0,890,100,100),sndButtons[sndNum],magStyle))
		{
			if(AudioListener.pause)
				AudioListener.pause = false;
			else
				AudioListener.pause = true;
		}
		if(planetsPlaced == 8 && !correctPopUp)
			WinScreen();
		else
		{
			if(mainMenu)
				GUI.DrawTexture(new Rect(0,0,1000,1000),mainMenuTex);
			if(showInfo && !clicked)
				Info();
			if (!showInfo && !cameraMoving && !mainMenu) 
			{
//				GUI.DrawTexture (new Rect (835, 675, 150, 300), buttonTexs[2]);
				if (GUI.Button (new Rect (835, 675, 150, 300),"",winStyle)) 
				{
					Metrics.buttons++;
					Metrics.win = "False";
					Metrics.timeEnd = System.DateTime.Now.ToString("HHmmss");
					Metrics.OutputMetrics();
					sounds[4].Play ();
					//Process.Start ("SpaceBase.exe");
					Application.Quit ();
				}
//				if(tip)
//					GUI.DrawTexture(new Rect(50,790,200,125),tipBox);
				if(zoomIn == false)
				{
					if(GUI.Button(new Rect(112,320,250,250),"",magStyle))
					{
						Metrics.buttons++;
						sounds[4].Play();
						if(!zoomIn)
						{
							tip = false;
							zoomIn = true;
							zoomNum = 1;
							newCameraPosition = new Vector3 (368, -44, 0);
							cameraCurDist = Vector3.Distance(transform.position, newCameraPosition);
							ZoomedOutQuad.SetActive(true);
							//cameraCurDist = Vector3.Distance(transform.position, newCameraPosition);
						}
						else
						{
							zoomIn = false;
							zoomNum = 0;
							cameraCurDist = Vector3.Distance(transform.position, cameraOrigin);
						}
					}
				}
			}
			if(incorrect && !cameraMoving)
				DisplayIncorrect();
		}
		GUI.matrix = svMat;
	}
	
	void WinScreen()
	{
		winEmit.SetActive (true);
		GUI.DrawTexture (new Rect (50, 0, 900, 400), buttonTexs[4]);
		//		GUI.DrawTexture (new Rect (15, 675, 150, 300), buttonTexs[3]);
		if(GUI.Button (new Rect (115, 675, 140, 300),"",winStyle))
		{
			Metrics.buttons++;
			Application.LoadLevel(0);
			sounds[4].Play();
		}
//		GUI.DrawTexture (new Rect (835, 675,150, 300), buttonTexs[2]);
		if(GUI.Button (new Rect (835, 675, 150, 300),"",winStyle))
		{
			Metrics.buttons++;
			sounds[4].Play();
			//Process.Start("SpaceBase.exe");
			Application.Quit();
		}
	}
	void Info()
	{
		switch(planetName)
		{
		case "Mercury":
			discriptionNum = 0;
			break;
		case "Venus":
			discriptionNum = 1;
			break;
		case "Earth":
			discriptionNum = 2;
			break;
		case "Mars":
			discriptionNum = 3;
			break;
		case "Jupiter":
			discriptionNum = 4;
			break;
		case "Saturn":
			discriptionNum = 5;
			break;
		case "Uranus":
			discriptionNum = 6;
			break;
		case "Neptune":
			discriptionNum = 7;
			break;
		}
		GUI.skin = skin[0];
//		GUI.Label (new Rect (20, 26, 263, 490), planetDiscription[discriptionNum],zoomStyle);
		if(!correctPopUp)
		{
			//GUI.skin = skin[0];
			GUI.Label (new Rect (20, 26, 263, 490), planetDiscription[discriptionNum],zoomStyle);
			if(GUI.Button(new Rect(400,900,200,100),"",aStyle))
			{
				GameObject.Find(planetName).AddComponent<PlanetControls>();
				GameObject.Find (planetName).GetComponent<AudioSource>().Play ();
				menuPlanets.SetActive(false);
				frames.SetActive(false);
				showInfo = false;
				mercuryWrong = false; venusWrong = false; earthWrong = false; marsWrong = false; jupiterWrong = false; saturnWrong = false; uranusWrong = false; neptuneWrong = false;
				if(zoomIn)
				{
					newCameraPosition = new Vector3 (368, -44, 0);
					cameraCurDist = Vector3.Distance(transform.position, newCameraPosition);
				}
			}
			GUI.Label (new Rect(400,950,200,50),"Go Back");
			GUI.DrawTexture(new Rect(405,905,180,50),solarSystemTexture);
		}
		//GUI.Box (new Rect (700, 0, 300, 1000),"");
		if(mercuryPlaced == false)
		{
			if(mercuryWrong)
			{
				GUI.DrawTexture(new Rect (700,0,150,260), wrongTexture);
			}
			else if(GUI.Button (new Rect (700, 160, 150, 100), "Mercury") || GUI.Button(new Rect(700,0,150,160),"",zoomStyle))
			{
				Metrics.buttons++;
				if(planetName == "Mercury")
				{
					PlanetButton(0);
					mercuryPlaced = true;
					MercuryMenuPlanet.SetActive(false);
					GameObject.FindGameObjectWithTag("MercuryMenuPlanet").SetActive(false);
				}
				else if(!correctPopUp)
				{
					Metrics.wrong++;
					mercuryWrong = true;
					if(!sounds[1].isPlaying)
						sounds[1].Play();
					planetPicked = "Mercury";
					incorrect = true;
					StartCoroutine("Incorrect");
				}
			}
		}
		
		if(venusPlaced == false)
		{
			if(venusWrong)
			{
				GUI.DrawTexture(new Rect (850, 0, 150, 260), wrongTexture);
			}
			else if(GUI.Button (new Rect (855, 160, 140, 90), "Venus") || GUI.Button(new Rect(850,0,150,160),"",zoomStyle))
			{
				Metrics.buttons++;
				if(planetName == "Venus")
				{
					PlanetButton(1);
					venusPlaced = true;
					VenusMenuPlanet.SetActive(false);
					GameObject.FindGameObjectWithTag("VenusMenuPlanet").SetActive(false);
				}
				else if(!correctPopUp)
				{
					Metrics.wrong++;
					venusWrong = true;
					if(!sounds[1].isPlaying)
						sounds[1].Play();
					planetPicked = "Venus";
					incorrect = true;
					StartCoroutine("Incorrect");
				}
				
			}
		}
		
		if(earthPlaced == false)
		{
			if(earthWrong)
			{
				GUI.DrawTexture(new Rect (700,260,150,260), wrongTexture);
			}
			else if(GUI.Button (new Rect (700, 405, 150, 100), "Earth") || GUI.Button (new Rect (700,260,150,145),"",zoomStyle))
			{
				Metrics.buttons++;
				if(planetName == "Earth")
				{
					PlanetButton(2);
					earthPlaced = true;
					EarthMenuPlanet.SetActive(false);
					GameObject.FindGameObjectWithTag("EarthMenuPlanet").SetActive(false);
				}
				else if(!correctPopUp)
				{
					Metrics.wrong++;
					earthWrong = true;
					if(!sounds[1].isPlaying)
						sounds[1].Play();
					planetPicked = "Earth";
					incorrect = true;
					StartCoroutine("Incorrect");
				}
				
			}
		}
		
		if(marsPlaced == false)
		{
			if(marsWrong)
			{
				GUI.DrawTexture(new Rect (850,260,150,260), wrongTexture);
			}
			else if(GUI.Button (new Rect (850, 405, 150, 100), "Mars") || GUI.Button (new Rect (850,260,150,145),"",zoomStyle))
			{
				Metrics.buttons++;
				if(planetName == "Mars")
				{
					PlanetButton(3);
					marsPlaced = true;
					MarsMenuPlanet.SetActive(false);
					GameObject.FindGameObjectWithTag("MarsMenuPlanet").SetActive(false);
				}
				else if(!correctPopUp)
				{
					Metrics.wrong++;
					marsWrong = true;
					if(!sounds[1].isPlaying)
						sounds[1].Play();
					planetPicked = "Mars";
					incorrect = true;
					StartCoroutine("Incorrect");
				}
				
			}
		}
		
		if(jupiterPlaced == false)
		{
			if(jupiterWrong)
			{
				GUI.DrawTexture(new Rect (700,505,150,260), wrongTexture);
			}
			else if(GUI.Button (new Rect (700, 645, 150, 100), "Jupiter") || GUI.Button (new Rect (700,505,150,140),"",zoomStyle))
			{
				Metrics.buttons++;
				if(planetName == "Jupiter")
				{
					PlanetButton(4);
					jupiterPlaced = true;
					JupiterMenuPlanet.SetActive(false);
					GameObject.FindGameObjectWithTag("JupiterMenuPlanet").SetActive(false);
				}
				else if(!correctPopUp)
				{
					Metrics.wrong++;
					jupiterWrong = true;
					if(!sounds[1].isPlaying)
						sounds[1].Play();
					planetPicked = "Jupiter";
					incorrect = true;
					StartCoroutine("Incorrect");
					
				}
				
			}
		}
		
		if(saturnPlaced == false)
		{
			if(saturnWrong)
			{
				GUI.DrawTexture(new Rect (850,505,150,260), wrongTexture);
			}
			else if(GUI.Button (new Rect (850, 645, 150, 100), "Saturn") || GUI.Button (new Rect (850,505,150,140),"",zoomStyle))
			{
				Metrics.buttons++;
				if(planetName == "Saturn")
				{
					PlanetButton(5);
					saturnPlaced = true;
					SaturnMenuPlanet.SetActive(false);
					GameObject.FindGameObjectWithTag("SaturnMenuPlanet").SetActive(false);
				}
				else if(!correctPopUp)
				{
					Metrics.wrong++;
					saturnWrong = true;
					if(!sounds[1].isPlaying)
						sounds[1].Play();
					planetPicked = "Saturn";
					incorrect = true;
					StartCoroutine("Incorrect");
				}
				
			}
		}
		
		if(uranusPlaced == false)
		{
			if(uranusWrong)
			{
				GUI.DrawTexture(new Rect (700,745,150,260), wrongTexture);
			}
			else if(GUI.Button (new Rect (700, 895, 150, 100), "Uranus") || GUI.Button (new Rect (700,745,150,150),"",zoomStyle))
			{
				Metrics.buttons++;
				if(planetName == "Uranus")
				{
					PlanetButton(6);
					uranusPlaced = true;
					UranusMenuPlanet.SetActive(false);
					GameObject.FindGameObjectWithTag("UranusMenuPlanet").SetActive(false);
				}
				else if(!correctPopUp)
				{
					Metrics.wrong++;
					uranusWrong = true;
					if(!sounds[1].isPlaying)
						sounds[1].Play();
					planetPicked = "Uranus";
					incorrect = true;
					StartCoroutine("Incorrect");
				}
			}
		}
		
		if(neptunePlaced == false)
		{
			if(neptuneWrong)
			{
				GUI.DrawTexture(new Rect (850,745,150,260), wrongTexture);
			}
			else if(GUI.Button (new Rect (850, 895, 150, 100), "Neptune") || GUI.Button (new Rect (850,745,150,150),"",zoomStyle))
			{
				Metrics.buttons++;
				if(planetName == "Neptune")
				{
					PlanetButton(7);
					neptunePlaced = true;
					NeptuneMenuPlanet.SetActive(false);
					GameObject.FindGameObjectWithTag("NeptuneMenuPlanet").SetActive(false);
				}
				else if(!correctPopUp) 
				{
					Metrics.wrong++;
					neptuneWrong = true;
					if(!sounds[1].isPlaying)
						sounds[1].Play();
					planetPicked = "Neptune";
					incorrect = true;
					StartCoroutine("Incorrect");
				}
				
			}
		}
		if(correctPopUp)
		{
			//GUI.skin = skin[0];
			GUI.Label (new Rect (20, -50, 263, 490), planetBigDescriptions[discriptionNum],zoomStyle);
			GUI.Box (new Rect(305,300,390,300),"<size=64>Good Job</size>",correctStyle);
			if(GUI.Button(new Rect(450,530,100,50),"Okay",correctStyle))
			{
				menuPlanets.SetActive(false);
				frames.SetActive(false);
				showInfo = false;
				mercuryWrong = false; venusWrong = false; earthWrong = false; marsWrong = false; jupiterWrong = false; saturnWrong = false; uranusWrong = false; neptuneWrong = false;
				if(zoomIn)
				{
					newCameraPosition = new Vector3 (368, -44, 0);
					cameraCurDist = Vector3.Distance(transform.position, newCameraPosition);
				}
				if(planetsPlaced == 8)
				{
					Metrics.win = "True";
					Metrics.timeEnd = System.DateTime.Now.ToString("HHmmss");

					Metrics.OutputMetrics();

					sounds[0].Stop();
					cameraCurDist = Vector3.Distance(transform.position, cameraEndPos) * 2;
					sounds[3].Play();
				}
				else if(zoomIn)
				{
					newCameraPosition = new Vector3 (368, -44, 0);
					cameraCurDist = Vector3.Distance(transform.position, newCameraPosition);
				}
				correctPopUp = false;
			}
		}
	}
	void SpawnPlanets()
	{
		if(mainMenu)
			mainMenu = false;
		if(transform.position == newCameraPosition)
		{
			menuPlanets.SetActive(true);
			frames.SetActive(true);
			clicked = false;
			//			for(int i = 0; i < 8; i++ )
			//			{	
			//				if(i < 4)
			//					Instantiate(planets[i],Camera.main.ScreenToWorldPoint(new Vector3(775,250*i,3)),planets[i].transform.rotation);
			//				else
			//					Instantiate(planets[i],Camera.main.ScreenToWorldPoint(new Vector3(925,250*(i-4),3)),planets[i].transform.rotation);
			//				if(i == 7)
			//					clicked = false;
			//			}
		}
		if(!clicked)
			StopCoroutine("SpawnPlanets");
	}
	int CreatePlanet (GameObject planet, int planetNum)
	{
		if(planet.transform.GetComponent<Renderer>().material = planetMaterial[planetNum]);
		if(planetNum == 5)
			GameObject.Find (planetName).transform.FindChild("Rings").GetComponent<Renderer>().material = planetMaterial[planetNum+4];
		planet.GetComponent<Orbit> ().enabled = true;
		Orbit ob = (Orbit)planet.GetComponent ("Orbit");
		ob.origScale = planet.transform.localScale;
		//planet.transform.localScale = (new Vector3(0,0,0));
		return 0;
	}
	int DisplayIncorrect ()
	{
		GUI.Box (new Rect (300, 400, 400, 200),"This planet is not " + planetPicked);
		return 0;
	}
	IEnumerator Incorrect()
	{
		yield return new WaitForSeconds (2.0f);
		incorrect = false;
		StopCoroutine ("Incorrect");
	}
	void Zoom()
	{
		ZoomedOutQuad.SetActive(true);
		transform.position = Vector3.MoveTowards(transform.position, newCameraPosition,cameraSpeed * Time.deltaTime);
		if(transform.position != newCameraPosition)
		{
			if(!sounds[2].isPlaying)
				sounds[2].Play(); 
			cameraMoving = true;
		}
		else
		{
			cameraMoving = false;
		}
	}
	void PlanetButton(int plntNum)
	{
		incorrect = false;
		StopCoroutine ("Incorrect");
		if(!sounds[0].isPlaying)
			sounds[0].Play();sounds[1].Stop();
		GameObject thisPlanet = GameObject.Find(planetName);
		CreatePlanet(thisPlanet, plntNum);
		//thisPlanet.AddComponent("PlanetControls");
		planetsPlaced += 1;
		//menuPlanets.SetActive(false);
		correctPopUp = true;
		//showInfo = false;
//		if(planetsPlaced == 8)
//		{
//			Metrics.win = "True";
//			Metrics.timeEnd = System.DateTime.Now.ToString("HHmmss");
//			Metrics.OutputMetrics();
//			sounds[0].Stop();
//			cameraCurDist = Vector3.Distance(transform.position, cameraEndPos) * 2;
//			sounds[3].Play();
//		}
		//		else if(zoomIn)
		//		{
		//			newCameraPosition = new Vector3 (368, -44, 0);
		//			cameraCurDist = Vector3.Distance(transform.position, newCameraPosition);
		//		}
	}
	Vector2 VP(float pointX, float pointY)
	{
		Vector3 viewPoint = Camera.main.ScreenToViewportPoint(new Vector3(pointX,pointY,0));
		Vector2 returnPoint = new Vector2(viewPoint.x,viewPoint.y);
		return returnPoint;
	}
}
