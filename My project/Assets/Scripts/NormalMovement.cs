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
                JumpAndGravity();
                Move();
            }
        }


        private void Move()
            {

            float Speed = _input.sprint ? SprintSpeed : MoveSpeed;
            _move.ySpeed = _verticalVelocity;
                
            //if there is no input decelerate the _move.xSpeed and _move.zSpeed until they reach 0;
            if(_input.move.magnitude==0){
                _move.xSpeed=Mathf.Lerp(_move.xSpeed, 0, Friction);
                _move.zSpeed=Mathf.Lerp(_move.zSpeed, 0, Friction);

                return;
            }
         
                
            
            float was=new Vector3(_move.xSpeed, 0.0f, _move.zSpeed).magnitude;
            bool bigger=was>Speed;
            
            Vector3 inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            inputDirection*=Acceleration;
            // calculate current speed
            
            _move.xSpeed+=inputDirection.x;
            _move.zSpeed+=inputDirection.z;
            
            if(bigger){
                if(new Vector3(_move.xSpeed, 0.0f, _move.zSpeed).magnitude>was){
                    Vector3 temp = new Vector3(_move.xSpeed, 0.0f, _move.zSpeed);
                    temp.Normalize();
                    temp*=was;
                    _move.xSpeed=temp.x;
                    _move.zSpeed=temp.z;
                }
                else{
                    Vector3 temp = new Vector3(_move.xSpeed, 0.0f, _move.zSpeed);
                    temp.Normalize();
                    temp*=Speed;
                    _move.xSpeed=Mathf.Lerp(_move.xSpeed, temp.x, Friction);
                    _move.zSpeed=Mathf.Lerp(_move.zSpeed, temp.y, Friction);
                }
            }
            else{ 
                if(new Vector3(_move.xSpeed, 0.0f, _move.zSpeed).magnitude>Speed){
                    Vector3 temp = new Vector3(_move.xSpeed, 0.0f, _move.zSpeed);
                    temp.Normalize();
                    temp*=Speed;
                    _move.xSpeed=temp.x;
                    _move.zSpeed=temp.z;
                }
            }
        }


        private void JumpAndGravity()
        {
            // stop our velocity dropping infinitely when grounded
            if (_move.ySpeed< 0.0f)
            {
                _verticalVelocity = -2f;
            }
            // Jump
            if (_input.jump )
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }

            
        }
    }
}