﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    //public static int folderNameCount = 0;
    //public string saveName;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        //usekeycode to test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }
        //usekeycode to test
        if (Input.GetKeyDown(KeyCode.R))
        {
            Load();
        }
    }

    public void Save()
    {
        ///USE THIS STRUCTURE WHEN SAVES ARE SELECTABLE ONLY!!
        //saveName = "save";
        ///////////////////////////////////////////////////////
        try
        {
            //string fullSaveName = saveName + folderNameCount + ".dat";
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + "save0.dat" , FileMode.OpenOrCreate);

            SaveData data = new SaveData();
            Debug.Log("Before calling save()");
            SavePlayer(data);
            Debug.Log("After calling save()");

            bf.Serialize(file, data);

            file.Close();
            //int is not keeping its count between scene loads
            //folderNameCount += 1;
            
        }
        catch (System.Exception e)
        {
            //delete savegame if corrupted
            Debug.LogError(e.Message);
        }
    }

    //set values in data to be saved with file pointer.
    //manipulates given data
    public void SavePlayer(SaveData data)
    {
        /////////////////////////////////////////////////////////////////////////////
        ///TODO: revamp inventory to store in some container for saving and loading
        ///or figure out how to save game objects
        ///save player direction sprite.
        /////////////////////////////////////////////////////////////////////////////

        //creates the game object that will store the data.
        //without new declaration, will receive errors.
        data.MyPlayerData = new PlayerData();
        Debug.Log("done creating instance");
        //save player direction
        float posx = PlayerController.instance.myAnim.GetFloat("lastMoveX");
        float posy = PlayerController.instance.myAnim.GetFloat("lastMoveY");
        //save the current scene
        Scene currScene = SceneManager.GetActiveScene();
        string currSceneName = currScene.name;
        //save the current player position in the world
        Vector3 temp = PlayerController.instance.transform.position;
        SerializableVector3 target = (SerializableVector3)(temp);
        //save the data
        data.MyPlayerData.playerpos = target;
        data.MyPlayerData.areaToLoad = currSceneName;
        data.MyPlayerData.dirX = posx;
        data.MyPlayerData.dirY = posy;
        
    }

    public void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            //restructure when saves become selectable
            FileStream file = File.Open(Application.persistentDataPath + "/" + "save0.dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            //LoadPlayer(data);

            file.Close();

            LoadPlayer(data);
        }
        catch (System.Exception e)
        {
            //delete savegame if corrupted
            Debug.LogError(e.Message);
        }
    }

    //set values in data to be saved with file pointer
    public void LoadPlayer(SaveData data)
    {
        SerializableVector3 oldData = data.MyPlayerData.playerpos;
        string areaToLoad = data.MyPlayerData.areaToLoad;
        //load saved scene, player world pos, and player direction.
        //think about saving current scene name at start of file
        //and doing a check to avoid unnecessary loading.
        SceneManager.LoadScene(areaToLoad);
        PlayerController.instance.transform.position = (Vector3)oldData;
        PlayerController.instance.myAnim.SetFloat("lastMoveX", data.MyPlayerData.dirX);
        PlayerController.instance.myAnim.SetFloat("lastMoveY", data.MyPlayerData.dirY);
    }
}
