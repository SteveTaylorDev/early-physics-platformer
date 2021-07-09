using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public const float DEFAULTGRAVITY = 47f;

	public bool coopMode;

	[HideInInspector] public Transform spawnpoint;
	[HideInInspector] public Transform spawnpointP2;

	[HideInInspector] public GameObject activeCamera;
	[HideInInspector] public GameObject secondCamera;

	[HideInInspector] public Rigidbody playerRB;
	[HideInInspector] public Rigidbody player2RB;

	[HideInInspector] public float gravityAmount;
	[HideInInspector] public float ringCount;
	[HideInInspector] public float crystalRingCount;

	[HideInInspector] public int terrainLayer;

	[HideInInspector] public bool vrEnabled;

	private CameraController cameraController;

	private GameObject mainCamera;
	private GameObject vrCamera;
	private GameObject playerObject;
	private GameObject player2Object;
	private GameObject vrObjects;

	public bool defaultGravity;

	void Awake()
	{
		playerObject = GameObject.FindWithTag ("Player");
		playerRB = playerObject.GetComponent<Rigidbody> ();

		if (coopMode) 
		{
			player2Object = GameObject.FindWithTag ("Player 2");
			player2RB = player2Object.GetComponent<Rigidbody> ();
		}

		spawnpoint = GameObject.FindWithTag ("Spawnpoint").transform;
		spawnpointP2 = GameObject.FindWithTag ("SpawnpointP2").transform;

	//	vrCamera = GameObject.FindWithTag ("VRCamera");
		mainCamera = GameObject.FindWithTag ("MainCamera");
		activeCamera = mainCamera;

		if(coopMode) secondCamera = GameObject.FindWithTag ("SecondCamera");

		//vrObjects = GameObject.Find ("VR Objects");
		SetVRMode ();

		playerRB.position = spawnpoint.position;
		playerObject.transform.forward = spawnpoint.forward;

		if (coopMode) 
		{
			player2RB.position = spawnpoint.position;
			player2Object.transform.forward = spawnpoint.forward;
		}
	}
	void Start () 
	{
		cameraController = mainCamera.GetComponent<CameraController> ();
		cameraController.motionBlurMode = true;

		SetLayers ();
	}
	void Update () 
	{
		CursorManager ();
		SetVRMode ();

		if (defaultGravity == true) SetDefaultGravity ();
	}

	void SetLayers ()
	{
		terrainLayer = 8;
	}

	void CursorManager()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	void SetVRMode()
	{
		if (!vrEnabled)
		{
			activeCamera = mainCamera;
			//vrObjects.SetActive (false);
		}

		else
		{
			//	vrObjects.SetActive (true);
			//	activeCamera = vrCamera;
		}

		activeCamera = mainCamera;
	}

	void SetDefaultGravity()
	{
		gravityAmount = DEFAULTGRAVITY;
	}


}
