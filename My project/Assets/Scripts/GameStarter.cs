using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GameStarter : NetworkBehaviour
{

    [SerializeField]
    
    public NetworkVariable<bool> started=new NetworkVariable<bool>(false);

    public NetworkVariable<float> timeOfGame=new NetworkVariable<float>(0f);

    public GameObject UI;
    public GameObject WinnerScreen;
    public float maxTimeOfGame=120f;

    //map of players and how much time they have been it
    public Dictionary<string,float> playerTimes=new Dictionary<string, float>();

   

    void Update(){
        if(!IsServer){
            return;
        }
        if(started.Value){
            timeOfGame.Value+=Time.deltaTime;
            if(timeOfGame.Value>=maxTimeOfGame){
                //end game
                started.Value=false;
                timeOfGame.Value=0f;
                //go through all players and find the one with the least time
                float leastTime=0f;
                string leastTimePlayer="";
                foreach(KeyValuePair<string,float> playerTime in playerTimes){
                    if(leastTimePlayer==""){
                        leastTimePlayer=playerTime.Key;
                        leastTime=playerTime.Value;
                    }else{
                        if(playerTime.Value<leastTime){
                            leastTimePlayer=playerTime.Key;
                            leastTime=playerTime.Value;
                        }
                    }
                }
                spawnWinnerUIClientRpc(leastTimePlayer);
                resetChasersClientRpc();
                //set all players to not be chasers
                return;

                
            }
            //loop through all players and add time to their times
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players){
                //get text from  textmeshpro-text child
                Debug.Log("GameStarter: Update: player: "+player.name);
                string name= player.GetComponentInChildren<TMP_Text>().text;
                
                //if the player is not a chaser skip them
                if(!player.GetComponent<TagManager>().isChaser.Value){
                    continue;
                }
                if(playerTimes.ContainsKey(name)){
                    playerTimes[name]+=Time.deltaTime;
                }else{
                    playerTimes.Add(name,Time.deltaTime);
                }
            }
            //loop through all players if non are chasers something went wrong

            bool foundChaser=false;
            foreach(GameObject player in players){
                if(player.GetComponent<TagManager>().isChaser.Value){
                    foundChaser=true;
                    break;
                }
            }
            if(!foundChaser){
                Debug.Log("No chasers found");
                started.Value=false;
                timeOfGame.Value=0f;
                playerTimes.Clear();
            }
        }

        
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("GameStarter: OnTriggerEnter: entered");
        
        //if already started or the player is not the host
        if(started.Value||!IsServer){
            return;
        }
        //get all objects with the player tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length<1){
            Debug.Log("Not enough players to start game");
            return;
        }
        //choose a random player to be the chaser
        int chaserIndex = Random.Range(0,players.Length);
        //set the chaser to be the chaser use serverrpc
        players[chaserIndex].GetComponent<TagManager>().ChaserServerRpc();
        //set the game to started
        Debug.Log("GameStarter: OnTriggerEnter: game started1");
        started.Value=true;
        Debug.Log("GameStarter: OnTriggerEnter: game started2");

        //spawn the UI
        spawnUIClientRpc();

    }

    [ClientRpc]
    public void spawnUIClientRpc(){
        GameObject temp=Instantiate(UI);
        temp.gameObject.GetComponentInChildren<TimeTagScript>().StartTimer(maxTimeOfGame);
     
    }
    [ClientRpc]
    public void spawnWinnerUIClientRpc(string leastTimePlayer){
        //spawn the winner screen
        GameObject winnerScreen = Instantiate(WinnerScreen);
        winnerScreen.GetComponentInChildren<WinnerDisplayScript>().winner=leastTimePlayer+" won!";
    }
    [ClientRpc]
    public void resetChasersClientRpc(){
        //set all players to not be chasers
        GameObject[] players2 = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players2){
            player.GetComponent<TagManager>().isChaser.Value=false;
        }
    }

    [ServerRpc]
    public void StartGameServerRpc(){

    }
}
