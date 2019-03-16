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
        if (clickable.MyCount == 0) //no more items
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0); //hide icon
        }
    }
    //open and close anything
    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
