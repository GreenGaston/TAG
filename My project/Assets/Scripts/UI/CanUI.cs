using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace Cans{
    public class CanUI : NetworkBehaviour
    {
        int test=0;
        void Awake(){
            if(IsOwner){
                //set tag as "MainUI"
                gameObject.tag = "UI";
            }
        }

        public GameObject redCanUI;
        public GameObject blueCanUI;
        public GameObject greenCanUI;

        public GameObject currentCanUI=null;
        public KindOfCan currentCanKind;


        
        public void setCanUI(KindOfCan canKind){
            if(currentCanUI!=null){
                despawnCan();
            }
            
            
            SpawnCan(canKind);
            
        }
        
        void SpawnCan(KindOfCan canKind){
            switch(canKind){
                case KindOfCan.Red:
                    currentCanUI = Instantiate(redCanUI,transform);
                    break;
                case KindOfCan.Blue:
                    currentCanUI = Instantiate(blueCanUI,transform);
                    break;
                case KindOfCan.Green:
                    currentCanUI = Instantiate(greenCanUI,transform);
                    break;
                case KindOfCan.None:
                    //do something
                    break;
            }
            //set layer to UI
            currentCanUI.layer = 5;
        }

        void despawnCan(){
            //deload the current can
            Destroy(currentCanUI);
        }
        
    }
}
