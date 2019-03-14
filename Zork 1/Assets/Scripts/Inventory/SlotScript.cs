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

    //probably need to save this
    //used if want to stack items in one slot
    private Stack<Item> items = new Stack<Item>();

    [SerializeField]
    private Image icon;

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
    public Stack<Item> MyItems { get => items; set => items = value; }

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
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    //since it is in our slot script, it has a click function
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("POINTER EVENT CAPTURED");
        //inventory script slots call this.
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
        /*else if (eventData.button == PointerEventData.InputButton.Left)
        {
           RemoveItem();
        }*/
    }

    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
    }
}
