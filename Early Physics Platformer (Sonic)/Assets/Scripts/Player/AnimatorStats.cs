using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStats : MonoBehaviour 
{
	private PlayerController playerController;
	private Animator sonicAnimator;

	// Use this for initialization
	void Start () 
	{
		playerController = gameObject.GetComponent<PlayerController> ();
		sonicAnimator = gameObject.GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float animationSpeed = playerController.currentSpeed;

		if (animationSpeed <= 1) animationSpeed = 1;

		sonicAnimator.SetFloat ("currentSpeed", animationSpeed);
		sonicAnimator.SetBool ("isGrounded", playerController.isGrounded);
		sonicAnimator.SetFloat ("currentYSpeed", playerController.finalAirMoveVector.y);
	}
}
