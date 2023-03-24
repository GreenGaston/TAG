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

        public LayerMask WallRidingLayers;
        
        [Header("Detection")]
		public float wallCheckDistance;
		public float minDistanceFromFloor;
        public RaycastHit SlopeHit;
        public Vector3 WallDirection;
        public RaycastHit wallHit;
		private RaycastHit hitLeft;
		private RaycastHit hitRight;
        private bool onBooster;
		public bool wallLeft;
		public bool wallRight;
        public bool isOnSlope;
        
        public GameObject boosterObject;
        
        
        public PlayerState playerState =PlayerState.Normal;
        public PlayerState previousState = PlayerState.Normal;
        public FinalMove _move;

        void Start()
        {
            _move = GetComponent<FinalMove>();
            _input = GetComponent<StarterAssetsInputs>();
        }

        void LateUpdate()
        {
     
            GroundedCheck();
            AgainstWallCheck();
            isOnSlope=onSlope();
            previousState = playerState;
            DecideState();
            
        }

        public void DecideState(){
            
			if (AgainstWall&&!Grounded){
				if(canWallRide()){
					playerState = PlayerState.WallRunning;
				}
                else{
                    playerState = PlayerState.Falling;
                    
                }
			}
              
			else if (Grounded){
				if(onBooster){
                    playerState = PlayerState.Boosting;
                    
                }
                else if(_input.slide){
                    playerState = PlayerState.Sliding;
                   
                }
                else{
				    playerState = PlayerState.Normal;
                }
				
			}
			else if (!Grounded){
				playerState = PlayerState.Falling;
                //Debug.Log("falling");
			}

            if(previousState==PlayerState.Sliding&&playerState!=PlayerState.Sliding){
                playerState=PlayerState.StandingUp;
            }
            
		}

        
		public bool canWallRide(){
         
			//cast out a ray from the players left and right to see if they are against a wall
			wallLeft=Physics.Raycast(transform.position,transform.TransformDirection(Vector3.left),out hitLeft,wallCheckDistance,WallRidingLayers);
            
			wallRight=Physics.Raycast(transform.position,transform.TransformDirection(Vector3.right),out hitRight,wallCheckDistance,WallRidingLayers);
       
			//if they are against a wall, check if they are far enough from the ground
			if (wallLeft||wallRight){
				if(wallLeft){
                    wallHit=hitLeft;
                }
                else{
                    wallHit=hitRight;
                }
                WallDirection=Vector3.Cross(wallHit.normal, Vector3.up).normalized;
              
                Vector3 speed= _move.getHorizontalVector();
               


                //calculate the angle between the wall and the player speed vector
                float angle=Vector3.Dot(WallDirection, speed)/(WallDirection.magnitude*speed.magnitude);

                //calculate the speed of the player in the wall normal direction
                float lengthInDirection=speed.magnitude*angle;
                //Debug.Log(lengthInDirection);
             

            
                if(Mathf.Abs(lengthInDirection)>5){

          
                    return true;
                }
                else{
               
                    
                    return false;
                }
            
			}
			else{
				return false;
			}

		}


        public bool onSlope(){
            if(!Grounded){
                return false;
            }
            
            if(Physics.Raycast(transform.position,Vector3.down,out SlopeHit,GroundedRadius+0.5f,GroundLayers)){
                if(SlopeHit.normal!=Vector3.up){
                    return true;
                }
            }
            return false;
        }
		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
            // if(playerState==PlayerState.Sliding||previousState==PlayerState.Sliding){
            //     if(Grounded){
            //         Debug.Log("Grounded : "+spherePosition+" GroundedRadius: "+GroundedRadius+ " GroundedOffset: "+GroundedOffset);
            //     }
            //     else{
            //         Debug.Log("Not Grounded : "+spherePosition+" GroundedRadius: "+GroundedRadius+ " GroundedOffset: "+GroundedOffset);
            //     }
            // }
            //check if the player is on a booster
            if(Grounded){
                //check for boosters
                Collider[] colliders = Physics.OverlapSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
                if(colliders.Length>0){
                    onBooster=false;
                    boosterObject=null;
                    //check if the object is a booster by checking its layer
                    foreach (Collider col in colliders){

                        if(col.gameObject.layer==LayerMask.NameToLayer("Boosters")){
                            onBooster=true;
                            boosterObject=col.gameObject;
                            break;
                        }

                    }

                }
                else{
                    onBooster=false;
                    boosterObject=null;
                }
            }

		}
		public void AgainstWallCheck(){
			// set sphere position, with offset
			Vector3 wallsphere = new Vector3(transform.position.x, transform.position.y+WallOffset, transform.position.z);
			AgainstWall = Physics.CheckSphere(wallsphere, WallSphereRadius, WallLayers, QueryTriggerInteraction.Ignore);
		}

        //method that draws the sphere used for the grounded check
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position-GroundedOffset*Vector3.up,0.6f);
        }

    }
}