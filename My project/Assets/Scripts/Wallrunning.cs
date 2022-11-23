using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Movement{
    public class Wallrunning : MonoBehaviour
    {

       
        [Header("speedboost in m/s")]
        public float speedBoost=0f;
        public float jumpXZ=2;
        public float jumpY=2;
        public float WallGravity = -4f;
        public float TerminalVelocity = -7f;
        public float WallFriction = 4f;

      
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
            if(stateManager.playerState==PlayerState.WallRunning)
            {       
              
               
                WallRun();
            }
        }


        void WallRun(){
            //Debug.Log("Wallrunning");
            Vector3 WallDirection1=stateManager.WallDirection.normalized;
            if(Vector3.Dot(_move.getHorizontalVector(), WallDirection1)<0){
                WallDirection1=-WallDirection1;
            }
        
            Vector3 WallDirection=WallDirection1*_move.getHorizontalVector().magnitude;
          
            _move.setXSpeedGlobal(WallDirection.x);
            _move.setZSpeedGlobal(WallDirection.z);
            _move.setYSpeedGlobal(0);
            if(_move.getYSpeed()<TerminalVelocity){
                _move.setYSpeedGlobal(TerminalVelocity);
                Debug.Log("TerminalVelocity");
            }
            if(_move.getYSpeed()>0){
                _move.addYSpeedGlobal(WallGravity*Time.deltaTime);
            }
            // if(_move.ySpeed>TerminalVelocity){
            //     _move.ySpeed+=WallGravity*Time.deltaTime;
            // }
            // else{
            //     _move.ySpeed+=WallFriction*Time.deltaTime;
            // }

            if(stateManager.previousState!=PlayerState.WallRunning){
                
                _move.addSpeedGlobal(WallDirection1*speedBoost);
            }
           if(_input.jump){
                WallJump();
            }



        }


        void WallJump(){
            //we want to jump away from the wall we do this by adding speed to the x and z axis using the normal of the wall
            // float magnitude=_move.getHorizontalMagnitude();
            // Vector3 normal=stateManager.wallHit.normal;
            // _move.xSpeed+=normal.x*jumpXZ;
            // _move.zSpeed+=normal.z*jumpXZ;
            // _move.ySpeed+=jumpY;

            // //normalize the vector and multiply it by the magnitude
            // Vector3 newSpeed=new Vector3(_move.xSpeed, 0.0f, _move.zSpeed).normalized*magnitude;
            // _move.xSpeed=newSpeed.x;
            // _move.zSpeed=newSpeed.z;



        }
    }
}