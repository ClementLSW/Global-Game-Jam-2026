using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    public InputActionReference restart;
    public InputActionReference pause;
    public InputActionReference start;

    private bool isPaused = false;

    public RectTransform pauseMenuUI;

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

    private void OnEnable()
    {
        restart.action.performed += OnRestartPerformed;
        restart.action.Enable();

        pause.action.performed += OnPausePerformed;
        pause.action.Enable();

        start.action.performed += OnStartPerformed;
        start.action.Enable();
    }

    private void OnDisable()
    {
        restart.action.performed -= OnRestartPerformed;
        restart.action.Disable();

        pause.action.performed -= OnPausePerformed;
        pause.action.Disable();

        start.action.performed -= OnStartPerformed;
        start.action.Disable();
    }

    private void OnRestartPerformed(InputAction.CallbackContext context)
    {
        RestartScene();
    }
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    private void OnStartPerformed(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "Mainmenu")
        {
            LoadGameScene();
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        RestartScene();
    //    }

    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        TogglePause();
    //    }
    //}

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
        LoadSceneByName("Game Test");
        pauseMenuUI = GameObject.Find("Pause").GetComponent<RectTransform>();
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
            pauseMenuUI.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuUI.gameObject.SetActive(true);
        }
    }
}
