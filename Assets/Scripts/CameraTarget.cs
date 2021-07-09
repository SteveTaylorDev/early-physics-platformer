using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour 
{
	private GameController gameController;

	private Transform playerTransform;
	private Transform player2Transform;

	private bool targetP2;

	void Start () 
	{
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController>();

		playerTransform = GameObject.FindWithTag ("Player").transform;
		if(gameController.coopMode) player2Transform = GameObject.FindWithTag ("Player 2").transform;

		if (gameObject.CompareTag ("CameraTargetP2") && gameController.coopMode) targetP2 = true;
	}
	void Update () 
	{
		FollowPlayer ();
	}

	void FollowPlayer()
	{ 
		Vector3 targetPosition = playerTransform.position;
		if (targetP2) targetPosition = player2Transform.position;

		transform.position = targetPosition;
	}
}
