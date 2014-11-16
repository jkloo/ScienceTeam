using UnityEngine;
using System.Collections;

public class PlayButtonController : MonoBehaviour {

    public string nextScene;
    public float loadNextLevelTime = 0.5f;

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
