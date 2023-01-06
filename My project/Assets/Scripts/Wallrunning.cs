using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Movement{
    public class Wallrunning : MonoBehaviour
    {

       
        [Header("speedboost in m/s")]
        public float speedBoost=0f;
        public float jumpY=7;
        public float WallGravity = -4f;
        public float TerminalVelocity = -7f;
        public float WallFriction = 4f;
        public float jumpAngle=45f;

      
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
                Debug.Log("Jumping");
                WallJump();
            }



        }


        void WallJump(){
        
            Vector3 WallDirection1=stateManager.WallDirection.normalized;
            if(Vector3.Dot(_move.getHorizontalVector(), WallDirection1)<0){
                WallDirection1=-WallDirection1;
            }
            Vector3 jumpDirection;
            if(stateManager.wallLeft){
                //calculate the horizontal direction of the jump
                //the direction should be jumpangle degrees to the right or left of the wall depending on which side of the wall the player is on
                jumpDirection=Quaternion.AngleAxis(jumpAngle, Vector3.up)*WallDirection1;
            }
            else{
                jumpDirection=Quaternion.AngleAxis(-jumpAngle, Vector3.up)*WallDirection1;
            }

            //reorient the horizontal direction of the player
            Vector3 newHorizontalDirection=_move.getHorizontalVector();
            float length=newHorizontalDirection.magnitude;
            newHorizontalDirection.x=jumpDirection.x*length;
            newHorizontalDirection.z=jumpDirection.z*length;
            newHorizontalDirection.y=jumpY;
            _move.setSpeedGlobal(newHorizontalDirection);


        

            



        }
    }
}