using UnityEngine;
using System.Collections;

public enum ItemType
{
    NONE = -1,
    STAR = 0,
}

public class ItemPickup : MonoBehaviour
{
    public ItemType itemType;

    public void PickedUp()
    {}
}
