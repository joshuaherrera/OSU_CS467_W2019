using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
     public string newGameScene;

     public string fileName = "save0";
     public string fileExtension = ".dat";

     public GameObject continueButton;

     public SaveManager loadSave;
     
     // Start is called before the first frame update
     void Start()
     {
          if (PlayerPrefs.GetInt("saved_game") == 1)
          {
               continueButton.SetActive(true);
          }
          else
          {
               continueButton.SetActive(false);
          }
     }

     // Update is called once per frame
     void Update()
     {

     }

     public void Continue()
     {

          loadSave.Load();
     }

     public void NewGame()
     {
          string filePath = Application.dataPath + "/" + fileName + fileExtension;
          if (File.Exists(filePath))
          {
               File.Delete(filePath);
               #if UNITY_EDITOR
                    UnityEditor.AssetDatabase.Refresh();
               #endif
          }
          SceneManager.LoadScene(newGameScene);
          PlayerPrefs.SetInt("cyclops_deaths", 0);
          PlayerPrefs.SetInt("troll_deaths", 0);
          PlayerPrefs.SetInt("bat_deaths", 0);
          PlayerPrefs.SetInt("saved_game", 0);
     }

     public void Exit()
     {
          // currently not saving on exit qsCD
          Application.Quit();
     }

}
