using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
        AudioManager.instace.PlayClipByName("AboutUs");
    }
    public void StartGame()
    {
        if (SaveLoad.SaveExits("player"))
        {
            QuestionDialogUI.Instance.ShowQuestion("Start new game, previous game will be delete", () => {
                SaveLoad.SerioyslyDeleteallSave();
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }, () => { });
            return;
        }
        AudioManager.instace.StopClipByName("AboutUs");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        if (SaveLoad.SaveExits("GameTime"))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SaveLoad.Load<GameSave>("GameTime").lastScene);
        }
        else
        {
            NotifyDialogUI.Instance.ShowDialog("No gamedata found");
        }
    }

    public void QuitGame()
    {
        QuestionDialogUI.Instance.ShowQuestion("Exit game", () => { Application.Quit(); }, () => { });
    }
}
