using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement{
    public class FinalMove : MonoBehaviour
    {



        // void Awake(){
        //     Application.targetFrameRate=1;
        // }
        [Header("movement and momentum")]

        //momentum vector in global space
        public Vector3 moveDirection;
        public float totalSpeed;
        public float HorizontalSpeed;
        public float maximumSpeed = 100f;
        private CharacterController controller;
        public float speedMultiplier = 1f;
        public bool ChaserLocked = false;

        public bool lockforframe = false;
        public int lockforframecounter = 0;

        private StateManager stateManager;

      

        public float getYSpeed(){
            return moveDirection.y;
        }
        public float getXSpeed(){
            return moveDirection.x;
        }
        public float getZSpeed(){
            return moveDirection.z;
        }
        public void setSpeedGlobal(Vector3 speed)
        {
            moveDirection = speed;
        }
        public void setSpeedLocal(Vector3 speed)
        {
            moveDirection = transform.TransformDirection(speed);
        }

        public void addSpeedLocal(Vector3 speed)
        {
            moveDirection += transform.TransformDirection(speed);
        }
        public void addSpeedGlobal(Vector3 speed)
        {
            moveDirection += speed;
        }
        public void addXSpeedGlobal(float speed)
        {
            moveDirection += new Vector3(speed, 0, 0);
        }
        public void addYSpeedGlobal(float speed)
        {
            moveDirection += new Vector3(0, speed, 0);
        }
        public void addZSpeedGlobal(float speed)
        {
            moveDirection += new Vector3(0, 0, speed);
        }
        public void addXSpeedLocal(float speed)
        {
            moveDirection += transform.TransformDirection(new Vector3(speed, 0, 0));
        }
        public void addYSpeedLocal(float speed)
        {
            moveDirection += transform.TransformDirection(new Vector3(0, speed, 0));
        }
        public void addZSpeedLocal(float speed)
        {
            moveDirection += transform.TransformDirection(new Vector3(0, 0, speed));
        }
        public void setXSpeedGlobal(float speed)
        {
            moveDirection = new Vector3(speed, moveDirection.y, moveDirection.z);
        }
        public void setYSpeedGlobal(float speed)
        {
            moveDirection = new Vector3(moveDirection.x, speed, moveDirection.z);
        }
        public void setZSpeedGlobal(float speed)
        {
            moveDirection = new Vector3(moveDirection.x, moveDirection.y, speed);
        }
    

        public void setMagnitude(float speed)
        {
            moveDirection = moveDirection.normalized * speed;
        }

        public void addMagnitude(float speed)
        {
            moveDirection = moveDirection.normalized * (moveDirection.magnitude + speed);
        }
        public float getMagnitude()
        {
            return moveDirection.magnitude;
        }

        public float getHorizontalMagnitude()
        {
            return new Vector3(moveDirection.x, 0, moveDirection.z).magnitude;
        }
        public void setHorizontalMagnitude(float speed)
        {

            //set the magnitude of the horizontal vector but keep the vertical vector
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z).normalized * speed + new Vector3(0, moveDirection.y, 0);
        }

        public Vector3 getHorizontalVector()
        {
            return new Vector3(moveDirection.x, 0, moveDirection.z);
        }

        public Vector3 getVector()
        {
            return moveDirection;
        }
        public Vector3 getRelativeHorizontalVector()
        {
            return transform.InverseTransformDirection(new Vector3(moveDirection.x, 0, moveDirection.z));
        }

        public void MoveImmediately(Vector3 movement)
        {
            controller.Move(movement);
        }
        


        public void Start()
        {
            //character controller is on parent object
            controller = GetComponentInParent<CharacterController>();
            stateManager = GetComponent<StateManager>();
        }
        void FixedUpdate()
        {
           if(lockforframe){
               lockforframecounter++;
                if(lockforframecounter>20){
                     lockforframe = false;
                     lockforframecounter = 0;
                }
                return;
           }

            //Debug.Log("Final: " + moveDirection);
            //print parent xyz coordinates
            //Debug.Log(transform.parent.position);
            Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z)*speedMultiplier;
            if(movement.magnitude>maximumSpeed){
                movement.Normalize();
                movement*=maximumSpeed;
            }
        
            if(ChaserLocked){
                //remove horizontal movement
                movement = new Vector3(0, movement.y, 0);
            }
            //convert the movement vector to local space
            //movement = transform.InverseTransformDirection(movement);

            //if the character is walking
            controller.Move(movement * Time.fixedDeltaTime);

            //convert the movement vector to local space
            
            totalSpeed = movement.magnitude;
            HorizontalSpeed = new Vector3(movement.x, 0, movement.z).magnitude;
            

        }
        //method to cancel the momentum of the player when they hit a wall



      

    }
}
