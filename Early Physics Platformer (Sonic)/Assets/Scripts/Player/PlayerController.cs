using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
// Constants
	private const float ACCELERATION = 18f;
	private const float AIRACCELERATION = 20f;
	private const float DECCELERATION = 45f;
	private const float BACKSPINDECCEL = 26f;
	private const float FRICTION = 17f;
	private const float TOPACCEL = 60f;
	private const float TOPSPINDASHSPEED = 100;
	private const float TOPSPEED = 180f;
	private const float TOPTURNSPEED = 1f;
	private const float MINTURNSPEED = 0.05f;

// Public Variables
	public Transform cameraRefTransform;

	[HideInInspector] public Rigidbody playerRB;
	[HideInInspector] public MeshRenderer capsuleMesh;
	[HideInInspector] public CapsuleCollider playerCollider;

	[HideInInspector] public float currentSpeed;
	[HideInInspector] public float airLatSpeed;
	[HideInInspector] public float groundYDir;
	[HideInInspector] public float rotationAngle;
	[HideInInspector] public float vertRotationAngle;
	[HideInInspector] public float targetRotationAngle;
	[HideInInspector] public float turnSpeed;
	[HideInInspector] public float slopeSpeed;
	[HideInInspector] public float angleDifference;
	[HideInInspector] public float jumpTimer;
	[HideInInspector] public float currentBackspin;
	[HideInInspector] public float playerHeight;
	[HideInInspector] public float gravityRayLength;
	[HideInInspector] public float chaosEnergy;

	public bool player2;

	[HideInInspector]public bool isGrounded;
	[HideInInspector]public bool backGrounded;
	[HideInInspector]public bool downGrounded;
	[HideInInspector]public bool frontGrounded;
	[HideInInspector]public bool leftGrounded;
	[HideInInspector]public bool rightGrounded;
	[HideInInspector]public bool gravityGrounded;
	[HideInInspector]public bool backHighest;
	[HideInInspector]public bool centerHighest;
	[HideInInspector]public bool frontHighest;
	[HideInInspector]public bool leftHighest;
	[HideInInspector]public bool rightHighest;
	[HideInInspector]public bool groundStick;
	[HideInInspector]public bool smoothFall;
	[HideInInspector]public bool smoothFallDirection;
	[HideInInspector]public bool airToGroundCalc;
	[HideInInspector]public bool uphillMode;
	[HideInInspector]public bool downhillMode;
	[HideInInspector]public bool lockInput;
	[HideInInspector]public bool turnFriction;
	[HideInInspector]public bool slopeTurn;
	[HideInInspector]public bool jumpBool;
	[HideInInspector]public bool debugMovement;
	[HideInInspector]public bool slopeDisableAccel;
	[HideInInspector]public bool firstJumpFrame;
	[HideInInspector]public bool curlBool;
	[HideInInspector]public bool backspinBool;
	[HideInInspector]public bool highSpeedMode;
	[HideInInspector]public bool runningStart;

	[HideInInspector]public Vector3 moveVector;
	[HideInInspector]public Vector3 latMoveVector;
	[HideInInspector]public Vector3 airLatMoveVector;
	[HideInInspector]public Vector3 airVertMoveVector;
	[HideInInspector]public Vector3 gravityVector;
	[HideInInspector]public Vector3 finalAirMoveVector;
	[HideInInspector]public Vector3 jumpVector;
	[HideInInspector]public Vector3 averageNormal;
	[HideInInspector]public Vector3 inputDirection;
	[HideInInspector]public Vector3 directionRotation;
	[HideInInspector]public Vector3 vertDirectionRotation;
	[HideInInspector]public Vector3 latDirectionRotation;
	[HideInInspector]public Vector3 lastDirectionRotation;
	[HideInInspector]public Vector3 finalDirectionRotation;
	[HideInInspector]public Vector3 gravityDirection;

	[HideInInspector]public RaycastHit raycastGravityHit;
	[HideInInspector]public RaycastHit highestHit;

// Private Variables
	private GameController gameController;
	private InputManager inputManager;

	private AudioSource[] audioSources;
	private AudioSource jumpAudio;
	private AudioSource spinAudio;
	private AudioSource brakeAudio;
	private AudioSource curlAudio;
	private AudioSource highSpeedAudio;
	private AudioSource boomAudio;
	private AudioSource ringAudio;
	private AudioSource crystalRingAudio;

	private Transform camTransform;
	private Transform leftSensor;
	private Transform rightSensor;
	private Transform backSensor;
	private Transform frontSensor;
	private Transform gravitySensor;

	private bool waitForJumpUp;
	private bool waitForGround;
	private bool waitForCurlUp;
	private bool airHitBool;
	private bool airMoveHitMid;
	private bool airMoveHitLow;
	private bool airMoveHitLeft;
	private bool airMoveHitRight;
	private bool normalTurnSpeed;
	private bool reapplyCurl;
	private bool airUncurl;
	private bool firstAirFrame;
	private bool hasJumped;
	private bool jumpStop;
	private bool jumpStopDisable;

	private ContactPoint airHit;

	private float raycastPadding;
	private float globalGravityAmount;
	private float groundRayAmount;
	private float debugYSpeed;

	private Vector3 rawInputDirection;
	private Vector3 groundAngleDirection;
	private Vector3 slopeDirection;
	private Vector3 jumpVectorLatDirection;

	private Quaternion targetQuaternion;

	private RaycastHit raycastDownHit;
	private RaycastHit raycastFrontDownHit;
	private RaycastHit raycastBackDownHit;
	private RaycastHit raycastLeftDownHit;
	private RaycastHit raycastRightDownHit;
	private RaycastHit airVectorHit;

	private GameObject forwardRef;

	private float inputX;
	private float inputY;

	private Vector3 inputVector;

	private bool noInput;

	private bool inputJumpPress;
	private bool inputJumpRelease;
	private bool inputJumpHold;
	private bool inputCurl;
	private bool inputCurlPress;
	private bool inputCurlRelease;
	private bool inputBackspin;
	//private bool inputUse;
	//private bool inputInteract;
	private bool inputAbility;
	//private bool inputAbilityCycle;
	//private bool inputStats;


	void Start ()
	{
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();

		playerRB = gameObject.GetComponent<Rigidbody> ();
		playerCollider = gameObject.GetComponent<CapsuleCollider> ();
		capsuleMesh = gameObject.GetComponent<MeshRenderer> ();

		inputManager = GameObject.FindWithTag ("InputManager").GetComponent<InputManager> ();

		forwardRef = GameObject.FindWithTag ("ForwardRef");

		audioSources = gameObject.GetComponents<AudioSource> ();
		jumpAudio = audioSources [0];
		spinAudio = audioSources [1];
		brakeAudio = audioSources [2];
		curlAudio = audioSources [3];
		highSpeedAudio = audioSources [4];
		boomAudio = audioSources [5];
		ringAudio = audioSources [6];
		crystalRingAudio = audioSources [7];

		if (!player2) 
		{
			leftSensor = GameObject.FindWithTag ("LeftSensor").transform;
			rightSensor = GameObject.FindWithTag ("RightSensor").transform;
			backSensor = GameObject.FindWithTag ("BackSensor").transform;
			frontSensor = GameObject.FindWithTag ("FrontSensor").transform;
			gravitySensor = GameObject.FindWithTag ("GravitySensor").transform;	
		} 

		else 
		{
			leftSensor = GameObject.FindWithTag ("LeftSensorP2").transform;
			rightSensor = GameObject.FindWithTag ("RightSensorP2").transform;
			backSensor = GameObject.FindWithTag ("BackSensorP2").transform;
			frontSensor = GameObject.FindWithTag ("FrontSensorP2").transform;
			gravitySensor = GameObject.FindWithTag ("GravitySensorP2").transform;	
		}

		globalGravityAmount = gameController.gravityAmount;
		gravityDirection = Vector3.down;

		rawInputDirection = transform.forward;
		inputDirection = transform.forward;
		directionRotation = transform.forward;
		lastDirectionRotation = transform.forward;

		runningStart = false;
	}
	void Update()
	{
		chaosEnergy = 100;
		playerHeight = playerCollider.height;

		LocalInputManager ();

		RaycastDown ();
		RaycastFrontDown ();
		RaycastBackDown ();
		RaycastLeftDown ();
		RaycastRightDown ();
		RaycastGravity ();

		AttractSphere ();

		GroundStateManager ();
		RotationManager ();
		DebugRaycasts ();
		MovePlayer ();
	}
		
	void OnCollisionEnter(Collision collision)
	{
		StickToWall (collision);
	}
	void OnCollisionStay(Collision collision)
	{
		StickToWall (collision);
	}
	void OnCollisionExit(Collision collision)
	{
		if (!isGrounded) 
		{
			airHitBool = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Ring") || other.gameObject.CompareTag ("CrystalRing")) 
		{
			CollectRing (other);
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag ("Ring") || other.gameObject.CompareTag ("CrystalRing")) 
		{
			CollectRing (other);
		}
	}


// Main Loop
	void LocalInputManager()
	{
		if (!player2) 
		{
			inputX = inputManager.inputX;
			inputY = inputManager.inputY;
			inputVector = inputManager.inputVector;
			noInput = inputManager.noInput;
			inputJumpPress = inputManager.inputJumpPress;
			inputJumpHold = inputManager.inputJumpHold;
			inputJumpRelease = inputManager.inputJumpRelease;
			inputCurl = inputManager.inputCurl;
			inputCurlPress = inputManager.inputCurlPress;
			inputCurlRelease = inputManager.inputCurlRelease;
			inputBackspin = inputManager.inputBackspin;
			//inputUse = inputManager.inputUse;
			//inputInteract = inputManager.inputInteract;
			inputAbility = inputManager.inputAbility;
			//inputAbilityCycle = inputManager.inputAbilityCycle;
			//inputStats = inputManager.inputStats;
		} 

		else 
		{
			inputX = inputManager.inputX2;
			inputY = inputManager.inputY2;
			inputVector = inputManager.inputVector2;
			noInput = inputManager.noInput2;
			inputJumpPress = inputManager.inputJumpPress2;
			inputJumpHold = inputManager.inputJumpHold2;
			inputJumpRelease = inputManager.inputJumpRelease2;
			inputCurl = inputManager.inputCurl2;
			inputCurlPress = inputManager.inputCurlPress2;
			inputCurlRelease = inputManager.inputCurlRelease2;
			inputBackspin = inputManager.inputBackspin2;
			//inputUse = inputManager.inputUse2;
			//inputInteract = inputManager.inputInteract2;
			inputAbility = inputManager.inputAbility2;
			//inputAbilityCycle = inputManager.inputAbilityCycle2;
			//inputStats = inputManager.inputStats2;
		}
	}
	void RaycastDown()
	{
		Debug.DrawRay (transform.position, -playerRB.transform.up * ((playerHeight/2) + raycastPadding), Color.white);

		Ray downRay = new Ray (transform.position, -playerRB.transform.up);

		if (Physics.Raycast (downRay, out raycastDownHit, (playerHeight / 2) + raycastPadding))
		{
			if (raycastDownHit.collider.gameObject.layer == gameController.terrainLayer)
			{
				if (downGrounded == false) groundRayAmount += 1;
				downGrounded = true;
			}
		}

		else
		{
			if(downGrounded == true) groundRayAmount -= 1;
			downGrounded = false;
		}
	}
	void RaycastFrontDown()
	{
		Vector3 rayOrigin = frontSensor.position;

		Debug.DrawRay (rayOrigin, -playerRB.transform.up * ((playerHeight /2) + raycastPadding), Color.red);

		Ray frontDownRay = new Ray (rayOrigin, -playerRB.transform.up);

		if(Physics.Raycast (frontDownRay, out raycastFrontDownHit, (playerHeight /2) + raycastPadding))
		{
			if (raycastFrontDownHit.collider.gameObject.layer == gameController.terrainLayer)
			{
				if (frontGrounded == false) groundRayAmount += 1;
				frontGrounded = true;
			}
		}

		else
		{
			if(frontGrounded == true) groundRayAmount -= 1;
			frontGrounded = false;
		}
	}
	void RaycastBackDown()
	{
		Vector3 rayOrigin = backSensor.position;

		Debug.DrawRay (rayOrigin, -playerRB.transform.up * ((playerHeight /2) + raycastPadding), Color.red);

		Ray backDownRay = new Ray (rayOrigin, -playerRB.transform.up);

		if(Physics.Raycast (backDownRay, out raycastBackDownHit, (playerHeight /2) + raycastPadding))
		{
			if (raycastBackDownHit.collider.gameObject.layer == gameController.terrainLayer)
			{
				if (backGrounded == false) groundRayAmount += 1;
				backGrounded = true;
			}
		}

		else
		{
			if(backGrounded == true) groundRayAmount -= 1;
			backGrounded = false;
		}
	}
	void RaycastLeftDown()
	{
		Vector3 rayOrigin = leftSensor.position;

		Debug.DrawRay (rayOrigin, -playerRB.transform.up * ((playerHeight /2) + raycastPadding), Color.red);

		Ray leftDownRay = new Ray (rayOrigin, -playerRB.transform.up);

		if(Physics.Raycast (leftDownRay, out raycastLeftDownHit, (playerHeight /2) + raycastPadding))
		{
			if (raycastLeftDownHit.collider.gameObject.layer == gameController.terrainLayer)
			{
				if (leftGrounded == false) groundRayAmount += 1;
				leftGrounded = true;
			}
		}

		else
		{
			if(leftGrounded == true) groundRayAmount -= 1;
			leftGrounded = false;
		}
	}
	void RaycastRightDown()
	{
		Vector3 rayOrigin = rightSensor.position;

		Debug.DrawRay (rayOrigin, -playerRB.transform.up * ((playerHeight /2) + raycastPadding), Color.red);

		Ray rightDownRay = new Ray (rayOrigin, -playerRB.transform.up);

		if(Physics.Raycast (rightDownRay, out raycastRightDownHit, (playerHeight /2) + raycastPadding))
		{
			if (raycastRightDownHit.collider.gameObject.layer == gameController.terrainLayer)
			{
				if (rightGrounded == false) groundRayAmount += 1;
				rightGrounded = true;
			}
		}

		else
		{
			if(rightGrounded == true) groundRayAmount -= 1;
			rightGrounded = false;
		}
	}
	void RaycastGravity()
	{
		gravityRayLength = 6;

		Debug.DrawRay (transform.position, gravityDirection * gravityRayLength, Color.magenta);

		Ray downRay = new Ray (transform.position, gravityDirection);

		if (Physics.Raycast (downRay, out raycastGravityHit, gravityRayLength))
		{
			if (raycastGravityHit.collider.gameObject.layer == gameController.terrainLayer) gravityGrounded = true;
		}

		else gravityGrounded = false;
	}

	void AttractSphere()
	{
		float maxSphereSize = 6;

		float radiusFactor = 0.04f;
		if (currentSpeed >= 60) radiusFactor = 0.037f;
		float radiusAmount = currentSpeed * radiusFactor;
		if (!isGrounded) radiusAmount = finalAirMoveVector.magnitude * radiusFactor;
		if (radiusAmount >= maxSphereSize) radiusAmount = maxSphereSize;

		Collider[] hitColliders = Physics.OverlapSphere (playerRB.position, radiusAmount);
		for (int i = 0; i < hitColliders.Length; i++)
		{
			if (hitColliders [i].gameObject.CompareTag ("Ring") || hitColliders [i].gameObject.CompareTag("CrystalRing")) 
			{
				RingController hitRingController = hitColliders [i].GetComponent<RingController> ();
				if (!player2) hitRingController.attractToPlayer = true;
				else hitRingController.attractToPlayer2 = true;
			}
		}
	}

	void GroundStateManager()
	{
		if (downGrounded == true && (frontGrounded == true || backGrounded == true || leftGrounded == true || rightGrounded == true)) isGrounded = true;
		if (downGrounded == false && frontGrounded == false && backGrounded == false && leftGrounded == false && rightGrounded == false || smoothFall == true || debugMovement == true) isGrounded = false;
	}
	void RotationManager()
	{
		camTransform = gameController.activeCamera.transform;
		if (player2) camTransform = gameController.secondCamera.transform;

		TurnSpeed ();

		if (noInput == false) rawInputDirection = inputVector;

		Vector3 latCamForward = camTransform.forward;
		latCamForward.y = 0;
		Vector3.Normalize (latCamForward);

		cameraRefTransform.forward = latCamForward;

		inputDirection = cameraRefTransform.TransformDirection(rawInputDirection);

		if (!isGrounded) 
		{
			if (smoothFallDirection == true)
			{
				inputDirection = airLatMoveVector.normalized;
				directionRotation = airLatMoveVector.normalized;
				lastDirectionRotation = directionRotation;
			}
		}

		if (lockInput == false && currentSpeed > 3 || !isGrounded) directionRotation += (inputDirection / turnSpeed) * Time.deltaTime;
		if (currentSpeed <= 3 && isGrounded) directionRotation += ((inputDirection / 1.5f) * 50) * Time.deltaTime;
		directionRotation = Vector3.Normalize (directionRotation);

		if (noInput == false) lastDirectionRotation = directionRotation;
		if (noInput == true) directionRotation = lastDirectionRotation;

		//if (isGrounded) SlopeInfluence ();

		latDirectionRotation = new Vector3 (directionRotation.x, 0, directionRotation.z);
		vertDirectionRotation = new Vector3 (0, directionRotation.y, 0);

		finalDirectionRotation = transform.TransformDirection (latDirectionRotation);
		Vector3.Normalize (finalDirectionRotation);

		if(debugMovement == false) rotationAngle = Vector3.Angle (Vector3.forward, directionRotation.normalized) * Mathf.Sign (directionRotation.normalized.x);
		targetRotationAngle = Vector3.Angle (Vector3.forward, inputDirection.normalized) * Mathf.Sign (inputDirection.normalized.x);

		if (noInput == true) 
		{
			targetRotationAngle = Vector3.Angle (Vector3.forward, new Vector3 (groundAngleDirection.x, 0, groundAngleDirection.z) * Mathf.Sign (groundAngleDirection.normalized.x));
			if (slopeDirection.magnitude <= 0) targetRotationAngle = rotationAngle;
		}

		TurnFriction ();
	}
	/*void RotationManager()
	{
		if(isGrounded)
		{ 
			camTransform = gameController.activeCamera.transform;
			if (player2) camTransform = gameController.secondCamera.transform;

			Vector3 latCamForward = camTransform.forward;
			latCamForward.y = 0;
			Vector3.Normalize (latCamForward);

			cameraRefTransform.forward = latCamForward;

			rawInputDirection = cameraRefTransform.TransformDirection (inputVector);
			inputDirection = rawInputDirection;

			directionRotation = inputDirection;

			if (noInput) directionRotation = lastDirectionRotation;
			else lastDirectionRotation = directionRotation;

			finalDirectionRotation = transform.TransformVector(directionRotation);

			if(debugMovement == false) rotationAngle = Vector3.Angle (Vector3.forward, directionRotation.normalized) * Mathf.Sign (directionRotation.normalized.x);
			targetRotationAngle = Vector3.Angle (Vector3.forward, inputDirection.normalized) * Mathf.Sign (inputDirection.normalized.x);

			if (noInput == true) 
			{
				targetRotationAngle = Vector3.Angle (Vector3.forward, new Vector3 (groundAngleDirection.x, 0, groundAngleDirection.z) * Mathf.Sign (groundAngleDirection.normalized.x));
				if (slopeDirection.magnitude <= 0) targetRotationAngle = rotationAngle;
			}

			targetQuaternion = Quaternion.FromToRotation (-gravityDirection, averageNormal) * Quaternion.Euler(0,targetRotationAngle,0);
		}
	}*/
	void TurnSpeed()
	{
		float turnSpeedFactor = 0.7f;
		if (currentSpeed >= 60) turnSpeedFactor = 0.75f;

		if (!backspinBool || !isGrounded)
		{
			normalTurnSpeed = true;

			turnSpeed = (currentSpeed / TOPSPEED) * turnSpeedFactor;

			if (!isGrounded) 
			{
				turnSpeed = (airLatSpeed / TOPSPEED) * turnSpeedFactor;
				if (turnSpeed <= 0.075f) turnSpeed = 0.075f;
			}

			if (curlBool && isGrounded) turnSpeed *= 1.65f;
		}

		if (backspinBool && isGrounded)
		{
			if (normalTurnSpeed == true) 
			{
				turnSpeed = (currentSpeed / TOPSPEED) * turnSpeedFactor;
				normalTurnSpeed = false;
			}

			float backspinTime = 5;
			
			if (currentSpeed <= TOPSPINDASHSPEED && turnSpeed >= 0.17f) turnSpeed = Mathf.Lerp (turnSpeed, 0.17f, backspinTime * Time.deltaTime);
			if (currentSpeed > TOPSPINDASHSPEED && currentSpeed <= TOPSPEED - 50) turnSpeed = Mathf.Lerp(turnSpeed, 0.21f, (backspinTime * 1.25f) * Time.deltaTime);
			if (currentSpeed > TOPSPEED - 50 && currentSpeed <= TOPSPEED - 20) turnSpeed = Mathf.Lerp(turnSpeed, 0.27f, (backspinTime * 1.5f) * Time.deltaTime);
			if (currentSpeed > TOPSPEED - 20) turnSpeed = Mathf.Lerp(turnSpeed, 0.3f, (backspinTime * 2) * Time.deltaTime);

			if((currentSpeed / TOPSPEED) * 0.9f < 0.17f) turnSpeed = (currentSpeed / TOPSPEED) * turnSpeedFactor;
		}

		if (debugMovement == true) turnSpeed = 0.1f;

		if (turnSpeed <= MINTURNSPEED) turnSpeed = MINTURNSPEED;
		if (turnSpeed >= TOPTURNSPEED) turnSpeed = TOPTURNSPEED;
	}
	void SlopeInfluence()
	{
		float factorAmount = 5f;
		float gripLimit = 2.5f;
		float angleDifference = Vector3.Angle (groundAngleDirection, finalDirectionRotation);
		float minAngleDifference = 22;
		float lowAngleSpeed = 10; 

		slopeDirection = -groundAngleDirection * factorAmount;

		if (curlBool) gripLimit = 0.2f;
		if (slopeDirection.magnitude <= gripLimit) slopeDirection = Vector3.zero; 

		if (currentSpeed <= lowAngleSpeed) minAngleDifference *= 0.5f;

		if (angleDifference <= minAngleDifference) uphillMode = true;
		if (curlBool) uphillMode = false;

		if (180 - angleDifference <= minAngleDifference) downhillMode = true;
		else downhillMode = false;

		if (currentSpeed <= 2 && vertRotationAngle > 30 && vertRotationAngle < 85) slopeTurn = true;
		if (currentSpeed >= 10 || vertRotationAngle >= 85 || vertRotationAngle <= 30) slopeTurn = false;

		if ((slopeDirection.magnitude > 0 && (angleDifference > minAngleDifference || slopeTurn == true || curlBool) && isGrounded && vertRotationAngle > 10))
		{
			float speedAdjuster = (currentSpeed * 2 / (TOPSPEED / 2));
			if (speedAdjuster < 0.25f) speedAdjuster = 0.25f;

			if (slopeTurn && !downhillMode) speedAdjuster = 0.035f;

			if (angleDifference > minAngleDifference) uphillMode = false;

			Vector3 targetRotation = groundAngleDirection;
			targetRotation.y = 0;
			targetRotation = targetRotation.normalized;

			if (noInput == false || slopeTurn == true || curlBool || backspinBool) 
			{
				if (curlBool && noInput == false || backspinBool) 
				{
					speedAdjuster *= 0.35f;

					if (speedAdjuster >= 0.5f) speedAdjuster = 0.5f;
				}

				directionRotation -= (targetRotation / (speedAdjuster)* 0.35f) * Time.deltaTime;

				if (slopeTurn) 
				{
					inputDirection = directionRotation;
				}
			}

			if (noInput == true || slopeTurn) lastDirectionRotation -= (targetRotation/speedAdjuster) * Time.deltaTime;

			directionRotation = directionRotation.normalized;
			lastDirectionRotation = lastDirectionRotation.normalized;
		}
	}
	void TurnFriction()
	{
		float frictionDifference = 35;
		angleDifference = Mathf.DeltaAngle (rotationAngle, targetRotationAngle);

		if (currentSpeed >= TOPACCEL) frictionDifference = 37; 
		if(currentSpeed >= TOPSPINDASHSPEED || curlBool) frictionDifference = 40;

		if (Mathf.Abs(angleDifference) >= frictionDifference && !backspinBool && noInput == false) turnFriction = true;
		if (Mathf.Abs(angleDifference) < frictionDifference || noInput == true) turnFriction = false;
	}
	void DebugRaycasts()
	{
		if(noInput == false) Debug.DrawRay (transform.position, inputDirection, Color.white);
		Debug.DrawRay (transform.position, finalDirectionRotation, Color.cyan);
		if(isGrounded) Debug.DrawRay (transform.position, groundAngleDirection, Color.yellow);
		if(isGrounded) Debug.DrawRay (transform.position, slopeDirection, Color.green);
		if(isGrounded) Debug.DrawRay (transform.position, moveVector/20 , Color.blue);
		if(!isGrounded) Debug.DrawRay (transform.position, finalAirMoveVector.normalized * (playerCollider.radius + 0.3f), new Color (0.3f, 0f, 0.95f));
		if(!isGrounded) Debug.DrawRay (transform.position - new Vector3 (0, (playerHeight/2), 0), finalAirMoveVector.normalized * (playerCollider.radius + 0.3f), new Color (0.35f, 0f, 1f));
		if(!isGrounded) Debug.DrawRay (transform.position, airVertMoveVector / 10, new Color (0.9f, 0.5f, 0.95f));
	}

	void MovePlayer()
	{
		if (debugMovement) DebugMovement();
		if (!debugMovement) PlayerMovement ();
	}
// Main Loop end

	void DebugMovement()
	{
		if ((Mathf.Abs(inputY) > 0 || Mathf.Abs(inputX) > 0))
		{
			currentSpeed += (ACCELERATION*12) * Time.deltaTime;
		}

		if (inputJumpHold == true)
		{
			debugYSpeed += (ACCELERATION*10) * Time.deltaTime;
		}

		if (inputCurl == true)
		{
			debugYSpeed -= (ACCELERATION*10) * Time.deltaTime;
		}

		if (inputY == 0 && inputX == 0)
		{ 
			currentSpeed = 0;
			moveVector.x = 0;
			moveVector.z = 0;
			airLatMoveVector = Vector3.zero;
			airVertMoveVector = Vector3.zero;
			finalAirMoveVector = Vector3.zero;
		}

		if (inputJumpHold == false && inputCurl == false || moveVector.y < 0 && inputJumpHold == true || moveVector.y > 0 && inputCurl == true)
		{
			debugYSpeed = 0;
			moveVector.y = 0;
			airVertMoveVector = Vector3.zero;
		}

		directionRotation = (inputDirection.normalized / 1.3f) * currentSpeed;
		moveVector = directionRotation;
		moveVector.y = debugYSpeed;

		rotationAngle = Vector3.Angle (Vector3.forward, directionRotation.normalized) * Mathf.Sign (directionRotation.normalized.x);
		if(noInput == true) rotationAngle = Vector3.Angle (Vector3.forward, inputDirection.normalized) * Mathf.Sign (inputDirection.normalized.x);

		playerRB.position += moveVector * Time.deltaTime;
	}
	void PlayerMovement()
	{	
		LockInput ();
		HighSpeedAudio ();

		if (isGrounded)
		{
			GroundAngleCalc ();

			GravityVectorReset ();
			JumpReset ();

			AirToGroundCalc ();
			HighestHit ();
			AverageGroundedNormal ();

			RaycastExtender ();

			AirHitReset ();

			Jump ();

			Curl ();

			Backspin ();

			// Boost Code
			if (inputAbility == true && noInput == false && chaosEnergy > 0) 
			{
				float accelAmount = 0;
				float drainAmount = 10;

				if (currentSpeed < TOPSPINDASHSPEED) accelAmount = ACCELERATION * 2;
				if (currentSpeed >= TOPSPINDASHSPEED) accelAmount = ACCELERATION;

				currentSpeed += accelAmount * Time.deltaTime;
				chaosEnergy -= drainAmount * Time.deltaTime;
			}

			SmoothFall ();
			RotatePlayer ();

			Accelerating ();
			Friction ();

			SlopeSpeed ();

			TopSpeedLimiter ();

			GroundMoveVector ();

			playerRB.velocity = moveVector;

			GroundStick ();
		}	

		if (!isGrounded) 
		{
			SetAirToGroundCalc ();
			CalcAirLatSpeed ();
			JumpTimer ();
			ResetSlopeTurn ();

			RaycastExtender ();

			//AirJump ();

			JumpStop ();

			Curl ();

			//ElectricShieldJump ();

			AirBackspin();

			RotatePlayer ();

			SmoothFallReset ();

			AirAccelerating();
			AirDrag ();

			LocalGravity ();

			TopSpeedLimiter ();

			FinalAirMoveVector ();

			playerRB.velocity = finalAirMoveVector;
		}
	}

	void LockInput()
	{
		if (smoothFall == true && smoothFallDirection == true || jumpBool == true || (slopeTurn && !downhillMode && vertRotationAngle >= 40 && isGrounded) || curlBool && isGrounded && currentSpeed <= 15 && (vertRotationAngle >= 40 || currentSpeed <= 3)) lockInput = true;
		if (smoothFall == false && jumpBool == false) 
		{
			if (slopeTurn) 
			{
				if (downhillMode && currentSpeed >= 15) lockInput = false;
				return;
			} 

			if (curlBool && isGrounded) 
			{
				if (currentSpeed > 20 || currentSpeed > 3 && vertRotationAngle < 20 || vertRotationAngle < 10) lockInput = false;
				return;
			}

			lockInput = false;
		}
	}
	void HighSpeedAudio()
	{
		if (highSpeedMode && !highSpeedAudio.isPlaying) highSpeedAudio.Play ();
		if (!highSpeedMode && highSpeedAudio.isPlaying) highSpeedAudio.Stop ();
	}

	void GroundAngleCalc()
	{
		float speedAdjuster = (currentSpeed / (TOPSPEED/5));
		if (speedAdjuster >= 1) speedAdjuster = 1;
		if (speedAdjuster <= 0.5f) speedAdjuster = 0.5f;

		groundAngleDirection = raycastDownHit.point - gravitySensor.position;
		groundAngleDirection = groundAngleDirection / speedAdjuster;

		if (groundAngleDirection.magnitude >= 2) groundAngleDirection = groundAngleDirection.normalized * 2;
	}

	void GravityVectorReset()
	{
		if(gravityVector.magnitude != 0) gravityVector = Vector3.zero;
	}
	void JumpReset()
	{
		jumpTimer = 1;
		waitForGround = false;
		hasJumped = false;
		jumpStopDisable = false;
		jumpStop = false;
	}

	void SetAirToGroundCalc()
	{
		airToGroundCalc = true;
	}
	void CalcAirLatSpeed()
	{
		airLatSpeed = latMoveVector.magnitude;
	}
	void JumpTimer()
	{
		float timerSpeed = 10f;

		if (jumpBool)
		{
			jumpTimer -= timerSpeed * Time.deltaTime;
		}

		if (jumpTimer <= 0 && jumpBool == true) 
		{
			jumpTimer = 0;
			jumpBool = false;
		}
	}
	void ResetSlopeTurn()
	{
		slopeTurn = false;
	}

	void AirToGroundCalc()
	{
		if (airToGroundCalc == true)
		{
			currentSpeed = airLatSpeed;

			if (inputCurl == false) 
			{
				curlBool = false;
				reapplyCurl = false;
			}

			if (inputCurl == true) 
			{
				if(highSpeedMode) spinAudio.Play ();
				if(!curlBool && (currentSpeed > 10 || vertRotationAngle > 15)) curlAudio.Play ();
				curlBool = true;
			}

			if(runningStart) RunningStart ();

			airToGroundCalc = false;
		}
	}
	void RunningStart()
	{
		float runningStartSpeed = TOPSPINDASHSPEED;

		currentSpeed = runningStartSpeed;
		runningStart = false;
	}
	void HighestHit()
	{
		float backHit = 0;
		float centerHit = 0;
		float frontHit = 0;
		float leftHit = 0;
		float rightHit = 0;

		backHighest = false;
		centerHighest = false;
		frontHighest = false;
		leftHighest = false;
		rightHighest = false;

		if (backGrounded == true) backHit = raycastBackDownHit.point.y;
		if (downGrounded == true) centerHit = raycastDownHit.point.y;
		if (frontGrounded == true) frontHit = raycastFrontDownHit.point.y;
		if (leftGrounded == true) leftHit = raycastLeftDownHit.point.y;
		if (rightGrounded == true) rightHit = raycastRightDownHit.point.y;

		float highestGround = Mathf.Max (backHit, centerHit, frontHit, leftHit, rightHit);

		highestHit = raycastDownHit;

		if (highestGround == backHit) 
		{
			backHighest = true;
			highestHit = raycastBackDownHit;
		}

		if (highestGround == frontHit)
		{
			frontHighest = true;
			highestHit = raycastFrontDownHit;
		}

		if (highestGround == leftHit)
		{
			leftHighest = true;
			highestHit = raycastLeftDownHit;
		}

		if (highestGround == rightHit)
		{
			rightHighest = true;
			highestHit = raycastRightDownHit;
		}

		if (highestGround == centerHit)
		{
			centerHighest = true;
			highestHit = raycastDownHit;
		}
	}
	void AverageGroundedNormal()
	{
		Vector3 backHitNorm = Vector3.zero;
		Vector3 centerHitNorm = Vector3.zero;
		Vector3 frontHitNorm = Vector3.zero;
		Vector3 leftHitNorm = Vector3.zero;
		Vector3 rightHitNorm = Vector3.zero;

		if (backGrounded == true) backHitNorm = raycastBackDownHit.normal;
		if (downGrounded == true) centerHitNorm = raycastDownHit.normal;
		if (frontGrounded == true) frontHitNorm = raycastFrontDownHit.normal;
		if (leftGrounded == true) leftHitNorm = raycastLeftDownHit.normal;
		if (rightGrounded == true) rightHitNorm = raycastRightDownHit.normal;

		averageNormal = (backHitNorm + centerHitNorm + frontHitNorm + leftHitNorm + rightHitNorm) / groundRayAmount;
	}
		
	void RaycastExtender()
	{
		if (isGrounded) 
		{
			raycastPadding = highestHit.distance + 1.5f;
			if (curlBool) raycastPadding = highestHit.distance + 2f;
		}

		if (!isGrounded)
		{
			if (finalAirMoveVector.y > 0) raycastPadding = -playerHeight/2;
			if (finalAirMoveVector.y <= 0 || airHitBool == true) raycastPadding = 1f;
		}
	}

	void AirHitReset()
	{
		airHitBool = false;
	}

	void Jump()
	{
		if ((inputJumpPress || inputJumpHold) && waitForJumpUp == false) 
		{
			float jumpPower = 32;

			if (!jumpBool) jumpVector = averageNormal.normalized * jumpPower;

			if (!firstJumpFrame) firstJumpFrame = true;
			if (!jumpBool) 
			{
				jumpAudio.Play();
				jumpBool = true;
				hasJumped = true;
				noInput = true;
			}
				
			isGrounded = false;
			waitForJumpUp = true;
		}

		if (inputJumpRelease || !inputJumpHold) waitForJumpUp = false;
	}
	void AirJump()
	{
		if ((inputJumpPress || inputJumpHold) && waitForGround == false && waitForJumpUp == false && smoothFall == false && smoothFallDirection == false)
		{
			float jumpPower = 15;

			if (finalAirMoveVector.y <= 0)
			{
				gravityVector = Vector3.zero;
				currentSpeed = 0;
				jumpVector = -gravityDirection * jumpPower;
				firstJumpFrame = true;
				jumpBool = true;
			}

			if (finalAirMoveVector.y > 0)
			{
				jumpVector = -gravityDirection * jumpPower;
				firstJumpFrame = true;
				jumpBool = true;
			}

			if (!curlBool) curlBool = true;

			waitForJumpUp = true;
			waitForGround = true;
		}

		if (inputJumpRelease || !inputJumpHold) waitForJumpUp = false;
	}

	void JumpStop()
	{
		if (hasJumped && inputJumpRelease && !jumpStopDisable) 
		{
			if (finalAirMoveVector.y <= 60) 
			{
				jumpStop = true;
			}
		}

		if (finalAirMoveVector.y > 60) 
		{
			jumpStop = false;
			jumpStopDisable = true;
			return;
		}

		if (jumpStop && !jumpStopDisable) 
		{
			float stopSpeed = 6;

			gravityVector = Vector3.Lerp (gravityVector, Vector3.zero, (stopSpeed) * Time.deltaTime);

			if (finalAirMoveVector.y > 20) stopSpeed *= 0.5f;

			moveVector = Vector3.Lerp (moveVector, Vector3.zero, stopSpeed * Time.deltaTime);

			if (finalAirMoveVector.y <= 0) jumpStop = false;
		}
	}

	void ElectricShieldJump()
	{
		if ((inputAbility) && waitForGround == false && smoothFall == false && smoothFallDirection == false)
		{
			float jumpPower = 30;

			if (finalAirMoveVector.y <= 0) 
			{
				groundYDir = 0;
				gravityVector = Vector3.zero;
				airVertMoveVector = Vector3.zero;
			}

			gravityVector += (-gravityDirection * jumpPower);

			if (!curlBool) curlBool = true;

			waitForGround = true;
		}
	}

	void Curl()
	{
		if (inputCurlPress && !inputBackspin && isGrounded) 
		{
			if (!curlBool) 
			{
				if (highSpeedMode) 
				{
					spinAudio.Play ();
				}

				if (currentSpeed > 10 || vertRotationAngle > 15) curlAudio.Play ();

				curlBool = true;
			} 

			else 
			{
				curlAudio.Play ();
				curlBool = false;
			}
		}

		if (currentSpeed <= 10 && vertRotationAngle <= 15 && isGrounded) curlBool = false;

		if (inputCurlPress && !isGrounded)
		{
			waitForCurlUp = true;
		}

		if (inputCurlRelease && !inputBackspin && waitForCurlUp && !isGrounded) 
		{
			if (!curlBool) 
			{
				curlAudio.Play ();
				curlBool = true;
			}

			else 
			{
				curlAudio.Play ();
				curlBool = false;

				if (airVertMoveVector.y >= 0 && hasJumped) 
				{
					airUncurl = true;
				}
			}
		}

		if (airUncurl) 
		{
			float uncurlSpeed = 12;

			gravityVector = Vector3.Lerp(gravityVector, Vector3.zero, (uncurlSpeed) * Time.deltaTime);

			if (finalAirMoveVector.y > 20) uncurlSpeed *= 0.5f;

			moveVector = Vector3.Lerp(moveVector, Vector3.zero, uncurlSpeed * Time.deltaTime);

			if (finalAirMoveVector.y <= 0)  airUncurl = false;
		}

		if (isGrounded)
		{
			waitForCurlUp = false;
			airUncurl = false;
		}

		//Hold Curl Code: if (!inputCurl && !jumpBool || inputBackspin) curlBool = false;
	}

	void Backspin()
	{
		if (inputBackspin) backspinBool = true;
		else if (backspinBool == true) 
		{
			if (reapplyCurl == true) curlBool = true;
			backspinBool = false;
			reapplyCurl = false;
		}

		if (backspinBool) 
		{
			if (!brakeAudio.isPlaying) brakeAudio.Play();
			if (brakeAudio.isPlaying && currentSpeed <= 15) brakeAudio.Stop (); 
			if (curlBool == true) 
			{
				curlBool = false;
				reapplyCurl = true;
			}

			if (inputCurlPress) reapplyCurl = true;

			float backspinFactor = 1;
			if (currentSpeed >= TOPSPEED - 30) backspinFactor *= 1.8f;
			if (currentSpeed >= TOPSPINDASHSPEED && currentSpeed < TOPSPEED - 30) backspinFactor *= 1.4f;

			float backspinAmount = BACKSPINDECCEL * backspinFactor;
			float backspinTime = 5;

			currentBackspin = Mathf.Lerp (currentBackspin, backspinAmount, backspinTime * Time.deltaTime);

			currentSpeed -= currentBackspin * Time.deltaTime;
		}

		else 
		{ 
			brakeAudio.Stop ();
			currentBackspin = 0;
		}
	}
	void AirBackspin()
	{
		brakeAudio.Stop ();

		if (inputBackspin) 
		{
			backspinBool = true;
		}
		else if (backspinBool == true) 
		{
			if (reapplyCurl == true) curlBool = true;
			backspinBool = false;
			reapplyCurl = false;
		}

		if (backspinBool == true) 
		{
			if (curlBool == true) 
			{
				curlBool = false;
				reapplyCurl = true;
			}
		}
	}

	void SmoothFall()
	{
		if (vertRotationAngle >= 70 && (noInput == true || currentSpeed < TOPACCEL / 2.5f)) 
		{
			if (!curlBool) 
			{
				if (uphillMode == true && currentSpeed < TOPACCEL - 50 || (noInput == true && currentSpeed < TOPACCEL - 20) || uphillMode == false && currentSpeed < TOPACCEL / 2.5f) smoothFall = true;
			
				if (vertRotationAngle >= 90 && currentSpeed >= TOPACCEL / 2.5f && noInput == false) smoothFall = false;

				if (vertRotationAngle >= 90 && currentSpeed < TOPACCEL / 2.5f) smoothFall = true;

				if (smoothFall == true) smoothFallDirection = true;
			}

			if (currentSpeed < TOPACCEL / 2.5f && curlBool && vertRotationAngle >= 80) 
			{
				smoothFall = true;

				if (smoothFall == true) smoothFallDirection = true;
			}
		}

		if (jumpBool == true)
		{
			smoothFall = true;
			if (vertRotationAngle >= 5) smoothFallDirection = true;
		}

		if (downhillMode == true && jumpBool == false) 
		{
			smoothFall = false;
			smoothFallDirection = false;
		}
	}
	void SmoothFallReset()
	{
		smoothFall = true;
		smoothFallDirection = true;

		if (vertRotationAngle <= 5 || airHitBool == true) 
		{
			if (jumpBool == false)
			{
				smoothFall = false;
				smoothFallDirection = false;
			}
		}
	}
	void RotatePlayer()
	{
		if (isGrounded) 
		{
			float rotateSpeed = 40f;

			if (currentSpeed > TOPACCEL) rotateSpeed *= 2f;
			if (currentSpeed >= TOPSPEED - 20) rotateSpeed *= 3f;
			 
			playerRB.MoveRotation (Quaternion.FromToRotation (transform.up, averageNormal) * forwardRef.transform.rotation);
		}

		if (!isGrounded) 
		{
			float rotateSpeed = 10;

			if (curlBool == true) rotateSpeed = 15;

			if (airHitBool == true) 
			{
				playerRB.MoveRotation (Quaternion.FromToRotation (-gravityDirection, airHit.normal));
			}
				
			if (airHitBool == false)
			{
				playerRB.MoveRotation (Quaternion.Lerp (playerRB.rotation, Quaternion.AngleAxis (0, playerRB.position), rotateSpeed * Time.deltaTime));
			}
		}

		vertRotationAngle = Vector3.Angle (playerRB.transform.up, -gravityDirection);
	}

	void Accelerating()
	{
		float accelFactor = 1;

		if (currentSpeed >= TOPACCEL - 20) accelFactor = 0.2f;

		if ((inputX != 0 || inputY != 0) && currentSpeed < TOPACCEL && slopeDisableAccel == false && curlBool == false && vertRotationAngle < 160 && backspinBool == false) 
		{
			currentSpeed += (ACCELERATION * accelFactor) * Time.deltaTime;

			if (Mathf.Abs(currentSpeed) >= TOPACCEL) currentSpeed = TOPACCEL * Mathf.Sign(currentSpeed);
		}
	}
	void AirAccelerating()
	{
		float accelFactor = 0.9f;

		if (airLatSpeed >= TOPACCEL - 20) accelFactor = 0.2f;

		if ((inputX != 0 || inputY != 0) && airLatSpeed < TOPACCEL && angleDifference < 40) 
		{
			airLatSpeed += (ACCELERATION * accelFactor) * Time.deltaTime;

			if (Mathf.Abs (airLatSpeed) >= TOPACCEL) 
			{
				airLatSpeed = TOPACCEL * Mathf.Sign (airLatSpeed);
			}

			latMoveVector = latMoveVector.normalized * airLatSpeed;
		}
	}
	void Friction()
	{
		float frictionFactor = 1;

		if ((currentSpeed <= TOPACCEL -19) && noInput == false && turnFriction) frictionFactor *= 3.5f;
		if ((currentSpeed <= TOPACCEL && currentSpeed > TOPACCEL -20) && noInput == false && turnFriction) frictionFactor *= 1.4f;
		if (currentSpeed < TOPSPINDASHSPEED && currentSpeed > TOPACCEL && noInput == false && turnFriction) frictionFactor *= 1.2f;

		if (Mathf.Abs (angleDifference) < 70 && noInput == false && turnFriction) frictionFactor *= 0.6f;
		if (Mathf.Abs (angleDifference) >= 70 && noInput == false && turnFriction) frictionFactor *= 1.5f;

		if (curlBool) 
		{
			frictionFactor *= 0.45f;
			if (turnFriction)frictionFactor *= 1.5f;
		}

		if (currentSpeed >= TOPSPEED - 30 && noInput == false && turnFriction) frictionFactor *= 2.2f;
		if (currentSpeed >= TOPSPINDASHSPEED && noInput == false && turnFriction) frictionFactor *= 1.85f;

		if (((noInput == true || turnFriction == true || curlBool) && backspinBool == false) || vertRotationAngle >= 160) 
		{
			if (vertRotationAngle >= 160) 
			{
				if (curlBool == false) frictionFactor *= 0.6f;
				else frictionFactor *= 3f;
			}

			if (currentSpeed > (FRICTION * frictionFactor) * Time.deltaTime) currentSpeed -= (FRICTION * frictionFactor) * Time.deltaTime;
			if (currentSpeed <= ((FRICTION * frictionFactor) * Time.deltaTime) && !slopeTurn) currentSpeed = 0;
		}
	}
	void AirDrag()
	{
		float airFrictionFactor = 0.5f;
		if (airLatSpeed < TOPSPINDASHSPEED && airLatSpeed > TOPACCEL && noInput == false && turnFriction) airFrictionFactor *= 1.1f;
		if ((airLatSpeed <= TOPACCEL && airLatSpeed > TOPACCEL -20) && noInput == false && turnFriction) airFrictionFactor *= 1.4f;
		if ((airLatSpeed <= TOPACCEL -19) && noInput == false && turnFriction) airFrictionFactor *= 3.5f;
		if (airLatSpeed >= TOPSPEED - 30 && noInput == false && turnFriction) airFrictionFactor *= 2.5f;
		if (airLatSpeed >= TOPSPINDASHSPEED && noInput == false && turnFriction) airFrictionFactor *= 1.9f;

		if (turnFriction || noInput && !jumpBool && !smoothFall && !smoothFallDirection)
		{
			airLatSpeed -= (FRICTION * airFrictionFactor) * Time.deltaTime;

			if (airLatSpeed <= 0) airLatSpeed = 0;

			latMoveVector = latMoveVector.normalized * airLatSpeed;
		}
	}

	void SlopeSpeed()
	{
		Vector3 finalAngle = new Vector3 (1, finalDirectionRotation.y, 1);
		Vector3 inputAngle = new Vector3 (1, inputDirection.y, 1);

		slopeSpeed = -Vector3.Angle (finalAngle, inputAngle) * Mathf.Sign(finalDirectionRotation.y);

		if (finalDirectionRotation.y > 0) slopeSpeed *= 1.19f;
		if (finalDirectionRotation.y <= 0) slopeSpeed *= 1.2f;
		if (finalDirectionRotation.y <= 0 && vertRotationAngle >= 100) slopeSpeed *= 1.5f;

		if (curlBool)
		{
			if (finalDirectionRotation.y > 0) slopeSpeed *= 0.87f;
			if (finalDirectionRotation.y <= 0) slopeSpeed *= 1.54f;
		}

		if (backspinBool)
		{
			if (finalDirectionRotation.y > 0) slopeSpeed *= 0.9f;
			if (finalDirectionRotation.y <= 0) slopeSpeed *= 0.5f;
		}

		if (slopeSpeed > -0.1f && slopeSpeed <= 0 && curlBool == false) slopeSpeed = 0;
		if (slopeSpeed < 0.1f && slopeSpeed >= 0 && curlBool == false) slopeSpeed = 0;

		if (currentSpeed < 1 && vertRotationAngle <= 15) slopeSpeed = 0; 

		if (vertRotationAngle >= 65 && vertRotationAngle < 165 && slopeSpeed >= -10f && slopeSpeed <= 0 && uphillMode == false) 
		{
			slopeSpeed = -10f;
		}

		if (vertRotationAngle >= 165 && slopeSpeed <= 0) 
		{
			slopeSpeed = -10f;
		}

		if (downhillMode == false && Mathf.CeilToInt (currentSpeed) <= 0.1f || slopeTurn) 
		{
			slopeDisableAccel = true;
		}

		if ((downhillMode || slopeSpeed >= -0.05) && !slopeTurn) slopeDisableAccel = false;

		currentSpeed += slopeSpeed * Time.deltaTime;

		LimitSpeedToZero ();
	}
	void LimitSpeedToZero()
	{
		if (currentSpeed < 0) currentSpeed = 0;
	}

	void LocalGravity ()
	{
		globalGravityAmount = gameController.gravityAmount;
		gravityVector += (gravityDirection * globalGravityAmount) * Time.deltaTime;
	}

	void TopSpeedLimiter()
	{
		if (Mathf.Abs(currentSpeed) >= TOPSPEED) currentSpeed = TOPSPEED * Mathf.Sign(currentSpeed);
	}

	void GroundMoveVector()
	{
		moveVector = (finalDirectionRotation * currentSpeed);

		latMoveVector = new Vector3 (moveVector.x, 0, moveVector.z);

		groundYDir = finalDirectionRotation.y;

		if (currentSpeed >= 100) 
		{
			if (highSpeedMode == false) boomAudio.Play (); 
			highSpeedMode = true;
		}
		if (currentSpeed < 80) highSpeedMode = false;
	}
	void GroundStick()
	{
		float stickPadding = 1.2f;

		if (curlBool || backspinBool) 
		{
			stickPadding = 0.73f;
		}

		if (smoothFall == false)
		{
			groundStick = true;
		}

		if (smoothFall == true || debugMovement == true || jumpBool == true) 
		{
			groundStick = false;
		}

		Vector3 stickPosition = playerRB.position - ((highestHit.distance - (stickPadding +0.02f)) * highestHit.normal.normalized);
		float stickStrength = 25;

		if (currentSpeed >= TOPSPINDASHSPEED) stickStrength *= 0.83f;

		if(groundStick == true) playerRB.MovePosition(Vector3.Lerp(playerRB.position, stickPosition, stickStrength * Time.deltaTime));
	}
	void FinalAirMoveVector()
	{
		if (noInput == false && (smoothFall == false || jumpBool == true)) 
		{
			airLatMoveVector = directionRotation * latMoveVector.magnitude;
			latMoveVector = airLatMoveVector.normalized * latMoveVector.magnitude;
		}

		if (noInput == true || smoothFall == true) 
		{
			airLatMoveVector = latMoveVector;
		}

		airVertMoveVector = new Vector3 (0, moveVector.y, 0) + gravityVector;

		finalAirMoveVector = airLatMoveVector + airVertMoveVector;

		if (finalAirMoveVector.magnitude >= 100) 
		{
			highSpeedMode = true;
		}

		if (finalAirMoveVector.magnitude <= 80) highSpeedMode = false;

		FirstJumpFrame ();
	}
	void FirstJumpFrame()
	{
		if (firstJumpFrame) 
		{
			curlBool = true;
			finalAirMoveVector += jumpVector;
			latMoveVector = new Vector3 (finalAirMoveVector.x, 0, finalAirMoveVector.z);
			airLatMoveVector = latMoveVector;
			gravityVector.y += jumpVector.y;
			jumpVector = Vector3.zero;

			float groundDirectionDiffAngle = Vector3.Angle (new Vector3 (directionRotation.x, 0, directionRotation.z), new Vector3 (finalAirMoveVector.x, 0, finalAirMoveVector.z));

			if (groundDirectionDiffAngle > 90) 
			{
				Vector3 targetRotation = groundAngleDirection;
				targetRotation.y = 0;
				targetRotation = targetRotation.normalized;

				inputDirection = -targetRotation;
				directionRotation = -targetRotation;
				lastDirectionRotation = -targetRotation;
			}

			firstJumpFrame = false;
		}
	}

	void StickToWall(Collision collision)
	{
		if (!isGrounded && collision.collider.gameObject.layer == gameController.terrainLayer) 
		{
			airHit = collision.contacts [0];
			if(smoothFall == false) airHitBool = true;
		}
	}

	void CollectRing(Collider other)
	{
		if (other.CompareTag ("Ring")) 
		{
			ringAudio.Play ();
		}
		if (other.CompareTag ("CrystalRing")) crystalRingAudio.Play ();
		other.gameObject.SetActive (false);
		if (other.CompareTag("Ring")) gameController.ringCount += 1;
		if (other.CompareTag ("CrystalRing")) gameController.crystalRingCount += 1;
		chaosEnergy += 1;
	}
}
