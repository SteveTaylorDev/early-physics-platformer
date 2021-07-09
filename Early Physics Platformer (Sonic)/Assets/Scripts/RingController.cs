using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour 
{

	[HideInInspector] public bool attractToPlayer;
	[HideInInspector] public bool attractToPlayer2;

	private GameController gameController;

	private float attractSpeed;

	void Start () 
	{
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
	}
	void Update () 
	{
		RotateRing ();
		if (attractToPlayer || attractToPlayer2) AttractToPlayer ();
	}

	void OnDisable()
	{
		attractSpeed = 0;
	}


	void RotateRing()
	{
		float rotateSpeed = 190;

		transform.rotation *= Quaternion.Euler (0, rotateSpeed * Time.deltaTime, 0);
	}

	void AttractToPlayer()
	{
		float maxAttractSpeed = 180;
		attractSpeed = Mathf.Lerp (attractSpeed, maxAttractSpeed, 5 * Time.deltaTime);

		Vector3 attractPosition = gameController.playerRB.position;
		if (attractToPlayer2) attractPosition = gameController.player2RB.position;
		transform.position = Vector3.Lerp (transform.position, attractPosition, attractSpeed * Time.deltaTime);
	}
}
