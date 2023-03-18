using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace Movement{
    public class Sliding : NetworkBehaviour
    {
        // Start is called before the first frame update

        private StateManager _stateManager;
        private StarterAssetsInputs _input;
        private FinalMove _move;

        private CharacterController _characterController;

        public float MoveSpeed = 3f;
        public float Friction = 0.5f;

        public float JumpHeight = 1.2f;
        public float Acceleration = 1f;

        public float Gravity = -15f;

        private float _verticalVelocity = 0f;

        public float fallConvertSpeed = 1f;

        private float previousHeight;
        private float previousOffset;

        public GameObject parentobject;

        public float StandSphereRadius = 0.5f;
        public float StandSphereOffset = 0.5f;

    



            
        void Awake(){
            
            _stateManager = GetComponent<StateManager>();
            _input = GetComponent<StarterAssetsInputs>();
            _move = GetComponent<FinalMove>();
            //character controller is in parent
            parentobject=transform.parent.gameObject;
            _characterController = parentobject.GetComponent<CharacterController>();
        
        }
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(!IsOwner)
                return;
            if(_stateManager.playerState==PlayerState.StandingUp){
                StandUp();
            }
            if(_stateManager.playerState==PlayerState.Sliding)
            {
                if(_stateManager.previousState!=PlayerState.Sliding){
                    Crouch();
                }
                
                if(_stateManager.previousState==PlayerState.Falling&&_stateManager.isOnSlope){
                    
                    ConvertFallingSpeed();
                    
                }

                JumpAndGravity();
                Move();
            }
        }


        private void Crouch(){
            //change the height of the capsule collider
           
            previousHeight=_characterController.height;
            _characterController.height=0.5f;
            //move the player down to the new height
   

            //Debug.Log("parentobject:"+parentobject.transform.position);
            // move the transform of the parent object down by 0.35f
            _move.MoveImmediately(new Vector3(0,-3.5f,0));
            //print new xyz coordinates of parentobject
            //Debug.Log("parentobject:"+parentobject.transform.position);
            previousOffset=_stateManager.GroundedOffset;
            _stateManager.GroundedOffset=-0.5f;


        }

        private void StandUp(){
            //check sphere if there is space to stand up
            Vector3 sphereCenter=transform.position+Vector3.up*StandSphereOffset;
            Collider[] colliders=Physics.OverlapSphere(sphereCenter,StandSphereRadius);
            if(colliders.Length>0){
                _stateManager.playerState=PlayerState.Sliding;
                return;
                
            }
            _characterController.height=previousHeight;
            //_move.MoveImmediately(new Vector3(0,3.5f,0));
            _stateManager.GroundedOffset=previousOffset;
            //_stateManager.GroundedOffset=previousOffset;

        }



        private void JumpAndGravity()
        {
            // stop our velocity dropping infinitely when grounded     
            if (_move.getYSpeed()<0f)
            {
                _verticalVelocity = -2f;
            }
            

            if(_stateManager.isOnSlope&&!(_move.getYSpeed()<-5f)&&!(_move.getYSpeed()>5f)){
                _verticalVelocity=-5f;
                
                return;
            }  
        }

        private void ConvertFallingSpeed()
        {
            
            //calculate the amount of speed the falling speed has in the direction of the slope
            
            Vector3 groundSlopeDirection=Vector3.Cross(_stateManager.SlopeHit.normal,Vector3.up);

            Vector3 groundSlopeDir=Vector3.Cross(groundSlopeDirection,_stateManager.SlopeHit.normal).normalized;
            

            Vector3 fallingSpeed=new Vector3(0,_move.getYSpeed(),0);

            float lengthInDir=Vector3.Dot(fallingSpeed,groundSlopeDir);

            Vector3 speedInDir=groundSlopeDir*lengthInDir*fallConvertSpeed;
            Debug.Log("speedInDir:"+speedInDir);
            //add only the horizontal speed to the _move.xSpeed and _move.zSpeed
            Debug.Log("before:"+_move.getVector());

            _move.addSpeedGlobal(new Vector3(speedInDir.x,0,speedInDir.z));
            Debug.Log("after:"+_move.getVector());
                
        }

        private void Move()
                {
                
                _move.setYSpeedGlobal(_verticalVelocity);
                    
                float was=_move.getHorizontalMagnitude();	
                //Debug.Log(was);
                bool bigger=was>MoveSpeed;

        
                Vector3 inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            
                inputDirection*=Acceleration;
                
                // calculate current speed
                
                _move.addSpeedGlobal(inputDirection);
            
                
                if(bigger){
                    if(_move.getHorizontalMagnitude()>was){
                        _move.setHorizontalMagnitude(was);
                    }
                    _move.setHorizontalMagnitude(Mathf.Max(_move.getHorizontalMagnitude()-Friction*Time.deltaTime,MoveSpeed));
                    
                }
                else{ 
                    if(_move.getHorizontalMagnitude()>MoveSpeed){
                        _move.setHorizontalMagnitude(MoveSpeed);
                        
                    }
                }
                
            }
         private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position+Vector3.up*StandSphereOffset,StandSphereRadius);
        }
    }
}
