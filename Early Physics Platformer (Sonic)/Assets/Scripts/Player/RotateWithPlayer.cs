using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithPlayer : MonoBehaviour 
{
	private PlayerController playerController;


	void Start () 
	{
		playerController = gameObject.GetComponentInParent<PlayerController> ();
	}
	void Update () 
	{
		transform.rotation = playerController.playerRB.rotation * Quaternion.Euler (0, playerController.rotationAngle, 0);
	}
}
