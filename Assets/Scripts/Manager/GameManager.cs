using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager m_gameManager;
    public GameState CurrentState { get; private set; }
    public static GameManager Instance
    {
        get
        {
            if(m_gameManager == null)
            {
                m_gameManager = new GameManager();
            }
            return m_gameManager;
        }
    }

    public void UpdateState(GameState newState)
    {
        switch (newState)
        {
            case GameState.NewGame:
                
                break;
            case GameState.ResumeGame:
                
                break;
        }
    }
}

public enum GameState
{
    NewGame,
    ResumeGame,
    Playing,
    Pause,
    Exit
}