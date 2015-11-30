using UnityEngine;
using System.Collections;

public class PlanetControls : MonoBehaviour {

	public float cameraSpeed;
	public float cameraStartDist;
	public float cameraCurDist;
	public float orbitSpeed;
	public bool dragging;
	public bool placed;
	public bool showInfo;
	public GameObject sun;
	public AudioSource click;
	private Vector3 offset;
	private Vector3 screenPoint;
	private Vector3 origScale;
	//protected Animator animator = Camera.main.transform.GetComponent<GameController>().astronaut.Animator;



	void Start()
	{
		click = this.GetComponent<AudioSource> ();
		showInfo = false;
		origScale = transform.localScale;
		placed = false;
		dragging = false;
		//animator = Camera.main.transform.GetComponent<GameController>().astronaut.Animator;
	}
	//transform.Rotate(Vector3.up * Time.deltaTime * 50);
	void OnMouseUp()
	{
		Metrics.buttons++;
		//Debug.Log(Vector3.Distance(transform.position,sun.transform.position));
		if (!placed && tag == "Planet" && !GameController.mainMenu && !GameController.cameraMoving && !GameController.showInfo) 
		{
			click.Play();
			showInfo = true;
			GameController.showInfo = true;
			GameController.clicked = true;
			GameController.planetName = transform.name;
			GameController.newCameraPosition = transform.position + (new Vector3 (0, -(transform.localScale.x), 0));
			GameController.cameraCurDist = Vector3.Distance(Camera.main.transform.position, GameController.newCameraPosition);
			cameraStartDist = Vector3.Distance (Camera.main.transform.position, GameController.newCameraPosition) * 15;
			Destroy (this.gameObject.GetComponent<PlanetControls> ());
			
		}
		else if(GameController.mainMenu && !placed && tag == "Planet" && !GameController.cameraMoving)
		{
			click.Play();
			Camera.main.transform.GetComponent<GameController>().music[0].Stop();
			Camera.main.transform.GetComponent<GameController>().music[1].Play();
			Debug.Log(System.DateTime.Now.ToString("HHmmss"));
			Metrics.timeStart = System.DateTime.Now.ToString("HHmmss");
			//Camera.main.transform.GetComponent<GameController>().astronaut.GetComponent<Animator>().SetBool("MainMenu",false);
			GameController.mainMenu = false;
			GameController.newCameraPosition = GameController.cameraOrigin;
			GameController.cameraCurDist = Vector3.Distance(Camera.main.transform.position, GameController.cameraOrigin);
		}
//		else if(transform.parent.tag == "Planet")
//		{
//			showInfo = true;
//			GameController.showInfo = true;
//			GameController.clicked = true;
//			GameController.planetName = transform.parent.name;
//			GameController.newCameraPosition = transform.parent.position + (new Vector3 (0, -(transform.parent.localScale.x), 0));
//			cameraStartDist = Vector3.Distance (Camera.main.transform.position, GameController.newCameraPosition) * 15;
//			Destroy (this.transform.parent.gameObject.GetComponent<PlanetControls> ());
//		}
		//dragging = false;
	}
}
