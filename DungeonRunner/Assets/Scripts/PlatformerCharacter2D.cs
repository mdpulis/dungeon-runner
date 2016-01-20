using UnityEngine;
using System;
using System.Collections.Generic;
using SynodicArc.DungeonRunner.Items;
using SynodicArc.DungeonRunner.StatusEffects;

// DEPRECATED FILE -- USE ONLY FOR REFERENCE
//namespace UnityStandardAssets._2D
//{
//    public class PlatformerCharacter2D : MonoBehaviour
//    {
//        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
//        [SerializeField] private float m_JumpForce = 200f;                  // Amount of force added when the player jumps.
//        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
//        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
//        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
//		
//		public bool alowMove = true;			// alows X movment when True
//		private bool hasDoubleJumped = false;	//tracks double jumping
//		private bool jumpBuffer = false;		//buffers jump comands for usage once landed
//		
//        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
//        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
//        private bool m_Grounded;            // Whether or not the player is grounded.
//        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
//        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
//        private Animator m_Anim;            // Reference to the player's animator component.
//        private Rigidbody2D m_Rigidbody2D;
//        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
//
//		public List<StatusEffect> StatusEffectModifiers; //the list of status effects that modify our player in various ways
//
//
//
//        private void Awake()
//        {
//            // Setting up references.
//            m_GroundCheck = transform.Find("GroundCheck");
//            m_CeilingCheck = transform.Find("CeilingCheck");
//            m_Anim = GetComponent<Animator>();
//            m_Rigidbody2D = GetComponent<Rigidbody2D>();
//			// Setting our list to empty by default
//			StatusEffectModifiers = new List<StatusEffect>();
//        }
//
//
//        private void FixedUpdate()
//        {
//            m_Grounded = false;
//
//            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
//            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
//            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
//            for (int i = 0; i < colliders.Length; i++)
//            {
//                if (colliders[i].gameObject != gameObject)
//                    m_Grounded = true;
//            }
//            m_Anim.SetBool("Ground", m_Grounded);
//
//            // Set the vertical animation
//            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
//        }
//
//
//        public void Move(float move, bool crouch, bool jump)
//        {
//            // If crouching, check to see if the character can stand up
//            if (!crouch && m_Anim.GetBool("Crouch"))
//            {
//                // If the character has a ceiling preventing them from standing up, keep them crouching
//                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
//                {
//                    crouch = true;
//                }
//            }
//
//            // Set whether or not the character is crouching in the animator
//            m_Anim.SetBool("Crouch", crouch);
//
//            //only control the player if grounded or airControl is turned on
//            if (m_Grounded || m_AirControl)
//            {
//                // Reduce the speed if crouching by the crouchSpeed multiplier
//                move = (crouch ? move*m_CrouchSpeed : move);
//
//                // The Speed animator parameter is set to the absolute value of the horizontal input.
//                m_Anim.SetFloat("Speed", Mathf.Abs(move));
//
//                // Move the character if permitted to move
//				if (alowMove){
//					//Debug.Log("true");
//					m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);
//					//m_Rigidbody2D.velocity = new Vector2(m_MaxSpeed, 0);
//				}
//
//                // If the input is moving the player right and the player is facing left...
//                if (move > 0 && !m_FacingRight)
//                {
//                    // ... flip the player.
//                    Flip();
//                }
//                    // Otherwise if the input is moving the player left and the player is facing right...
//                else if (move < 0 && m_FacingRight)
//                {
//                    // ... flip the player.
//                    Flip();
//                }
//            }
//			
//			if (m_Grounded) {
//				if (jumpBuffer) {
//					//fires a jump if a jump has been buffered
//					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
//				}
//				//clears the doublejump and jumpbuffer limiter ONLY when grounded
//				hasDoubleJumped = false;
//				jumpBuffer = false;
//			}
//            // If the player should jump...
//            if (m_Grounded && jump && m_Anim.GetBool("Ground")) {
//                // Add vertical velocity to the player.
//                m_Grounded = false;
//                m_Anim.SetBool("Ground", false);
//				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
//			} else if (!m_Grounded && jump && !hasDoubleJumped) {
//				//handels double jumping
//				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
//				hasDoubleJumped = true;
//			} else if (!m_Grounded && jump && hasDoubleJumped) {
//				//invokes a temporary state in which hte character will jump at the next posible chance. times out after 1/8th of a second
//				jumpBuffer = true;
//				Invoke("clearJumpBuffer", 0.125f);
//			}
//        }
//		private void clearJumpBuffer() {
//			jumpBuffer = false;
//			Debug.Log("buffered jump spent");
//			//Move(0f,false,true);
//		}
//
//        private void Flip()
//        {
//            // Switch the way the player is labelled as facing.
//            m_FacingRight = !m_FacingRight;
//
//            // Multiply the player's x local scale by -1.
//            Vector3 theScale = transform.localScale;
//            theScale.x *= -1;
//            transform.localScale = theScale;
//        }
//    }
//}
