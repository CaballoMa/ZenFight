using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEvent : MonoBehaviour
{
    public void ReturnStart()
    {
        SceneManager.LoadScene(GlobalSetting.SceneName_Start);
    }

    public void EnterGameLevel()
    {
        if (Random.Range(0, 10) > 4)
        {
            SceneManager.LoadScene(GlobalSetting.SceneName_GameLevel);
        }
        else
        {
            SceneManager.LoadScene(GlobalSetting.SceneName_GameLevel2);
        }
    }

    public void GoWin()
    {
        SceneManager.LoadScene(GlobalSetting.SceneName_Win);
    }

    public void GoLose()
    {
        SceneManager.LoadScene(GlobalSetting.SceneName_Lose);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
