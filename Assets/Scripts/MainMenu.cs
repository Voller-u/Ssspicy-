using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startBtn;
    public Button exitbtn;

    public GameObject snake;
    public GameObject mainMenuPanel;
    public GameObject levelChoosePanel;
    public GameObject lockedPanel;

    private void Awake()
    {
        if(GameManager.instance.intoLevelChoose)
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        snake.SetActive(false);
        mainMenuPanel.SetActive(false);
        levelChoosePanel.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ChooseLevel(int levelId)
    {
        if (levelId == 1) SceneManager.LoadScene("SampleScene");
        Debug.Log("Level choose : " + levelId);
        if (SceneManager.GetSceneByName("Level" + levelId).IsValid())
            SceneManager.LoadScene("Level" + levelId);
        else
            StartCoroutine(ShowLockedPanel());
    }

    public void BackToMainMenu()
    {
        snake.SetActive(true);
        mainMenuPanel.SetActive(true);
        levelChoosePanel.SetActive(false);

    }

    IEnumerator ShowLockedPanel()
    {
        lockedPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        lockedPanel.SetActive(false);
    }
    
    

}
