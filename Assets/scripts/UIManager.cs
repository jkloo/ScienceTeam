using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

    public Texture2D blueAlienToken;
    public Texture2D greenAlienToken;
    public Texture2D pinkAlienToken;
    public Texture2D beigeAlienToken;
    public Texture2D yellowAlienToken;

    public Texture2D star;
    public Texture2D zero;
    public Texture2D one;
    public Texture2D two;
    public Texture2D three;
    public Texture2D four;
    public Texture2D five;
    public Texture2D six;
    public Texture2D seven;
    public Texture2D eight;
    public Texture2D nine;

    private Dictionary<int, Texture2D> numberMap = new Dictionary<int, Texture2D>();

    private GameObject manager;
    private ItemManager itemManager;
    private LevelManager levelManager;
    private Color inactive = new Color(1, 1, 1, 0.25f);
    private Color active = new Color(1, 1, 1, 1);
    private Color complete = new Color(0.98f, 0.78f, 0.2f);

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        levelManager = manager.GetComponent<LevelManager>();
        itemManager = manager.GetComponent<ItemManager>();
        numberMap[0] = zero;
        numberMap[1] = one;
        numberMap[2] = two;
        numberMap[3] = three;
        numberMap[4] = four;
        numberMap[5] = five;
        numberMap[6] = six;
        numberMap[7] = seven;
        numberMap[8] = eight;
        numberMap[9] = nine;
    }

    void OnGUI()
    {
        Dictionary<AlienType, bool> activatedAliens = levelManager.GetActiveAliens();

        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.MiddleCenter;

        float buttonRowY = Screen.height * 0.05f;
        float screenMiddleX = Screen.width / 2.0f;
        float buttonSize = Screen.height * 0.1f;
        float buttonGap = Screen.width * 0.01f;

        Rect blueRect = new Rect(screenMiddleX - 2 * buttonSize, buttonRowY, buttonSize, buttonSize);
        Rect greenRect = new Rect(screenMiddleX - buttonSize, buttonRowY, buttonSize, buttonSize);
        Rect pinkRect = new Rect(screenMiddleX, buttonRowY, buttonSize, buttonSize);
        Rect beigeRect = new Rect(screenMiddleX + buttonSize, buttonRowY, buttonSize, buttonSize);
        Rect yellowRect = new Rect(screenMiddleX + 2 * buttonSize, buttonRowY, buttonSize, buttonSize);

        GUI.color = activatedAliens[AlienType.BLUE] ? active : inactive;
        if(GUI.Button(blueRect, blueAlienToken, centeredStyle))
        {
            Debug.Log("Blue button Clicked");
            levelManager.SetActiveAlienByType(AlienType.BLUE);
        }

        GUI.color = activatedAliens[AlienType.GREEN] ? active : inactive;
        if(GUI.Button(greenRect, greenAlienToken, centeredStyle))
        {
            Debug.Log("Green button Clicked");
            levelManager.SetActiveAlienByType(AlienType.GREEN);
        }

        GUI.color = activatedAliens[AlienType.PINK] ? active : inactive;
        if(GUI.Button(pinkRect, pinkAlienToken, centeredStyle))
        {
            Debug.Log("Pink button Clicked");
            levelManager.SetActiveAlienByType(AlienType.PINK);
        }

        GUI.color = activatedAliens[AlienType.BEIGE] ? active : inactive;
        if(GUI.Button(beigeRect, beigeAlienToken, centeredStyle))
        {
            Debug.Log("Beige button Clicked");
            levelManager.SetActiveAlienByType(AlienType.BEIGE);
        }

        GUI.color = activatedAliens[AlienType.YELLOW] ? active : inactive;
        if(GUI.Button(yellowRect, yellowAlienToken, centeredStyle))
        {
            Debug.Log("Yellow button Clicked");
            levelManager.SetActiveAlienByType(AlienType.YELLOW);
        }

        Rect starRect = new Rect(buttonGap, buttonRowY, buttonSize, buttonSize);
        Rect numberRect = new Rect(buttonSize, buttonRowY, buttonSize, buttonSize);


        Texture2D nCollectedTexture = zero;
        if(numberMap.ContainsKey(itemManager.nCollectedStars))
        {
            nCollectedTexture = numberMap[itemManager.nCollectedStars];
        }

        GUI.color = active;
        GUI.Label(starRect, star, centeredStyle);

        GUI.color = (itemManager.nCollectedStars >= itemManager.nTotalStars) ? complete : active;
        GUI.Label(numberRect, nCollectedTexture, centeredStyle);


    }
}
