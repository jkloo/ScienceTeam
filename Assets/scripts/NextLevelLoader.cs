using UnityEngine;
using System.Collections;

public class NextLevelLoader : MonoBehaviour {

    public string nextLevel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Application.LoadLevel(nextLevel);
        }
    }

}
