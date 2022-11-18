using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Movement{
public class StateManager : MonoBehaviour
    {


        public StarterAssetsInputs _input;
        [Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

        [Header("Against walls")]
		[Tooltip("If the character is against a wall or not. Not part of the CharacterController built in grounded check")]
		public bool AgainstWall = false;
		[Tooltip("wallsphere radius")]
		public float WallSphereRadius = 0.5f;
		[Tooltip("wall offset")]
		public float WallOffset = 0.5f;
        [Tooltip("What layers the character uses as walls")]
		public LayerMask WallLayers;

        
        [Header("Detection")]
		public float wallCheckDistance;
		public float minDistanceFromFloor;
		private RaycastHit hitLeft;
		private RaycastHit hitRight;
		private bool wallLeft;
		private bool wallRight;
        
        public PlayerState playerState =PlayerState.Normal;

        void LateUpdate()
        {
            GroundedCheck();
            AgainstWallCheck();
            DecideState();
        }

        public void DecideState(){

			if (AgainstWall&&!Grounded){
				if(canWallRide()){
					playerState = PlayerState.WallRunning;
					return;
				}
			}
			if (Grounded){
				
				playerState = PlayerState.Normal;
				
			}
			else if (!Grounded){
				playerState = PlayerState.Falling;
			}
		}

        
		public bool canWallRide(){
            return false;
			// //cast out a ray from the players left and right to see if they are against a wall
			// wallLeft=Physics.Raycast(transform.position,transform.TransformDirection(Vector3.left),out hitLeft,wallCheckDistance,WallLayers);
			// wallRight=Physics.Raycast(transform.position,transform.TransformDirection(Vector3.right),out hitRight,wallCheckDistance,WallLayers);
			// //if they are against a wall, check if they are far enough from the ground
			// if (wallLeft||wallRight){
			// 	if (!Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),minDistanceFromFloor,WallLayers)){
			// 		//check if they are moving fast enough perpendicular to the wall
			// 		if (Vector3.Dot(transform.forward,hitLeft.normal)<-minimalRunninSpeed||Vector3.Dot(transform.forward,hitRight.normal)<-minimalRunninSpeed){
			// 			return true;
			// 		}
			// 		return false;
			// 	}
			// 	else{
			// 		return false;
			// 	}
			// }
			// else{
			// 	return false;
			// }

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

    }
}