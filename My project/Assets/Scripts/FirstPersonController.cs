using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 1.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;
		[Tooltip("decreased gravity when up against a wall")]
		public float WallGravity = -4.0f;
		[Tooltip("terminal velocity")]
		public float TerminalVelocity = 53.0f;
		[Tooltip("terminal velocity when up against a wall")]
		public float WallTerminalVelocity = 20.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;
		 [Space(10)]
    	[Header("Against walls")]
    	[Tooltip("If the character is against a wall or not. Not part of the CharacterController built in grounded check")]
    	public bool AgainstWall = false;
   	 	[Tooltip("wallsphere radius")]
    	public float WallSphereRadius = 0.5f;
		[Tooltip("wall offset")]
		public float WallOffset = 0.5f;
    	[Tooltip("What layers the character uses as walls")]
    	public LayerMask WallLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		[Header("wallRunning variables")]
		public float minimalRunninSpeed = 5f;
		public float wallRunSpeed = 7f;

		[Header("Detection")]
		public float wallCheckDistance;
		public float minDistanceFromFloor;
		private RaycastHit hitLeft;
		private RaycastHit hitRight;
		private bool wallLeft;
		private bool wallRight;



		//variable dictating the state of the player
		public enum PlayerState
		{
			Normal,
			Sprinting,
			Falling,
			Sliding,
			WallRunning
		}
		public PlayerState playerState = PlayerState.Normal;
		
		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

	
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			
			GroundedCheck();
			AgainstWallCheck();
			DecideState();
			Move();
			JumpAndGravity();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}


		public void DecideState(){

			if (AgainstWall&&!Grounded){
				if(canWallRide()){
					playerState = PlayerState.WallRunning;
					return;
				}
			}
			if (Grounded){
				if(_input.sprint){
					playerState = PlayerState.Sprinting;
				}
				else{
					playerState = PlayerState.Normal;
				}
			}
			else if (!Grounded){
				playerState = PlayerState.Falling;
			}
		}

		public bool canWallRide(){
			//cast out a ray from the players left and right to see if they are against a wall
			wallLeft=Physics.Raycast(transform.position,transform.TransformDirection(Vector3.left),out hitLeft,wallCheckDistance,WallLayers);
			wallRight=Physics.Raycast(transform.position,transform.TransformDirection(Vector3.right),out hitRight,wallCheckDistance,WallLayers);
			//if they are against a wall, check if they are far enough from the ground
			if (wallLeft||wallRight){
				if (!Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),minDistanceFromFloor,WallLayers)){
					//check if they are moving fast enough perpendicular to the wall
					if (Vector3.Dot(transform.forward,hitLeft.normal)<-minimalRunninSpeed||Vector3.Dot(transform.forward,hitRight.normal)<-minimalRunninSpeed){
						return true;
					}
					return false;
				}
				else{
					return false;
				}
			}
			else{
				return false;
			}

		}
		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}
		public void AgainstWallCheck(){
        	// set sphere position, with offset
        	Vector3 wallsphere = new Vector3(transform.position.x, transform.position.y+WallOffset, transform.position.z);
        	AgainstWall = Physics.CheckSphere(wallsphere, WallSphereRadius, WallLayers, QueryTriggerInteraction.Ignore);
    	}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private  void MoveNormal(){
			//
		}

		private void MoveSliding(){
			//
		}

		private void MoveWallRunning(){

		}
		private void Move()
		{
			
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

			// round speed to 3 decimal places
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
			
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}
			if(AgainstWall && _verticalVelocity < 0 && _verticalVelocity> WallTerminalVelocity){
				//only apply decreased gravity if we are against a wall and going down
				_verticalVelocity += WallGravity * Time.deltaTime;
			}
			else{
				// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
				if (_verticalVelocity < TerminalVelocity)
				{
					_verticalVelocity += Gravity * Time.deltaTime;
				}
			}
		}

		


		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}