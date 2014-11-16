using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    public float acceleration;

    public Sprite zeroImage;
    public Sprite oneImage;
    public Sprite twoImage;
    public Sprite threeImage;
    public Sprite fourImage;
    public Sprite fiveImage;
    public Sprite sixImage;
    public Sprite sevenImage;
    public Sprite eightImage;
    public Sprite nineImage;

    public Image starCount;
    public Image blueToken;
    public Image greenToken;
    public Image pinkToken;
    public Image beigeToken;
    public Image yellowToken;
    public Image leftButton;
    public Image rightButton;
    public Image upButton;
    public Image downButton;
    public Image aButton;
    public Image bButton;

    private int leftButtonAnchor = -1;
    private int rightButtonAnchor = -1;

    public Dictionary<int, Sprite> numberMap = new Dictionary<int, Sprite>();
    private Dictionary<AlienState, Color> alienStateColorMap = new Dictionary<AlienState, Color>();
    private Dictionary<AlienType, AlienState> alienStates = new Dictionary<AlienType, AlienState>();

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
        numberMap[0] = zeroImage;
        numberMap[1] = oneImage;
        numberMap[2] = twoImage;
        numberMap[3] = threeImage;
        numberMap[4] = fourImage;
        numberMap[5] = fiveImage;
        numberMap[6] = sixImage;
        numberMap[7] = sevenImage;
        numberMap[8] = eightImage;
        numberMap[9] = nineImage;
#if !UNITY_ANDROID
        aButton.gameObject.SetActive(false);
        bButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        upButton.gameObject.SetActive(false);
        downButton.gameObject.SetActive(false);
#endif
    }

    void Update()
    {
#if UNITY_ANDROID
        foreach(Touch touch in Input.touches)
        {
            if(aButton.guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.activeAlien.GetComponent<AlienController>().Jump();
            }
            else if(bButton.guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.activeAlien.GetComponent<AlienController>().ToggleSpecial();
            }
            else if(blueToken.guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.BLUE);
            }
            else if(greenToken.guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.GREEN);
            }
            else if(pinkToken.guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.PINK);
            }
            else if(beigeToken.guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.BEIGE);
            }
            else if(yellowToken.guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.YELLOW);
            }
            else if(rightButtonAnchor == touch.fingerId || rightButton.guiTexture.HitTest(touch.position))
            {
                rightButtonAnchor = touch.fingerId;
                float hSpeed = levelManager.activeAlien.GetComponent<AlienController>().hSpeed;
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    rightButtonAnchor = -1;
                    hSpeed = 0.0f;
                }
                else if(leftButton.guiTexture.HitTest(touch.position))
                {
                    rightButtonAnchor = -1;
                    leftButtonAnchor = touch.fingerId;
                    hSpeed = 0.0f;
                }
                else
                {
                    hSpeed += Time.deltaTime * acceleration;
                }
                hSpeed = Mathf.Clamp(hSpeed, 0.0f, 1.0f);
                levelManager.activeAlien.GetComponent<AlienController>().hSpeed = hSpeed;
            }
            else if(leftButtonAnchor == touch.fingerId || leftButton.guiTexture.HitTest(touch.position))
            {
                leftButtonAnchor = touch.fingerId;
                float hSpeed = levelManager.activeAlien.GetComponent<AlienController>().hSpeed;
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    leftButtonAnchor = -1;
                    hSpeed = 0.0f;
                }
                else if(rightButton.guiTexture.HitTest(touch.position))
                {
                    leftButtonAnchor = -1;
                    rightButtonAnchor = touch.fingerId;
                    hSpeed = 0.0f;
                }
                else
                {
                    hSpeed -= Time.deltaTime * acceleration;
                }
                hSpeed = Mathf.Clamp(hSpeed, -1.0f, 0.0f);
                levelManager.activeAlien.GetComponent<AlienController>().hSpeed = hSpeed;
            }
        }
#endif
		// starCount.sprite = numberMap[itemManager.nCollectedStars];
        alienStates = levelManager.GetAlienStates();
        if(itemManager.nCollectedStars == itemManager.nTotalStars)
        {
            starCount.color = completeColor;
        }

        blueToken.color = alienStateColorMap[alienStates[AlienType.BLUE]];
        greenToken.color = alienStateColorMap[alienStates[AlienType.GREEN]];
        pinkToken.color = alienStateColorMap[alienStates[AlienType.PINK]];
        beigeToken.color = alienStateColorMap[alienStates[AlienType.BEIGE]];
        yellowToken.color = alienStateColorMap[alienStates[AlienType.YELLOW]];
    }
}
