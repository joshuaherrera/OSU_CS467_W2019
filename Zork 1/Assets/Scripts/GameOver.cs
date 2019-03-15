using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
     public SaveManager loadSave;

     // Start is called before the first frame update
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {

     }


     public void ExitGame()
     {
          // currently not saving on exit qsCD
          Application.Quit();
     }

     public void Continue()
     {
          loadSave.Load();
     }

}
