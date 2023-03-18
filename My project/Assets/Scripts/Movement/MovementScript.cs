// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MovementScript : MonoBehaviour
// {
//     private StarterAssetsInputs _input;
//     [Header("Player")]


//     [Header("horizontal movement")]
//     [Tooltip("Top speed the character can move in m/s by normal means")]
//     public float MaxSpeed=8.0f;
//     [Tooltip("Acceleration and deceleration")]
//     public float SpeedChangeRate = 10.0f;
//     [Tooltip("Rotation speed of the character")]
//     public float RotationSpeed = 1.0f;

//     [Space(10)]
//     [Header("vertical movement")]
//     [Tooltip("The height the player can jump")]
//     public float JumpHeight = 1.2f;
//     [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
//     public float Gravity = -15.0f;
    

//     [Space(10)]
//     [Header("grounded variables")]
//     [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
//     public bool Grounded = true;
//     [Tooltip("Useful for rough ground")]
//     public float GroundedOffset = -0.14f;
//     [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
//     public float GroundedRadius = 0.5f;
//     [Tooltip("What layers the character uses as ground")]
//     public LayerMask GroundLayers;

//     [Space(10)]
//     [Header("Against walls")]
//     [Tooltip("If the character is against a wall or not. Not part of the CharacterController built in grounded check")]
//     public bool AgainstWall = true;
//     [Tooltip("wallsphere radius")]
//     public float WallSphereRadius = 0.5f;
//     [Tooltip("What layers the character uses as walls")]
//     public LayerMask WallLayers;


//     [Header("Cinemachine")]
// 	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
// 	public GameObject CinemachineCameraTarget;
// 	[Tooltip("How far in degrees can you move the camera up")]
// 	public float TopClamp = 90.0f;
// 	[Tooltip("How far in degrees can you move the camera down")]
// 	public float BottomClamp = -90.0f;


//     private float speed=0.0f;
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     public void GroundedCheck()
//     {
//         // set sphere position, with offset
// 		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
// 		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
//     }

//     public void AgainstWallCheck()
//     {
//         // set sphere position, with offset
//         Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
//         AgainstWall = Physics.CheckSphere(spherePosition, WallSphereRadius, WallLayers, QueryTriggerInteraction.Ignore);
//     }


//     private void CameraRotation()
// 		{
// 			// if there is an input
// 			if (_input.look.sqrMagnitude >= _threshold)
// 			{
// 				//Don't multiply mouse input by Time.deltaTime
// 				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
// 				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
// 				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

// 				// clamp our pitch rotation
// 				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

// 				// Update Cinemachine camera target pitch
// 				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

// 				// rotate the player left and right
// 				transform.Rotate(Vector3.up * _rotationVelocity);
// 			}
// 		}
    
//     private void Movement(){
        
//     }

// }
