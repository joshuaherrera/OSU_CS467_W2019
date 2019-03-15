using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
     public float moveSpeed;
     public Rigidbody2D theRB;

     public Animator myAnim;

     public static PlayerController instance;

     public string areaTransitionName;

     public string mainMenuScene;

     //currently unused, if we want to save upon exiting to main menu add above load Main Menu
     public SaveManager SaveToMain;

     public bool canMove = true;

     // Start is called before the first frame update
    void Start()
    {
         if(instance == null)
         { 
               instance = this;
         }
         else
         {
              Destroy(gameObject);
         }

         DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
		if (canMove)
		{
			theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
		}
		else
		{
			theRB.velocity = Vector2.zero;
		}

         myAnim.SetFloat("moveX", theRB.velocity.x);
         myAnim.SetFloat("moveY", theRB.velocity.y);

         if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || 
             Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
         {
               myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
               myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
         }

         // check for escape when not in CLI
         else if (Input.GetKeyDown("escape"))
         {
              // add save here if we want to save every time we exit to main
              // not enabled currently so saving in a conscious player effort
              // also this way we don't save over something without player knowledge
              SaveToMain.Save();
              SceneManager.LoadScene(mainMenuScene);
         }

    }
}
