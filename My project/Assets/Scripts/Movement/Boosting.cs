using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Movement{
    public class Boosting : MonoBehaviour
    {
        
              private StateManager stateManager;
        private StarterAssetsInputs _input;
        private FinalMove _move;
        
        void Awake(){
            stateManager = GetComponent<StateManager>();
            _input = GetComponent<StarterAssetsInputs>();
            _move = GetComponent<FinalMove>();
        }
        // Update is called once per frame
        void FixedUpdate()
        {
         
            if(stateManager.playerState==PlayerState.Boosting&&stateManager.previousState!=PlayerState.Boosting)
            {   
                Boost();
                Debug.Log("Boosting");
            }
            
        }

        private void Boost(){
            //Debug.Log("Boosting");
            Booster booster=stateManager.boosterObject.GetComponent<Booster>();
            Vector3 boostDirection=booster.boostDirection;
            float force = booster.boostForce;
            bool rickiseeneigenwijs = booster.OverrideSpeed;

            if(rickiseeneigenwijs){
                _move.setXSpeedGlobal(boostDirection.x*force);
                _move.setYSpeedGlobal(boostDirection.y*force);
                _move.setZSpeedGlobal(boostDirection.z*force);
            }
            else{
                _move.addSpeedGlobal(boostDirection*force);
            }
            
            
            
           
            

        }
    }
}
