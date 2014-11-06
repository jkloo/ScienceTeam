using UnityEngine;
using System.Collections;

public class PlayButtonController : MonoBehaviour {

    public string nextScene;
    public float loadNextLevelTime = 0.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<AlienController>().Spin();
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelWait(nextScene));
    }

    IEnumerator LoadNextLevelWait(string nextScene)
    {
        yield return new WaitForSeconds(loadNextLevelTime);
        Application.LoadLevel(nextScene);
    }
}
