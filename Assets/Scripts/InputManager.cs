using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour 
{
	private GameController gameController;
	private PlayerController playerController;
	private PlayerController player2Controller;

	// Player 1

	[HideInInspector] public float inputX;
	[HideInInspector] public float inputY;
	[HideInInspector] public float cameraInputX;
	[HideInInspector] public float cameraInputY;
	[HideInInspector] public float inputTriggers;

	[HideInInspector] public Vector3 inputVector;

	[HideInInspector] public bool noInput;

	[HideInInspector] public bool inputJumpPress;
	[HideInInspector] public bool inputJumpRelease;
	[HideInInspector] public bool inputJumpHold;
	[HideInInspector] public bool inputCurl;
	[HideInInspector] public bool inputCurlPress;
	[HideInInspector] public bool inputCurlRelease;
	[HideInInspector] public bool inputBackspin;
	[HideInInspector] public bool inputUse;
	[HideInInspector] public bool inputInteract;
	[HideInInspector] public bool inputAbility;
	[HideInInspector] public bool inputAbilityCycle;
	[HideInInspector] public bool inputStats;

	private bool lockInput;

	// Player 2

	[HideInInspector] public float inputX2;
	[HideInInspector] public float inputY2;
	[HideInInspector] public float cameraInputX2;
	[HideInInspector] public float cameraInputY2;
	[HideInInspector] public float inputTriggers2;

	[HideInInspector] public Vector3 inputVector2;

	[HideInInspector] public bool noInput2;

	[HideInInspector] public bool inputJumpPress2;
	[HideInInspector] public bool inputJumpRelease2;
	[HideInInspector] public bool inputJumpHold2;
	[HideInInspector] public bool inputCurl2;
	[HideInInspector] public bool inputCurlPress2;
	[HideInInspector] public bool inputCurlRelease2;
	[HideInInspector] public bool inputBackspin2;
	[HideInInspector] public bool inputUse2;
	[HideInInspector] public bool inputInteract2;
	[HideInInspector] public bool inputAbility2;
	[HideInInspector] public bool inputAbilityCycle2;
	[HideInInspector] public bool inputStats2;

	private bool lockInputP2;


	void Start ()
	{
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		if (gameController.coopMode) player2Controller = GameObject.FindWithTag ("Player 2").GetComponent<PlayerController> ();
	}
	void Update () 
	{
		AssignInput ();
		if(gameController.coopMode) AssignInputP2 ();
	}


	void AssignInput()
	{
		lockInput = playerController.lockInput;

		if(lockInput != true) inputX = Input.GetAxis ("Horizontal");
		if(lockInput != true) inputY = Input.GetAxis ("Vertical");

		inputVector = new Vector3 (inputX, 0, inputY).normalized;

		if (inputX == 0 && inputY == 0 || lockInput == true) noInput = true;
		if ((inputX != 0 || inputY != 0) && lockInput == false) noInput = false;

		cameraInputX = Input.GetAxis ("CameraHorizontal");
		cameraInputY = Input.GetAxis ("CameraVertical");

		inputTriggers = Input.GetAxis ("Triggers");

		inputJumpPress = Input.GetButtonDown ("Jump");
		inputJumpRelease = Input.GetButtonUp ("Jump");
		inputJumpHold = Input.GetButton ("Jump");

		inputCurl = Input.GetButton ("Curl");
		inputCurlPress = Input.GetButtonDown ("Curl");
		inputCurlRelease = Input.GetButtonUp ("Curl");

		inputBackspin = Input.GetButton ("Backspin");
		inputInteract = Input.GetButton ("Interact");
		inputAbility = Input.GetButton ("Ability");
		inputAbilityCycle = Input.GetButton ("AbilityCycle");
		inputStats = Input.GetButtonDown ("Stats");
	}
	void AssignInputP2()
	{
		lockInputP2 = player2Controller.lockInput;

		if(lockInputP2 != true) inputX2 = Input.GetAxis ("Horizontal2");
		if(lockInputP2 != true) inputY2 = Input.GetAxis ("Vertical2");

		inputVector2 = new Vector3 (inputX2, 0, inputY2).normalized;

		if (inputX2 == 0 && inputY2 == 0 || lockInput == true) noInput2 = true;
		if ((inputX2 != 0 || inputY2 != 0) && lockInput == false) noInput2 = false;

		cameraInputX2 = Input.GetAxis ("CameraHorizontal2");
		cameraInputY2 = Input.GetAxis ("CameraVertical2");

		inputTriggers2 = Input.GetAxis ("Triggers2");

		inputJumpPress2 = Input.GetButtonDown ("Jump2");
		inputJumpRelease2 = Input.GetButtonUp ("Jump2");
		inputJumpHold2 = Input.GetButton ("Jump2");

		inputCurl2 = Input.GetButton ("Curl2");
		inputCurlPress2 = Input.GetButtonDown ("Curl2");
		inputCurlRelease2 = Input.GetButtonUp ("Curl2");

		inputBackspin2 = Input.GetButton ("Backspin2");
		inputInteract2 = Input.GetButton ("Interact2");
		inputAbility2 = Input.GetButton ("Ability2");
		inputAbilityCycle2 = Input.GetButton ("AbilityCycle2");
		inputStats2 = Input.GetButtonDown ("Stats2");
	}
}
