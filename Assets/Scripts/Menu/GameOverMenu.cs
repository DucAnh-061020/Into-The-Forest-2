using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private void Start()
    {
        GameEvents.GameWon += OnGameWon;
        GameEvents.GameOver += OnGameOver;
        gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        for (int i = 0; i < FindObjectsOfType<KeepOnLoad>().Length; i++)
        {
            Destroy(FindObjectsOfType<KeepOnLoad>()[i].gameObject);
        }
        List<GameClear> datalist = new List<GameClear>();
        if (SaveLoad.SaveExits("GameRecord", "TopRecords"))
        {
            datalist = SaveLoad.Load<List<GameClear>>("GameRecord", "TopRecords");
        }

        datalist.Add(new GameClear(GameLogic.instance.totalTime, GameLogic.instance.level));
        List<GameClear> saveList = datalist.OrderBy(y => y.m_levelClear).OrderBy(x => x.m_timeClear).Take(10).ToList();
        SaveLoad.Save(saveList, "GameRecord", "TopRecords");
        SaveLoad.SerioyslyDeleteallSave();
        SceneManager.LoadScene(0);
    }

    private void OnGameWon()
    {
        if (gameObject.name == "GameWon")
            gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnGameOver()
    {
        if (gameObject.name == "GameOver")
            gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        GameEvents.GameWon -= OnGameWon;
        GameEvents.GameOver -= OnGameOver;
    }
}