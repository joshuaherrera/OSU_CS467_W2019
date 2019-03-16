using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //creates singleton for reuse, want to access from 
    //other places
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            //clickable.MyStackText.text = clickable.MyCount.ToString(); //set amount of items to string to display
            //clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            //clickable.MyStackText.color = new Color(0, 0, 0, 0);
            //Debug.Log("COUNT IS 1");
        }
        if (clickable.MyCount == 0) //no more items
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0); //hide icon
            //clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }
    //open and close anything
    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
