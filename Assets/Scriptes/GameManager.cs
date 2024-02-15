using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void WinGame(float time)
    {
        StartCoroutine(WinGameRoutine(time));
    }

    public void LoseGame(float time)
    {
        StartCoroutine(LoseGameRoutine(time));
    }

    private IEnumerator WinGameRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(GlobalSetting.SceneName_Win);
    }

    private IEnumerator LoseGameRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(GlobalSetting.SceneName_Lose);
    }
}

