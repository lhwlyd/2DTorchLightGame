﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObjects : MonoBehaviour {


	//The time it takes for this object to move
	public float moveTime = 0.5f;
	public LayerMask blockingLayer;


    private CircleCollider2D circleCollider2d;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
        circleCollider2d = GetComponent<CircleCollider2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}

	protected IEnumerator SmoothMovement(Vector3 end){
		//Cheaper calculation
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while(sqrRemainingDistance > float.Epsilon){
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
			//Actual move action
			rb2D.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}


	}


	protected bool Move( float xDir, float yDir, out RaycastHit2D hit){
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		circleCollider2d.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		circleCollider2d.enabled = true;

		if (hit.transform == null) {
			//StartCoroutine (SmoothMovement (end));
			return true;
		}

		return false;

	}


}
