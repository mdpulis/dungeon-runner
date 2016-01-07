using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;



public class PlayerMovment : MonoBehaviour {
	
	public 		bool 		alowMove = true;		// tracks X movment permissions
	public		bool		alowAirAceleration = true;	// permits the player to acelerate on asecnt of jumps
	public 		bool 		hasDoubleJumped = false;// tracks if player has double jumped
	public 		bool 		jumpBuffer = false;		// tracks jump comands, useed for buffering when jumped
	public 		float 		maxSpeed = 10f;			// max speed the player can travel
	public 		float 		jumpForce = 200f;		// vertical force when jumping
	public		bool		stopPlayer = false;		// prevents input but alows physics
	public		bool		recentDamage = false;	// tracks if the damage thigns been triggered
	public		bool		alowAirAcelerationWhenDamage = false; // prevents air aceleration from interupting damage feedback
	private		float		deAceleration = 10f;	// slows player movments, only use for gradent aceleration.
	public		float		deAcelerationDamage = 20f;	// varable sets how much to deacelerate by.
	public		float		deAcelerationStoped = 10f;	// varable sets how much to deacelerate by.
	
	[SerializeField] private 	LayerMask 	whatIsCeiling;			// A mask determining what is the ceiling to the character
	[SerializeField] private 	LayerMask 	whatIsGround;			// A mask determining what is ground to the character
	[SerializeField] private 	LayerMask 	whatIsObstacle;			// A mask determining what is an obstacle to the character
	const 		float 		groundedRadius = .2f;	// Radius of the overlap circle to determine if grounded
	const 		float 		forwardRadius = .2f;	// Radius of the overlap circle to determine if coliding with anythign in front
	private 	Transform 	ceilingCheck;   		// A position marking where to check for ceilings
	private 	Transform 	groundCheck;			// A position marking where to check if the player is grounded.
	private 	Transform 	forwardCheck;			// A position marking where to check if the player is coliding with anything.
	private 	bool 		grounded; 				// Whether or not the player is grounded.
	private 	Animator 	animate;            	// Reference to the player's animator component.
	private 	Rigidbody2D m_Rigidbody2D;			// references the player object
	
	private bool m_Jump;
	
	private void Awake() {
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		forwardCheck = transform.Find("ForwardCheck");
		animate = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	//handels ground checks on update
	private void FixedUpdate() {
		//bool crouch = Input.GetKey(KeyCode.LeftControl);
		
		grounded = false;
		
		//check if the player is grounded using a circlecast to ground. if it hits, it sticks
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				grounded = true;
				if (!recentDamage) {alowAirAcelerationWhenDamage = true;}
			}
		}
		//check if the player is hitting the ceiling using a circlecast to ceiling.
		Collider2D[] collidersCeiling = Physics2D.OverlapCircleAll(ceilingCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < collidersCeiling.Length; i++) {
			if (collidersCeiling[i].gameObject != gameObject) {
				Debug.Log("Hit the ceiling");
				//grounded = true;
				if (!recentDamage) {alowAirAcelerationWhenDamage = true;}
			}
		}
		//check if the player is hitting an obstical using a circlecast to obstacle.
		Collider2D[] collidersForward = Physics2D.OverlapCircleAll(forwardCheck.position, forwardRadius, whatIsObstacle);
		for (int i = 0; i < collidersForward.Length; i++) {
			if (collidersForward[i].gameObject != gameObject) {
				Debug.Log("we colided!");
				//grounded = true;
				//if (!recentDamage) {alowAirAcelerationWhenDamage = true;}
			}
		}
		
		Move (1f, false, m_Jump, false, false);
		animate.SetBool("Ground", grounded);
		animate.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
	}
	
	private void Update() {
		if (!m_Jump) {
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}
	
	public void Damage(float force) {
		//force is how hard far the player jumps back
		//stopPlayer = true;
		Debug.Log("Damage feedback triggered for force of: " + force);
		Move (force, false, false, false, true);
	}
	
	public void Notmoving() {
		deAcelerationStoped = 10f;
		animate.SetBool("standing", grounded);
		animate.SetFloat("Speed", Mathf.Abs(0f));
	}
	
	public void Move(float move, bool crouch, bool jump, bool sprint, bool damage) {
		if (!stopPlayer) {  //prevents player movment, may be usefull?
			
//------------------------------------//!!!SETS FORWARD MOVMENT!!!//-----------------------------------------------------
			
			
			//reduces speed if crouching
			move = (crouch ? move*0.5f : move);
			//only sets the characters forward vector when grounded or air aceleration is permitted
			if (!recentDamage && grounded) {
				if (recentDamage) {Debug.Log("!!! GROUND");}
				//
				if (deAceleration > 1f) {
					deAceleration = deAceleration - (0.25f * deAceleration);
					if (deAceleration < 1) {deAceleration = 1f;};
					Debug.Log("acelerating-"+deAceleration);
				}
				
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				animate.SetFloat("Speed", Mathf.Abs(move));
				//sets the forward velocity
				m_Rigidbody2D.velocity = new Vector2(move*maxSpeed/deAceleration, m_Rigidbody2D.velocity.y);
			} else if (alowAirAcelerationWhenDamage && alowAirAceleration) {
				if (recentDamage) {Debug.Log("!!! AIR");}
				animate.SetFloat("Speed", Mathf.Abs(move));
				
				//only acelerates on the asent of the jump.
				if (m_Rigidbody2D.velocity.y > 5) {
					m_Rigidbody2D.velocity = new Vector2(move*maxSpeed/deAceleration, m_Rigidbody2D.velocity.y);
				}
			}
			
//----------------------------------------------------------------------------------------------------------------------
			
			if (grounded) {
				if (jumpBuffer) {
					//fires a jump if a jump has been buffered
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
				}
				//clears the doublejump and jumpbuffer limiter ONLY when grounded
				hasDoubleJumped = false;
				jumpBuffer = false;
			}
			
//----------------------------------------------------------------------------------------------------------------------
			
			// If the player should jump...
            if (grounded && jump && animate.GetBool("Ground")) {
                // Add vertical velocity to the player.
                Debug.Log("jump1");
				grounded = false;
                animate.SetBool("Ground", false);
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
			} else if (!grounded && jump && !hasDoubleJumped) {
				//handels double jumping
				Debug.Log("jump2");
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
				hasDoubleJumped = true;
			} else if (!grounded && jump && hasDoubleJumped) {
				//invokes a temporary state in which hte character will jump at the next posible chance. times out after 1/8th of a second
				Debug.Log("jump3");
				jumpBuffer = true;
				Invoke("ClearJumpBuffer", 0.125f);
			}
			m_Jump = false;
			
//----------------------------------------------------------------------------------------------------------------------
			
			//damage
			if (damage) {
				recentDamage = true;
				grounded = false;
				alowAirAcelerationWhenDamage = false;
				animate.SetBool("Ground", false);
				//animate.SetBool("Damage", true);
				m_Rigidbody2D.velocity = new Vector2(((move*maxSpeed) / -1f), jumpForce);
				deAceleration = deAcelerationDamage;
				Invoke("ClearDamage", 0.5f);
			}
		}
	}
	
	//clears the jump buffer
	public void ClearJumpBuffer() {
		jumpBuffer = false;
		Debug.Log("buffered jump spent");
	}
	//clears the recent damage value
	public void ClearDamage() {
		recentDamage = false;
		Debug.Log("Damage jump cleared");
	}
}