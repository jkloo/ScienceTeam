using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    public float acceleration;

    public Texture2D zeroImage;
    public Texture2D oneImage;
    public Texture2D twoImage;
    public Texture2D threeImage;
    public Texture2D fourImage;
    public Texture2D fiveImage;
    public Texture2D sixImage;
    public Texture2D sevenImage;
    public Texture2D eightImage;
    public Texture2D nineImage;

    public GUITexture starCount;
    public GUITexture blueToken;
    public GUITexture greenToken;
    public GUITexture pinkToken;
    public GUITexture beigeToken;
    public GUITexture yellowToken;
    public GUITexture leftButton;
    public GUITexture rightButton;
    public GUITexture upButton;
    public GUITexture downButton;
    public GUITexture aButton;
    public GUITexture bButton;

    private int leftButtonAnchor = -1;
    private int rightButtonAnchor = -1;

    private Dictionary<int, Texture2D> numberMap = new Dictionary<int, Texture2D>();
    private Dictionary<AlienType, bool> activeAliens = new Dictionary<AlienType, bool>();

    private GameObject manager;
    private ItemManager itemManager;
    private LevelManager levelManager;
    private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);
    private Color activeColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color completeColor = new Color(0.49f, 0.39f, 0.1f, 0.5f);


    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        levelManager = manager.GetComponent<LevelManager>();
        itemManager = manager.GetComponent<ItemManager>();

        activeAliens = levelManager.GetActivatedAliens();

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
        activeAliens = levelManager.GetActivatedAliens();

#if UNITY_ANDROID
        foreach(Touch touch in Input.touches)
        {
            if(aButton.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.activeAlien.GetComponent<AlienController>().Jump();
            }
            else if(bButton.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.activeAlien.GetComponent<AlienController>().ToggleSpecial();
            }
            else if(blueToken.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.BLUE);
            }
            else if(greenToken.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.GREEN);
            }
            else if(pinkToken.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.PINK);
            }
            else if(beigeToken.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.BEIGE);
            }
            else if(yellowToken.HitTest(touch.position) && touch.phase == TouchPhase.Began)
            {
                levelManager.SetActiveAlienByType(AlienType.YELLOW);
            }
            else if(rightButtonAnchor == touch.fingerId || rightButton.HitTest(touch.position))
            {
                rightButtonAnchor = touch.fingerId;
                float hSpeed = levelManager.activeAlien.GetComponent<AlienController>().hSpeed;
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    rightButtonAnchor = -1;
                    hSpeed = 0.0f;
                }
                else if(leftButton.HitTest(touch.position))
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
            else if(leftButtonAnchor == touch.fingerId || leftButton.HitTest(touch.position))
            {
                leftButtonAnchor = touch.fingerId;
                float hSpeed = levelManager.activeAlien.GetComponent<AlienController>().hSpeed;
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    leftButtonAnchor = -1;
                    hSpeed = 0.0f;
                }
                else if(rightButton.HitTest(touch.position))
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
    }

    void OnGUI()
    {
        starCount.texture = numberMap[itemManager.nCollectedStars];
        if(itemManager.nCollectedStars == itemManager.nTotalStars)
        {
            starCount.color = completeColor;
        }

        blueToken.color = activeAliens[AlienType.BLUE] ? activeColor : inactiveColor;
        greenToken.color = activeAliens[AlienType.GREEN] ? activeColor : inactiveColor;
        pinkToken.color = activeAliens[AlienType.PINK] ? activeColor : inactiveColor;
        beigeToken.color = activeAliens[AlienType.BEIGE] ? activeColor : inactiveColor;
        yellowToken.color = activeAliens[AlienType.YELLOW] ? activeColor : inactiveColor;
    }
}
