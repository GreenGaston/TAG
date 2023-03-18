using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Cans{
    public class CanScooper : NetworkBehaviour
    {
        CanApplier canApplier;
        void Start()
        {
            canApplier = GetComponent<CanApplier>();
        }
        //when the player collides with the can
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("collided with something");
            //if the player collides with the can
            if(other.gameObject.tag=="Can")
            {
                //Debug.Log("collided with can");
                //get the canscript
                CanScript canScript = other.gameObject.GetComponent<CanScript>();
                canScript.pickUp();
                handleCan(canScript.kindOfCan, canScript.amountOfCans);
            }
            if(other.gameObject.tag=="Bottle")
            {
                //Debug.Log("collided with bottle");
                //get the canscript
                CanScript canScript = other.gameObject.GetComponent<CanScript>();
                canScript.pickUp();
                handleBottle();
            }
        }


        private void handleCan(KindOfCan canKind, int amount=1){
            canApplier.applyCans(canKind, amount);
        }

        private void handleBottle(){
            canApplier.applyBottle();
        }
    }
}
