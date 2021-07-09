using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	private const float Y_ANGLE_MIN = -80.0F;		// Clamped minimum for the Y camera rotation
	private const float Y_ANGLE_MAX = 80.0F;		// Clamped maximum for the Y camera rotation
	private const float Z_DIST = 12f;				// The default Z distance the camera is from the target
	private const float Z_DIST_MIN = -5F;			// Clamped minimum for the Z camera distance
	private const float Z_DIST_MAX = 16F;			// Clamped maximum for the Z camera distance

	[HideInInspector]public bool lockedFollowMode;
	[HideInInspector]public bool debugFollowMode;
	[HideInInspector]public bool motionBlurMode;
	[HideInInspector] public bool highSpeedMode;

	private GameController gameController;
	private PlayerController playerController;
	private InputManager inputManager;
	private Camera thisCamera;
	private Kino.Motion motionBlurController;
	private DebugMode debugMode;

	private bool vrFollowMode;
	private bool uiCamera;
	private bool secondCamera;

	private float ySensitivity = 150f;				// Multiplied by currentX before moving camera
	private float xSensitivity = 200f;				// Multiplied by currentY before moving camera
	private float zoomSensitivity = 100;			// Multiplied by currentZ before moving camera
	private float currentX = 0.0f;					// The current rotation position of the camera on the x axis
	private float currentY = 0.0f;					// The current rotation position of the camera on the y axis
	private float currentZ = 0.0f;					// The current distance the camera is from the target on the z axis

	private Vector3 targetDirection;
	private Vector3 dist;

	private Quaternion rotation;

	private Transform cameraTarget;
	private Transform cameraTargetP2; 
	private Transform camTransform;


	void Start ()
	{
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		inputManager = GameObject.FindWithTag ("InputManager").GetComponent<InputManager> ();
		motionBlurController = GetComponent<Kino.Motion> ();
		thisCamera = gameObject.GetComponent<Camera> ();
		debugMode = GameObject.FindWithTag ("GameController").GetComponent<DebugMode> ();

		if (gameObject.tag == "UICamera") uiCamera = true;
		if (gameObject.tag == "SecondCamera") secondCamera = true;

		cameraTarget = GameObject.FindWithTag ("CameraTarget").transform;
		cameraTargetP2 = GameObject.FindWithTag ("CameraTargetP2").transform;
		camTransform = transform;
		currentY = 20;
	}
	void Update()
	{
		if (!uiCamera)
		{
			MotionBlurToggle ();
			HighSpeedMode ();
		}

		if (gameController.coopMode) SplitscreenMode ();
		InputRotate ();
		LockedFollowManager ();
	}
	void LateUpdate () 
	{
		if (!lockedFollowMode && !vrFollowMode && !debugFollowMode) FollowTarget ();
		if (lockedFollowMode || vrFollowMode || debugFollowMode) LockedFollowTarget ();
		LookAtTarget ();
	}

	void SplitscreenMode()
	{ 
		Rect targetRect = Rect.zero;

		if (!secondCamera)
		{
			targetRect = new Rect (new Vector2 (-0.5f, 0), new Vector2 (1, 1));
		} 

		else
		{
			targetRect = new Rect (new Vector2 (0.5f, 0), new Vector2 (1, 1));
		}

		thisCamera.rect = targetRect;
	}
	void InputRotate()
	{
		float joypadFactor = 1.4f;
		float triggerFactor = 2f;

		float cameraX = inputManager.cameraInputX;
		float cameraY = inputManager.cameraInputY;
		float triggers = inputManager.inputTriggers;

		if (secondCamera) 
		{
			cameraX = inputManager.cameraInputX2;
			cameraY = inputManager.cameraInputY2;
			triggers = inputManager.inputTriggers2;
		}

		if (!vrFollowMode && !lockedFollowMode && !debugFollowMode)
		{
			currentX += Input.GetAxis ("Mouse X") * (xSensitivity) * Time.fixedDeltaTime;
			currentY += -Input.GetAxis ("Mouse Y") * (ySensitivity) * Time.fixedDeltaTime;
			currentZ += -Input.GetAxis ("Mouse Scroll") * zoomSensitivity * Time.deltaTime;

			currentX += cameraX * (xSensitivity * joypadFactor) * Time.deltaTime;
			currentY += -cameraY * (ySensitivity * joypadFactor) * Time.deltaTime;
			currentX += triggers * (zoomSensitivity * triggerFactor) * Time.deltaTime;
		}

		if (vrFollowMode || lockedFollowMode || debugFollowMode)
		{
			float lockMultiplier = 0.85f;
			
			currentX += Input.GetAxis ("Mouse X") * (xSensitivity) * lockMultiplier * Time.fixedDeltaTime;
			currentY += -Input.GetAxis ("Mouse Y") * (ySensitivity)* lockMultiplier * Time.fixedDeltaTime;
			currentZ += -Input.GetAxis ("Mouse Scroll") * zoomSensitivity * Time.deltaTime;

			currentX += cameraX * (xSensitivity * lockMultiplier * joypadFactor) * Time.deltaTime;
			currentY += -cameraY * (ySensitivity * lockMultiplier * joypadFactor) * Time.deltaTime;
			currentX += triggers * (zoomSensitivity * lockMultiplier * triggerFactor) * Time.deltaTime;
		}

		currentY = Mathf.Clamp (currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		currentZ = Mathf.Clamp (currentZ, Z_DIST_MIN, Z_DIST_MAX);

		dist = new Vector3 (0, 0, -Z_DIST - currentZ);
		rotation = Quaternion.Euler (currentY, currentX, 0);
	}
	void LockedFollowManager()
	{
		if (gameController.vrEnabled) vrFollowMode = true;
		else vrFollowMode = false;

		if (Input.GetKeyDown(KeyCode.C) && debugMode.debugMode == true)
		{
			if (lockedFollowMode) lockedFollowMode = false;
			else lockedFollowMode = true;
		}
	}

	void MotionBlurToggle()
	{
		if (!motionBlurMode) motionBlurController.enabled = false;
		else motionBlurController.enabled = true;

		motionBlurController.sampleCount = 10;
	}

	void HighSpeedMode()
	{
		float fovChangeSpeed = 5;

		if (playerController.highSpeedMode) highSpeedMode = true;
		else highSpeedMode = false;

		float fps = 1 / Time.smoothDeltaTime;

		if (highSpeedMode) 
		{
			if (motionBlurMode) 
			{
				if(fps > 150 && fps < 250) motionBlurController.frameBlending = 0.001f;
				if (fps >= 250) motionBlurController.frameBlending = 0.4f;
				motionBlurController.shutterAngle = 270;
			}

			if(!lockedFollowMode) thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, 90, fovChangeSpeed * Time.deltaTime);
			else thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, 100, fovChangeSpeed * Time.deltaTime);
		}

		else
		{
			if (motionBlurMode)
			{
				if (fps >= 250) motionBlurController.frameBlending = 0.001f;
				else motionBlurController.frameBlending = 0;

				motionBlurController.shutterAngle = 300;
			}

			thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, 80, fovChangeSpeed * Time.deltaTime);
		}
	}

	void FollowTarget()
	{
		float followSpeed = 10;

		Vector3 targetPosition = cameraTarget.position;
		if (secondCamera) targetPosition = cameraTargetP2.position;

		camTransform.position = Vector3.Lerp(camTransform.position, targetPosition + rotation * dist, followSpeed * Time.deltaTime);
		//camTransform.position = cameraTarget.position + rotation * dist;
	}
		
	void LockedFollowTarget()
	{
		camTransform.position = cameraTarget.position + rotation * dist;
	}
	void LookAtTarget()
	{
		Vector3 targetPosition = cameraTarget.position;
		if (secondCamera) targetPosition = cameraTargetP2.position;

		targetDirection = Vector3.Normalize(targetPosition - camTransform.position);

		if (!lockedFollowMode && !vrFollowMode && !debugFollowMode) 
		{
			float followSpeed = 20;

			camTransform.rotation = Quaternion.Lerp(camTransform.rotation, Quaternion.LookRotation (targetDirection, Vector3.up ), followSpeed * Time.deltaTime);
			//camTransform.rotation = Quaternion.Lerp(camTransform.rotation, Quaternion.LookRotation (targetDirection, playerController.playerRB.transform.up ), followSpeed * Time.deltaTime);
		}
		if (vrFollowMode || lockedFollowMode || debugFollowMode) camTransform.rotation = Quaternion.LookRotation (targetDirection, Vector3.up);
	}
}
