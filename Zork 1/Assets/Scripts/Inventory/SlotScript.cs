using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    //maybe not needed

    private static SlotScript instance;

    public static SlotScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SlotScript>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    //probably need to save this
    //used if want to stack items in one slot
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text stackSize;

    //added refernce to bagscript slot belongs to
    public BagScript MyBag {get; set;}

    public int MySlotIndex { get; set; }

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }
            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }
            return null;
        }
    }
    //Iclickable interface
    public Image MyIcon
    {
        get => icon;
        set => icon = value;
    }

    public int MyCount
    {
        get => MyItems.Count;
    }
    public ObservableStack<Item> MyItems
    {
        get => items; set => items = value;
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
}

    //end interface
    public bool AddItem(Item item)
    {
        //Debug.Log("AddItem called from SlotScript");
        //Debug.Log(items.Count);
        //use bag instance to add to array in bag object... TODO
        //eg Bag.MyBagInstance.addItem(item);
        //would allow for storage of item in bag parent
        //this way, all slots would reference the same array.
        //otherwise, all were saving are sprites and colors, and not an actual array
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }
    //popping bag, not slot?
    public void RemoveItem(Item toRemove)
    {
        if(!IsEmpty)
        {
            MyItems.Pop();
            //look for video to make MyInstance
            //UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    //since it is in our slot script, it has a click function
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("POINTER EVENT CAPTURED");
        //inventory script slots call this.
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty)
            {
                HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                InventoryScript.MyInstance.FromSlot = this;
            }
            //if holding something
            else if (InventoryScript.MyInstance.FromSlot != null)
            {
                if (PutItemBack() || SwapItems(InventoryScript.MyInstance.FromSlot) ||AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
        }
        
    }

    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
    }

    public bool PutItemBack()
    {
        //we picked it up and try to put it back
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }
        return false;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;
            //check if curr slot full
            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }
                AddItem(newItems.Pop());
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// TO DO IS THIS!!!!!!!!!!!!!!!!!!!!! video 11.9 11:44 minutes
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    public bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            //copy all items we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);
            //clear slot A to replace with B
            from.items.Clear();
            //take from B into A
            from.AddItems(items);
            //do the inverse
            items.Clear();
            AddItems(tmpFrom);
            return true;
        }
        return false;
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
        {
            items.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;

    }
    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }
}
