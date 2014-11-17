using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class LevelEndPanelController : MonoBehaviour
{
    public Text starCount;
    public Image blueToken;
    public Image greenToken;
    public Image pinkToken;
    public Image beigeToken;
    public Image yellowToken;

    private GameObject manager;
    private ItemManager itemManager;

    private Dictionary<AlienState, Color> alienStateColorMap = new Dictionary<AlienState, Color>();

    private Color activeColor = new Color(1f, 1f, 1f, 1f);
    private Color inactiveColor = new Color(1f, 1f, 1f, 0.5f);
    private Color completeColor = new Color(0.98f, 0.78f, 0.2f, 1f);



    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        itemManager = manager.GetComponent<ItemManager>();
        alienStateColorMap[AlienState.ACTIVE] = activeColor;
        alienStateColorMap[AlienState.INACTIVE] = activeColor;
        alienStateColorMap[AlienState.DISABLED] = inactiveColor;
    }

    void Update ()
    {
        starCount.text = itemManager.nCollectedStars + "/" + itemManager.nTotalStars;
        if(itemManager.nCollectedStars == itemManager.nTotalStars)
        {
            starCount.color = completeColor;
        }
        blueToken.color = alienStateColorMap[LevelManager.alienStates[AlienType.BLUE]];
        greenToken.color = alienStateColorMap[LevelManager.alienStates[AlienType.GREEN]];
        pinkToken.color = alienStateColorMap[LevelManager.alienStates[AlienType.PINK]];
        beigeToken.color = alienStateColorMap[LevelManager.alienStates[AlienType.BEIGE]];
        yellowToken.color = alienStateColorMap[LevelManager.alienStates[AlienType.YELLOW]];
    }
}
