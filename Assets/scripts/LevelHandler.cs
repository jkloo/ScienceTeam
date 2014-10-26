using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour {

    private GameObject activeAlien;
    private GameObject blueAlien;
    private GameObject greenAlien;


    void Awake()
    {
        blueAlien = Instantiate(Resources.Load("blueAlien")) as GameObject;
        SetupAlien(blueAlien, "blueAlien");

        greenAlien = Instantiate(Resources.Load("greenAlien")) as GameObject;
        SetupAlien(greenAlien, "greenAlien");

        SetActiveAlien(blueAlien);
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

    public void SetActiveAlienByType(AlienType alienType)
    {
        GameObject newAlien;
        switch(alienType)
        {
            case AlienType.BLUE:
                newAlien = blueAlien;
                break;
            case AlienType.GREEN:
                newAlien = greenAlien;
                break;
            default:
                return;
        }
        SetActiveAlien(newAlien);
    }
}
