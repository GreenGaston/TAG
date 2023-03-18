using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Loader
{
    public enum Scene{
        MainMenu,
        Searching,
        Lobby,
        Game
    }

    private static Scene targetScene;
    public static void Load(Scene targetScene){
        Loader.targetScene=targetScene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene.ToString());
        
    }
    public static void LoadNetwork(Scene targetScene){
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(),LoadSceneMode.Single);
    }
}
