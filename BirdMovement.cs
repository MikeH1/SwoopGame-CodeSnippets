using UnityEngine;
using System.Collections;

public class BirdMovement : MonoBehaviour {

	Vector3 velocity = Vector3.zero;
	public Vector3 gravity;
	public float maxSpeed;
	public float minSpeed; 

	public float fowardSpeed;
	public float ySpeed;
	public float Acc;

	float deathCooldown;

	bool KO = false;
	bool holdingBack = false;
	bool speedCheck = false;

	// Use this for initialization
	void Start () {
		SkyMover.speed = 0.9f; //Speed of the background
	}

	//collision dectection
	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log ("Player triggered: " + collider.name);

		//KO script - stops all movement of the background and character while in KO state
		if (collider.tag != "ScoreBox" && KO == false) {
			KO = true;
			ySpeed = 10;
			fowardSpeed = 0;
			SkyMover.speed = 0;
			
		}
	}

	// Do graphic and input updates here
	void Update() {
		if(KO) {
			deathCooldown -= Time.deltaTime;
			if(deathCooldown <= 0) 
			{
				if (Input.GetMouseButton (0)){
				Application.LoadLevel ( Application.loadedLevel );
				}
			}
		}
		if (Input.GetMouseButton (0) && KO == false) {
		holdingBack = true;
		}
	}

	// Do physics engine updates here
	void FixedUpdate () {
		velocity.x = fowardSpeed;
		velocity.y = ySpeed;

		if (ySpeed > 0.5f)
						speedCheck = true;

		velocity += gravity * Time.deltaTime;

		if (holdingBack == true && KO == false) 
			{
				holdingBack = false;

			if (ySpeed < 0f) //Vertical Speed
				ySpeed = ySpeed * 1.3f;
				ySpeed = Mathf.Abs(ySpeed);
				ySpeed -= Acc;
			} 
		else 
			{
			if (speedCheck == true && ySpeed > 0.5f) //to slow down vertical speed if you're not holding down
			{ySpeed -= 0.49f;}

				ySpeed -= Acc;
			}

		if (ySpeed > maxSpeed)
				ySpeed = maxSpeed;

		if (ySpeed < minSpeed)
				ySpeed = minSpeed;

		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

		transform.position += velocity * Time.deltaTime;

		float angle = 0;
		if(velocity.y < 0) 
		{
			angle = Mathf.Lerp(0, -90, -velocity.y / maxSpeed);
		}

		transform.rotation = Quaternion.Euler (0, 0, angle);

	}
}
