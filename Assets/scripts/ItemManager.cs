using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemType
{
    NONE = -1,
    STAR = 0,
    TOKEN,
}

public class ItemManager : MonoBehaviour
{
    public int nCollectedStars;
    public int nTotalStars;
    private List<GameObject> disabledObjects = new List<GameObject>();
    private GameObject manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
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
                DisableObject(obj);
                break;
            case ItemType.TOKEN:
                AlienType alienType = obj.GetComponent<TokenDetail>().alienType;
                LevelManager levelManager = manager.GetComponent<LevelManager>();
                levelManager.ActivateAlien(alienType);
                DisableObject(obj);
                break;
            default:
                break;
        }
    }

    void DisableObject(GameObject obj)
    {
        obj.SetActive(false);
        disabledObjects.Add(obj);
    }

}
