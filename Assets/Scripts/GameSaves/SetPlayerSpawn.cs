using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SetPlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        GameEvents.SceneLoadComplete += SceneLoadComplete;
        Transform player = FindObjectOfType<Player>().transform;
        if (SaveLoad.SaveExits("GameTime"))
        {
            if (SaveLoad.Load<GameSave>("GameTime").lastScene == SceneManager.GetActiveScene().buildIndex)
                if (SaveLoad.SaveExits("player"))
                {
                    PlayerSave playerSave = SaveLoad.Load<PlayerSave>("player");
                    Vector3 loadPoint = new Vector3(playerSave.position[0], playerSave.position[1], playerSave.position[2]);
                    player.position = loadPoint;
                    return;
                }
        }
        player.position = transform.position;
        FindObjectOfType<CinemachineVirtualCamera>().Follow = player;
    }

    private void SceneLoadComplete()
    {
        Transform player = FindObjectOfType<Player>().transform;
        if (SaveLoad.SaveExits("GameTime"))
        {
            if (SaveLoad.Load<GameSave>("GameTime").lastScene == SceneManager.GetActiveScene().buildIndex)
                if (SaveLoad.SaveExits("player"))
                {
                    PlayerSave playerSave = SaveLoad.Load<PlayerSave>("player");
                    Vector3 loadPoint = new Vector3(playerSave.position[0], playerSave.position[1], playerSave.position[2]);
                    player.position = loadPoint;
                    return;
                }
        }
        player.position = transform.position;
        FindObjectOfType<CinemachineVirtualCamera>().Follow = player;
    }

    private void OnDestroy()
    {
        GameEvents.SceneLoadComplete -= SceneLoadComplete;
    }
}
