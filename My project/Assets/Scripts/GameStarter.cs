using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
public class GameStarter : NetworkBehaviour
{

    [SerializeField]
    
    public NetworkVariable<bool> started=new NetworkVariable<bool>(false);

    //public NetworkVariable<FixedString64> winner=new NetworkVariable<FixedString64>("");
    public NetworkVariable<float> timeOfGame=new NetworkVariable<float>(0f);

    public GameObject UI;
    public GameObject WinnerScreen;
    public float maxTimeOfGame=120f;

    //map of players and how much time they have been it
    public Dictionary<string,float> playerTimes=new Dictionary<string, float>();


    public NetworkVariable<FixedString64Bytes> currentgamemode=new NetworkVariable<FixedString64Bytes>("Chase");


    public Vector3 OutPosition;

   

    void Update(){
        if(!IsServer){
            return;
        }
        string gamemode=currentgamemode.Value.ToString();
        if(gamemode=="Chase"){
            GameMode1();
        }
        else if(gamemode=="Elimination"){
            GameMode2();
        }

        
    }



    void GameMode1(){
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
                //Debug.Log("GameStarter: Update: player: "+player.name);
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

    void GameMode2(){
        //gamemode that ends when all players are tagged they are move out of the game 
        //and the game ends when all players are out of the game
        //this game is played once for each player so the game ends when all players have played
        //then we count the
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
    public void MovePlayerServerRpc(string name,Vector3 pos){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            if(player.GetComponentInChildren<TMP_Text>().text==name){
                player.transform.position=pos;

            }
        }
    }


    //ownership not required
    [ServerRpc(RequireOwnership = false)]
    public void HandleCollisionServerRpc(string name1,string name2){
        Debug.Log("name1: "+name1+"     name2: "+name2);
        //assumes player1 is a chaser
        //if the game is not started do nothing
        if(!started.Value){
            return;
        }
        //if the game is chase
        if(currentgamemode.Value.ToString()=="Chase"){
            //if the player is a chaser
            //set the player to not be a chaser

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players){
                if(player.GetComponentInChildren<TMP_Text>().text==name1){
                    player.GetComponent<TagManager>().ChaserServerRpc();
                    Debug.Log("GameStarter: HandleCollisionServerRpc: player: "+name1+" is now a chaser");
                }
            }

            foreach(GameObject player in players){
                if(player.GetComponentInChildren<TMP_Text>().text==name2){
                    player.GetComponent<TagManager>().ChaserServerRpc();
                    Debug.Log("GameStarter: HandleCollisionServerRpc: player: "+name2+" is now a not a chaser");
                }
            }
            
        }
        else if(currentgamemode.Value.ToString()=="Elimination"){
            //if the the player is not a chaser do nothing
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players){
                if(player.GetComponentInChildren<TMP_Text>().text==name1){
                    if(!player.GetComponent<TagManager>().isChaser.Value){
                        return;
                    }
                }
            }
            //if the player is a chaser move the other player to the out position
            foreach(GameObject player in players){
                if(player.GetComponentInChildren<TMP_Text>().text==name2){
                    player.transform.position=OutPosition;
                }
            }


        }
        
    }
}
