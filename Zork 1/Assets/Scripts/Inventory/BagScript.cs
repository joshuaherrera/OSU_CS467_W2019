using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    //maybe not needed
    
    private static BagScript instance;

    public static BagScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BagScript>();
            }

            return instance;
        }
    }

    public List<SlotScript> MySlots { get => slots;}

    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup canvasGroup; //used to hide/show ui

    private List<SlotScript> slots = new List<SlotScript>();

    public int MyBagIndex { get; set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //makes slots for bag
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>(); //slotprefab child of current bagscipt's transform
            slot.MySlotIndex = i; //used to save items
            slot.MyBag = this;
            MySlots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        //look for an empty slot and put item
        foreach(SlotScript slot in MySlots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                //Debug.Log(MySlots.Count);
                return true;
            }
        }
        Debug.Log("Inventory full!");
        return false;
    }
    /*
    public int MyEmptySlotCount()
    {
        int emptySlots = 0;
        foreach (SlotScript slot in MySlots)
        {
            if (slot.IsEmpty)
            {
                emptySlots++;
            }
        }
        Debug.Log(emptySlots);
        return emptySlots;
    }
    */
    public int MyEmptySlotCount
    {
        get
        {
            int emptySlots = 0;
            foreach (SlotScript slot in MySlots)
            {
                if (slot.IsEmpty)
                {
                    emptySlots++;
                }
            }
            //Debug.Log(emptySlots);
            return emptySlots;
        }
    }


    public void OpenClose(Bag bag)
    {
        //Debug.Log("openclose called");
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1; //shown: 1, hidden: 0
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true; //similar logic
        bag.Clicked = bag.Clicked == true ? false : true;
    }
}
