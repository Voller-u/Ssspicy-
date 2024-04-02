using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject dyingPanel;
    public GameObject passPanel;
    public GameObject lockedTip;
    public Player player;


    private void Awake()
    {
        GameManager.instance.InitGame();
    }

    private void Start()
    {
        //GameManager.instance.GenerateItems(SceneManager.GetActiveScene().buildIndex - 1);
        player = GameObject.Find("Player").GetComponent<Player>();
        passPanel.SetActive(false);
        dyingPanel.SetActive(false);
        lockedTip.SetActive(false);
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
        if(GameManager.instance.gameOver)
        {
            dyingPanel.SetActive(true);
        }
        if(GameManager.instance.gamePass)
        {
            passPanel.SetActive(true);
        }
    }


    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        if(player == null)
            player = GameObject.Find("Player").GetComponent<Player>();
        player.onShow = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        if (player == null)
            player = GameObject.Find("Player").GetComponent<Player>();
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
        GameManager.instance.InitGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    public void GoNextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (GameManager.instance.gameLevel +1 <= GameManager.instance.maxLevelId)
        {
            GameManager.instance.gameLevel++;
            SceneManager.LoadScene(index);
        }
        else
            StartCoroutine(ShowTip());
    }

    IEnumerator ShowTip()
    {
        lockedTip.SetActive(true);
        yield return new WaitForSeconds(5f);
        lockedTip.SetActive(false);
    }
}
