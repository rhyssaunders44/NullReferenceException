using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NullFrameworkException.CharacterMovement.Rigidbody3d
{
	/// <summary> This is a basic character controller. </summary>
	[RequireComponent(typeof(Rigidbody))] public class PlayerMovementRigidbody3d : MonoBehaviour
	{
		/// <summary> PlayerVariables are listed here. </summary>
		[Header("Player Variables"), Tooltip("The height of the player collider"), SerializeField] 
			private float playerHeight = 2f;

		/// <summary> Speed the player moves at normally. </summary>
		[Header("Movement")] [SerializeField] private float moveSpeed = 6f;
		/// <summary> Because we have 2 different drags we need a mutiplier for when in the air. </summary>
		[SerializeField] private float airMultiplier = 0.4f;
		[SerializeField] private GameObject cameraManager;

		/// <summary> Normal walking speed the player moves at. </summary>
		[Header("Sprinting")] [SerializeField] private float walkSpeed = 4f;
		/// <summary> The player sprinting speed. </summary>
		[SerializeField] float sprintSpeed = 9f;
		/// <summary> How fast the player comes to either of these speeds. </summary>
		[SerializeField] float acceleration = 10f;

		/// <summary> The jump force suddenly applied on the player when they jump. </summary>
		[Header("Jumping")] public float jumpForce = 17f;
		[SerializeField] private float fallMultiplier = 2;
		[SerializeField] private float lowJumpMultiplier = 2f;
		[SerializeField] private bool leftground;
		[SerializeField] private float rememberGroundedFor = 0.4f;

		/// <summary> The keycode to jump. Can be changed in inspector. </summary>
		[Header("Key-binds")] [SerializeField] KeyCode jumpKey = KeyCode.Space;
		[SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

		/// <summary> The player's physics drag on the ground. </summary>
		[Header("Drag")] [SerializeField] private float groundDrag = 6f;
		/// <summary> The player's physics drag in the air. It needs to be larger for it to feel better. </summary>
		[SerializeField] private float airDrag = 2f;

		/// <summary> This is just an empty gameObject under player facing the same way as the player.</summary>
		[Header("Ground Detection")] [SerializeField, Tooltip("An empty child at Transform(0,0,(-playerHeight/2)) & Rotation(0,0,0)")]
		private Transform groundCheck;
		/// <summary> The layer the ground is on. </summary>
		[SerializeField, Tooltip("You might need to make a 'ground' layer.")]
		private LayerMask groundMask;
		/// <summary> How close player is to the ground to reset jump.</summary>
		[SerializeField, Tooltip("How close player is to the ground to reset jump.")]
		private float groundDistance = 0.3f;

		// HIDDEN VARIABLES.
		/// <summary> bool check for player is "Grounded" state. </summary>
		public bool isGrounded { get; private set; }
		
		/// <summary> The current normalised direction the player is to move at. </summary>
		private Vector3 moveDirection;
		
		/// <summary> The current normalised slope direction the player is to move at. </summary>
		private Vector3 slopeMoveDirection;
		
		/// <summary> The current horizontal movement value of the player. </summary>
		private float horizontalMovement, verticalMovement;

		/// <summary> The Rigidbody attached to the player. </summary>
		private Rigidbody myRigidbody;
		
		/// <summary> The internal movement multiplier of the player so they move at a normal speed. </summary>
		private float movementMultiplier = 10f;
		
		/// <summary> Check if the player is on a slope. </summary>
		private RaycastHit slopeHit;
		
		/// <summary> If the player has jumped. </summary>
		private bool jumpingNow;
		
		/// <summary> Current time since the player was grounded (for if the player airborne) </summary>
		private float lastTimeGrounded;

		/// <summary>Rigidbody settings are initialised here</summary>
		private void Start()
		{
			myRigidbody = GetComponent<Rigidbody>();
			myRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		}

		private void Update()
		{
			// Check if the player is grounded before applying movement inputs
			isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
			MovementInput(myRigidbody);
			ControlDrag();
			ControlSpeed();

			if(Input.GetKeyDown(jumpKey) && isGrounded)
			{
				SimpleJump(myRigidbody);
			}

			AdvancedJump();
			CheckIfGrounded();
			
			slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
			gameObject.transform.rotation = cameraManager.transform.rotation;
		}

		/// <summary>Performs a check to determin if the player is airbourne</summary>
		private void CheckIfGrounded()
		{
			if(isGrounded && !jumpingNow)
			{
				leftground = false;
			}
			else
			{
				if(!leftground)
				{
					lastTimeGrounded = Time.time;
				}
				leftground = true;
				isGrounded = false;
			}
		}

		/// <summary> Press jump input to jump. </summary>
		private void SimpleJump(Rigidbody playerRB)
		{
			if(isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor)
			{
				var velocity = playerRB.velocity;
				velocity = new Vector3(velocity.x, 0, velocity.z);
				playerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse);
				jumpingNow = true;
			}
		}
		
		/// <summary> This makes the Jumping feel better </summary>
		private void AdvancedJump()
		{
			if(myRigidbody.velocity.y < 0)
			{
				Vector2 fallMovement = Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
				myRigidbody.velocity += new Vector3(fallMovement.x, fallMovement.y, 0);
				jumpingNow = false;
			}
			else if(myRigidbody.velocity.y > 0 && !Input.GetButtonDown("Jump"))
			{
				Vector2 jumpFallMovement = Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
				myRigidbody.velocity += new Vector3(jumpFallMovement.x, jumpFallMovement.y, 0);
			}
		}

		/// <summary> This handles the input of all the WASD movement, Foward, Back, Left and Right. </summary>
		private void MovementInput(Rigidbody playerRB)
		{
			horizontalMovement = Input.GetAxisRaw("Horizontal");
			verticalMovement = Input.GetAxisRaw("Vertical");
			var move = playerRB.transform;
			
			// Move in the direction the player is looking.
			moveDirection = move.forward * verticalMovement + move.right * horizontalMovement;
		}
		
		/// <summary> Check if we are on a slope. </summary>
		/// <returns> Returns true if on a slope. </returns>
		private bool OnSlope()
		{
			if(Physics.Raycast(groundCheck.transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
			{
				return slopeHit.normal != Vector3.up;
			}
			return false;
		}

		/// <summary> Control the changing acceleration of the player. Used For sprinting </summary>
		private void ControlSpeed()
		{
			if(Input.GetKey(sprintKey) && isGrounded)
			{
				moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
			}
			else
			{
				moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
			}
		}

		/// <summary> If the player is in the air they have less drag, this improves jump feel. </summary>
		private void ControlDrag() => myRigidbody.drag = isGrounded ? groundDrag : airDrag;

		/// <summary> FixedUpdate has the frequency of the movement system.
		/// Since movement is physics based we will need to use this. </summary>
		private void FixedUpdate()
		{
			MovePlayer(myRigidbody);
		}

		/// <summary> This controls the physics movement of the player, considers if the player is on a slope </summary>
		private void MovePlayer(Rigidbody player)
		{
			if((isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor) && !OnSlope())
			{
				player.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
			}
			else if((isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor) && OnSlope())
			{
				player.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
			}
			else if(!(isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor))
			{
				player.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
			}
			
			player.rotation = cameraManager.transform.rotation;
		}
	}
}