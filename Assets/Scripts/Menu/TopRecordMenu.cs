using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TopRecordMenu : MonoBehaviour
{
    private List<GameClear> gameScores;
    public Transform recordListTemplate;

    // Start is called before the first frame update
    void Start()
    {
        gameScores = new List<GameClear>();
        if (SaveLoad.SaveExits("GameRecord", "TopRecords"))
            gameScores = SaveLoad.Load<List<GameClear>>("GameRecord", "TopRecords");
        GameObject recordTemplate = recordListTemplate.GetChild(0).gameObject;
        GameObject g;

        if (gameScores.Count > 0)
        {
            for (int i = 0; i < gameScores.Count; i++)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(gameScores[i].m_timeClear);
                g = Instantiate(recordTemplate, recordListTemplate);
                g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText((i+1).ToString());
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(timeSpan.ToString("hh':'mm':'ss"));
                g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("Level " + gameScores[i].m_levelClear);
            }
        }
        Destroy(recordTemplate);
    }
}
