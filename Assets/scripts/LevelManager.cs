using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    public float loadNextLevelTime = 0.5f;

    public float maxX = 30f;
    public float maxY = 30f;
    public float minX = -30f;
    public float minY = 2f;

    public string backgroundPrefab;
    private float bgwidth = 10.24f;
    private float yCameraOffset = 2.7f;
    private float yBgOffset = 2.2f;

    private GameObject mainCamera;

    public GameObject activeAlien;
    private GameObject blueAlien;
    private GameObject greenAlien;
    private GameObject pinkAlien;
    private GameObject yellowAlien;
    private GameObject beigeAlien;

    private GameObject levelStart;
    private GameObject levelEnd;
    private GameObject spawnPosition;
    private GameObject[] phasableObjects;

    private Color phaseableColor = new Color(1f, 1f, 1f, 0.9f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);

    private Dictionary<AlienType, bool> activatedAliens = new Dictionary<AlienType, bool>();


    void Awake()
    {
        mainCamera = Instantiate(Resources.Load("camera")) as GameObject;

        Transform[] platforms = GameObject.FindObjectsOfType(typeof(Transform)) as Transform[];
        maxX = platforms[0].position.x;
        minX = platforms[0].position.x;
        minY = platforms[0].position.y;
        foreach(Transform platform in platforms)
        {
            if(platform.position.x > maxX)
            {
                maxX = platform.position.x;
            }
            if(platform.position.x < minX)
            {
                minX = platform.position.x;
            }
            if(platform.position.y < minY)
            {
                minY = platform.position.y;
            }
        }

        // Set up camera bounds
        CameraFollow cameraController = mainCamera.GetComponent<CameraFollow>();
        cameraController.minX = minX;
        cameraController.maxX = maxX;
        cameraController.minY = minY + yCameraOffset;
        cameraController.maxY = maxY;

        // Set up background
        float x = minX - bgwidth;
        while(x <= maxX + bgwidth)
        {
            GameObject bgInst = Instantiate(Resources.Load(backgroundPrefab)) as GameObject;
            bgInst.transform.position = new Vector2(x, minY + yBgOffset);
            x += bgwidth;
        }


        levelStart = GameObject.FindGameObjectWithTag("Start");
        levelEnd = GameObject.FindGameObjectWithTag("Finish");
        spawnPosition = levelStart;

        phasableObjects = GameObject.FindGameObjectsWithTag("Phaseable");

        activatedAliens.Add(AlienType.BLUE, false);
        activatedAliens.Add(AlienType.GREEN, false);
        activatedAliens.Add(AlienType.PINK, false);
        activatedAliens.Add(AlienType.BEIGE, false);
        activatedAliens.Add(AlienType.YELLOW, false);

        blueAlien = Instantiate(Resources.Load("blueAlien")) as GameObject;
        SetupAlien(blueAlien, "blueAlien");

        greenAlien = Instantiate(Resources.Load("greenAlien")) as GameObject;
        SetupAlien(greenAlien, "greenAlien");

        pinkAlien = Instantiate(Resources.Load("pinkAlien")) as GameObject;
        SetupAlien(pinkAlien, "pinkAlien");

        beigeAlien = Instantiate(Resources.Load("beigeAlien")) as GameObject;
        SetupAlien(beigeAlien, "beigeAlien");

        yellowAlien = Instantiate(Resources.Load("yellowAlien")) as GameObject;
        SetupAlien(yellowAlien, "yellowAlien");

        SetActiveAlien(blueAlien);
        activatedAliens[AlienType.BLUE] = true;
    }

    void SetupAlien(GameObject alien, string name)
    {
        alien.name = name;
        alien.GetComponent<AlienController>().Start();
        alien.SetActive(false);
    }

    private void SetActiveAlien(GameObject alien)
    {
        if(activeAlien)
        {
            alien.transform.position = activeAlien.transform.position;
            activeAlien.SetActive(false);
        }
        else
        {
            alien.transform.position = spawnPosition.transform.position;
        }
        activeAlien = alien;
        mainCamera.GetComponent<CameraFollow>().player = activeAlien.transform;
        activeAlien.SetActive(true);
        activeAlien.GetComponent<AlienController>().StopSpecial();
        activeAlien.GetComponent<AlienController>().Spin();
    }

    public void MoveAlienToRespawn()
    {
        activeAlien.transform.position = spawnPosition.transform.position;
    }

    public void SetCheckpoint(GameObject checkpoint)
    {
        if(spawnPosition != levelStart)
        {
            spawnPosition.GetComponent<Animator>().SetBool("active", false);
        }
        spawnPosition = checkpoint;
        spawnPosition.GetComponent<Animator>().SetBool("active", true);
    }

    public bool SetActiveAlienByType(AlienType alienType)
    {
        if(!activatedAliens[alienType])
        {
            return false;
        }

        if(activeAlien.GetComponent<AlienController>().alienType == alienType)
        {
            return false;
        }

        GameObject newAlien;
        switch(alienType)
        {
            case AlienType.BLUE:
                newAlien = blueAlien;
                break;
            case AlienType.GREEN:
                newAlien = greenAlien;
                break;
            case AlienType.PINK:
                newAlien = pinkAlien;
                break;
            case AlienType.BEIGE:
                newAlien = beigeAlien;
                break;
            case AlienType.YELLOW:
                newAlien = yellowAlien;
                break;
            default:
                return false;
        }
        SetActiveAlien(newAlien);
        return true;
    }

    public void StartPhaseObjects()
    {
        foreach(GameObject obj in phasableObjects)
        {
            obj.collider2D.isTrigger = true;
            obj.GetComponent<SpriteRenderer>().color = phaseableColor;
        }
    }

    public bool CanStopPhaseObjects()
    {
        foreach(GameObject obj in phasableObjects)
        {
            if(activeAlien.collider2D.bounds.Intersects(obj.collider2D.bounds))
            {
                return false;
            }
        }
        return true;
    }

    public void StopPhaseObjects()
    {
        foreach(GameObject obj in phasableObjects)
        {
            obj.collider2D.isTrigger = false;
            obj.GetComponent<SpriteRenderer>().color = normalColor;
        }
    }

    public void ActivateAlien(AlienType alienType)
    {
        activatedAliens[alienType] = true;
    }

    public Dictionary<AlienType, bool> GetActivatedAliens()
    {
        return activatedAliens;
    }

    public void LoadNextLevel()
    {
        string nextLevel = levelEnd.GetComponent<NextLevelLoader>().nextLevel;
        StartCoroutine(LoadNextLevelWait(nextLevel));
    }

    IEnumerator LoadNextLevelWait(string nextLevel)
    {
        yield return new WaitForSeconds(loadNextLevelTime);
        Application.LoadLevel(nextLevel);
    }
}
