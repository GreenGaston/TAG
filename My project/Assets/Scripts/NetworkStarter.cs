using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Movement{
        
    public class NetworkStarter : NetworkBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private GameObject GUI;

        [SerializeField]
        private GameObject playerFollowCamera;

        [SerializeField]
        private GameObject maincamera;

        [SerializeField]
        private GameObject movementHandler;
        void Start()
        {
            //script enables all components of the player prefab if it is the owner
            if(!IsOwner){
                return;
            }
            //current object that need to be enabled by object
            //GUI
            //has an object named Canvas which itself has 2 objects a quad and a camera
            //the quad is GUI and needs its mesh renderer enabled
            //the camera has a camera component and needs its enabled
            //Main Camera
            //this has 3 components a camera, a audio listener and a CinemachineBrain
            //they all need to be enabled
            //playerFollowCamera
            //this has the cinemachine virtual camera component and needs to be enabled
            //movementHandler
            //this has the following script that need to be enabled:
            //Finalmove.cs , NormalMovement.cs , AirMovement.cs , CameraScript.cs , StarterAssetsInputs.cs , Wallrunning.cs, Boosting.cs ,Sliding.cs , StateManager.cs
            //and a player input component which needs to be enabled
            
            //enable all components of the player prefab
            GameObject quad=GUI.transform.GetChild(0).GetChild(0).gameObject;
            GameObject camera=GUI.transform.GetChild(0).GetChild(1).gameObject;
            quad.GetComponent<MeshRenderer>().enabled=true;
            camera.GetComponent<Camera>().enabled=true;
            maincamera.GetComponent<Camera>().enabled=true;
            maincamera.GetComponent<AudioListener>().enabled=true;
            maincamera.GetComponent<Cinemachine.CinemachineBrain>().enabled=true;
            playerFollowCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled=true;
            movementHandler.GetComponent<FinalMove>().enabled=true;
            movementHandler.GetComponent<NormalMovement>().enabled=true;
            movementHandler.GetComponent<AirMovement>().enabled=true;
            movementHandler.GetComponent<CameraScript>().enabled=true;
            movementHandler.GetComponent<StarterAssetsInputs>().enabled=true;
            movementHandler.GetComponent<Wallrunning>().enabled=true;
            movementHandler.GetComponent<Boosting>().enabled=true;
            movementHandler.GetComponent<Sliding>().enabled=true;
            movementHandler.GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled=true;
            movementHandler.GetComponent<StateManager>().enabled=true;

            
            //get the gameobject with the tag MenuCamera and destroy it
            GameObject menuCamera=GameObject.FindGameObjectWithTag("MenuCamera");
            Destroy(menuCamera);



        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
