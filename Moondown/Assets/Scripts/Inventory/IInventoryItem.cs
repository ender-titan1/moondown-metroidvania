using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    string Name { get; set; }
    string Description { get; set; }

    Sprite Image { get; set; }
}
