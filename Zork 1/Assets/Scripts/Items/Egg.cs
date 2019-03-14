using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="egg", menuName ="Items/Egg", order =3)]
public class Egg : Item, IUseable
{
    public void Use()
    {
        //need code to reveal treasure
        //prompt user if want to rm
        Remove();
    }
}