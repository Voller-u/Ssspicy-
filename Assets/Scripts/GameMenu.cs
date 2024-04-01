using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Player player;

    private void Start()
    {
        GameManager.instance.GenerateItems(SceneManager.GetActiveScene().buildIndex - 1);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeInHierarchy)
                ShowPauseMenu();
            else
                Resume();
        }
    }


    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        player.onShow = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        player.onShow = false;
    }

    public void LevelChoose()
    {
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.intoLevelChoose = true;
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.intoLevelChoose = false;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
