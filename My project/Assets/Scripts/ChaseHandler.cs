using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

 public enum ChaseState{
        Chaser,
        Runner,
        Spectator
    }
public class ChaseHandler : MonoBehaviour
{

   
    [SerializeField]
    private Movement.FinalMove _Move;
    [SerializeField]
    private TagManager _TagManager;
    [SerializeField]
    //camera orientation of player
    private Transform _CameraTransform;
    public ChaseState _ChaseState=ChaseState.Runner;

    public bool isChasing = false;
    public float chaseSpeedMultiplier = 1.05f;
    public float chaseLockTime = 5f;
    public float chaseLockClock = 0f;
    public bool isLocked = false;
    public float RunnerSpeedMultiplier = 1.3f;
    public float RunnerSpeedTime = 5f;
    public float RunnerSpeedClock = 0f;
    public bool hasRunnerSpeed = false;

    //time player needs to look at another player to become chasing
    public float LookTime= 5f;
    public float LookClock = 0f;

    public float ChaserSpeedBonus = 1.2f;

    private GameStarter _GameStarter;

    private string name;


    void Start(){
        _GameStarter=GameObject.Find("TagStarter").GetComponent<GameStarter>();
        //get name from children in parent NameApplier in parent object
        name=transform.parent.GetComponent<NameApplier>().name.Value.ToString();
    }

    void Update(){
        name=transform.parent.GetComponent<NameApplier>().name.Value.ToString();
        if(isLocked){
            chaseLockClock+=Time.deltaTime;
            if(chaseLockClock>=chaseLockTime){
                chaseLockClock=0f;
                isLocked=false;
                _Move.ChaserLocked=false;
            }
            return;
        }
        if(hasRunnerSpeed){
            RunnerSpeedClock+=Time.deltaTime;
            if(RunnerSpeedClock>=RunnerSpeedTime){
                RunnerSpeedClock=0f;
                hasRunnerSpeed=false;
                _Move.speedMultiplier/=RunnerSpeedMultiplier;
            }
        }

        if(_ChaseState==ChaseState.Chaser&&!isChasing){
            //cast out ray to see if player is looking at another player
            RaycastHit hit;
            if(Physics.Raycast(_CameraTransform.position,_CameraTransform.forward,out hit,10)){
                if(hit.collider.gameObject.tag=="Player"){
                    LookClock+=Time.deltaTime*2;
                    if(LookClock>=LookTime){
                        LookClock=0f;
                        isChasing=true;
                        _Move.speedMultiplier*=chaseSpeedMultiplier;
                    }
                }
            }
            LookClock-=Time.deltaTime;
            LookClock=Mathf.Min(LookClock,0f);
        }
    }

    public void BecomeChaser(){
        Debug.Log("ChaseHandler: BecomeChaser");
        _Move.speedMultiplier=ChaserSpeedBonus;
        _Move.ChaserLocked=true;
        isLocked=true;
        _ChaseState=ChaseState.Chaser;
        hasRunnerSpeed=false;
        RunnerSpeedClock=0f;

        
    }

    public void BecomeRunner(){
        _Move.speedMultiplier=1f;
        _Move.speedMultiplier*=RunnerSpeedMultiplier;
        hasRunnerSpeed=true;
        _ChaseState=ChaseState.Runner;
        isChasing=false;

        
    }

    public void BecomeSpectator(){
        _Move.speedMultiplier=1f;
        _ChaseState=ChaseState.Spectator;
        isChasing=false;
        hasRunnerSpeed=false;
        RunnerSpeedClock=0f;
    }

    //collider for when player enters another player's collider
    void OnTriggerEnter(Collider other){
        if(_ChaseState!=ChaseState.Chaser){
            return;
        }
        if(isLocked){
            return;
        }
        if(other.gameObject.tag=="Player"){
            Debug.Log("ChaseHandler: OnTriggerEnter");
            //let the game starter handle the collision
            NameApplier temp=other.transform.GetComponent<NameApplier>();
            string othername=temp.name.Value.ToString();
            _GameStarter.HandleCollisionServerRpc(name,othername);
        }
    }

    //draw the line used to check if player is looking at another player
    void OnDrawGizmos(){
       
        Gizmos.color=Color.red;
        Gizmos.DrawLine(_CameraTransform.position,_CameraTransform.position+_CameraTransform.forward*10);
    
    }
}
