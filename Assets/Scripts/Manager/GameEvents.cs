using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    #region Events
    public static event Action SaveInitiated;
    public static event Action<UpgradeChoose> UpgradeInitiated;
    public static event Action<int> EnemyDestroyed;
    public static event Action WaveEndUpgrade;
    public static event Action GameWon;
    public static event Action GameOver;
    public static event Action<float,float> SetHealth;
    public static event Action<float> UpdateHealth;
    public static event Action LoadNextScene;
    public static event Action SceneLoadComplete;
    public static event Action StartBoss;
    #endregion

    #region Event handles
    public static void OnSaveInitiated()
    {
        SaveInitiated?.Invoke();
    }

    public static void OnUpgradeInitiated(UpgradeChoose upgrade)
    {
        UpgradeInitiated?.Invoke(upgrade);
    }

    public static void OnEnemyDestroyed(int id)
    {
        EnemyDestroyed?.Invoke(id);
    }

    public static void OnWaveEndUpgrade()
    {
        WaveEndUpgrade?.Invoke();
    }

    public static void OnGameWon()
    {
        GameWon?.Invoke();
    }

    public static void OnGameOver()
    {
        GameOver?.Invoke();
    }

    public static void OnSetHealth(float maxHp, float currentHp)
    {
        SetHealth?.Invoke(maxHp, currentHp);
    }

    public static void OnUpdateHealth(float hp)
    {
        UpdateHealth?.Invoke(hp);
    }

    public static void OnLoadNextScene()
    {
        LoadNextScene?.Invoke();
    }

    public static void OnSceneLoadCommplete()
    {
        SceneLoadComplete?.Invoke();
    }

    public static void OnStartingBoss()
    {
        StartBoss?.Invoke();
    }
    #endregion
}