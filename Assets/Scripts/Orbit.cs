using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {
	public float orbitSpeed;
	public GameObject sun;
//	public GameObject[] wayPoint;
//	public int nextPos;
//	public int lastPos;
	public float centerX;
	public float centerY;
	public float newX;
	public float newY;
	public float xAxis;
	public float yAxis; 
	public float orbitPos;
	public Vector3 origScale;
	private Vector3 planetScale;
	// Use this for initialization
	void Start () {
		//Destroy(transform.FindChild("MagnifineGlass").gameObject);
		//origScale = transform.localScale;
		transform.localScale = (new Vector3(0,0,0));
	}
	
	// Update is called once per frame
	void Update () {
		if(GameController.planetsPlaced == 8 && !this.GetComponent<TrailRenderer>().enabled)
		{
			this.GetComponent<TrailRenderer>().enabled = true;
		}
		transform.localScale = planetScale;
		if(transform.localScale.x < origScale.x)
		{
			planetScale += new Vector3(1,1,1) * 20 * Time.deltaTime;
		}
		else
		{
			planetScale = origScale;
		
			if(!GameController.cameraMoving && !GameController.correctPopUp)
			{
				orbitPos += orbitSpeed * Time.deltaTime;
				//Debug.Log (orbitPos);
				newX = centerX + (xAxis * Mathf.Cos(orbitPos * 0.005f));
				newY = centerY + (yAxis * Mathf.Sin (orbitPos * 0.005f));
				transform.position = new Vector3(newX, newY, 0);
	//			//transform.RotateAround (sun.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
	//			transform.position = Vector3.MoveTowards(transform.position, wayPoint[nextPos].transform.position, orbitSpeed * Time.deltaTime);
	//			if(Vector3.Distance(
	//			
			}
		}
	}
//	void OnCollisionEnter(Collision hit)
//	{
//		if(hit.transform.tag == "WayPoint")
//		{
//			hit.gameObject.SetActive(false);
//			nextPos += 1;
//			wayPoint[nextPos].SetActive(true);
//		}
//	}
}
