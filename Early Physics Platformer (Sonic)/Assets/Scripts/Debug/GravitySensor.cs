using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySensor : MonoBehaviour 
{
	private PlayerController playerController;


	void Start ()
	{
		playerController = gameObject.GetComponentInParent<PlayerController> ();
	}
	void LateUpdate ()
	{
		if (playerController.gravityGrounded == true) transform.position = playerController.raycastGravityHit.point;
		if (playerController.gravityGrounded == false) transform.position = playerController.transform.position - (-playerController.gravityDirection * playerController.gravityRayLength);
	}
}
