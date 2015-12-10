using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;



public class PlayerMovment : MonoBehaviour {
	
	public 		bool 		alowMove = true;		// tracks X movment permissions
	public		bool		AlowAirAceleration = true;	// permits the player to acelerate on asecnt of jumps
	public 		bool 		hasDoubleJumped = false;// tracks if player has double jumped
	public 		bool 		jumpBuffer = false;		// tracks jump comands, useed for buffering when jumped
	public 		float 		maxSpeed = 10f;			// max speed the player can travel
	public 		float 		jumpForce = 200f;		// vertical force when jumping
	public		bool		stopPlayer = false;		// prevents input but alows physics
	public		bool		recentDamage = false;	// tracks if the damage thigns been triggered
	public		bool		AlowAirAcelerationWhenDamage = false; // prevents air aceleration from interupting damage feedback
	
	[SerializeField] private 	LayerMask 	whatIsGround;			// A mask determining what is ground to the character
	const 		float 		groundedRadius = .2f;	// Radius of the overlap circle to determine if grounded
	private 	Transform 	ceilingCheck;   		// A position marking where to check for ceilings
	private 	Transform 	groundCheck;			// A position marking where to check if the player is grounded.
	private 	bool 		grounded; 				// Whether or not the player is grounded.
	private 	Animator 	animate;            	// Reference to the player's animator component.
	private 	Rigidbody2D m_Rigidbody2D;			// references the player object
	
	

	private bool m_Jump;	
	
	
	
	
	
	private void Awake() {
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		animate = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	//handels ground checks on update
	private void FixedUpdate() {
		//Read the inputs.
		//bool crouch = Input.GetKey(KeyCode.LeftControl);
		//float h = CrossPlatformInputManager.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		//m_Character.Move(h, crouch, m_Jump);
		
		grounded = false;
		
		//check if the player is grounded using a circlecast to ground. if it hits, it sticks
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject)
				grounded = true;
				if (!recentDamage) {AlowAirAcelerationWhenDamage = true;}
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
		//stopPlayer = true;
		Debug.Log("Damage feedback triggered");
		Move (1f, false, false, false, true);
	}
	
	public void Move(float move, bool crouch, bool jump, bool sprint, bool damage) {
		if (!stopPlayer) {
			
//----------------------------------------------------------------------------------------------------------------------
			move = (crouch ? move*0.5f : move);
			//only sets the characters forward vector when grounded or air aceleration is permitted
			if (!recentDamage && grounded) {
				if (recentDamage) {Debug.Log("!!! GROUND");}
				//reduces speed if crouching
				
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				animate.SetFloat("Speed", Mathf.Abs(move));
				//sets the forward velocity
				m_Rigidbody2D.velocity = new Vector2(move*maxSpeed, m_Rigidbody2D.velocity.y);
			} else if (AlowAirAcelerationWhenDamage && AlowAirAceleration) {
				if (recentDamage) {Debug.Log("!!! AIR");}
				animate.SetFloat("Speed", Mathf.Abs(move));
				
				//only acelerates on the asent of the jump.
				if (m_Rigidbody2D.velocity.y > 5) {
					m_Rigidbody2D.velocity = new Vector2(move*maxSpeed, m_Rigidbody2D.velocity.y);
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
				Invoke("clearJumpBuffer", 0.125f);
			}
			m_Jump = false;
			
//----------------------------------------------------------------------------------------------------------------------
			
			//damage
			if (damage) {
				recentDamage = true;
				grounded = false;
				AlowAirAcelerationWhenDamage = false;
				animate.SetBool("Ground", false);
				//animate.SetBool("Damage", true);
				m_Rigidbody2D.velocity = new Vector2(((move*maxSpeed) / -1f), jumpForce);
				Invoke("clearDamage", 0.5f);
			}
		}
	}
	
	//clears the jump buffer
	public void clearJumpBuffer() {
		jumpBuffer = false;
		Debug.Log("buffered jump spent");
	}
	//clears the recent damage value
	public void clearDamage() {
		recentDamage = false;
		Debug.Log("Damage jump cleared");
	}
	
	
	
}