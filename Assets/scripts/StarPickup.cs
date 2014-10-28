using UnityEngine;
using System.Collections;

public class StarPickup : ItemPickup
{

    void Start()
    {
        itemType = ItemType.STAR;
    }

    public void PickedUp()
    {
        gameObject.SetActive(false);
    }
}
