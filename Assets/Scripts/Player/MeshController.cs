using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour 
{
	[HideInInspector]public GameObject debugMesh;

	private Transform meshTransform;

	public SkinnedMeshRenderer playerModel;

	public GameObject playerCurlModel;
	public GameObject playerBackspinModel;

	private GameObject leftSensor;
	private GameObject rightSensor;
	private GameObject frontSensor;
	private GameObject backSensor;

	private PlayerController playerController;

	private CameraController cameraController;

	private TrailRenderer meshTrail;


	void Start () 
	{
		playerController = gameObject.GetComponentInParent<PlayerController> ();
		cameraController = GameObject.FindWithTag ("MainCamera").GetComponent<CameraController> ();

		debugMesh = GameObject.FindWithTag ("DebugMesh");
		meshTransform = transform;

		//playerModel = GameObject.FindWithTag ("PlayerModel").GetComponent<SkinnedMeshRenderer>();

		//playerCurlModel = GameObject.FindWithTag ("PlayerCurlModel");
		//playerBackspinModel = GameObject.FindWithTag ("PlayerBackspinModel");

		meshTrail = GetComponent<TrailRenderer> ();

		if (!playerController.player2) 
		{
			leftSensor = GameObject.FindWithTag ("LeftSensor");
			rightSensor = GameObject.FindWithTag ("RightSensor");
			backSensor = GameObject.FindWithTag ("BackSensor");
			frontSensor = GameObject.FindWithTag ("FrontSensor");
		} 

		else 
		{
			leftSensor = GameObject.FindWithTag ("LeftSensorP2");
			rightSensor = GameObject.FindWithTag ("RightSensorP2");
			backSensor = GameObject.FindWithTag ("BackSensorP2");
			frontSensor = GameObject.FindWithTag ("FrontSensorP2");
		}
	}
	void Update () 
	{
		meshTransform.position = playerController.transform.position;

		RotateMesh ();

		if (playerController.curlBool || playerController.backspinBool) CurlModelMode ();
		if (!playerController.curlBool && !playerController.backspinBool) MainModelMode ();

		TrailManager ();
	}
		

	void RotateMesh()
	{
		if (playerController.isGrounded)
		{
			float rotateSpeed = 20f;

			if (playerController.currentSpeed >= 1f)
			{
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.FromToRotation (Vector3.up, playerController.averageNormal) * Quaternion.Euler (0, playerController.rotationAngle, 0), rotateSpeed * Time.smoothDeltaTime);

				if (playerController.curlBool || playerController.backspinBool) 
				{
					float localRotateSpeed = 23;
					if (playerController.currentSpeed < 14) localRotateSpeed = 19;
					if(playerController.currentSpeed >= 40) localRotateSpeed = 40;

					if (playerController.curlBool)
					{
						float rotateAmount = playerController.currentSpeed;
						playerCurlModel.transform.rotation *= Quaternion.Euler ((rotateAmount * localRotateSpeed) * Time.deltaTime, 0, 0);
					}

					if (playerController.backspinBool) 
					{
						float rotateAmount = -playerController.currentSpeed;
						if (rotateAmount >= 50) rotateAmount = 50;
						playerBackspinModel.transform.rotation *= Quaternion.Euler ((rotateAmount * localRotateSpeed) * Time.deltaTime, 0, 0);
					}
				}
			}

			if (playerController.currentSpeed < 1f) 
			{
				rotateSpeed = 15f;
				if (playerController.debugMovement == true) rotateSpeed = 5;
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.AngleAxis (0, transform.position) * Quaternion.Euler (0, playerController.rotationAngle, 0), rotateSpeed * Time.smoothDeltaTime);
			}
		}

		if (!playerController.isGrounded) 
		{ 
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.AngleAxis (0, transform.position) * Quaternion.Euler (0, playerController.rotationAngle, 0), 5f * Time.smoothDeltaTime);
			if (playerController.curlBool == true) 
			{
				float localRotateSpeed = 23;
				if (playerController.finalAirMoveVector.magnitude < 14) localRotateSpeed = 19;
				if(playerController.finalAirMoveVector.magnitude >= 40) localRotateSpeed = 40;

				float rotateAmount = playerController.finalAirMoveVector.magnitude;
				if (playerController.finalAirMoveVector.magnitude <= 15) rotateAmount = 15;
				playerCurlModel.transform.rotation *= Quaternion.Euler ((rotateAmount * localRotateSpeed) * Time.deltaTime, 0, 0);
			}
		}
	}

	void CurlModelMode()
	{
		DisablePlayerModel ();
		EnableCurlModel ();
	}
	void MainModelMode()
	{
		DisableCurlModel ();
		EnablePlayerModel ();
	}
	void DisablePlayerModel()
	{
		playerModel.enabled = false;
	}
	void EnablePlayerModel()
	{		
		playerModel.enabled = true;
		playerController.playerCollider.height = 2.4f;
		playerController.playerCollider.radius = 0.7f;
		playerController.playerCollider.center = new Vector3 (0, 0, 0);

		leftSensor.transform.localPosition = new Vector3 (-0.5f, 0, 0);
		rightSensor.transform.localPosition = new Vector3 (0.5f, 0, 0);
		frontSensor.transform.localPosition = new Vector3 (0, 0, 0.5f);
		backSensor.transform.localPosition = new Vector3 (0, 0, -0.5f);
	}
	void DisableCurlModel()
	{
		if (!playerController.curlBool && !playerController.backspinBool) 
		{
			playerCurlModel.SetActive (false);
			playerBackspinModel.SetActive (false);
		}
	}
	void EnableCurlModel()
	{
		if (playerController.curlBool)
		{
			playerCurlModel.SetActive (true);
			playerBackspinModel.SetActive (false);
		}

		if (playerController.backspinBool)
		{
			playerBackspinModel.SetActive (true);
			playerCurlModel.SetActive (false);
		}

		playerController.playerCollider.height = 1;

		playerController.playerCollider.radius = 0.8f;

		playerController.playerCollider.center = new Vector3 (0, 0, 0);

		leftSensor.transform.localPosition = new Vector3 (-0.85f, 0, 0);
		rightSensor.transform.localPosition = new Vector3 (0.85f, 0, 0);
		frontSensor.transform.localPosition = new Vector3 (0, 0, 0.85f);
		backSensor.transform.localPosition = new Vector3 (0, 0, -0.85f);
	}

	void TrailManager()
	{
		float trailStartSpeed = 10;
		float trailStopSpeed = 20;

		if (cameraController.highSpeedMode == true && (playerController.curlBool == true || playerController.backspinBool == true)) 
		{
			meshTrail.enabled = true;
			meshTrail.time = Mathf.Lerp (meshTrail.time, 0.085f, trailStartSpeed * Time.deltaTime);
		}

		if (cameraController.highSpeedMode == false || !playerController.curlBool && !playerController.backspinBool) 
		{
			if (meshTrail.enabled == true) meshTrail.time = Mathf.Lerp (meshTrail.time, 0, trailStopSpeed * Time.deltaTime);
			if (meshTrail.time <= 0.001f) meshTrail.enabled = false;
		}
	}
}
