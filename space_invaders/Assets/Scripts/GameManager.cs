using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _loadingScene = false;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Credits" && !_loadingScene) // If scene is credits load main menu after 5 seconds
        {
            _loadingScene = true;
            StartCoroutine(LoadSceneDelay("Main Menu", 5f));
        }
    }

    IEnumerator LoadSceneDelay(string sceneName, float delay) // Load scene after x seconds
    {
        yield return new WaitForSeconds(delay);
        LoadScene(sceneName);
        _loadingScene = false;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
