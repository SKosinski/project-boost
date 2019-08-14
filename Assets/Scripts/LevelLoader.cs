using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 2f;

    public void LoadNextLevel()
    {
        Invoke("ApplyLoadNextLevel", levelLoadDelay);
    }

    public void ReloadLevel()
    {
        Invoke("ApplyReloadLevel", levelLoadDelay);
    }

    public void ApplyLoadNextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings - 1 != SceneManager.GetActiveScene().buildIndex)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
    }

    public void ApplyReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
