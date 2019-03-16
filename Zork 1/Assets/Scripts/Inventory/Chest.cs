using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public static Chest MyChestInstance;

    void Start()
    {
        
        if (MyChestInstance == null)
        {
            MyChestInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }
    

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private Sprite openSprite, closeSprite;

    private bool isOpen;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private List<Item> items; //contains item after chest closed

    [SerializeField]
    private BagScript bag;

    public List<Item> MyItems
    {
        get => items; set => items = value;
    }
    public BagScript MyBag
    {
        get => bag; set => bag = value;
    }

    private void Awake()
    {
        items = new List<Item>();
    }

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();//close the chest
        }
        else
        {
            AddItems();
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        StoreItems();
        MyBag.Clear();
        isOpen = false;
        spriteRenderer.sprite = closeSprite;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void AddItems()
    {
        if (MyItems != null)
        {
            foreach (Item item in MyItems)
            {
                //item always reference slot it belongs to
                //even when moved to chest so it adds itself there
                item.MySlot.AddItem(item);
            }
        }
    }

    public void StoreItems()
    {
        //called when chest is closed
        MyItems = MyBag.GetItems();//stores items we can see into a list for tracking
    }
}
