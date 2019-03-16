using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable
{
    [SerializeField]
    private Sprite icon; //all children of this superclass must have an icon

    [SerializeField]
    private int stacksize; //if items can be stacked

    private SlotScript slot;

    [SerializeField]
    private string title;

    public Sprite MyIcon
    {
        get => icon;
    }
    public int MyStackSize
    {
        get => stacksize;
    }
    public SlotScript MySlot 
    {
        get => slot;
        set => slot = value;
    }
    public string MyTitle
    {
        get => title;
        set => title = value;
    }

    public string GetDescription()
    {
        return MyTitle;
    }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}
