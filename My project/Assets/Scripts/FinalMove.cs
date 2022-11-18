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
        public float xSpeed;
        public float ySpeed;
        public float zSpeed;
        public float maximumSpeed = 100f;
        public CharacterController controller;

        public void setSpeed(Vector3 speed)
        {
            xSpeed = speed.x;
            ySpeed = speed.y;
            zSpeed = speed.z;
        }

        void LateUpdate()
        {

            Vector3 movement = new Vector3(xSpeed, ySpeed, zSpeed);
            if(movement.magnitude>maximumSpeed){
                movement.Normalize();
                movement*=maximumSpeed;
            }
            controller.Move(movement * Time.deltaTime);
            
        }
    }
}
