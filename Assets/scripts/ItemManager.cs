using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemType
{
    NONE = -1,
    STAR = 0,
}

public class ItemManager : MonoBehaviour
{
    private List<GameObject> disabledObjects = new List<GameObject>();
    public int nCollectedStars;
    public int nTotalStars;

    void Start()
    {
        nCollectedStars = 0;
        nTotalStars = 0;
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item"))
        {
            if(item.GetComponent<ItemDetail>().itemType == ItemType.STAR)
            {
                nTotalStars += 1;
            }
        }
    }

    public void PickUp(GameObject obj)
    {
        ItemType itemType = obj.GetComponent<ItemDetail>().itemType;
        switch(itemType)
        {
            case ItemType.NONE:
                break;
            case ItemType.STAR:
                nCollectedStars += 1;
                obj.SetActive(false);
                disabledObjects.Add(obj);
                break;
            default:
                break;
        }
    }

}
