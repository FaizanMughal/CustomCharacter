using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour {

	Character_Input characterInput;
	Rigidbody mybody;

	private float speed;
	public float walkSpeed;
	public float runSpeed;
	public float crouchSpeed;

	[Space (10)]
	public Camera fpsCam;
	public Camera tpsCam;

	[Space (10)]
	public float jumpForce;

	[Space (10)]
	public float maxFallForce;//much force player can take before gettting injured
	public float baseFallDamage;//doing damage
	private float fallForce;//how much force player hit the ground with

	[Space (10)]
	public Transform groundCheck;
	CapsuleCollider theCollider;


	bool isCroched;
	bool isGrounded;
	bool canJump;
	bool canFly;
	bool fpsView = true;


	void Start () {
		characterInput = GetComponent <Character_Input> ();
		mybody = GetComponent <Rigidbody> ();

		theCollider = GetComponent <CapsuleCollider> ();

		Cursor.lockState = CursorLockMode.Locked;
		speed = walkSpeed;
	}
	

	void Update () {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		Vector3 moveDirection = new Vector3 (horizontal, 0f, vertical) * speed * Time.deltaTime;
		transform.Translate (moveDirection);

		isGrounded = Physics.CheckSphere (groundCheck.transform.position, 0.1f);

		//------------------------fall damage--------------
		if (!isGrounded) {
			float vY = Mathf.Abs( mybody.velocity.y);
			fallForce = vY;
		}

		if (isGrounded) {
			if (fallForce > maxFallForce) {
				float damage = Mathf.RoundToInt( fallForce * baseFallDamage);
				fallForce = 0;
				//call a take damage method here
			}
		}
		//---------------------------------------

		if (Input.GetKeyDown (characterInput.jumpKey) && isGrounded) {
			canJump = !canJump;
		}
		if (Input.GetKeyUp (characterInput.crouchKey)) {
			DoCrouch();
		}
		if (Input.GetKeyDown (characterInput.run) && !isCroched) {
			speed = runSpeed;
		}
		if (Input.GetKeyUp (characterInput.run)) {
			speed = walkSpeed;
		}
		if (Input.GetKeyDown (characterInput.toggleFly)) {
			Fly ();
		}
		if (Input.GetKey (characterInput.flyUp) && canFly) {
			transform.position += transform.up * speed * Time.deltaTime;
		}
		if (Input.GetKey (characterInput.flyDown) && canFly) {
			transform.position -= transform.up * speed * Time.deltaTime;
		}
		if (Input.GetKeyDown (characterInput.toggleCam)) {
			ToggleCamera ();
		}
	}


	void FixedUpdate(){
		if (canJump) {
			mybody.AddForce (Vector3.up * jumpForce);
			canJump = !canJump;
		}
	}
		
	void DoCrouch(){
		if (isCroched) {
			theCollider.height += 1f;
		} else {
			theCollider.height -= 1f;
		}
		isCroched = !isCroched;
	}

	void Fly(){
		canFly = !canFly;
		mybody.isKinematic = canFly;
	}

	void ToggleCamera(){
		fpsView = !fpsView;
		fpsCam.gameObject.SetActive (fpsView);
		tpsCam.gameObject.SetActive (!fpsView);
	}

}
