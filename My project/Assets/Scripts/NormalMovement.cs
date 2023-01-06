using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement{
    public class NormalMovement : MonoBehaviour
    {

        //reference to the FirstPersonController script
        public StateManager _controller;
        public StarterAssetsInputs _input;
        public FinalMove _move;
        

        public float MoveSpeed = 4f;
        public float SprintSpeed = 6f;
        public float rotationSpeed = 1f;
        public float Acceleration= 5f;
        public float Friction= 0.5f;
        

        public float JumpHeight = 1.2f;
        public float Gravity = -15f;

        private float _verticalVelocity=0f;

        

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(_controller.playerState==PlayerState.Normal)
            {


                if(_controller.previousState==PlayerState.Falling&&_controller.isOnSlope){
                
                    ConvertFallingSpeed();
                    
                }
                //Debug.Log("before:"+_move.getVector());
                JumpAndGravity();
                Move();
            }
        }

 


        private void Move()
            {
            
            float Speed = _input.sprint ? SprintSpeed : MoveSpeed;
            _move.setYSpeedGlobal(_verticalVelocity);
                
            //if there is no input decelerate the _move.xSpeed and _move.zSpeed until they reach 0;
            if(_input.move.magnitude==0&&!_input.sprint){
                _move.setXSpeedGlobal(Mathf.Lerp(_move.getXSpeed(),0,Friction));
                _move.setZSpeedGlobal(Mathf.Lerp(_move.getZSpeed(),0,Friction));
                //Debug.Log("decelerating");
                return;
            }
         
           
            
            float was=_move.getHorizontalMagnitude();	
            //Debug.Log(was);
            bool bigger=was>Speed;

       
            Vector3 inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
         
            inputDirection*=Acceleration;
            
            // calculate current speed
            
            _move.addSpeedGlobal(inputDirection);
        
            
            if(bigger){
                if(_move.getHorizontalMagnitude()>was){
                    _move.setHorizontalMagnitude(was);
                }
                _move.setHorizontalMagnitude(Mathf.Max(_move.getHorizontalMagnitude()-Friction*Time.deltaTime,Speed));
                
            }
            else{ 
                if(_move.getHorizontalMagnitude()>Speed){
                    _move.setHorizontalMagnitude(Speed);
                    
                }
                
            }
            
        }


        private void JumpAndGravity()
        {
            // stop our velocity dropping infinitely when grounded

           
            
            if (_move.getYSpeed()<0f)
            {
                _verticalVelocity = -2f;
            }
            // Jump
            if (_input.jump )
            {
            
               
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                return;
            }

            if(_controller.isOnSlope&&!(_move.getYSpeed()<-5f)&&!(_move.getYSpeed()>5f)){
                _verticalVelocity=-5f;
                
                return;
            }  

            
        }

        private void ConvertFallingSpeed(){
            //convert any falling speed to horizontal speed 
            //calculate the amount of speed the falling speed has in the direction of the slope
            
            Vector3 groundSlopeDirection=Vector3.Cross(_controller.SlopeHit.normal,Vector3.up);

            Vector3 groundSlopeDir=Vector3.Cross(groundSlopeDirection,_controller.SlopeHit.normal).normalized;
            

            Vector3 fallingSpeed=new Vector3(0,_move.getYSpeed(),0);

            float lengthInDir=Vector3.Dot(fallingSpeed,groundSlopeDir);

            Vector3 speedInDir=groundSlopeDir*lengthInDir;
            Debug.Log("speedInDir:"+speedInDir);
            //add only the horizontal speed to the _move.xSpeed and _move.zSpeed
            Debug.Log("before:"+_move.getVector());

            _move.addSpeedGlobal(new Vector3(speedInDir.x,0,speedInDir.z));
            Debug.Log("after:"+_move.getVector());
            


        }
    }
}