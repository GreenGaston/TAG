using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using Movement;
public class GameStarter : NetworkBehaviour
{

    [SerializeField]
    
    public NetworkVariable<bool> started=new NetworkVariable<bool>(false);

    //public NetworkVariable<FixedString64> winner=new NetworkVariable<FixedString64>("");
    public NetworkVariable<float> timeOfGame=new NetworkVariable<float>(0f);

    public GameObject UI;
    public GameObject WinnerScreen;
    public float maxTimeOfGame=120f;

    public Transform[] spawnPoints;

    //map of players and how much time they have been it
    public Dictionary<string,float> playerTimes=new Dictionary<string, float>();


    public NetworkVariable<FixedString64Bytes> currentgamemode=new NetworkVariable<FixedString64Bytes>("Elimination");


    public Vector3 OutPosition;

   void Start(){
        currentgamemode.Value="Elimination";
        Debug.Log("GameStarter: Server started, gamemode: "+currentgamemode.Value.ToString());
       
   }

    void FixedUpdate(){
        if(!IsServer||!started.Value){
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
                resetChasersServerRpc();
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
                if(!(player.GetComponent<TagManager>().TagState.Value==ChaseState.Chaser)){
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
                if(player.GetComponent<TagManager>().TagState.Value==ChaseState.Chaser){
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




    public float GameTime=0f;
    public float GameTimeLimit=180f;

    public float TimeBetweenGames=5f;
    void GameMode2(){
        if(waiting){
            waittimeclock+=Time.deltaTime;
            if(waittimeclock>=waitTime){
                waiting=false;
                waittimeclock=0f;
                UnlockPlayerClientRpc();
                Debug.Log("GameStarter: GameMode2: Unlocking players, done waiting");
            }
            return;
        }
        if(betweenRounds){
            betweenRoundTimeClock+=Time.deltaTime;
            if(betweenRoundTimeClock>=betweenRoundTime){
                betweenRounds=false;
                betweenRoundTimeClock=0f;
                StartRoundGameMode2();
                Debug.Log("GameStarter: GameMode2: Starting round, time between rounds over");
            }
            return;
        }
        GameTime+=Time.fixedDeltaTime;
        bool allTagged=true;
        foreach(bool tag in tagged){
            if(!tag){
                allTagged=false;
                break;
            }
        }
        if(GameTime>=GameTimeLimit||allTagged){
            Debug.Log("GameStarter: GameMode2: Ending round");
            //end round
            GameTime=0f;
            betweenRounds=true;
            bool allPlayed=true;
            foreach(bool played in hasPlayed){
                if(!played){
                    allPlayed=false;
                    break;
                }
            }
            if(allPlayed){
                Debug.Log("GameStarter: GameMode2: everyone has played, ending game");
                //end game
                started.Value=false;
                //find the player with the least score
                int leastScore=0;
                int leastScoreIndex=0;
                for(int i=0;i<playerScores.Length;i++){
                    if(i==0){
                        leastScore=playerScores[i];
                        leastScoreIndex=i;
                    }
                    else{
                        if(playerScores[i]<leastScore){
                            leastScore=playerScores[i];
                            leastScoreIndex=i;
                        }
                    }
                }
                spawnWinnerUIClientRpc(players[leastScoreIndex].GetComponentInChildren<TMP_Text>().text);
                //reset all players
                resetChasersServerRpc();
                MovePlayersToSpawnsServerRpc();
                return;
            }
            else{
                Debug.Log("GameStarter: GameMode2: not everyone has played, waiting for next round");
                betweenRounds=true;
            }
            
            
        }
        else{

            int nontagged=0;
            for(int i=0;i<tagged.Length;i++){
                if(!tagged[i]){
                    nontagged++;
                }
            }
            playerScores[currentChaserIndex]+=nontagged;

        }
           
        


    }



    float waitTime=2f;
    float waittimeclock=0f;
    bool waiting=false;

    float betweenRoundTime=5f;
    float betweenRoundTimeClock=0f;
    bool betweenRounds=false;


    //lists are related to each other
    int currentChaserIndex=0;
    GameObject[] players;
    bool[] hasPlayed;
    bool[] tagged;
    int[] playerScores;
    void StartGameMode2(){
        players= GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("GameStarter: StartGameMode2: players.Length: "+players.Length);
        hasPlayed=new bool[players.Length];
        tagged=new bool[players.Length];
        playerScores=new int[players.Length];
        started.Value=true;
        StartRoundGameMode2();

    }

    void StartRoundGameMode2(){
        resetChasersServerRpc();
        MovePlayersToSpawnsServerRpc();
        //lock all players
        LockPlayerClientRpc();
        //select a random player to be the first chaser
        currentChaserIndex=Random.Range(0,players.Length);
        while(hasPlayed[currentChaserIndex]){
            currentChaserIndex=Random.Range(0,players.Length);
        }
        hasPlayed[currentChaserIndex]=true;
        waiting=true;
        for(int i=0;i<players.Length;i++){
            tagged[i]=false;
        }
        //set the player to be a chaser
        players[currentChaserIndex].GetComponent<TagManager>().TagState.Value=ChaseState.Chaser;
        tagged[currentChaserIndex]=true;
    }





    [ServerRpc(RequireOwnership = false)]
    void MovePlayersToSpawnsServerRpc(){
        players= GameObject.FindGameObjectsWithTag("Player");
        //move each player to the a spawn point without using spawnpoints twice
        List<int> usedIndexes=new List<int>();
        foreach(GameObject player in players){
            int index=Random.Range(0,spawnPoints.Length);
            while(usedIndexes.Contains(index)){
                index=Random.Range(0,spawnPoints.Length);
            }
            usedIndexes.Add(index);
            string name= player.GetComponentInChildren<TMP_Text>().text;
            Debug.Log("GameStarter: MovePlayersToSpawnsServerRpc: name: "+name);
            MovePlayerClientRpc(index,name);
        
        }
    }

    [ClientRpc]
    void MovePlayerClientRpc(int index,string name){
        players= GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            if(player.GetComponent<NetworkObject>().IsOwner&&player.GetComponentInChildren<TMP_Text>().text==name){
                player.GetComponentInChildren<FinalMove>().lockforframe=true;
                player.transform.position=spawnPoints[index].position;
                Debug.Log("GameStarter: MovePlayerClientRpc: moved player "+name+" to spawn point "+index);
                
                
                return;
            }
            
        }
        Debug.Log("GameStarter: MovePlayerClientRpc: could not find player "+name);
    }
    [ClientRpc]
    void MovePlayerClientRpc(Vector3 position,string name){
        players= GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            if(player.GetComponent<NetworkObject>().IsOwner&&player.GetComponentInChildren<TMP_Text>().text==name){
                player.GetComponentInChildren<FinalMove>().lockforframe=true;
                player.transform.position=position;
                break;
            }
        }
    }
    
    
    void OnTriggerEnter(Collider other){
        
     
        
        //if already started or the player is not the host
        if(started.Value||!IsServer){
            return;
        }
        Debug.Log("GameStarter: OnTriggerEnter: game started");
        string gamemode=currentgamemode.Value.ToString();
        Debug.Log("GameStarter: OnTriggerEnter: gamemode: "+gamemode);
        if(gamemode=="Chase"){
            StartGameMode1();
        }
        else if(gamemode=="Elimination"){
            StartGameMode2();
            //MovePlayersToSpawnsServerRpc();
        }

    }




    void StartGameMode1(){
        //get all objects with the player tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length<1){
            Debug.Log("Not enough players to start game");
            return;
        }
        //choose a random player to be the chaser
        int chaserIndex = Random.Range(0,players.Length);
        //set the chaser to be the chaser use serverrpc
        players[chaserIndex].GetComponent<TagManager>().ChaserServerRpc(ChaseState.Chaser);
        //set the game to started
        Debug.Log("GameStarter: OnTriggerEnter: game started1");
        started.Value=true;
        Debug.Log("GameStarter: OnTriggerEnter: game started2");

        //spawn the UI
        spawnUIClientRpc();
    }

    


    [ClientRpc]
    public void LockPlayerClientRpc(){
        //lock the player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            if(player.GetComponent<NetworkObject>().IsOwner){
                player.GetComponentInChildren<FinalMove>().enabled=false;
            }
        }
    }
    [ClientRpc]
    public void UnlockPlayerClientRpc(){
        //unlock the player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            if(player.GetComponent<NetworkObject>().IsOwner){
                player.GetComponentInChildren<FinalMove>().enabled=true;
            }
        }
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
    [ServerRpc]
    public void resetChasersServerRpc(){
        //set all players to not be chasers
        GameObject[] players2 = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players2){
            player.GetComponent<TagManager>().TagState.Value=ChaseState.Runner;
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
                    player.GetComponent<TagManager>().ChaserServerRpc(ChaseState.Runner);
                    
                }
            }

            foreach(GameObject player in players){
                if(player.GetComponentInChildren<TMP_Text>().text==name2){
                    player.GetComponent<TagManager>().ChaserServerRpc(ChaseState.Chaser);
                    
                }
            }
            
        }
        else if(currentgamemode.Value.ToString()=="Elimination"){
            //find the player that was tagged

            //MovePlayersToSpawnsServerRpc();
            for(int i=0;i<players.Length;i++){
                if(players[i].GetComponentInChildren<TMP_Text>().text==name2){
                    //set the player to be tagged
                    players[i].GetComponent<TagManager>().TagState.Value=ChaseState.Chaser;
                    //move the player to the oob
                    MovePlayerClientRpc(OutPosition,players[i].GetComponentInChildren<TMP_Text>().text);
                    tagged[i]=true;
                    break;
                }
            }
        }
        
    }
}
