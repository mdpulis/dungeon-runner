using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;



public class PlayerMovment : MonoBehaviour {
	
	public 		bool 		alowMove = true;		// tracks X movment permissions
	private 	bool 		hasDoubleJumped = false;// tracks if player has double jumped
	private 	bool 		jumpBuffer = false;		// tracks jump comands, useed for buffering when jumped
	public 		float 		maxSpeed = 10f;			// max speed the player can travel
	public 		float 		jumpForce = 200f;		// vertical force when jumping
	public		bool 		airAceleration = false;		// Whether or not a player can steer while jumping;
	public		bool		stopPlayer = false;		//prevents input but alos physics
	
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
		groundCheck = transform.Find("groundCheck");
		ceilingCheck = transform.Find("ceilingCheck");
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
		Move (1f, false, m_Jump, false);
		
		//m_Jump = false;
		//grounded = false;
		
		//check if the player is grounded using a circlecast to ground. if it hits, it sticks
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject)
				grounded = true;
		}
		
		animate.SetBool("Ground", grounded);
		animate.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
	}
	
	private void Update() {
		if (!m_Jump) {
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}
	
	public void Move(float move, bool crouch, bool jump, bool sprint) {
		if (!stopPlayer) {
			
//----------------------------------------------------------------------------------------------------------------------
			
			//only sets the characters forward vector when grounded or air aceleration is permitted
			if (grounded || airAceleration) {
				//reduces speed if crouching
				move = (crouch ? move*0.5f : move);
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				animate.SetFloat("Speed", Mathf.Abs(move));
				//sets the forward velocity
				m_Rigidbody2D.velocity = new Vector2(move*maxSpeed, m_Rigidbody2D.velocity.y);
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
                grounded = false;
                animate.SetBool("Ground", false);
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
			} else if (!grounded && jump && !hasDoubleJumped) {
				//handels double jumping
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
				hasDoubleJumped = true;
			} else if (!grounded && jump && hasDoubleJumped) {
				//invokes a temporary state in which hte character will jump at the next posible chance. times out after 1/8th of a second
				jumpBuffer = true;
				Invoke("clearJumpBuffer", 0.125f);
			}
			
			
// experimental re write
			
			/*
			
			if (grounded) {
				//reduces speed if crouching
				move = (crouch ? move*0.5f : move);
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				animate.SetFloat("Speed", Mathf.Abs(move));
				//sets the forward velocity
				m_Rigidbody2D.velocity = new Vector2(move*maxSpeed, m_Rigidbody2D.velocity.y);
				//exicutes a jump if one is buffered
				if (jumpBuffer) {
					//fires a jump if a jump has been buffered
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
				}
				//clears the doublejump and jumpbuffer limiter
				hasDoubleJumped = false;
				jumpBuffer = false;
				if (jump) {
					// Add vertical velocity to the player.
					grounded = false;
					animate.SetBool("Ground", false);
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
				}
			} else {
				if (airAceleration) {
					//reduces speed if crouching
					move = (crouch ? move*0.5f : move);
					// The Speed animator parameter is set to the absolute value of the horizontal input.
					animate.SetFloat("Speed", Mathf.Abs(move));
					//sets the forward velocity
					m_Rigidbody2D.velocity = new Vector2(move*maxSpeed, m_Rigidbody2D.velocity.y);					
				}
				if (jump && !hasDoubleJumped) {
					//exicutes a second jump regardless of colission
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
					hasDoubleJumped = true;
				} else if (jump && hasDoubleJumped){
					//sets the jump buffer and then clears it 1/8th of a secodn later.
					//this is intended to help with people atempting to jump right when they colide with the ground but at slightly early.
					jumpBuffer = true;
					Invoke("clearJumpBuffer", 0.125f);
				}
				
				
			}
			
			
			
			*/
			
			
			
			
			
			
			
			
			
			
			
			
			
			
		}
	}
	
	//clears the jump buffer
	public void clearJumpBuffer() {
		jumpBuffer = false;
		Debug.Log("buffered jump spent");
	}
	
	
	
	
}