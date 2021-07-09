using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugMode : MonoBehaviour 
{
	[HideInInspector]public bool debugMode;

	private GameController gameController;
	private PlayerController playerController;
	private MeshController meshController;
	private CameraController cameraController;
	private InputManager inputManager;
	private GameObject debugDisplay;
	private GameObject inputDisplay;

	private MeshRenderer leftSensor;
	private MeshRenderer rightSensor;
	private MeshRenderer backSensor;
	private MeshRenderer frontSensor;
	private MeshRenderer gravitySensor;

	private Text fpsText;

	// Debug Display
	private Text speedText;
	private Text velXText;
	private Text velYText;
	private Text velZText;
	private Text airLatSpeedText;
	private Text groundedText;
	private Text backGroundedText;
	private Text centerGroundedText;
	private Text frontGroundedText;
	private Text leftGroundedText;
	private Text rightGroundedText;
	private Text gravityGroundedText;
	private Text backHighestText;
	private Text centerHighestText;
	private Text frontHighestText;
	private Text leftHighestText;
	private Text rightHighestText;
	private Text moveVectorXText;
	private Text moveVectorYText;
	private Text moveVectorZText;
	private Text airMoveVectorXText;
	private Text airMoveVectorYText;
	private Text airMoveVectorZText;
	private Text currentRotationText;
	private Text targetRotationText;
	private Text rotationDifferenceText;
	private Text vertAngleText;
	private Text turnSpeedText;
	private Text slopeSpeedText;
	private Text groundYDirText;
	private Text uphillModeText;
	private Text downhillModeText;
	private Text slopeTurnText;
	private Text groundStickText;
	private Text smoothFallText;
	private Text smoothFallDirText;
	private Text lockInputText;
	private Text jumpBoolText;
	private Text curlBoolText;
	private Text backspinBoolText;

	// Input Display
	private Text inputXText;
	private Text inputYText;
	private Text noInputText;
	private Text inputCurlText;
	private Text inputJumpPressText;
	private Text inputJumpHoldText;
	private Text inputJumpReleaseText;
	private Text inputBackspinText;
	private Text inputAbilityText;
	private Text inputCycleText;
	private Text inputInteractText;

	private bool debugMesh;
	private bool waitForInteractUp;
	private bool waitForCycleUp;

	void Start () 
	{
		debugMode = false;

		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		playerController = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		meshController = GameObject.FindWithTag ("MeshController").GetComponent<MeshController> ();
		cameraController = GameObject.FindWithTag ("MainCamera").GetComponent<CameraController> ();
		inputManager = GameObject.FindWithTag ("InputManager").GetComponent<InputManager> ();
		debugDisplay = GameObject.FindWithTag ("DebugDisplay");
		inputDisplay = GameObject.FindWithTag ("InputDisplay");
		leftSensor = GameObject.FindWithTag ("LeftSensor").GetComponent<MeshRenderer> ();
		rightSensor = GameObject.FindWithTag ("RightSensor").GetComponent<MeshRenderer> ();
		backSensor = GameObject.FindWithTag ("BackSensor").GetComponent<MeshRenderer> ();
		frontSensor = GameObject.FindWithTag ("FrontSensor").GetComponent<MeshRenderer> ();
		gravitySensor = GameObject.FindWithTag ("GravitySensor").GetComponent<MeshRenderer> ();

		fpsText = GameObject.Find ("FPSText").GetComponent<Text> ();

		speedText = GameObject.Find ("SpeedText").GetComponent<Text> ();
		velXText = GameObject.Find ("VelXText").GetComponent<Text> ();
		velYText = GameObject.Find ("VelYText").GetComponent<Text> ();
		velZText = GameObject.Find ("VelZText").GetComponent<Text> ();
		airLatSpeedText = GameObject.Find ("AirLatSpeedText").GetComponent<Text> ();
		groundedText = GameObject.Find ("GroundedText").GetComponent<Text> ();
		backGroundedText = GameObject.Find ("BackGroundedText").GetComponent<Text> ();
		centerGroundedText = GameObject.Find ("CenterGroundedText").GetComponent<Text> ();
		frontGroundedText = GameObject.Find ("FrontGroundedText").GetComponent<Text> ();
		leftGroundedText = GameObject.Find ("LeftGroundedText").GetComponent<Text> ();
		rightGroundedText = GameObject.Find ("RightGroundedText").GetComponent<Text> ();
		gravityGroundedText = GameObject.Find ("GravityGroundedText").GetComponent<Text> ();
		backHighestText = GameObject.Find ("BackHighestText").GetComponent<Text> ();
		centerHighestText = GameObject.Find ("CenterHighestText").GetComponent<Text> ();
		frontHighestText = GameObject.Find ("FrontHighestText").GetComponent<Text> ();
		leftHighestText = GameObject.Find ("LeftHighestText").GetComponent<Text> ();
		rightHighestText = GameObject.Find ("RightHighestText").GetComponent<Text> ();
		moveVectorXText = GameObject.Find ("MoveVectorXText").GetComponent<Text> ();
		moveVectorYText = GameObject.Find ("MoveVectorYText").GetComponent<Text> ();
		moveVectorZText = GameObject.Find ("MoveVectorZText").GetComponent<Text> ();
		airMoveVectorXText = GameObject.Find ("AirMoveVectorXText").GetComponent<Text> ();
		airMoveVectorYText = GameObject.Find ("AirMoveVectorYText").GetComponent<Text> ();
		airMoveVectorZText = GameObject.Find ("AirMoveVectorZText").GetComponent<Text> ();
		currentRotationText = GameObject.Find ("CurrentRotationText").GetComponent<Text> ();
		targetRotationText = GameObject.Find ("TargetRotationText").GetComponent<Text> ();
		rotationDifferenceText = GameObject.Find ("RotationDifferenceText").GetComponent<Text> ();
		vertAngleText = GameObject.Find ("VertAngleText").GetComponent<Text> ();
		turnSpeedText = GameObject.Find ("TurnSpeedText").GetComponent<Text> ();
		slopeSpeedText = GameObject.Find ("SlopeSpeedText").GetComponent<Text> ();
		groundYDirText = GameObject.Find ("GroundYDirText").GetComponent<Text> ();
		uphillModeText = GameObject.Find ("UphillModeText").GetComponent<Text> ();
		downhillModeText = GameObject.Find ("DownhillModeText").GetComponent<Text> ();
		slopeTurnText = GameObject.Find ("SlopeTurnText").GetComponent<Text> ();
		groundStickText = GameObject.Find ("GroundStickText").GetComponent<Text> ();
		smoothFallText = GameObject.Find ("SmoothFallText").GetComponent<Text> ();
		smoothFallDirText = GameObject.Find ("SmoothFallDirText").GetComponent<Text> ();
		lockInputText = GameObject.Find ("LockInputText").GetComponent<Text> ();
		jumpBoolText = GameObject.Find ("JumpBoolText").GetComponent<Text> ();
		curlBoolText = GameObject.Find ("CurlBoolText").GetComponent<Text> ();
		backspinBoolText = GameObject.Find ("BackspinBoolText").GetComponent<Text> ();

		inputXText = GameObject.Find ("InputXText").GetComponent<Text> ();
		inputYText = GameObject.Find ("InputYText").GetComponent<Text> ();
		noInputText = GameObject.Find ("NoInputText").GetComponent<Text> ();
		inputCurlText = GameObject.Find ("InputCurlText").GetComponent<Text> ();
		inputJumpPressText = GameObject.Find ("InputJumpPressText").GetComponent<Text> ();
		inputJumpHoldText = GameObject.Find ("InputJumpHoldText").GetComponent<Text> ();
		inputJumpReleaseText = GameObject.Find ("InputJumpReleaseText").GetComponent<Text> ();
		inputBackspinText = GameObject.Find ("InputBackspinText").GetComponent<Text> ();
		inputAbilityText = GameObject.Find ("InputAbilityText").GetComponent<Text> ();
		inputCycleText = GameObject.Find ("InputCycleText").GetComponent<Text> ();
		inputInteractText = GameObject.Find ("InputInteractText").GetComponent<Text> ();
	}
	void Update () 
	{
		DisableDisplayIfNotDebug ();
		ToggleDebug ();
		AssignStats ();
		ResetPosition ();
	}


	void DisableDisplayIfNotDebug()
	{
		if (debugMode == false && (debugDisplay.activeSelf == true || meshController.debugMesh.activeSelf == true)) 
		{
			debugDisplay.SetActive (false);
			inputDisplay.SetActive (false);
			playerController.debugMovement = false;
			playerController.playerRB.isKinematic = false;
			leftSensor.enabled = false;
			rightSensor.enabled = false;
			backSensor.enabled = false;
			frontSensor.enabled = false;
			gravitySensor.enabled = false;
			meshController.debugMesh.SetActive (false);
		}
	}
	void ToggleDebug()
	{
		if (inputManager.inputStats)
		{				
			if (debugMode == false)
			{
				debugDisplay.SetActive (true);
				inputDisplay.SetActive (true);
				leftSensor.enabled = true;
				rightSensor.enabled = true;
				backSensor.enabled = true;
				frontSensor.enabled = true;
				gravitySensor.enabled = true;
				debugMode = true;
				debugMesh = true;
				return;
			} 

			if (debugMode == true) 
			{
				debugDisplay.SetActive (false);
				inputDisplay.SetActive (false);
				playerController.debugMovement = false;
				playerController.playerRB.isKinematic = false;
				leftSensor.enabled = false;
				rightSensor.enabled = false;
				backSensor.enabled = false;
				frontSensor.enabled = false;
				gravitySensor.enabled = false;
				debugMode = false;
				debugMesh = false;
				cameraController.debugFollowMode = false;
				return;
			}
		}

		if (debugMode == true && Input.GetKeyDown (KeyCode.M)) 
		{
			if (debugMesh == false) 
			{
				debugMesh = true;
				return;
			}

			if (debugMesh == true)
			{
				debugMesh = false;
				return;
			}
		}

		if (debugMode == true && Input.GetKeyDown (KeyCode.F)) 
		{
			if (QualitySettings.vSyncCount == 0) 
			{
				QualitySettings.vSyncCount = 1;
				return;
			}

			if (QualitySettings.vSyncCount == 1)
			{
				QualitySettings.vSyncCount = 0;
				return;
			}
		}

		if (debugMode == true && Input.GetKeyDown (KeyCode.B)) 
		{
			if (cameraController.motionBlurMode == true) cameraController.motionBlurMode = false;
			else cameraController.motionBlurMode = true;
		}

		if (debugMesh) meshController.debugMesh.SetActive (true);
		else meshController.debugMesh.SetActive (false);

		if (playerController.curlBool || playerController.backspinBool)
		{
			if (debugMesh == true)
			{
				meshController.debugMesh.SetActive (false);
			}
		}

		if (debugMode == true && Input.GetKeyDown(KeyCode.I)) 
		{
			if (inputDisplay.activeSelf == false)
			{
				inputDisplay.SetActive (true);
				return;
			}

			if (inputDisplay.activeSelf == true)
			{
				inputDisplay.SetActive (false);
				return;
			}
		}

		if (debugMode == true && inputManager.inputInteract == true) 
		{
			if (playerController.debugMovement == false && waitForInteractUp == false)
			{
				playerController.debugMovement = true;
				playerController.playerRB.isKinematic = true;
				cameraController.debugFollowMode = true;
				playerController.moveVector = Vector3.zero;
				playerController.latMoveVector = Vector3.zero;
				playerController.airLatMoveVector = Vector3.zero;
				playerController.airVertMoveVector = Vector3.zero;
				playerController.finalAirMoveVector = Vector3.zero;
				playerController.playerRB.velocity = Vector3.zero;
				playerController.groundYDir = 0;
				playerController.airLatSpeed = 0;
				playerController.gravityVector = Vector3.zero;
				playerController.currentSpeed = 0;
				playerController.jumpVector = Vector3.zero;
				playerController.jumpBool = false;
				playerController.curlBool = false;
				playerController.firstJumpFrame = false;
				playerController.smoothFall = false;
				playerController.smoothFallDirection = false;
				playerController.lockInput = false;
				waitForInteractUp = true;
				return;
			}

			if (playerController.debugMovement == true && waitForInteractUp == false)
			{
				playerController.debugMovement = false;
				playerController.playerRB.isKinematic = false;
				cameraController.debugFollowMode = false;
				playerController.moveVector = Vector3.zero;
				playerController.latMoveVector = Vector3.zero;
				playerController.airLatMoveVector = Vector3.zero;
				playerController.airVertMoveVector = Vector3.zero;
				playerController.finalAirMoveVector = Vector3.zero;
				playerController.playerRB.velocity = Vector3.zero;
				playerController.groundYDir = 0;
				playerController.airLatSpeed = 0;
				playerController.gravityVector = Vector3.zero;
				playerController.currentSpeed = 0;
				playerController.jumpVector = Vector3.zero;
				playerController.jumpBool = false;
				playerController.curlBool = false;
				playerController.firstJumpFrame = false;
				playerController.smoothFall = false;
				playerController.smoothFallDirection = false;
				playerController.lockInput = false;
				waitForInteractUp = true;
				return;
			}
		}

		if (inputManager.inputInteract == false) waitForInteractUp = false;

		if (Input.GetKeyDown (KeyCode.V))
		{
			if (debugMode) 
			{
				if (gameController.vrEnabled) gameController.vrEnabled = false;
				else gameController.vrEnabled = true;
			}
		}
	}
	void AssignStats()
	{
		float fps = 1 / Time.smoothDeltaTime;
		fpsText.text = "FPS: " + fps.ToString("F0");

		speedText.text = "Speed: " + playerController.currentSpeed.ToString("F0");
		velXText.text = "Vel. X: " + playerController.playerRB.velocity.x.ToString("F0");
		velYText.text = "Vel. Y: " + playerController.playerRB.velocity.y.ToString("F0");
		velZText.text = "Vel. Z: " + playerController.playerRB.velocity.z.ToString("F0");
		airLatSpeedText.text = "Air Lat: " + playerController.airLatSpeed.ToString("F0");
		groundedText.text = "Grounded: " + playerController.isGrounded;
		backGroundedText.text = "Back: " + playerController.backGrounded;
		centerGroundedText.text = "Center: " + playerController.downGrounded;
		frontGroundedText.text = "Front: " + playerController.frontGrounded;
		leftGroundedText.text = "Left: " + playerController.leftGrounded;
		rightGroundedText.text = "Right: " + playerController.rightGrounded;
		gravityGroundedText.text = "Grav. Sensor: " + playerController.gravityGrounded;

		if (playerController.backHighest == true) backHighestText.text = "Highest";
		else backHighestText.text = "";

		if (playerController.centerHighest == true) centerHighestText.text = "Highest";
		else centerHighestText.text = "";

		if (playerController.frontHighest == true) frontHighestText.text = "Highest";
		else frontHighestText.text = "";

		if (playerController.leftHighest == true) leftHighestText.text = "Highest";
		else leftHighestText.text = "";

		if (playerController.rightHighest == true) rightHighestText.text = "Highest";
		else rightHighestText.text = "";

		moveVectorXText.text = "X: " + playerController.moveVector.x.ToString("F0");
		moveVectorYText.text = "Y: " + playerController.moveVector.y.ToString("F0");
		moveVectorZText.text = "Z: " + playerController.moveVector.z.ToString("F0");
		airMoveVectorXText.text = "Air X: " + playerController.finalAirMoveVector.x.ToString("F0");
		airMoveVectorYText.text = "Air Y: " + playerController.finalAirMoveVector.y.ToString("F0");
		airMoveVectorZText.text = "Air Z: " + playerController.finalAirMoveVector.z.ToString("F0");
		currentRotationText.text = "Current: " + playerController.rotationAngle.ToString("F0") + "°";
		targetRotationText.text = "Target: " + playerController.targetRotationAngle.ToString("F0") + "°";
		rotationDifferenceText.text = "Diff.: " + playerController.angleDifference.ToString("F0") + "°";
		vertAngleText.text = "Vertical: " + playerController.vertRotationAngle.ToString ("F0") + "°";
		turnSpeedText.text = "Turn Speed: " + playerController.turnSpeed.ToString ("F2");
		slopeSpeedText.text = "Speed: " + playerController.slopeSpeed.ToString ("F2");
		groundYDirText.text = "Ground Y: " + playerController.groundYDir.ToString("F2");
		uphillModeText.text = "Uphill: " + playerController.uphillMode;
		downhillModeText.text = "Downhill: " + playerController.downhillMode;
		slopeTurnText.text = "Slope Turn: " + playerController.slopeTurn;
		groundStickText.text = "Ground Stick: " + playerController.groundStick;
		smoothFallText.text = "Smooth Fall: " + playerController.smoothFall;
		smoothFallDirText.text = "S.Fall Dir.: " + playerController.smoothFallDirection;
		lockInputText.text = "Lock Input: " + playerController.lockInput;
		jumpBoolText.text = "Jump: " + playerController.jumpBool;
		curlBoolText.text = "Curl: " + playerController.curlBool;
		backspinBoolText.text = "Backspin: " + playerController.backspinBool;

		inputXText.text = "Dir. (X): " + inputManager.inputX.ToString("F1");
		inputYText.text = "Dir. (Y): " + inputManager.inputY.ToString("F1");
		noInputText.text = "" + inputManager.noInput;
		inputCurlText.text = "Curl: " + inputManager.inputCurl;
		inputJumpPressText.text = "Press: " + inputManager.inputJumpPress;
		inputJumpHoldText.text = "Hold: " + inputManager.inputJumpHold;
		inputJumpReleaseText.text = "Release: " + inputManager.inputJumpRelease;
		inputBackspinText.text = "Backspin: " + inputManager.inputBackspin;
		inputAbilityText.text = "Ability: " + inputManager.inputAbility;
		inputCycleText.text = "Cycle: " + inputManager.inputAbilityCycle;
		inputInteractText.text = "Interact: " + inputManager.inputInteract;
	}

	void ResetPosition()
	{
		if (debugMode == true && inputManager.inputAbilityCycle && waitForCycleUp == false)
		{
			playerController.playerRB.position = gameController.spawnpoint.position;
			playerController.moveVector = Vector3.zero;
			playerController.latMoveVector = Vector3.zero;
			playerController.airLatMoveVector = Vector3.zero;
			playerController.airVertMoveVector = Vector3.zero;
			playerController.finalAirMoveVector = Vector3.zero;
			playerController.playerRB.velocity = Vector3.zero;
			playerController.groundYDir = 0;
			playerController.airLatSpeed = 0;
			playerController.gravityVector = Vector3.zero;
			playerController.currentSpeed = 0;
			playerController.jumpVector = Vector3.zero;
			playerController.jumpBool = false;
			playerController.curlBool = false;
			playerController.firstJumpFrame = false;
			playerController.smoothFall = false;
			playerController.smoothFallDirection = false;
			playerController.lockInput = false;
			waitForCycleUp = true;
		}

		if (inputManager.inputAbilityCycle == false) waitForCycleUp = false;

	}
}
