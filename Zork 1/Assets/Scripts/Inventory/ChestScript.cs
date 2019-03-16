using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : BagScript
{
    // Init with 48 slots
    void Awake()
    {
        AddSlots(48);
    }
}
