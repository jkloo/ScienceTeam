using UnityEngine;
using System.Collections;

public class ButtonZoneController : MonoBehaviour
{

    public GameObject button;
    private GameObject player;
    private GameObject manager;
    private LevelManager levelManager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        levelManager = manager.GetComponent<LevelManager>();
        player = levelManager.activeAlien;
        if(player.collider2D.bounds.Intersects(collider2D.bounds))
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
    }

    void Update()
    {
        player = levelManager.activeAlien;
        button.transform.position = new Vector2(player.transform.position.x, button.transform.position.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            button.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            button.SetActive(false);
        }
    }
}
