using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//import file
using System.IO;

public class SettingsFile {
    //there is a txt file in the project folder called settings.txt
    //this file contains various setting from the game
    //this class reads the file and stores the settings in a dictionary
    //the dictionary can then be used to get the settings

    //the dictionary that stores the settings
    private static Dictionary<string,string> settings;
    private static bool fileRead=false;
    //constructor
 

    //read the file
    public static void ReadFile(){
        //set the file as read
        fileRead=true;
        //create the dictionary
        settings=new Dictionary<string,string>();
        
        //split the file into lines
        string[] lines=File.ReadAllText("Assets\\settings.txt").Split(new char[]{'\r','\n'},System.StringSplitOptions.RemoveEmptyEntries);
        //loop through the lines
        foreach(string line in lines){
            //split the line into key and value
            string[] keyvalue=line.Split(new char[]{'='},System.StringSplitOptions.RemoveEmptyEntries);
            //add the key and value to the dictionary
            settings.Add(keyvalue[0],keyvalue[1]);
        }

    }

    public static string getVariable(string key){
        //if the file has not been read
        if(!fileRead){
            //read the file
            ReadFile();
        }
        //return the value of the key
        return settings[key];
    }
    public static float getVariableFloat(string key){
        //if the file has not been read
        if(!fileRead){
            //read the file
            ReadFile();
        }
        //return the value of the key
        return convertString(settings[key]);
    }

    public static void setVariable(string key,string value){
        //if the file has not been read
        if(!fileRead){
            //read the file
            ReadFile();
        }
        //set the value of the key
        settings[key]=value;
    }


    public static void saveSettings(){
        
        //create a string to store the file
        string file="";
        //loop through the dictionary
        foreach(KeyValuePair<string,string> pair in settings){
            //add the key and value to the file
            file+=pair.Key+"="+pair.Value+"\r";
        }
        //save the file
        System.IO.File.WriteAllText("Assets\\settings.txt",file);
    }
    public static float convertString(string s){
        float f = 0f;
        float.TryParse(s, out f);
        return f;
    }

}
