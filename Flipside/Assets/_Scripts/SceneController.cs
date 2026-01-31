using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private bool isPaused = false;

    void Awake()
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void RestartScene()
    {
        if (isPaused)
        {
            TogglePause();
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // example function call:
    // SceneController.Instance.LoadSceneByName("Level2");
    public void LoadGameScene()
    {
        LoadSceneByName("Game");
    }
    public void LoadMenuScene()
    {
        LoadSceneByName("Mainmenu");
    }

    public void LoadSceneByName(string sceneName)
    {
        if (isPaused)
            TogglePause();

        SceneManager.LoadScene(sceneName);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
