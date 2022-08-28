using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_timerText;
    [HideInInspector]
    public float totalTime = 0;
    public int level = 1;

    public static GameLogic instance;

    // Start is called before the first frame update
    void Start()
    {
        totalTime = 0;
        GameEvents.SaveInitiated += Save;
        GameEvents.LoadNextScene += LoadNextScene;
        if (SaveLoad.SaveExits("GameTime"))
        {
            GameSave gameSave = SaveLoad.Load<GameSave>("GameTime");
            totalTime = gameSave.timeLast;
            level = gameSave.currentLevel;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        if(instance != this)
        {
            Destroy(gameObject);
        }
        AudioManager.instace.PlayRandomClipType(Sound.AudioType.BGM);
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalTime);
        m_timerText.SetText(timeSpan.ToString("mm':'ss"));
    }
    #region Event handles
    private void Save()
    {
        SaveLoad.Save(new GameSave(totalTime, SceneManager.GetActiveScene().buildIndex, level++), "GameTime");
    }

    private void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex <= SceneManager.sceneCountInBuildSettings)
            StartCoroutine(LoadSceneAsync());
    }
    #endregion

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        GameEvents.OnSceneLoadCommplete();
        AudioManager.instace.StopAllClip();
        AudioManager.instace.PlayRandomClipType(Sound.AudioType.BGM);
    }

    private void OnDestroy()
    {
        GameEvents.SaveInitiated -= Save;
        GameEvents.LoadNextScene -= LoadNextScene;
    }
}