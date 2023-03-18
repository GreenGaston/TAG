using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Cans{
    public class CanScript : MonoBehaviour
    {

        public float rotatationSpeed = 0.4f;
        public bool pickedUp = false;
        public float timeOut = 3f;
        public float timeOutCounter = 0f;
        public int amountOfCans=1;
        public KindOfCan kindOfCan = KindOfCan.Red;

        private MeshRenderer meshRenderer;
        public Collider coll;

        void Start()
        {
            //mesh is in grandchild
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            coll = GetComponent<Collider>();
        }
        void Update()
        {
            if(pickedUp){
                timeOutCounter += Time.deltaTime;
                if(timeOutCounter>=timeOut){
                    //reactivate the can
                    meshRenderer.enabled = true;
                    coll.enabled = true;
                    pickedUp = false;
                    timeOutCounter = 0f;
                }
            }
            else{
                //rotate the can
                transform.Rotate(0, rotatationSpeed,0);
            }
        }


        public KindOfCan pickUp(){
            if(!pickedUp){
                pickedUp = true;
                //make object invisible and disable collider
                meshRenderer.enabled = false;
                coll.enabled = false;
                return  kindOfCan;
            }
            return KindOfCan.None;
            
        }
    }
}