using UnityEngine;
using System.Collections;

public class ZoomOut : MonoBehaviour 
{
	void OnMouseUp()
	{
		GameController gc = Camera.main.GetComponent<GameController>();
		gc.zoomIn = false;
		gc.zoomNum = 0;
		GameController.cameraCurDist = Vector3.Distance(transform.position, GameController.cameraOrigin);
		this.gameObject.SetActive(false);
	}
}
