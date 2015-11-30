using UnityEngine;
using System.Collections;

public class AstroAnim : MonoBehaviour {

	public bool animate;
	public GameController gc;
	protected Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
		gc = GameObject.FindObjectOfType(typeof(GameController)) as GameController;
	}

	void Update()
	{
		if(GameController.planetsPlaced == 8)
			transform.Find("MysteryPlanet").gameObject.SetActive(true);
		animator.SetBool("MainMenu",GameController.mainMenu);
		animator.SetBool("Info",GameController.showInfo);
		animator.SetInteger("PlanetsPlaced",GameController.planetsPlaced);
		animator.SetBool ("correctPopUp", GameController.correctPopUp);
	}
	public void Restart()
	{
		animator.SetTrigger ("Restart");
	}
}
