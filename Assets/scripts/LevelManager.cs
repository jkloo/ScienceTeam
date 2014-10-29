using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    private GameObject activeAlien;
    private GameObject blueAlien;
    private GameObject greenAlien;
    private GameObject pinkAlien;
    private GameObject yellowAlien;
    private GameObject beigeAlien;

    private Dictionary<AlienType, bool> activatedAliens = new Dictionary<AlienType, bool>();


    void Awake()
    {
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
        GameObject spawnPosition = GameObject.FindGameObjectWithTag("Start");
        alien.GetComponent<AlienController>().respawnPosition = spawnPosition.transform;
        alien.SetActive(false);
    }

    private void SetActiveAlien(GameObject alien)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        if(activeAlien)
        {
            alien.GetComponent<AlienController>().respawnPosition = activeAlien.GetComponent<AlienController>().respawnPosition;
            alien.transform.position = activeAlien.transform.position;
            activeAlien.SetActive(false);
        }
        else
        {
            alien.transform.position = alien.GetComponent<AlienController>().respawnPosition.position;
        }
        activeAlien = alien;
        camera.GetComponent<CameraFollow>().player = activeAlien.transform;
        activeAlien.SetActive(true);
        activeAlien.GetComponent<AlienController>().Spin();

    }

    public bool SetActiveAlienByType(AlienType alienType)
    {
        if(!activatedAliens[alienType])
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

    public void ActivateAlien(AlienType alienType)
    {
        activatedAliens[alienType] = true;
    }
}
