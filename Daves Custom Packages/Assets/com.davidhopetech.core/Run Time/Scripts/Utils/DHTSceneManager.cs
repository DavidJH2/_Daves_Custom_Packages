using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DHTSceneManager : DHTService<DHTSceneManager>
{
    private List<string> scenesLoaded = new List<string>();

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }


    public void NewScene(string newSceneName)
    {
        foreach (var sceneName in scenesLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }    
        
        LoadScene(newSceneName);
    }
}
