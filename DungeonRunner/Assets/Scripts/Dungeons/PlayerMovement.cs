using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using SynodicArc.DungeonRunner.Items;
using SynodicArc.DungeonRunner.StatusEffects;


/// Player movement.
public class PlayerMovement : MonoBehaviour {

	#region Constants
	const 		float 		GROUNDED_RADIUS = .2f;	// Radius of the overlap circle to determine if grounded
	const 		float 		FORWARD_RADIUS = .2f;	// Radius of the overlap circle to determine if coliding with anythign in front
	const		float		ONE_SECOND = 1000f; 	// 1000 milliseconds is one second
	#endregion Constants

	//public 		bool 		AllowMove = true;		// tracks X movment permissions
	public		bool		AllowAirAceleration = true;	// permits the player to acelerate on asecnt of jumps
	public 		bool 		HasDoubleJumped = false;// tracks if player has double jumped
	public 		bool 		JumpBuffer = false;		// tracks jump comands, useed for buffering when jumped
	public 		float 		MaxSpeed = 10f;			// max speed the player can travel
	private		float		modMaxSpeed = 10f;		// max speed after mods
	public 		float 		JumpForce = 200f;		// vertical force when jumping
	public		bool		StopPlayer = false;		// prevents input but alows physics
	public		bool		RecentDamage = false;	// tracks if the damage thigns been triggered
	public		bool		AllowAirAccelerationWhenDamage = false; // prevents air aceleration from interupting damage feedback
	private		float		deceleration = 10f;	// slows player movements, only use for gradent aceleration.
	public		float		DecelerationDamage = 20f;	// variable sets how much to decelerate by.
	public		float		DecelerationStopped = 10f;	// variable sets how much to decelerate by.
	
	[SerializeField] private 	LayerMask 	whatIsCeiling;			// A mask determining what is the ceiling to the character
	[SerializeField] private 	LayerMask 	whatIsGround;			// A mask determining what is ground to the character
	[SerializeField] private 	LayerMask 	whatIsObstacle;			// A mask determining what is an obstacle to the character

	private 	Transform 	ceilingCheck;   		// A position marking where to check for ceilings
	private 	Transform 	groundCheck;			// A position marking where to check if the player is grounded.
	private 	Transform 	forwardCheck;			// A position marking where to check if the player is coliding with anything.
	private 	bool 		grounded; 				// Whether or not the player is grounded.
	private 	Animator 	animate;            	// Reference to the player's animator component.
	private 	Rigidbody2D m_Rigidbody2D;			// references the player object
	private		bool		alive = true;					// Is the player alive? Needs to be to perform actions.
	
	private bool m_Jump;

	//References to other parts of player
	private PlayerParameters playerParameters;


	private void Awake() {
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		forwardCheck = transform.Find("ForwardCheck");
		animate = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		playerParameters = this.GetComponent<PlayerParameters> ();

		//Setting up values
		modMaxSpeed = MaxSpeed;
	}
	
	///Handles collision checks on update
	private void FixedUpdate() {
		//if not alive, can't do anything
		if (alive) {
			//bool crouch = Input.GetKey(KeyCode.LeftControl);
		
			grounded = false;
		
			//check if the player is grounded using a circlecast to ground. if it hits, it sticks
			Collider2D[] colliders = Physics2D.OverlapCircleAll (groundCheck.position, GROUNDED_RADIUS, whatIsGround);
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject != gameObject) {
					grounded = true;
					if (!RecentDamage) {
						AllowAirAccelerationWhenDamage = true;
					}
				}
			}
			//check if the player is hitting the ceiling using a circlecast to ceiling.
			Collider2D[] collidersCeiling = Physics2D.OverlapCircleAll (ceilingCheck.position, GROUNDED_RADIUS, whatIsGround);
			for (int i = 0; i < collidersCeiling.Length; i++) {
				if (collidersCeiling [i].gameObject != gameObject) {
					Debug.Log ("Hit the ceiling");
					//grounded = true;
					if (!RecentDamage) {
						AllowAirAccelerationWhenDamage = true;
					}
				}
			}
			//check if the player is hitting an obstical using a circlecast to obstacle.
			Collider2D[] collidersForward = Physics2D.OverlapCircleAll (forwardCheck.position, FORWARD_RADIUS, whatIsObstacle);
			for (int i = 0; i < collidersForward.Length; i++) {
				if (collidersForward [i].gameObject != gameObject) {
					Debug.Log ("we collided!");
					NotMoving ();
					//if (!recentDamage) {alowAirAcelerationWhenDamage = true;}
				}
			}
		
			Move (1f, false, m_Jump, false, false);
			animate.SetBool ("Ground", grounded);
			animate.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
		}
	}
	
	private void Update() {
		if (!m_Jump) {
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}
	
	//handels damage comands
	public void Damage(float force) {
		//force is how hard far the player jumps back
		Debug.Log("Damage feedback triggered for force of: " + force);
		Move (force, false, false, false, true);
	}
	
	//handels setting the none moving state
	public void NotMoving() {
		DecelerationStopped = 10f;
	}
	
	public void Move(float move, bool crouch, bool jump, bool sprint, bool damage) {
		if (!StopPlayer) {  //prevents player movment, may be usefull?
			
//------------------------------------//!!!SETS FORWARD MOVEMENT!!!//-----------------------------------------------------
			
			//handels the ckecks for alowing double jump after taking damage
			if (jump && !RecentDamage) {
				//Debug.Log(recentDamage + " : " + grounded);
				deceleration = 1f;
				AllowAirAccelerationWhenDamage = true;
			}
			//reduces speed if crouching
			move = (crouch ? move*0.5f : move);
			//only sets the characters forward vector when grounded or air aceleration is permitted
			if (!RecentDamage && grounded) {
				if (deceleration > 1f) {
					deceleration = deceleration - (0.25f * deceleration);
					if (deceleration < 1) {deceleration = 1f;};
					//Debug.Log("acelerating-"+deAceleration);
				}
				
				//The Speed animator parameter is set to the absolute value of the horizontal input.
				animate.SetFloat("Speed", Mathf.Abs(move));
				//sets the forward velocity
				m_Rigidbody2D.velocity = new Vector2(move*modMaxSpeed/deceleration, m_Rigidbody2D.velocity.y);
			} else if (AllowAirAccelerationWhenDamage && AllowAirAceleration) {
				//if (RecentDamage) {Debug.Log("!!! AIR");}
				animate.SetFloat("Speed", Mathf.Abs(move));
				
				//only acelerates on the asent of the jump.
				if (m_Rigidbody2D.velocity.y > 5) {
					m_Rigidbody2D.velocity = new Vector2(move*modMaxSpeed/deceleration, m_Rigidbody2D.velocity.y);
				}
			}
			
//----------------------------------------------------------------------------------------------------------------------
			
			if (grounded) {
				if (JumpBuffer) {
					//fires a jump if a jump has been buffered
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, JumpForce);
				}
				//clears the doublejump and jumpbuffer limiter ONLY when grounded
				HasDoubleJumped = false;
				JumpBuffer = false;
			}
			
//----------------------------------------------------------------------------------------------------------------------
			
			// If the player should jump...
            if (grounded && jump && animate.GetBool("Ground")) {
                // Add vertical velocity to the player.
                Debug.Log("jump1");
				grounded = false;
                animate.SetBool("Ground", false);
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, JumpForce);
			} else if (!grounded && jump && !HasDoubleJumped && !RecentDamage) {
				//handels double jumping
				Debug.Log("jump2");
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, JumpForce);
				HasDoubleJumped = true;
			} else if (!grounded && jump && HasDoubleJumped) {
				//invokes a temporary state in which hte character will jump at the next posible chance. times out after 1/8th of a second
				Debug.Log("jump3");
				JumpBuffer = true;
				Invoke("ClearJumpBuffer", 0.125f);
			}
			m_Jump = false;
			
//----------------------------------------------------------------------------------------------------------------------
			
			//applies damage movment
			if (damage) {
				RecentDamage = true;
				grounded = false;
				AllowAirAccelerationWhenDamage = false;
				animate.SetBool("Ground", false);
				//damage force is multiplied by the MaxSpeed and JumpForce for aplication
				m_Rigidbody2D.velocity = new Vector2(((move*modMaxSpeed) / -1f), move*JumpForce/1.25f);
				deceleration = DecelerationDamage;
				Invoke("ClearDamage", 0.25f);
			}
		} else {
			m_Rigidbody2D.velocity = Vector2.zero;
		}
	}
	
	//clears the jump buffer
	public void ClearJumpBuffer() {
		JumpBuffer = false;
		Debug.Log("buffered jump spent");
	}
	//clears the recent damage value
	public void ClearDamage() {
		RecentDamage = false;
		Debug.Log("Damage jump cleared");
	}


	#region ActiveMovementModifiers
	public void SpeedUp(float speedUpAmt, float duration){
		Debug.Log ("Starting to speed up!");
		modMaxSpeed += speedUpAmt;
		StartCoroutine (EndSpeedUp (speedUpAmt, duration));
//		Timer tmr = new Timer (duration * ONE_SECOND);
//		tmr.Start ();
//		Debug.Log ("Starting timer.");
//		tmr.Elapsed += delegate {
//			modMaxSpeed -= speedUpAmt;
//			tmr.Stop();
//			tmr.Close();
//			Debug.Log ("Ending timer.");
//		};
	}

	private IEnumerator EndSpeedUp(float speedUpAmt, float duration){
		yield return new WaitForSeconds (duration);
		modMaxSpeed -= speedUpAmt;
	}

	public void SlowDown(float slowDownAmt, float duration){
		Debug.Log ("Starting to slow down.");
		modMaxSpeed -= slowDownAmt;
		StartCoroutine (EndSlowDown (slowDownAmt, duration));
	}

	private IEnumerator EndSlowDown(float slowDownAmt, float duration){
		yield return new WaitForSeconds (duration);
		modMaxSpeed += slowDownAmt;
	}

	#endregion ActiveMovementModifiers

	#region Death
	/// Kills the player and they can no longer move.
	public void KillPlayer(){
		alive = false;
	}
	#endregion Death
}