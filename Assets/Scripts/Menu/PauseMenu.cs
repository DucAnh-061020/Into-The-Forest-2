using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject HUD;

    private void Start()
    {
        GameEvents.GameWon += HideHud;
        GameEvents.GameOver += HideHud;
        GameEvents.WaveEndUpgrade += HideHud;
        GameEvents.SaveInitiated += ShowHud;
    }
    private void OnDestroy()
    {
        GameEvents.GameWon -= HideHud;
        GameEvents.GameOver -= HideHud;
        GameEvents.WaveEndUpgrade -= HideHud;
        GameEvents.SaveInitiated -= ShowHud;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        QuestionDialogUI.Instance.ShowQuestion("Exit current game?", () =>
        {
            for (int i = 0; i < FindObjectsOfType<KeepOnLoad>().Length; i++)
            {
                Destroy(FindObjectsOfType<KeepOnLoad>()[i].gameObject);
            }
            SceneManager.LoadScene(0);
        }, () =>
        {
            Time.timeScale = 1;
        });
    }

    private void HideHud()
    {
        HUD.SetActive(false);
    }

    private void ShowHud()
    {
        HUD.SetActive(true);
    }

}