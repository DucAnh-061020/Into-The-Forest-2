using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            GameEvents.OnLoadNextScene();
    }
    BoxCollider2D box;

    private void Start()
    {
        if (SaveLoad.SaveExits("GameTime"))
        {
            if (SaveLoad.Load<GameSave>("GameTime").lastScene == SceneManager.GetActiveScene().buildIndex)
                box.isTrigger = true;
        }
    }
    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        GameEvents.WaveEndUpgrade += TurnTrigger;
        box.isTrigger = false;
    }
    public void TurnTrigger()
    {
        box.isTrigger = true;
    }
    private void OnDestroy()
    {
        GameEvents.WaveEndUpgrade -= TurnTrigger;
    }
}