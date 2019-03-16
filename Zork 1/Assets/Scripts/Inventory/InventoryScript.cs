using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private SlotScript fromSlot;

    [SerializeField]
    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    //debugging purposes. Contains items we will use to make copies of
    public Item[] items;

    public bool CanAddBag
    {  
        get { return bags.Count < 1; }
    }

    public List<Bag> MyBags
    {
        get => bags;
    }

    //refactor
   
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }
            return count;
        }

        //set;
    }

    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }
        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
}

    /*
    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }
            return count;
        }
    }
    

    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }
    */



    //used for drag and drop in tutorial
    /*
    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }
        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }
    */

    private void Awake()
    {
        if(CanAddBag)
        {
           // Debug.Log("awake inventory called");
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            bag.Use();
            bag.Clicked = false;
            //call once to hide initialized bag
            bag.MyBagScript.OpenClose(bag);
        }
    }
    //debug
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.L))
        {
            //could I call this on collision?
            /*
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            bag.Use();*\/
            //instantiate uses the specified array to make a copy
            //of the object specified.
            //scene view shows weve stored a leaflet item at pos
            //1 and are making a copy to the leaflet declared here.
            Leaflet leaflet = (Leaflet)Instantiate(items[1]);
            AddItem(leaflet);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            //could I call this on collision?
            /*
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            bag.Use();*\/
            //instantiate uses the specified array to make a copy
            //of the object specified.
            //scene view shows weve stored a leaflet item at pos
            //1 and are making a copy to the leaflet declared here.
            Egg egg = (Egg)Instantiate(items[2]);
            AddItem(egg);
        }
        */
        //test to see if can close bag so I can know if
        //bag is not working or just not receiving input
        /*
        else if (Input.GetKeyDown(KeyCode.X))
        {
            MyBags[0].MyBagScript.OpenClose(MyBags[0]);
        }
        */
    }
    //end debuggging

    public void AddItem(Item item)
    {
        //checks each available bag. Curr using one bag
        foreach (Bag bag in MyBags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
    }

    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                MyBags.Add(bag); //add bag to count for display purposes
                bag.MyBagButton = bagButton;
                //bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
                break;
            }
        }
    }

    public void AddBag(Bag bag, BagButton bagButton)
    {
        MyBags.Add(bag);
        bagButton.MyBag = bag;
        //bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
    }

    public void AddBag(Bag bag, int bagIndex)
    {
        bag.SetupScript();
        MyBags.Add(bag);
        bag.MyBagButton = bagButtons[bagIndex];
        bagButtons[bagIndex].MyBag = bag;
    }

    public void PlaceInSpecific(Item item, int slotidx, int bagidx)
    {
        bags[bagidx].MyBagScript.MySlots[slotidx].AddItem(item);
    }

    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();
        foreach(Bag bag in MyBags)
        {
            foreach(SlotScript slot in bag.MyBagScript.MySlots)
            {
                //look for a slot thats no empty and add
                if (!slot.IsEmpty)
                {
                    slots.Add(slot);
                }
            }
        }
        return slots;
    }
}
