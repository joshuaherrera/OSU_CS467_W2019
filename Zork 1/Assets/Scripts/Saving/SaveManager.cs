using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;

    //store all items for loading. search through array and create new item object as needed
    [SerializeField]
    private Item[] items;

    public static SaveManager MySaveInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveManager>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        //usekeycode to test
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Save();
        }
        //usekeycode to test
        if (Input.GetKeyDown(KeyCode.F4))
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
             if (File.Exists(Application.persistentDataPath + "/" + "save0.dat"))
             {
                  File.Delete(Application.persistentDataPath + "/" + "save0.dat");
             }
            FileStream file = File.Open(Application.persistentDataPath + "/" + "save0.dat" , FileMode.OpenOrCreate);

            SaveData data = new SaveData();
            Debug.Log("Before calling save()");
            SaveBags(data);
            SaveInventory(data);
            SavePlayer(data);
            Debug.Log("After calling save()");

            bf.Serialize(file, data);

            file.Close();
            //int is not keeping its count between scene loads
            //folderNameCount += 1;
             PlayerPrefs.SetInt("saved_game", 1);

        }
        catch (System.Exception e)
        {
            //delete savegame if corrupted
            Debug.LogError(e.Message);
        }
    }

    //set values in data to be saved with file pointer.
    //manipulates given data
    //only for player data
    public void SavePlayer(SaveData data)
    {

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
        data.MyPlayerData.Playerpos = target;
        data.MyPlayerData.AreaToLoad = currSceneName;
        data.MyPlayerData.DirX = posx;
        data.MyPlayerData.DirY = posy;
        
    }

    public void SaveBags(SaveData data)
    {
        //shouldnt do anything because set limit to one bag, but
        //if we want to add more bags wed utilize this.
        //mybagbutton and index not implemented
        /*
        for (int i = 1; i < InventoryScript.MyInstance.MyBags.Count; i++)
        {
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.MyInstance.MyBags[i].MySlotCount, InventoryScript.MyInstance.MyBags[i].MyBagButton.MyBagIndex));
        }
        */
    }
    
    public void SaveInventory(SaveData data)
    {
        //get all current slots with items
        List<SlotScript> slots = InventoryScript.MyInstance.GetAllItems();
        foreach (SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MySlotIndex));
        }
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

            LoadBags(data);

            LoadInventory(data);

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
        SerializableVector3 oldData = data.MyPlayerData.Playerpos;
        string areaToLoad = data.MyPlayerData.AreaToLoad;
        //load saved scene, player world pos, and player direction.
        //think about saving current scene name at start of file
        //and doing a check to avoid unnecessary loading.
        SceneManager.LoadScene(areaToLoad);
        PlayerController.instance.transform.position = (Vector3)oldData;
        PlayerController.instance.myAnim.SetFloat("lastMoveX", data.MyPlayerData.DirX);
        PlayerController.instance.myAnim.SetFloat("lastMoveY", data.MyPlayerData.DirY);
    }

    //always have one bag. loads empty bags. need another fcn to load 
    //items
    public void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {
            Bag newBag = (Bag)Instantiate(items[0]);
            newBag.Initialize(bagData.MySlotCount);
            InventoryScript.MyInstance.AddBag(newBag);
        }
    }

    public void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            //searches debug items array, instantiated in unity view
            Item item = Array.Find(items, x=> x.MyTitle == itemData.MyTitle);

            for (int i = 0; i < 16; i++) //itemData.MyStackCount
            {
                InventoryScript.MyInstance.PlaceInSpecific(item, itemData.MySlotIdx, itemData.MyBagIdx);
            }
        }
    }

}
