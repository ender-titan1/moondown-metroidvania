using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEquipable 
{
    public abstract string Name { get; protected set; }
    public abstract string Description { get; protected set; }

    public abstract AbstractModule Module { get; set; }
}
