using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
  * 
  * 
  * 
  * 
  * 
  * 
  * 
  * 
  */

public class Launcher : MonoBehaviour
{
    public GameObject GameLauncher;

    private void Awake()
    {
        GameLauncher.SetActive(SceneManager.GetActiveScene().buildIndex == 0);
        Time.timeScale= 1.0f;
    }

    public void ChargerScene(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }

    public void ChargerLauncherGame()
    {
        ChargerScene(0);
    }
}
