using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement{
    public class AirMovement : MonoBehaviour

    {
        public float airSpeed = 5f;
        public float airAcceleration = 0.5f;
        public float airDrag = 0.5f;
       

        public bool bigger=false;
        public Vector3 inputDirection= Vector3.zero;

    

        public float Gravity = -15f;
        public float TerminalVelocity = -56f;
        public float WallGravity = -5f;
        public float WallTerminalVelocity = -7f;
     


        private StateManager stateManager;
        private StarterAssetsInputs _input;
        private FinalMove _move;
      
        void Awake(){
            stateManager = GetComponent<StateManager>();
            _input = GetComponent<StarterAssetsInputs>();
            _move = GetComponent<FinalMove>();
        }

        // Update is called once per frame
        void Update()
        {
          
            if(stateManager.playerState==PlayerState.Falling)
            {   
                Debug.Log("falling");
                //if the players previous state was normal or sliding and the y speed is negative, set the y speed to 0
                if(stateManager.previousState==PlayerState.Normal || stateManager.previousState==PlayerState.Sliding){
                    if(_move.getYSpeed()<0){
                        Debug.Log("set y speed to 0");
                        _move.setYSpeedGlobal(0);
                    }
                    
                }
                AirMove();
                
            }
            
        }

        public void AirMove(){
            // decelerate the _move.xSpeed and _move.zSpeed until they reach the airSpeed
            // calculate the length of the vector
            
            float was=_move.getHorizontalMagnitude();
            bigger=was>airSpeed;
            
            inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            inputDirection*=airAcceleration;
            // Debug.Log(inputDirection);
            // calculate current speed
            
            _move.addSpeedGlobal(inputDirection);
            //if the speed is bigger than the airSpeed, normalize the vector and multiply it by the airSpeed
            if(bigger){
                if(_move.getHorizontalMagnitude()>was){
                    //set the speed to what it was before
                    _move.setHorizontalMagnitude(was);
                }

                _move.setHorizontalMagnitude(Mathf.Max(_move.getHorizontalMagnitude()-airDrag*Time.deltaTime,airSpeed));
                
            }
            else{ 
                if(_move.getHorizontalMagnitude()>airSpeed){
                    _move.setHorizontalMagnitude(airSpeed);
                }
            }
            
            //gravity
            if(stateManager.AgainstWall&&_move.getYSpeed()<0){
                if(_move.getYSpeed()<WallTerminalVelocity){
                    _move.setYSpeedGlobal(Mathf.Lerp(_move.getYSpeed(),WallTerminalVelocity,Time.deltaTime));
                }
                else{
                    _move.addYSpeedLocal(WallGravity*Time.deltaTime);
                }
            }
            else{
                if(_move.getYSpeed()<TerminalVelocity){
                    _move.setYSpeedGlobal(TerminalVelocity);
                }
                else{
                    _move.addYSpeedGlobal(Gravity*Time.deltaTime);
                }
                
            }
            
            

        }
    }
}
