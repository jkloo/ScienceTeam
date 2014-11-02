using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

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
    public GUITexture dPad;
    public GUITexture aButton;
    public GUITexture bButton;

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
        dPad.gameObject.SetActive(false);
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
                levelManager.activeAlien.GetComponent<AlienController>().StartSpecial();
            }
            else if(bButton.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
            {
                levelManager.activeAlien.GetComponent<AlienController>().StopSpecial();
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
            else if(dPad.HitTest(touch.position))
            {
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    levelManager.activeAlien.GetComponent<AlienController>().hSpeed = 0.0f;
                    continue;
                }
                Rect bounds = dPad.GetScreenRect();
                if(touch.position.x > bounds.center.x)
                {
                    levelManager.activeAlien.GetComponent<AlienController>().hSpeed = 1.0f;;
                }
                else
                {
                    levelManager.activeAlien.GetComponent<AlienController>().hSpeed = -1.0f;;
                }
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
