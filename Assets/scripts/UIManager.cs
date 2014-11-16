using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    public float acceleration;
    private float moveSpeed;
    private bool direction;
    private bool moving;

    public Text starCount;
    public Image blueToken;
    public Image greenToken;
    public Image pinkToken;
    public Image beigeToken;
    public Image yellowToken;

    private Dictionary<AlienState, Color> alienStateColorMap = new Dictionary<AlienState, Color>();

    private GameObject manager;
    private ItemManager itemManager;
    private LevelManager levelManager;

    private Color activeColor = new Color(1f, 1f, 1f, 1f);
    private Color inactiveColor = new Color(1f, 1f, 1f, 0.5f);
    private Color disabledColor = new Color(1f, 1f, 1f, 0.1f);
    private Color completeColor = new Color(0.98f, 0.78f, 0.2f, 1f);

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        levelManager = manager.GetComponent<LevelManager>();
        itemManager = manager.GetComponent<ItemManager>();
        alienStateColorMap[AlienState.ACTIVE] = activeColor;
        alienStateColorMap[AlienState.INACTIVE] = inactiveColor;
        alienStateColorMap[AlienState.DISABLED] = disabledColor;
    }

    void Start()
    {
    }

    void Update()
    {
#if UNITY_ANDROID
        MoveAcceleration();
#endif
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

    public void Jump()
    {
        levelManager.activeAlien.GetComponent<AlienController>().Jump();
    }

    public void ToggleSpecial()
    {
        levelManager.activeAlien.GetComponent<AlienController>().ToggleSpecial();
    }

    public void Move(bool right)
    {
        if(right)
        {
            if(!direction)
            {
                moveSpeed = 0.0f;
            }
        }
        else
        {
            if(direction)
            {
                moveSpeed = 0.0f;
            }
        }
        direction = right;
        moving = true;
    }

    private void MoveAcceleration()
    {
        if(moving)
        {
            moveSpeed += Time.deltaTime * acceleration * (direction ? 1 : -1);
            moveSpeed = Mathf.Clamp(moveSpeed, -1.0f, 1.0f);
            levelManager.activeAlien.GetComponent<AlienController>().hSpeed = moveSpeed;
        }
        else
        {
            moveSpeed += Time.deltaTime * acceleration * (!direction ? 1 : -1);
            moveSpeed = Mathf.Clamp(moveSpeed, (direction ? 0.0f : -1.0f), (direction ? 1.0f : 0.0f));
            levelManager.activeAlien.GetComponent<AlienController>().hSpeed = moveSpeed;
        }
    }

    public void MoveStop()
    {
        moving = false;
    }

    public void SetActiveAlienByNumber(int n)
    {
        switch(n)
        {
            case 1:
                levelManager.SetActiveAlienByType(AlienType.BLUE);
                break;
            case 2:
                levelManager.SetActiveAlienByType(AlienType.GREEN);
                break;
            case 3:
                levelManager.SetActiveAlienByType(AlienType.PINK);
                break;
            case 4:
                levelManager.SetActiveAlienByType(AlienType.BEIGE);
                break;
            case 5:
                levelManager.SetActiveAlienByType(AlienType.YELLOW);
                break;
            default:
                break;
        }
    }
}
