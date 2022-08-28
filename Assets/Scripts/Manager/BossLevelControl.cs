using UnityEngine;

public class BossLevelControl : MonoBehaviour
{
    private void Start()
    {
        GameEvents.OnSceneLoadCommplete();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameEvents.OnStartingBoss();
            gameObject.SetActive(false);
        }
    }
}
