using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
//import array
using System;

namespace Cans{
    public class CanApplier : NetworkBehaviour
    {

        

        public float canTime = 3f;
        //currently selected can
        
        public int canIndex = 0;
        public Can[] canArray;
        public int[] canAmounts;
        public bool onCooldown = false;
        public float drinkCooldown = 0.5f;
        public float coolDownTimer = 0f;
        public float mouseTimeout = 0.5f;
        public float mouseTimer = 0f;
        public bool isPressed = false;


        private StarterAssetsInputs _input;



        public float MaxDosage = 100f;
        public float currentDosage = 0f;
        public bool isOverDosed = false;
        public float overdoseTime = 5f;
        public float overdoseTimer = 0f;
        public float recoveryRate = 0.1f;
        public Material material;

        public float BottleRecovery = 0.5f;



        public bool NewMovement = false;


        public int LastUsedCan = 0;


        private CanUI canUI;
        void Start(){
            
            //can ui is in child of child of child of child of child of child of this object
            canUI = GetComponentInChildren<CanUI>();
            //get cans from children
            _input = GetComponentInChildren<StarterAssetsInputs>();
            Can empty= GetComponentInChildren<EmptyCan>();
            Can blueCan = GetComponentInChildren<BlueCan>();
            Can redCan = GetComponentInChildren<RedCan>();
            Can greenCan = GetComponentInChildren<GreenCan>();
            canArray = new Can[]{empty, blueCan, redCan, greenCan};
            canAmounts = new int[]{0,0,0,0};
            canUI.setCanUI(canArray[canIndex].getKindOfCan());
            //get the ui material not stored on an object
           

        }

        public void applyCans(KindOfCan canKind, int amount=1){
            if(canKind==KindOfCan.All){
                for(int i=0;i<canArray.Length;i++){
                    canAmounts[i]+=amount;
                }
                return;
            }
            for(int i=0;i<canArray.Length;i++){
                if(canArray[i].getKindOfCan()==canKind){
                    canAmounts[i]+=amount;
                    //canUI.setCanUI(canArray[canIndex]);
                    break;
                }
            }
        }
        public void applyBottle(float BottleRecovery=0.5f){

            if(isOverDosed){
                return;
            }
            updateDosage(-BottleRecovery);

        }

        void Update(){



            
            if(isPressed){
                mouseTimer += Time.deltaTime;
                if(mouseTimer>mouseTimeout){
                    isPressed = false;
                    onCooldown = false;
                    mouseTimer = 0f;
                }
            }
            //if the player is scrolling increase or decrease the can index
            if(_input.scroll.y>0f){
                //Debug.Log("scrolling");
                canIndex++;
                if(canIndex>canArray.Length-1){
                    canIndex = 0;
                }
                canUI.setCanUI(canArray[canIndex].getKindOfCan());
            }
            if(_input.scroll.y<0f){
                //Debug.Log("scrolling");
                canIndex--;
                if(canIndex<0){
                    canIndex = canArray.Length-1;
                }
                canUI.setCanUI(canArray[canIndex].getKindOfCan());
            }


            //Debug.Log("can index: "+canIndex);
            if(isOverDosed){
                //player cannot drink while overdosed
                overdoseTimer += Time.deltaTime;
                float percentage = (overdoseTime-overdoseTimer)/overdoseTime;
                material.SetFloat("_Percentage", percentage);
                if(overdoseTimer>overdoseTime){
                    isOverDosed = false;
                    overdoseTimer = 0f;
                    currentDosage = 0f;
                    material.SetFloat("_Percentage", 0f);
                    material.SetFloat("_Boolean", 0f);
                    
                }
                else{
                    return;
                }
            }
            else{
                currentDosage= Mathf.Max(0f, currentDosage-recoveryRate*Time.deltaTime);
                material.SetFloat("_Percentage", currentDosage/MaxDosage);

            }




            if(NewMovement){
                NewCanApplier();
            }
            else{
                OldCanApplier();
            }


        }






        void OldCanApplier(){
            //current way of applying cans where multiple cans can be applied at once
            if(_input.leftMouse){
                if(amountOfCans()>0){
                    if(!isPressed){
                        removeCan();
                        if(onCooldown){
                            coolDownTimer += Time.deltaTime;
                            if(coolDownTimer>drinkCooldown){
                                onCooldown = false;
                                coolDownTimer = 0f;
                            }
                        }
                        else{
                            RunScript();
                            onCooldown = true;
                            Debug.Log("drank can");
                        }
                        isPressed = true;
                    }
                }
            }
            else{
                //Debug.Log("unpressed");
                isPressed = false;
                onCooldown = false;
                mouseTimer = 0f;
                coolDownTimer = 0f;
            }
            return;
        }

        void NewCanApplier(){
            //new way doesnt care for dosage cooldown, or mulitple cans
            //it only switches between cans
            if(_input.leftMouse){
                if(!isPressed){
                    
                    RunScriptNew(LastUsedCan, canIndex);
                    LastUsedCan = canIndex;
                    isPressed = true;

                }
            }

        }

        void RunScript(){
            canArray[canIndex].useCan();
            updateDosage(canArray[canIndex].getDosage());
        }

        void RunScriptNew(int old, int current){
            //undo the old can
            canArray[old].UndoCan();
            //apply the new can
            canArray[current].UseCanPermanently();

        }

        int amountOfCans(){
            return canAmounts[canIndex];
        }
        void removeCan(){
            canAmounts[canIndex]--;
        }

        void OverDose(){
            isOverDosed = true;
            //every can has a different overdose effect
            for(int i=0;i<canArray.Length;i++){
                canArray[i].OverDose();
            }
            material.SetFloat("_Boolean", 1f);


        }


        void updateDosage(float amount){
            currentDosage = Mathf.Max(0f, currentDosage+amount);
            float percentage= currentDosage/MaxDosage;
            material.SetFloat("_Percentage", percentage);
            if(currentDosage>MaxDosage){
                isOverDosed = true;
                OverDose();
            }
            else{
                isOverDosed = false;
            }
        }


       
    }
}