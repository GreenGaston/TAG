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
                AirMove();
                
            }
            
        }

        public void AirMove(){
            // decelerate the _move.xSpeed and _move.zSpeed until they reach the airSpeed
            // calculate the length of the vector
            
            float was=new Vector3(_move.xSpeed, 0.0f, _move.zSpeed).magnitude;
            bigger=was>airSpeed;
            
            inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            inputDirection*=airAcceleration;
            // calculate current speed
            
            _move.xSpeed+=inputDirection.x;
            _move.zSpeed+=inputDirection.z;
            //if the speed is bigger than the airSpeed, normalize the vector and multiply it by the airSpeed
            if(bigger){
                if(new Vector3(_move.xSpeed, 0.0f, _move.zSpeed).magnitude>was){
                    Vector3 temp = new Vector3(_move.xSpeed, 0.0f, _move.zSpeed);
                    temp.Normalize();
                    temp*=was;
                    _move.xSpeed=temp.x;
                    _move.zSpeed=temp.z;
                }
                else{
                    _move.xSpeed-=airDrag*Time.deltaTime;
                    _move.zSpeed-=airDrag*Time.deltaTime;
                }
            }
            else{ 
                if(new Vector3(_move.xSpeed, 0.0f, _move.zSpeed).magnitude>airSpeed){
                    Vector3 temp = new Vector3(_move.xSpeed, 0.0f, _move.zSpeed);
                    temp.Normalize();
                    temp*=airSpeed;
                    _move.xSpeed=temp.x;
                    _move.zSpeed=temp.z;
                }
            }
            

            _move.ySpeed+=Gravity*Time.deltaTime;
            
            

        }
    }
}
